import SkeletonCartItem from '@/components/skeletons/SkeletonCartItem'
import SkeletonCartSummary from '@/components/skeletons/SkeletonCartSummary'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useDebounce from '@/hooks/useDebounce'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setCartStore } from '@/redux/reducers/patientReducer'
import { Box, Button, Container, Grid, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CartItemSection from './sections/CartItemSection'
import CartSummarySection from './sections/CartSummarySection'

const CartPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const navigate = useNavigate()

	const cartStore = useReduxStore({ selector: (s) => s.patient.cart, setStore: setCartStore })
	const cartData = cartStore.data

	const [drafts, setDrafts] = useState({})
	const [pendingRemove, setPendingRemove] = useState(null)

	const setDraft = useCallback((key, value) => {
		setDrafts((prev) => ({ ...prev, [key]: value }))
	}, [])

	const deleteItem = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => cartStore.fetch(),
	})

	const updateItem = useAxiosSubmit({
		method: 'PUT',
		onSuccess: () => cartStore.fetch(),
	})

	const handleRemove = useCallback(async (skuCode) => {
		if (!cartData || !skuCode) return

		const originalItem = cartData.cartDetails?.find(
			(item) => (item.skuCode) === skuCode
		)
		const originalQty = originalItem?.quantity ?? 1

		const ok = await confirm({ title: t('checkout.confirm_delete') })
		if (!ok) {
			setDraft(skuCode, String(originalQty))
			return
		}

		await deleteItem.submit({
			overrideUrl: ApiUrls.CART.DELETE(skuCode),
		})
	}, [cartData, confirm, deleteItem, t, setDraft])

	useDebounce(() => {
		if (pendingRemove) {
			handleRemove(pendingRemove)
			setPendingRemove(null)
		}
	}, 1500, [pendingRemove])

	const updateQuantity = useCallback(async (skuCode, newQty, onComplete) => {
		if (!cartData || !skuCode) return

		if (newQty <= 0) {
			setPendingRemove(skuCode)
			onComplete?.()
			return
		}

		if (pendingRemove === skuCode) {
			setPendingRemove(null)
		}

		await updateItem.submit({
			overrideData: { skuCode, quantity: newQty },
			overrideUrl: ApiUrls.CART.UPDATE,
		})
		onComplete?.()
	}, [cartData, updateItem, pendingRemove])

	const isInitialLoading = cartStore.loading && !cartData
	const rawDetails = cartData?.cartDetails?.filter((item) => item.medicineSKU !== null) || []
	const validDetails = rawDetails.map((item) => {
		const skuCode = item.medicineSKU?.skuCode || item.skuCode
		const draftQty = drafts[skuCode]
		if (draftQty !== undefined) {
			return { ...item, quantity: Number(draftQty) }
		}
		return item
	})
	const total =
		validDetails.reduce(
			(sum, item) => sum + (item.discountPrice || item.medicineSKU?.price || 0) * item.quantity,
			0
		) || 0

	if (isInitialLoading || !cartData)
		return (
			<Container maxWidth='lg' sx={{ py: 6 }}>
				<Grid container spacing={3}>
					<Grid size={{ xs: 12 }}>
						<Box sx={{ maxHeight: '80vh', overflowY: 'auto', pr: 1 }}>
							<Stack spacing={2}>
								{Array.from({ length: 3 }).map((_, index) => (
									<SkeletonCartItem key={index} />
								))}
							</Stack>
						</Box>
					</Grid>
					<Grid size={{ xs: 12, md: 4 }}>
						<SkeletonCartSummary />
					</Grid>
				</Grid>
			</Container>
		)

	if (cartStore.error)
		return (
			<Container sx={{ py: 4 }}>
				<Typography color='error'>
					{t('common.error.loading_data')}: {cartStore.error.message || t('common.error.unknown')}
				</Typography>
			</Container>
		)

	const isEmpty = validDetails.length === 0

	return (
		<Container maxWidth='lg' sx={{ py: 6 }}>
			<Typography variant='h4' mb={4}>
				{t('cart.title')}
			</Typography>

			{isEmpty ? (
				<Paper sx={{ p: 6, textAlign: 'center' }}>
					<Typography variant='h6'>{t('cart.empty_cart')}</Typography>
					<Button variant='contained' sx={{ mt: 2 }} onClick={() => navigate(routeUrls.HOME.MEDICINE)}>
						{t('cart.continue_shopping')}
					</Button>
				</Paper>
			) : (
				<Grid container spacing={3}>
					<Grid size={{ xs: 12, md: 8 }}>
						<CartItemSection
							validDetails={validDetails}
							drafts={drafts}
							setDraft={setDraft}
							onRemove={handleRemove}
							onUpdate={updateQuantity}
							isSubmitting={updateItem.loading}
						/>
					</Grid>
					<Grid size={{ xs: 12, md: 4 }}>
						<CartSummarySection
							validDetails={validDetails}
							total={total}
							onCheckout={() => navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.CHECKOUT))}
						/>
					</Grid>
				</Grid>
			)}
		</Container>
	)
}

export default CartPage

import SkeletonCartItem from '@/components/skeletons/SkeletonCartItem'
import SkeletonCartSummary from '@/components/skeletons/SkeletonCartSummary'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useDebounce from '@/hooks/useDebounce'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setCartStore } from '@/redux/reducers/patientReducer'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Box, Button, Container, Grid, Paper, Stack, Typography, useTheme } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import CartItemDisplaySection from './sections/CartItemDisplaySection'
import CheckoutPaymentMethodSection from './sections/CheckoutPaymentMethodSection'
import CheckoutSummarySection from './sections/CheckoutSummarySection'
import CheckoutVoucherInputSection from './sections/CheckoutVoucherInputSection'
import CheckoutVoucherListSection from './sections/CheckoutVoucherListSection'

const CheckoutPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const theme = useTheme()

	const cartStore = useReduxStore({ selector: (s) => s.patient.cart, setStore: setCartStore })
	const cartData = cartStore.data

	const getVouchers = useFetch(ApiUrls.VOUCHER.MY_ALL_VOUCHERS)
	const [voucherList, setVoucherList] = useState([])

	useEffect(() => {
		setVoucherList(getVouchers.data || [])
	}, [getVouchers.data])

	const checkout = useAxiosSubmit({
		url: ApiUrls.CART.CHECKOUT,
		method: 'POST',
		onSuccess: async () => {
			await cartStore.fetch()
		},
	})

	const items = useMemo(
		() => cartData?.cartDetails?.filter((item) => item.medicineSKU !== null) || [],
		[cartData]
	)

	const total = useMemo(() => {
		return (
			items.reduce((sum, item) => {
				const unitPrice = item.discountPrice ?? item.originPrice ?? item.medicineSku?.price ?? 0
				return sum + unitPrice * item.quantity
			}, 0) || 0
		)
	}, [items])

	const [voucherDraft, setVoucherDraft] = useState('')
	const [appliedVoucher, setAppliedVoucher] = useState(null)
	const [isApplying, setIsApplying] = useState(false)
	const [isTyping, setIsTyping] = useState(false)
	const [isPickupAtStore, setIsPickupAtStore] = useState(false)

	useEffect(() => {
		if (
			isTyping &&
			appliedVoucher !== null &&
			voucherDraft?.toLowerCase() !== appliedVoucher?.toLowerCase()
		) {
			setAppliedVoucher(null)
		}
		setIsTyping(false)
	}, [voucherDraft, appliedVoucher, isTyping])

	const formatVoucherValue = (voucher) => {
		const discountType = voucher?.discountType
		const discountValue = voucher?.discountValue ?? 0
		const isPercentage = discountType === EnumConfig.VoucherDiscountType.Percentage
		if (isPercentage) return `${discountValue}%`
		return formatCurrencyBasedOnCurrentLanguage(discountValue)
	}

	const allVouchers = useMemo(() => {
		return (voucherList || []).map((v) => ({
			id: v.id,
			code: v.code,
			name: v.name,
			label: `${v.code} - ${formatVoucherValue(v)}`,
			description: v.description,
			valueLabel: formatVoucherValue(v),
			expiry: v.expireDate,
			minPrice: v.minPurchaseAmount,
			discountType: v.discountType,
			discountValue: v.discountValue,
			maxDiscount: v.maxDiscount,
			image: getImageFromCloud(v.image),
			isAvailable: !v.minPurchaseAmount || v.minPurchaseAmount <= total,
		}))
	}, [voucherList, total])

	const voucherOptions = allVouchers.filter((v) => v.isAvailable)

	const discountAmount = useMemo(() => {
		if (!appliedVoucher) return 0
		const voucher = voucherOptions.find(
			(v) => v.code?.toLowerCase() === appliedVoucher?.toLowerCase()
		)
		if (!voucher) return 0

		if (voucher.discountType === EnumConfig.VoucherDiscountType.Percentage) {
			const value = voucher.discountValue ?? 0
			let discount = (total * value) / 100
			if (voucher.maxDiscount && voucher.maxDiscount > 0) {
				discount = Math.min(discount, voucher.maxDiscount)
			}
			return discount
		}
		return voucher.discountValue || 0
	}, [appliedVoucher, voucherOptions, total])

	const finalTotal = useMemo(() => Math.max(0, total - discountAmount), [total, discountAmount])
	const [paymentType, setPaymentType] = useState(
		finalTotal === 0 ? EnumConfig.PaymentType.Cash : EnumConfig.PaymentType.PayOS
	)

	useEffect(() => {
		if (finalTotal === 0 && paymentType !== EnumConfig.PaymentType.Cash) {
			setPaymentType(EnumConfig.PaymentType.Cash)
		}
	}, [finalTotal, paymentType])

	const applyVoucher = (code) => {
		const trimmedCode = (code || '').trim()
		if (!trimmedCode) return

		if (appliedVoucher?.toLowerCase() === trimmedCode.toLowerCase()) return

		setIsApplying(true)
		const isValid = voucherOptions.some((v) => v.code?.toLowerCase() === trimmedCode.toLowerCase())
		if (!isValid) {
			toast.error(t('cart.summary.voucher_not_available'))
			setTimeout(() => setIsApplying(false), 1500)
			return
		}

		setVoucherDraft(trimmedCode)
		setAppliedVoucher(trimmedCode)
		toast.success(t('cart.summary.voucher_apply_success'))
		setIsApplying(false)
	}

	const [pendingCancel, setPendingCancel] = useState(false)
	const [isCancelling, setIsCancelling] = useState(false)

	const cancelVoucher = () => {
		setIsCancelling(true)
		setPendingCancel(true)
	}

	useDebounce(
		() => {
			if (pendingCancel) {
				setAppliedVoucher(null)
				setVoucherDraft('')
				toast.success(t('cart.summary.cancel_voucher_success'))
				setPendingCancel(false)
				setIsCancelling(false)
			}
		},
		500,
		[pendingCancel]
	)

	const isInitialLoading = cartStore.loading && !cartData

	useEffect(() => {
		if (!isInitialLoading && !cartStore.error && items.length === 0) {
			navigate(routeUrls.HOME.MEDICINE)
		}
	}, [isInitialLoading, cartStore.error, items.length])

	if (isInitialLoading || !cartData)
		return (
			<Container maxWidth='lg' sx={{ py: 6 }}>
				<Grid container spacing={3}>
					<Grid size={{ xs: 12, md: 8 }}>
						<Stack spacing={2}>
							{Array.from({ length: 3 }).map((_, i) => (
								<SkeletonCartItem key={i} />
							))}
						</Stack>
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
				<Typography color='error'>{t('common.error.loading_data')}</Typography>
			</Container>
		)

	const isEmpty = items.length === 0

	const handleCheckout = async () => {
		const selectedSKUCodes = items.map((item) => item.skuCode)
		const response = await checkout.submit({
			overrideData: {
				voucherCode: appliedVoucher || null,
				paymentType,
				description: t('cart.summary.checkout_description'),
				selectedSKUCodes,
				isPickupAtStore,
			},
		})
		const paymentUrl = response?.data?.paymentUrl
		if (paymentUrl) {
			if (paymentType === EnumConfig.PaymentType.SePay) {
				navigate(routeUrls.HOME.PAYMENT.SEPAY, {
					state: {
						paymentUrl,
						total: finalTotal,
						items,
					},
				})
			} else {
				navigate(routeUrls.HOME.MEDICINE)
				setTimeout(() => {
					window.location.href = paymentUrl
				}, 100)
			}
		} else {
			navigate(routeUrls.HOME.MEDICINE)
		}
	}

	return (
		<Box sx={{ backgroundColor: theme.palette.background.default, minHeight: '100vh', py: 6 }}>
			<Container maxWidth='lg'>
				<Stack direction='row' justifyContent='space-between' mb={3}>
					<Typography variant='h4' sx={{ fontWeight: 800 }}>
						{t('cart.summary.checkout')}
					</Typography>
					<Button onClick={() => navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.CART))}>
						{t('button.back')}
					</Button>
				</Stack>

				{isEmpty ? (
					<Paper sx={{ p: 6, textAlign: 'center' }}>
						<Typography variant='h6'>{t('cart.empty_cart')}</Typography>
					</Paper>
				) : (
					<Grid container spacing={3}>
						<Grid size={{ xs: 12, md: 6 }}>
							<CartItemDisplaySection validDetails={items} />
						</Grid>

						<Grid size={{ xs: 12, md: 6 }}>
							<Stack spacing={2}>
								<CheckoutVoucherInputSection
									voucherDraft={voucherDraft}
									appliedVoucher={appliedVoucher}
									onChangeDraft={setVoucherDraft}
									onApply={applyVoucher}
									onCancel={cancelVoucher}
									isApplying={isApplying || isCancelling}
									setIsTyping={setIsTyping}
								/>

								<CheckoutVoucherListSection
									allVouchers={allVouchers}
									appliedVoucher={appliedVoucher}
									onApply={applyVoucher}
									isApplying={isApplying}
								/>

								<CheckoutPaymentMethodSection
									paymentType={paymentType}
									onSelect={setPaymentType}
									isPickupAtStore={isPickupAtStore}
									onPickupChange={setIsPickupAtStore}
									disabled={finalTotal === 0}
								/>

								<CheckoutSummarySection
									total={total}
									discountAmount={discountAmount}
									finalTotal={finalTotal}
									paymentType={paymentType}
									isPickupAtStore={isPickupAtStore}
									onCheckout={handleCheckout}
									loading={checkout.loading}
								/>
							</Stack>
						</Grid>
					</Grid>
				)}
			</Container>
		</Box>
	)
}

export default CheckoutPage

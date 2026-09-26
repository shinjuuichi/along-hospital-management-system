import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setCartStore } from '@/redux/reducers/patientReducer'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { AddShoppingCart } from '@mui/icons-material'
import {
	Box,
	Button,
	Card,
	CardActions,
	CardContent,
	CardMedia,
	Divider,
	Stack,
	Typography,
} from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const MedicineCardSection = ({ medicine, sx = {} }) => {
	const { t } = useTranslation()
	const { auth } = useAuth()
	const navigate = useNavigate()

	const [loadingSkuCode, setLoadingSkuCode] = useState(null)
	const [selectedOptionValueIds, setSelectedOptionValueIds] = useState({})

	const isPatientRole = auth?.role === EnumConfig.Role.Patient || false
	const cartStore = useReduxStore({
		selector: (s) => s.patient.cart,
		setStore: isPatientRole ? setCartStore : null,
	})

	const { submit } = useAxiosSubmit({
		url: ApiUrls.CART.ADD_TO_CART,
		method: 'POST',
		onSuccess: async () => {
			if (isPatientRole) {
				await cartStore.fetch()
			}
		},
	})

	const activeSkus = (medicine?.skus ?? []).filter((sku) => sku?.isActive !== false)
	const inStockSkus = activeSkus.filter((sku) => sku?.inventory?.quantity > 0)
	const isOutOfStock = inStockSkus.length === 0

	const selectedSku = (() => {
		if (inStockSkus.length === 0) return null

		const selectedIds = Object.values(selectedOptionValueIds).filter((id) => typeof id === 'number')

		if (selectedIds.length === 0) return inStockSkus[0]

		const matchedSku = inStockSkus.find((sku) => {
			const skuValueIds = sku.skuValues.map((sv) => sv.optionValueId)
			return selectedIds.every((id) => skuValueIds.includes(id))
		})

		return matchedSku ?? inStockSkus[0]
	})()

	const handleCardClick = () => {
		navigate(`${routeUrls.HOME.MEDICINE}/${medicine.id}`)
	}

	const handleAddToCart = async (skuCode, e) => {
		e?.stopPropagation()
		if (!skuCode || loadingSkuCode) return

		setLoadingSkuCode(skuCode)
		try {
			await submit({
				overrideData: {
					skuCode,
					quantity: 1,
				},
			})
		} finally {
			setLoadingSkuCode(null)
		}
	}

	const optionValueText =
		selectedSku?.skuValues?.length > 0
			? selectedSku.skuValues
					.map((sv) => sv.optionValue?.valueName)
					.filter(Boolean)
					.join(' , ')
			: '—'

	return (
		<Card
			component='div'
			onClick={handleCardClick}
			sx={{
				height: '100%',
				display: 'flex',
				flexDirection: 'column',
				cursor: 'pointer',
				transition: 'transform 0.2s, box-shadow 0.2s',
				'&:hover': {
					transform: 'translateY(-4px)',
					boxShadow: 3,
				},
				...sx,
			}}
		>
			<Box
				sx={{
					height: 200,
					overflow: 'hidden',
					bgcolor: '#f5f5f5',
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
					flexShrink: 0,
				}}
			>
				<CardMedia
					component='img'
					image={getImageFromCloud(medicine.images?.[0])}
					alt={medicine.name}
					sx={{ height: 200, objectFit: 'cover', pointerEvents: 'none' }}
				/>
			</Box>

			<CardContent
				sx={{
					flex: 1,
					display: 'flex',
					flexDirection: 'column',
					alignItems: 'stretch',
					p: 2,
					'&:last-child': { pb: 2 },
				}}
			>
				<Typography variant='h6' noWrap>
					{medicine.name}
				</Typography>

				<Stack direction='row' spacing={2}>
					{medicine.brand && (
						<Typography variant='caption' color='text.secondary'>
							{medicine.brand}
						</Typography>
					)}
					{medicine.medicineUnit?.name && (
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.unit')}: {medicine.medicineUnit.name}
						</Typography>
					)}
				</Stack>

				<Typography
					variant='body2'
					color='text.secondary'
					sx={{
						fontSize: '0.8rem',
						lineHeight: 1.4,
						display: '-webkit-box',
						WebkitLineClamp: 2,
						WebkitBoxOrient: 'vertical',
						overflow: 'hidden',
						flexGrow: 0,
					}}
				>
					{medicine.description}
				</Typography>

				{activeSkus.length > 0 && (
					<Box onClick={(e) => e.stopPropagation()}>
						<Box sx={{ minHeight: 36 }}>
							<Typography
								variant='caption'
								color='text.secondary'
								sx={{
									display: 'block',
									lineHeight: 1.4,
									minHeight: '2.8em',
								}}
							>
								{t('shop.button.option_values')}: {optionValueText}
							</Typography>
						</Box>

						<Stack direction='row' spacing={1} alignItems='center' flexWrap='wrap' sx={{ mt: 0.5 }}>
							{selectedSku ? (
								<>
									{selectedSku.origionPrice && selectedSku.origionPrice > selectedSku.price && (
										<Typography
											variant='body2'
											color='text.disabled'
											sx={{ textDecoration: 'line-through', fontWeight: 'bold' }}
										>
											{formatCurrencyBasedOnCurrentLanguage(selectedSku.origionPrice)}
										</Typography>
									)}
									<Typography variant='h6' color='primary' fontWeight='bold'>
										{formatCurrencyBasedOnCurrentLanguage(selectedSku.price || 0)}
									</Typography>
									<Box
										sx={{
											backgroundColor:
												(selectedSku.inventory?.quantity ?? 0) > 0 ? 'success.light' : 'error.light',
											color: (selectedSku.inventory?.quantity ?? 0) > 0 ? 'success.dark' : 'error.dark',
											px: 1,
											py: 0.25,
											borderRadius: 1,
											fontWeight: 'bold',
											fontSize: '0.75rem',
										}}
									>
										{t('medicine.field.quantity')}: {selectedSku.inventory?.quantity ?? 0}
									</Box>
								</>
							) : (
								<Typography variant='h6' color='error' fontWeight='bold'>
									{t('shop.text.out_of_stock')}
								</Typography>
							)}
						</Stack>

						<Divider sx={{ my: 1 }} />

						<Box
							sx={{
								display: 'grid',
								gridTemplateColumns: 'repeat(auto-fill, minmax(64px, 1fr))',
								gap: 1,
							}}
						>
							{activeSkus.map((sku) => {
								const skuValues = sku.skuValues || []
								const firstValueName = sku.name

								const isSelected = selectedSku?.id === sku.id
								const isSkuOutOfStock = !sku?.inventory?.quantity || sku?.inventory?.quantity <= 0

								return (
									<Button
										key={sku.id}
										variant={isSelected ? 'contained' : 'outlined'}
										size='small'
										disabled={isSkuOutOfStock}
										onClick={(e) => {
											e.stopPropagation()
											const newSelected = {}
											skuValues.forEach((sv) => {
												if (sv.optionValue?.optionId && sv.optionValue?.id) {
													newSelected[sv.optionValue.optionId] = sv.optionValue.id
												}
											})
											setSelectedOptionValueIds(newSelected)
										}}
										sx={(theme) => ({
											fontSize: '0.75rem',
											textTransform: 'none',
											borderRadius: 1,
											backgroundColor: isSelected ? theme.palette.secondary.main : 'rgba(0,0,0,0.03)',
											color: isSelected ? theme.palette.secondary.contrastText : 'inherit',
										})}
									>
										{firstValueName}
									</Button>
								)
							})}
						</Box>
					</Box>
				)}
			</CardContent>

			<CardActions
				onClick={(e) => e.stopPropagation()}
				sx={{ px: 2, pb: 2, mt: 'auto', flexShrink: 0 }}
			>
				{isOutOfStock ? (
					<Button fullWidth variant='outlined' disabled sx={{ textTransform: 'none' }}>
						{t('shop.text.out_of_stock')}
					</Button>
				) : selectedSku ? (
					<Button
						fullWidth
						variant='contained'
						startIcon={<AddShoppingCart />}
						onClick={(e) => handleAddToCart(selectedSku.skuCode, e)}
						disabled={loadingSkuCode === selectedSku.skuCode}
						sx={{ textTransform: 'none' }}
					>
						{loadingSkuCode === selectedSku.skuCode
							? t('shop.button.adding_to_cart')
							: t('shop.button.add_to_cart')}
					</Button>
				) : null}
			</CardActions>
		</Card>
	)
}

export default MedicineCardSection

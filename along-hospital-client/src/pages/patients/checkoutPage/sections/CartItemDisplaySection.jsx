import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import ShoppingCartOutlinedIcon from '@mui/icons-material/ShoppingCartOutlined'
import { Avatar, Box, Card, Chip, Paper, Stack, Typography } from '@mui/material'
import { useMemo } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const CartItemDisplaySection = ({ validDetails }) => {
	const details = useMemo(() => validDetails || [], [validDetails])
	const { t } = useTranslation()

	return (
		<Paper
			sx={(theme) => ({
				p: 3,
				borderRadius: 2,
				border: `1px solid ${theme.palette.divider}`,
				backgroundColor: theme.palette.background.paper,
			})}
		>
			<Stack direction='row' alignItems='center' spacing={1} mb={2}>
				<ShoppingCartOutlinedIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
				<Typography
					variant='h6'
					sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
				>
					{t('cart.order_items') || 'Order Items'}
				</Typography>
			</Stack>

			<Box sx={{ maxHeight: '80vh', overflowY: 'auto', pr: 1 }}>
				<Stack spacing={2}>
					{details.map((item) => {
						const skuCode = item.medicineSKU?.skuCode || item.skuCode
						if (!skuCode) return null

						const displayName = item.medicineSKU?.medicineName || ''
						const displayBrand = item.medicineSKU?.medicineBrand || ''
						const displayUnit = item.medicineSKU?.medicineUnit || ''
						const optionValues = (item.medicineSKU?.skuValues || [])
							.map((sv) => sv.valueName || '')
							.filter(Boolean)
							.join(', ')

						const images = item.medicineSKU?.medicineImages ?? []
						const unitPrice = item.discountPrice ?? item.originPrice ?? item.medicineSKU?.price ?? 0
						const originalPrice = item.originPrice ?? item.medicineSKU?.price ?? 0
						const maxQuantity = item.medicineSKU?.quantity ?? 0
						const isOutOfStock = item.quantity > maxQuantity
						const hasDiscount = unitPrice < originalPrice

						const preview = images.length > 0 ? getImageFromCloud(images[0]) : null

						return (
							<Card key={skuCode} sx={{ display: 'flex', overflow: 'visible', width: '100%' }}>
								<Box
									sx={{
										width: 120,
										height: 120,
										bgcolor: 'background.default',
										flexShrink: 0,
										display: 'flex',
										alignItems: 'center',
										justifyContent: 'center',
									}}
								>
									{preview ? (
										<Avatar
											variant='rounded'
											src={preview}
											alt={displayName}
											sx={{ width: '100%', height: '100%' }}
										/>
									) : (
										<Typography color='text.secondary'>{t('cart.no_image')}</Typography>
									)}
								</Box>

								<Box sx={{ paddingLeft: 2, flex: 1 }}>
									<Typography variant='h6' sx={{ fontWeight: 700 }}>
										{displayName || t('cart.no_name')}
									</Typography>
									<Typography variant='body2' color='text.secondary'>
										{t('cart.brand')}: {renderEmptyFallback(displayBrand)}
									</Typography>
									<Typography variant='body2' color='text.secondary'>
										{t('cart.unit')}: {renderEmptyFallback(displayUnit)}
									</Typography>
									<Typography variant='body2' color='text.secondary'>
										{t('medicine_sku.field.option_values')}: {renderEmptyFallback(optionValues)}
									</Typography>
									{isOutOfStock && (
										<Chip
											label={t('cart.out_of_stock')}
											color='error'
											size='small'
											sx={{ width: 'fit-content', mt: 0.5 }}
										/>
									)}
									<Stack direction='row' spacing={1} alignItems='center' mt={1}>
										{hasDiscount && (
											<Typography
												variant='body2'
												color='text.disabled'
												sx={{ textDecoration: 'line-through' }}
											>
												{formatCurrencyBasedOnCurrentLanguage(originalPrice)}
											</Typography>
										)}
										<Typography variant='body2' sx={{ fontWeight: 600 }}>
											{formatCurrencyBasedOnCurrentLanguage(unitPrice)}
										</Typography>
										<Typography component='span' variant='body2' color='text.secondary'>
											x {item.quantity} = 
										</Typography>
										<Typography variant='body2' sx={{ fontWeight: 600 }}>
											{formatCurrencyBasedOnCurrentLanguage(unitPrice * item.quantity)}
										</Typography>
									</Stack>
								</Box>
							</Card>
						)
					})}
				</Stack>
			</Box>
		</Paper>
	)
}

export default CartItemDisplaySection

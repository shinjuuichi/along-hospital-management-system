import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { CardMedia, Grid, Paper, Stack, Typography } from '@mui/material'

export default function OrderDetailProductsSection({ order }) {
	const { t } = useTranslation()

	return (
		<Stack spacing={1.5}>
			<Typography variant='h6'>{t('order_detail.detail.products')}</Typography>
			{order.orderDetails.map((item) => {
				const snapshot = item.medicineSnapshot || {}
				const name = renderEmptyFallback(snapshot.medicineName)
				const brand = renderEmptyFallback(snapshot.medicineBrand)
				const medicineImages = snapshot.medicineImages || []
				const unitPrice = item.unitPrice ?? 0
				const discount = item.discountAmount ?? 0
				const rawTotal = item.quantity * unitPrice - discount * item.quantity
				const total = Math.max(rawTotal, 0)

				return (
					<Paper key={item.skuCode} sx={{ p: 2, borderLeft: '4px solid #1976d2' }}>
						<Grid container spacing={2} alignItems='center'>
							<Grid size={{ xs: 12, sm: 4 }}>
								<Stack direction='row' alignItems='center' spacing={1.5}>
									<CardMedia
										component='img'
										image={getImageFromCloud(medicineImages.length > 0 ? medicineImages[0] : null)}
										alt={name}
										sx={{ width: 44, height: 44, objectFit: 'cover', pointerEvents: 'none' }}
									/>
									<Stack spacing={0.3}>
										<Typography sx={{ fontWeight: 600 }}>{name}</Typography>
										<Typography variant='body2' sx={{ color: '#666' }}>
											{brand}
										</Typography>
									</Stack>
								</Stack>
							</Grid>
							<Grid size={{ xs: 6, sm: 2 }}>
								<Typography variant='body2' sx={{ color: '#999' }}>
									{t('order_detail.detail.quantity')}
								</Typography>
								<Typography sx={{ fontWeight: 600 }}>{item.quantity}</Typography>
							</Grid>
							<Grid size={{ xs: 6, sm: 2 }}>
								<Typography variant='body2' sx={{ color: '#999' }}>
									{t('order_detail.detail.unit_price')}
								</Typography>
								<Typography sx={{ fontWeight: 600 }}>
									{formatCurrencyBasedOnCurrentLanguage(unitPrice)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 6, sm: 2 }}>
								<Typography variant='body2' sx={{ color: '#999' }}>
									{t('order_detail.detail.discount')}
								</Typography>
								<Typography
									sx={{
										fontWeight: 600,
										color: discount > 0 ? '#d32f2f' : 'inherit',
									}}
								>
									{discount > 0
										? `-${formatCurrencyBasedOnCurrentLanguage(discount)}`
										: formatCurrencyBasedOnCurrentLanguage(discount)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 6, sm: 2 }}>
								<Typography variant='body2' sx={{ color: '#999' }}>
									{t('order_detail.detail.total')}
								</Typography>
								<Typography sx={{ fontWeight: 600, color: '#1976d2' }}>
									{formatCurrencyBasedOnCurrentLanguage(total)}
								</Typography>
							</Grid>
						</Grid>
					</Paper>
				)
			})}
		</Stack>
	)
}

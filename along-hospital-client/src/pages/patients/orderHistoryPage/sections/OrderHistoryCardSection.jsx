import { EnumConfig } from '@/configs/enumConfig'
import { defaultOrderStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Divider, Grid, Paper, Stack, Typography } from '@mui/material'

export default function OrderHistoryCard({ order, onDetailClick, onCancelClick, onRepayClick }) {
	const { t } = useTranslation()
	const _enum = useEnum()

	const statusLabel = getEnumLabelByValue(_enum.orderStatusOptions, order.orderStatus)
	const totalQuantity = order.orderDetails?.reduce((sum, d) => sum + d.quantity, 0) || 0
	const canRepay = order.orderStatus === EnumConfig.OrderStatus.Unpaid

	const deliveryDisplay = order.deliveryDate
		? formatDateBasedOnCurrentLanguage(order.deliveryDate)
		: t('order.history.no_delivery_date')

	const pickupTypeLabel = order.isPickupAtStore
		? t('order.history.pickup_at_store')
		: t('order.history.delivery')

	return (
		<Paper
			sx={{
				p: 2.5,
				mb: 2,
				borderRadius: 2,
				boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
				transition: 'all 0.3s ease',
				'&:hover': { boxShadow: '0 4px 12px rgba(0,0,0,0.15)' },
			}}
		>
			<Stack spacing={2}>
				<Box
					sx={{
						display: 'flex',
						justifyContent: 'space-between',
						alignItems: 'center',
						flexWrap: 'wrap',
						gap: 2,
					}}
				>
					<Box>
						<Typography variant='h6' sx={{ mb: 0.5 }}>
							{t('order.history.order_id')} {order.id}
						</Typography>
						</Box>
					<Stack direction='row' spacing={1}>
						<Chip
							label={statusLabel}
							color={defaultOrderStatusStyle(order.orderStatus)}
							size='small'
							sx={{ fontWeight: 600 }}
						/>
						<Chip
							label={pickupTypeLabel}
							variant='outlined'
							size='small'
							sx={{ fontWeight: 600 }}
						/>
					</Stack>
				</Box>

				<Divider />

				<Grid container spacing={2}>
					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order.history.order_date')}
						</Typography>
						<Stack direction='row' alignItems='center' spacing={1} mt={0.5}>
							<Typography sx={{ fontWeight: 600 }}>
								{formatDateBasedOnCurrentLanguage(order.orderDate)}
							</Typography>
							{order.paidDate && (
								<Typography variant='caption' sx={{ color: '#999' }}>
									| {t('order.history.paid_date')}: {formatDateBasedOnCurrentLanguage(order.paidDate)}
								</Typography>
							)}
						</Stack>
						<Typography variant='caption' sx={{ color: '#999' }}>
							{t('order.history.total_products')}: {totalQuantity}
						</Typography>
					</Grid>

					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order.history.delivery_date')}
						</Typography>
						<Typography sx={{ fontWeight: 600, mt: 0.5 }}>{deliveryDisplay}</Typography>
					</Grid>

					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order.history.voucher_code')}
						</Typography>
						<Typography sx={{ fontWeight: 600, mt: 0.5 }}>{renderEmptyFallback(order.voucherCode)}</Typography>
					</Grid>

				</Grid>

				<Divider />

				<Box sx={{ p: 2, borderRadius: 1.5 }}>
					<Stack spacing={1}>
						<Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
							<Typography sx={{ fontWeight: 700, fontSize: '1rem', color: '#1976d2' }}>
								{t('order.history.total')}: {formatCurrencyBasedOnCurrentLanguage(order.finalPrice)}
							</Typography>
							<Box sx={{ display: 'flex', gap: 1 }}>
								{(order.orderStatus === EnumConfig.OrderStatus.Unpaid || order.orderStatus === EnumConfig.OrderStatus.Paid) && (
									<Button
										variant='contained'
										color='error'
										size='small'
										onClick={() => onCancelClick?.(order.id)}
									>
										{t('button.cancel')}
									</Button>
								)}
								{canRepay && (
									<Button
										variant='contained'
										color='primary'
										size='small'
										onClick={() => onRepayClick?.(order.id)}
									>
										{t('order.button.repay')}
									</Button>
								)}
								<Button variant='outlined' size='small' onClick={() => onDetailClick?.(order.id)}>
									{t('order.history.detail_button')}
								</Button>
							</Box>
						</Box>
					</Stack>
				</Box>

			</Stack>
		</Paper>
	)
}

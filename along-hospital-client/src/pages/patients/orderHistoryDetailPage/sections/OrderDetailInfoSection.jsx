import { defaultOrderStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Chip, Divider, Grid, Paper, Stack, Typography } from '@mui/material'

export default function OrderDetailInfoSection({ order }) {
	const { t } = useTranslation()
	const _enum = useEnum()

	const getStatusLabel = (status) => getEnumLabelByValue(_enum.orderStatusOptions, status)

	return (
		<Paper sx={{ p: 3, borderRadius: 2, boxShadow: 1 }}>
			<Stack spacing={2}>
				<Box
					sx={{
						display: 'flex',
						justifyContent: 'space-between',
						flexWrap: 'wrap',
						gap: 2,
					}}
				>
					<Typography variant='h6'>{t('order_detail.detail.info_title')}</Typography>
					<Chip
						label={getStatusLabel(order.orderStatus)}
						color={defaultOrderStatusStyle(order.orderStatus)}
						size='small'
						sx={{ fontWeight: 600 }}
					/>
				</Box>
				<Divider />
				<Grid container spacing={2}>
					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order_detail.detail.order_date')}
						</Typography>
						<Stack direction='row' alignItems='center' spacing={1} mt={0.5}>
							<Typography sx={{ fontWeight: 600 }}>
								{formatDateBasedOnCurrentLanguage(order.orderDate)}
							</Typography>
							{order.paidDate && (
								<Typography variant='caption' sx={{ color: '#999' }}>
									| {t('order_detail.detail.paid_date')}: {formatDateBasedOnCurrentLanguage(order.paidDate)}
								</Typography>
							)}
						</Stack>
						<Typography variant='caption' sx={{ color: '#999' }}>
							{t('order_detail.detail.total_quantity')}: {order.orderDetails.reduce((sum, d) => sum + d.quantity, 0)}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order_detail.detail.delivery_date')}
						</Typography>
						<Typography sx={{ fontWeight: 600, mt: 0.5 }}>
							{order.deliveryDate
								? formatDateBasedOnCurrentLanguage(order.deliveryDate)
								: t('order.history.no_delivery_date')}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, sm: 4 }}>
						<Typography
							variant='body2'
							sx={{ color: '#999', fontSize: '0.75rem', textTransform: 'uppercase' }}
						>
							{t('order_detail.detail.voucher_code')}
						</Typography>
						<Typography sx={{ fontWeight: 600, mt: 0.5 }}>{renderEmptyFallback(order.voucherCode)}</Typography>
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

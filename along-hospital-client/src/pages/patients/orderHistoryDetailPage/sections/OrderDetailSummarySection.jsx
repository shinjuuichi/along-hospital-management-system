import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Box, Divider, Stack, Typography } from '@mui/material'

export default function OrderDetailSummarySection({ order }) {
	const { t } = useTranslation()

	const totalMedicineDiscount =
		order.orderDetails?.reduce((sum, item) => sum + (item.discountAmount * item.quantity || 0), 0) ||
		0
	const totalVoucherDiscount =
		order.voucherDiscountAmount ??
		Math.max((order.totalDiscountAmount || 0) - totalMedicineDiscount, 0)

	return (
		<Box sx={{ p: 2, borderRadius: 1.5 }}>
			<Stack spacing={1}>
				<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
					<Typography variant='body2'>{t('order_detail.detail.origin_price')}</Typography>
					<Typography variant='body2'>
						{formatCurrencyBasedOnCurrentLanguage(order.originPrice)}
					</Typography>
				</Box>
				{totalMedicineDiscount > 0 && (
					<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
						<Typography variant='body2' sx={{ color: '#00897b' }}>
							{t('order_detail.detail.medicine_discount')}
						</Typography>
						<Typography variant='body2' sx={{ color: '#00897b', fontWeight: 600 }}>
							-{formatCurrencyBasedOnCurrentLanguage(totalMedicineDiscount)}
						</Typography>
					</Box>
				)}
				{totalVoucherDiscount > 0 && (
					<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
						<Typography variant='body2' sx={{ color: '#e65100' }}>
							{t('order_detail.detail.voucher_discount')}
						</Typography>
						<Typography variant='body2' sx={{ color: '#e65100', fontWeight: 600 }}>
							-{formatCurrencyBasedOnCurrentLanguage(totalVoucherDiscount)}
						</Typography>
					</Box>
				)}
				<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
					<Typography variant='body2' sx={{ color: '#d32f2f' }}>
						{t('order_detail.detail.total_discount')}
					</Typography>
					<Typography variant='body2' sx={{ color: '#d32f2f', fontWeight: 600 }}>
						-{formatCurrencyBasedOnCurrentLanguage(order.totalDiscountAmount)}
					</Typography>
				</Box>
				<Divider sx={{ my: 1 }} />
				<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
					<Typography sx={{ fontWeight: 700, fontSize: '1rem' }}>
						{t('order_detail.detail.final_price')}
					</Typography>
					<Typography sx={{ fontWeight: 700, fontSize: '1rem', color: '#1976d2' }}>
						{formatCurrencyBasedOnCurrentLanguage(order.finalPrice)}
					</Typography>
				</Box>
			</Stack>
		</Box>
	)
}

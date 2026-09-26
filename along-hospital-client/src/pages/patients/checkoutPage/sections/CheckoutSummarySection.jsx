import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Button, Divider, Paper, Stack, Typography } from '@mui/material'

const CheckoutSummarySection = ({
	total,
	discountAmount,
	finalTotal,
	paymentType,
	onCheckout,
	loading,
	isPickupAtStore,
}) => {
	const { t } = useTranslation()

	return (
		<Paper
			sx={(theme) => ({
				p: 3,
				borderRadius: 2,
				border: `1px solid ${theme.palette.divider}`,
				backgroundColor: theme.palette.background.paper,
				position: { xs: 'static', md: 'sticky' },
				top: 20,
			})}
		>
			<Typography variant='h6' sx={{ fontWeight: 800, mb: 2 }}>
				{t('cart.summary.title')}
			</Typography>

			<Stack spacing={1}>
				<Stack direction='row' justifyContent='space-between'>
					<Typography>{t('cart.summary.subtotal')}</Typography>
					<Typography fontWeight={700}>{formatCurrencyBasedOnCurrentLanguage(total)}</Typography>
				</Stack>

				{discountAmount > 0 && (
					<Stack direction='row' justifyContent='space-between'>
						<Typography>{t('cart.summary.voucher')}</Typography>
						<Typography fontWeight={700} color='error.main'>
							-{formatCurrencyBasedOnCurrentLanguage(discountAmount)}
						</Typography>
					</Stack>
				)}

				<Divider sx={{ my: 1 }} />

				<Stack direction='row' justifyContent='space-between'>
					<Typography variant='body2' color='text.secondary'>
						{t('cart.summary.delivery_method')}
					</Typography>
					<Typography variant='body2' fontWeight={600}>
						{isPickupAtStore ? t('cart.pickup_at_store') : t('cart.delivery')}
					</Typography>
				</Stack>

				<Divider sx={{ my: 1 }} />

				<Stack direction='row' justifyContent='space-between' mb={2}>
					<Typography variant='h6' fontWeight={800}>
						{t('cart.summary.total_pay')}
					</Typography>
					<Typography
						variant='h5'
						fontWeight={900}
						color={discountAmount > 0 ? 'error.main' : 'primary.main'}
					>
						{formatCurrencyBasedOnCurrentLanguage(discountAmount > 0 ? finalTotal : total)}
					</Typography>
				</Stack>
			</Stack>

			<Button
				fullWidth
				size='large'
				variant='contained'
				disabled={loading || !paymentType}
				onClick={onCheckout}
			>
				{loading ? t('cart.summary.processing') : t('cart.summary.checkout')}
			</Button>
		</Paper>
	)
}

export default CheckoutSummarySection

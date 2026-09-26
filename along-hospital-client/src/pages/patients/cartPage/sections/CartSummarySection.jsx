import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Button, Divider, Paper, Stack, Typography } from '@mui/material'

const CartSummarySection = ({ validDetails, total, onCheckout }) => {
	const details = validDetails || []
	const { t } = useTranslation()

	const hasOutOfStock = details.some((item) => {
		const maxQuantity = item.medicineSKU?.quantity ?? 0
		return item.quantity > maxQuantity
	})

	return (
		<Paper
			sx={(theme) => ({
				p: 3,
				position: { xs: 'static', md: 'sticky' },
				top: 20,
				height: 'fit-content',
				backgroundColor: theme.palette.background.paper,
				border: `1px solid ${theme.palette.divider}`,
			})}
		>
			<Typography variant='h6' sx={{ fontWeight: 700, mb: 2 }}>
				{t('cart.summary.title')}
			</Typography>

			<Stack spacing={2}>
				{details.map((item) => {
					const displayName = item.medicineSKU?.medicineName || ''

					return (
						<Stack key={item.skuCode} direction='row' justifyContent='space-between' alignItems='center'>
							<Typography variant='body2' color='text.secondary'>
								{displayName} x {item.quantity}
							</Typography>
							<Stack direction='row' spacing={0.5} alignItems='center'>
								{(item.discountPrice ?? 0) < (item.originPrice ?? item.medicineSKU?.price ?? 0) && (
									<Typography
										variant='body2'
										color='text.disabled'
										sx={{ textDecoration: 'line-through' }}
									>
										{formatCurrencyBasedOnCurrentLanguage((item.originPrice ?? item.medicineSKU?.price ?? 0) * item.quantity)}
									</Typography>
								)}
								<Typography variant='body2' sx={{ fontWeight: 500 }}>
									{formatCurrencyBasedOnCurrentLanguage(
										(item.discountPrice ?? item.medicineSKU?.price ?? 0) * item.quantity
									)}
								</Typography>
							</Stack>
						</Stack>
					)
				})}

				<Divider sx={{ my: 1 }} />

				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h6' sx={{ fontWeight: 700 }}>
						{t('cart.summary.total')}
					</Typography>
					<Typography
						variant='h6'
						sx={{ fontWeight: 700, color: (theme) => theme.palette.primary.main }}
					>
						{formatCurrencyBasedOnCurrentLanguage(total)}
					</Typography>
				</Stack>

				<Button variant='contained' size='large' fullWidth sx={{ mt: 2 }} onClick={onCheckout} disabled={hasOutOfStock}>
					{t('cart.summary.checkout')}
				</Button>
			</Stack>
		</Paper>
	)
}

export default CartSummarySection

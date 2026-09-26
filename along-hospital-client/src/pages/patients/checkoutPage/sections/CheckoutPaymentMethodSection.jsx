import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import PaymentOutlinedIcon from '@mui/icons-material/PaymentOutlined'
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined'
import { Button, Checkbox, FormControlLabel, Paper, Stack, Typography } from '@mui/material'

const CheckoutPaymentMethodSection = ({ paymentType, onSelect, isPickupAtStore, onPickupChange, disabled }) => {
	const { t } = useTranslation()
	const { paymentTypeOptions } = useEnum()

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
				<PaymentOutlinedIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
				<Typography
					variant='h6'
					sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
				>
					{t('cart.summary.payment_method')}
				</Typography>
			</Stack>

			<Stack direction='row' spacing={1} flexWrap='wrap'>
				{paymentTypeOptions.map((p) => {
					const active = paymentType === p.value
					return (
						<Button
							key={p.value}
							variant={active ? 'contained' : 'outlined'}
							disabled={disabled}
							sx={(theme) => ({
								textTransform: 'none',
								backgroundColor: active ? theme.palette.primary.main : 'transparent',
								color: active ? theme.palette.primary.contrastText : theme.palette.text.primary,
								borderColor: theme.palette.primary.main,
								'&:hover': {
									backgroundColor: active ? theme.palette.primary.dark : theme.palette.action.hover,
								},
								...(disabled && {
									backgroundColor: 'transparent',
									color: theme.palette.text.disabled,
									borderColor: theme.palette.divider,
								}),
							})}
							onClick={() => onSelect(p.value)}
						>
							{p.label}
						</Button>
					)
				})}
			</Stack>

			<Stack direction='row' alignItems='center' spacing={1} mt={3} mb={2}>
				<LocalShippingOutlinedIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
				<Typography
					variant='h6'
					sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
				>
					{t('cart.summary.delivery_method')}
				</Typography>
			</Stack>

			<FormControlLabel
				control={
					<Checkbox
						checked={isPickupAtStore}
						onChange={(e) => onPickupChange(e.target.checked)}
					/>
				}
				label={t('cart.is_pickup_at_store')}
			/>
			<Typography variant='body2' color='text.secondary' sx={{ mt: 0.5 }}>
				{t('cart.delivery_default_note')}
			</Typography>
		</Paper>
	)
}

export default CheckoutPaymentMethodSection

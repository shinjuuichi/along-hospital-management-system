import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { Box, Radio, Stack, Typography } from '@mui/material'

const VerificationMethodSelector = ({ methods = [], value = '', onChange, disabled = false }) => {
	const { t } = useTranslation()

	return (
		<Stack spacing={1.25}>
			{methods.map((method) => {
				const deliveryMethod = method.deliveryMethod || ''
				const selected = value === deliveryMethod
				const title =
					deliveryMethod === EnumConfig.VerificationDeliveryMethod.Email
						? t('auth.delivery.get_code_email')
						: t('auth.delivery.get_code_sms')

				return (
					<Box
						key={deliveryMethod}
						onClick={() => {
							if (!disabled) onChange?.(deliveryMethod)
						}}
						sx={{
							display: 'flex',
							alignItems: 'center',
							justifyContent: 'space-between',
							gap: 2,
							p: { xs: 1.75, sm: 2 },
							borderRadius: 3,
							border: 1,
							borderColor: selected ? 'primary.main' : 'divider',
							bgcolor: selected ? 'primary.lighter' : 'background.paper',
							cursor: disabled ? 'default' : 'pointer',
							transition: 'border-color 0.2s ease, background-color 0.2s ease',
						}}
					>
						<Box sx={{ minWidth: 0 }}>
							<Typography variant='subtitle1' sx={{ fontWeight: 700 }}>
								{title}
							</Typography>
							<Typography variant='body2' color='text.secondary' sx={{ wordBreak: 'break-word' }}>
								{method.maskedDestination}
							</Typography>
						</Box>
						<Radio
							checked={selected}
							value={deliveryMethod}
							onChange={() => onChange?.(deliveryMethod)}
							disabled={disabled}
						/>
					</Box>
				)
			})}
		</Stack>
	)
}

export default VerificationMethodSelector

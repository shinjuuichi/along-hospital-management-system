import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useTranslation from '@/hooks/useTranslation'
import { CancelOutlined } from '@mui/icons-material'
import { Box, Paper, Stack, Typography } from '@mui/material'
import { keyframes } from '@mui/system'
import { useEffect } from 'react'
import { useSearchParams } from 'react-router-dom'

const pulse = keyframes`
	0% { transform: scale(1); box-shadow: 0 0 0 0 rgba(211, 47, 47, 0.35); }
	60% { transform: scale(1.04); box-shadow: 0 0 0 12px rgba(211, 47, 47, 0); }
	100% { transform: scale(1); box-shadow: 0 0 0 0 rgba(211, 47, 47, 0); }
`

const PaymentCancelPage = () => {
	const { t } = useTranslation()
	const [searchParams] = useSearchParams()
	const orderCode = searchParams.get('orderCode')

	const { submit } = useAxiosSubmit({
		url: ApiUrls.PAYMENT.CANCEL_PAYMENT(orderCode),
		method: 'PUT',
	})

	useEffect(() => {
		if (orderCode) {
			submit()
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [orderCode])

	return (
		<Box sx={{ py: 6 }}>
			<Paper elevation={0} sx={{ p: { xs: 3, md: 5 }, textAlign: 'center' }}>
				<Stack alignItems='center' spacing={1.5}>
					<Box
						sx={{
							p: 2,
							borderRadius: '50%',
							bgcolor: 'error.softBg',
							border: (theme) => `1px dashed ${theme.palette.error.light}`,
							display: 'inline-flex',
							animation: `${pulse} 1.8s ease-in-out infinite`,
						}}
					>
						<CancelOutlined color='error' sx={{ fontSize: 44 }} />
					</Box>
					<Typography variant='h5' fontWeight={800}>
						{t('payment.cancel.title')}
					</Typography>
					<Typography variant='body1' color='text.secondary'>
						{t('payment.cancel.subtitle')}
					</Typography>
					<Typography variant='body2' color='text.secondary' sx={{ fontStyle: 'italic' }}>
						{t('payment.cancel.help')}
					</Typography>
				</Stack>
			</Paper>
		</Box>
	)
}

export default PaymentCancelPage

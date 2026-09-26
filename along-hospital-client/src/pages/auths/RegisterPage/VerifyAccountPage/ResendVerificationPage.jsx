import AuthSuccessState from '@/components/auth/AuthSuccessState'
import VerificationIdentifierSummary from '@/components/auth/VerificationIdentifierSummary'
import VerificationMethodSelector from '@/components/auth/VerificationMethodSelector'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import { VerificationResendMs, VerificationStorageKey } from '@/constants/verificationConstants'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useResendTimer from '@/hooks/useResendTimer'
import useTranslation from '@/hooks/useTranslation'
import { Alert, Box, Button, CircularProgress, Link, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { Link as RouterLink, useNavigate, useSearchParams } from 'react-router-dom'

const ResendVerificationPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [params] = useSearchParams()
	const identifier = params.get('identifier') || ''
	const [options, setOptions] = useState(null)
	const [selectedMethod, setSelectedMethod] = useState('')
	const [emailSuccessDestination, setEmailSuccessDestination] = useState('')
	const { timer, start } = useResendTimer(VerificationStorageKey.REGISTER)

	const selectedOption = useMemo(
		() => options?.methods?.find((method) => method.deliveryMethod === selectedMethod) || null,
		[options, selectedMethod]
	)

	const optionsRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.REGISTER_RESEND_OPTIONS,
		method: 'POST',
		onSuccess: async (resp) => {
			const nextOptions = resp.data
			setOptions(nextOptions)
			setSelectedMethod(
				nextOptions?.defaultDeliveryMethod || nextOptions?.methods?.[0]?.deliveryMethod || ''
			)
		},
	})

	const resendRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.REGISTER_RESEND,
		method: 'POST',
		onSuccess: async () => {
			start(VerificationResendMs)

			if (selectedMethod === EnumConfig.VerificationDeliveryMethod.Sms) {
				navigate(
					`${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.VERIFY)}?identifier=${encodeURIComponent(identifier)}&deliveryMethod=${EnumConfig.VerificationDeliveryMethod.Sms}`,
					{ replace: true }
				)
				return
			}

			setEmailSuccessDestination(selectedOption?.maskedDestination || '')
		},
	})

	useEffect(() => {
		if (!identifier) {
			navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN), { replace: true })
			return
		}

		optionsRequest.submit({
			overrideData: {
				identifier,
			},
		})
	}, [identifier, navigate])

	const handleResend = async () => {
		if (!selectedMethod) return

		await resendRequest.submit({
			overrideData: {
				identifier,
				deliveryMethod: selectedMethod,
			},
		})
	}

	if (emailSuccessDestination) {
		return (
			<Stack spacing={3}>
				<AuthSuccessState
					title={t('auth.resend.email_sent_title')}
					description={t('auth.resend.email_sent_description', {
						destination: emailSuccessDestination,
					})}
				/>

				{timer > 0 ? (
					<Button
						variant='outlined'
						size='large'
						fullWidth
						disabled
						sx={{
							py: 1.5,
							borderRadius: 2,
							textTransform: 'none',
							fontSize: '1rem',
							fontWeight: 600,
						}}
					>
						{t('auth.resend.resend_in')} {timer} {t('auth.seconds')}
					</Button>
				) : (
					<Button
						variant='outlined'
						size='large'
						fullWidth
						onClick={handleResend}
						disabled={resendRequest.loading}
						startIcon={resendRequest.loading && <CircularProgress size={20} color='inherit' />}
						sx={{
							py: 1.5,
							borderRadius: 2,
							textTransform: 'none',
							fontSize: '1rem',
							fontWeight: 600,
						}}
					>
						{t('auth.resend.resend_email')}
					</Button>
				)}

				<Button
					component={RouterLink}
					to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)}
					variant='contained'
					size='large'
					sx={{
						py: { xs: 1.2, sm: 1.5 },
						borderRadius: 2,
						textTransform: 'none',
						fontSize: { xs: '0.9rem', sm: '1rem' },
						fontWeight: 600,
					}}
				>
					{t('auth.back_to_login')}
				</Button>
			</Stack>
		)
	}

	const loadingOptions = optionsRequest.loading && !options
	const hasOptions = Boolean(options?.methods?.length)

	return (
		<>
			<Box sx={{ mb: 3 }}>
				<Typography variant='h4' sx={{ mb: 1, fontWeight: 700 }}>
					{t('auth.resend.title')}
				</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('auth.resend.subtitle')}
				</Typography>
			</Box>

			<Stack spacing={3}>
				<VerificationIdentifierSummary
					title={t('auth.resend.account_title')}
					value={identifier}
					description={t('auth.resend.account_description')}
				/>

				{loadingOptions ? (
					<Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
						<CircularProgress />
					</Box>
				) : null}

				{!loadingOptions && !hasOptions ? (
					<Alert severity='error' sx={{ borderRadius: 2 }}>
						{t('auth.resend.no_methods')}
					</Alert>
				) : null}

				{hasOptions ? (
					<VerificationMethodSelector
						methods={options.methods}
						value={selectedMethod}
						onChange={setSelectedMethod}
						disabled={resendRequest.loading}
					/>
				) : null}

				{timer > 0 ? (
					<Button
						variant='outlined'
						size='large'
						fullWidth
						disabled
						sx={{
							py: 1.5,
							borderRadius: 2,
							textTransform: 'none',
							fontSize: '1rem',
							fontWeight: 600,
						}}
					>
						{t('auth.resend.resend_in')} {timer} {t('auth.seconds')}
					</Button>
				) : (
					<Button
						variant='contained'
						size='large'
						fullWidth
						onClick={handleResend}
						disabled={!selectedMethod || resendRequest.loading || loadingOptions}
						startIcon={resendRequest.loading && <CircularProgress size={20} color='inherit' />}
						sx={{
							py: 1.5,
							borderRadius: 2,
							textTransform: 'none',
							fontSize: '1rem',
							fontWeight: 600,
							bgcolor: 'primary.main',
							color: 'primary.contrastText',
							'&:hover': {
								bgcolor: 'primary.dark',
							},
						}}
					>
						{t('auth.resend.button')}
					</Button>
				)}

				<Box sx={{ textAlign: 'center' }}>
					<Typography variant='body2' color='text.secondary' component='span'>
						{t('auth.resend.already_have_account')}{' '}
					</Typography>
					<Link
						component={RouterLink}
						to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)}
						variant='body2'
						sx={{
							fontWeight: 600,
							textDecoration: 'none',
							'&:hover': { textDecoration: 'underline' },
						}}
					>
						{t('auth.login.submit')}
					</Link>
				</Box>
			</Stack>
		</>
	)
}

export default ResendVerificationPage

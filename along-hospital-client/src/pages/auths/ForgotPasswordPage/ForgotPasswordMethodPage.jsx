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
import { Link as RouterLink, useLocation, useNavigate, useSearchParams } from 'react-router-dom'

const ForgotPasswordMethodPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const location = useLocation()
	const [params] = useSearchParams()
	const identifier = params.get('identifier') || ''
	const prefetchedOptions = location.state?.options || null
	const [options, setOptions] = useState(prefetchedOptions)
	const [selectedMethod, setSelectedMethod] = useState(
		prefetchedOptions?.defaultDeliveryMethod || ''
	)
	const [emailSuccessDestination, setEmailSuccessDestination] = useState('')
	const { timer, start } = useResendTimer(VerificationStorageKey.FORGOT_PASSWORD)

	const selectedOption = useMemo(
		() => options?.methods?.find((method) => method.deliveryMethod === selectedMethod) || null,
		[options, selectedMethod]
	)

	const optionsRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.FORGOT_PASSWORD_OPTIONS,
		method: 'POST',
		onSuccess: async (resp) => {
			const nextOptions = resp.data
			setOptions(nextOptions)
			setSelectedMethod(
				nextOptions?.defaultDeliveryMethod || nextOptions?.methods?.[0]?.deliveryMethod || ''
			)
		},
	})

	const sendRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.FORGOT_PASSWORD,
		method: 'POST',
		onSuccess: async () => {
			start(VerificationResendMs)

			if (selectedMethod === EnumConfig.VerificationDeliveryMethod.Sms) {
				navigate(
					`${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.VERIFY_RESET_PASSWORD)}?identifier=${encodeURIComponent(identifier)}&deliveryMethod=${EnumConfig.VerificationDeliveryMethod.Sms}`,
					{ replace: true }
				)
				return
			}

			setEmailSuccessDestination(selectedOption?.maskedDestination || '')
		},
	})

	useEffect(() => {
		if (!identifier) {
			navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.FORGOT_PASSWORD), { replace: true })
			return
		}

		if (prefetchedOptions?.identifier === identifier && prefetchedOptions?.methods?.length) {
			return
		}

		optionsRequest.submit({
			overrideData: {
				identifier,
			},
		})
	}, [identifier, navigate, prefetchedOptions])

	const handleContinue = async () => {
		if (!selectedMethod) return

		await sendRequest.submit({
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
					title={t('auth.forgot.email_sent_title')}
					description={t('auth.forgot.email_sent_description', {
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
						onClick={handleContinue}
						disabled={sendRequest.loading}
						startIcon={sendRequest.loading && <CircularProgress size={20} color='inherit' />}
						sx={{
							py: 1.5,
							borderRadius: 2,
							textTransform: 'none',
							fontSize: '1rem',
							fontWeight: 600,
						}}
					>
						{t('auth.forgot.resend_email')}
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
			<Box sx={{ mb: { xs: 2, sm: 3 } }}>
				<Typography
					variant='h4'
					sx={{
						mb: 1,
						fontWeight: 700,
						fontSize: { xs: '1.5rem', sm: '1.75rem', md: '2rem' },
					}}
				>
					{t('auth.forgot.method_title')}
				</Typography>
				<Typography
					variant='body2'
					color='text.secondary'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.forgot.method_subtitle')}
				</Typography>
			</Box>

			<Stack spacing={{ xs: 2, sm: 2.5 }}>
				<VerificationIdentifierSummary
					title={t('auth.forgot.account_title')}
					value={identifier}
					description={t('auth.forgot.account_description')}
				/>

				{loadingOptions ? (
					<Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
						<CircularProgress />
					</Box>
				) : null}

				{!loadingOptions && !hasOptions ? (
					<Alert severity='error' sx={{ borderRadius: 2 }}>
						{t('auth.forgot.no_methods')}
					</Alert>
				) : null}

				{hasOptions ? (
					<VerificationMethodSelector
						methods={options.methods}
						value={selectedMethod}
						onChange={setSelectedMethod}
						disabled={sendRequest.loading}
					/>
				) : null}

				<Button
					variant='contained'
					size='large'
					disabled={!selectedMethod || sendRequest.loading || loadingOptions}
					startIcon={sendRequest.loading && <CircularProgress size={20} color='inherit' />}
					onClick={handleContinue}
					sx={{
						py: { xs: 1.2, sm: 1.5 },
						borderRadius: 2,
						textTransform: 'none',
						fontSize: { xs: '0.9rem', sm: '1rem' },
						fontWeight: 600,
					}}
				>
					{t('button.next')}
				</Button>

				<Link
					component={RouterLink}
					to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.FORGOT_PASSWORD)}
					variant='body2'
					sx={{
						textAlign: 'center',
						fontWeight: 600,
						textDecoration: 'none',
						'&:hover': { textDecoration: 'underline' },
					}}
				>
					{t('auth.forgot.change_identifier')}
				</Link>
			</Stack>
		</>
	)
}

export default ForgotPasswordMethodPage

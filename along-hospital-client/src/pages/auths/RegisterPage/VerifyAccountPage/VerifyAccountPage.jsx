import AuthSuccessState from '@/components/auth/AuthSuccessState'
import OtpCodeInput from '@/components/auth/OtpCodeInput'
import ValidationTextField from '@/components/textFields/ValidationTextField'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import { VerificationResendMs, VerificationStorageKey } from '@/constants/verificationConstants'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useForm from '@/hooks/useForm'
import useResendTimer from '@/hooks/useResendTimer'
import useTranslation from '@/hooks/useTranslation'
import { isPhoneOrEmail, maxLen } from '@/utils/validateUtil'
import { Alert, Box, Button, CircularProgress, Link, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useRef, useState } from 'react'
import { Link as RouterLink, useNavigate, useSearchParams } from 'react-router-dom'

const VerifyAccountPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [params] = useSearchParams()
	const { login } = useAuth()
	const token = useMemo(() => params.get('token') || '', [params])
	const identifierFromUrl = params.get('identifier') || ''
	const deliveryMethodFromUrl =
		params.get('deliveryMethod') ||
		(token ? EnumConfig.VerificationDeliveryMethod.Email : EnumConfig.VerificationDeliveryMethod.Sms)
	const isSmsFlow = deliveryMethodFromUrl === EnumConfig.VerificationDeliveryMethod.Sms && !token
	const otpInputRef = useRef(null)
	const submitRef = useRef(null)
	const [submitted, setSubmitted] = useState(false)
	const [success, setSuccess] = useState(false)
	const [verifyingEmail, setVerifyingEmail] = useState(!isSmsFlow)
	const [failed, setFailed] = useState(false)
	const [otp, setOtp] = useState('')
	const { timer, start } = useResendTimer(VerificationStorageKey.REGISTER)
	const { values, handleChange, registerRef, validateAll } = useForm({
		identifier: identifierFromUrl,
	})

	const { loading, submit } = useAxiosSubmit({
		url: ApiUrls.AUTH.REGISTER_VERIFY,
		method: 'POST',
		onSuccess: async (resp) => {
			const { accessToken } = resp.data
			if (!accessToken) {
				setFailed(true)
				setVerifyingEmail(false)
				return
			}

			await login(accessToken)
			setSuccess(true)

			setTimeout(() => {
				navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.COMPLETE_PROFILE), { replace: true })
			}, 2000)
		},
		onError: async (error) => {
			if (isSmsFlow) {
				const nextErrorText =
					error?.response?.data?.message || error?.response?.data?.title || t('auth.verify_otp.invalid')
				otpInputRef.current?.clear(nextErrorText)
				return
			}

			setFailed(true)
			setVerifyingEmail(false)
		},
	})

	const resendRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.REGISTER_RESEND,
		method: 'POST',
		onSuccess: async () => {
			start(VerificationResendMs)
			otpInputRef.current?.clear()
		},
	})

	useEffect(() => {
		submitRef.current = submit
	}, [submit])

	useEffect(() => {
		if (isSmsFlow) {
			setVerifyingEmail(false)
			return
		}

		if (!token) {
			setFailed(true)
			setVerifyingEmail(false)
			return
		}

		submitRef.current?.({
			overrideData: {
				deliveryMethod: EnumConfig.VerificationDeliveryMethod.Email,
				token,
			},
		})
	}, [isSmsFlow, token])

	const handleSubmit = async (event) => {
		event.preventDefault()
		setSubmitted(true)

		const fieldsOk = validateAll()
		const otpOk = otpInputRef.current?.validate()
		if (!fieldsOk || !otpOk) return

		await submit({
			overrideData: {
				deliveryMethod: EnumConfig.VerificationDeliveryMethod.Sms,
				identifier: values.identifier,
				otp,
			},
		})
	}

	const handleResend = async () => {
		const fieldsOk = validateAll()
		if (!fieldsOk) return

		await resendRequest.submit({
			overrideData: {
				identifier: values.identifier,
				deliveryMethod: EnumConfig.VerificationDeliveryMethod.Sms,
			},
		})
	}

	if (success) {
		return (
			<AuthSuccessState
				title={isSmsFlow ? t('auth.verify_otp.success') : t('auth.verify_link.success')}
				description={isSmsFlow ? t('auth.verify_otp.redirecting') : t('auth.verify_link.redirecting')}
			/>
		)
	}

	if (!isSmsFlow) {
		if (verifyingEmail && loading) {
			return (
				<Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', py: 4 }}>
					<CircularProgress size={48} sx={{ mb: 3 }} />
					<Typography variant='body1' color='text.secondary'>
						{t('auth.verify_link.processing')}
					</Typography>
				</Box>
			)
		}

		return (
			<>
				<Box sx={{ mb: 3 }}>
					<Typography variant='h4' sx={{ mb: 1, fontWeight: 700 }}>
						{t('auth.verify_link.title')}
					</Typography>
				</Box>

				<Stack spacing={3}>
					{failed ? (
						<Alert
							severity='error'
							sx={{
								borderRadius: 2,
								bgcolor: 'error.softBg',
								border: 1,
								borderColor: 'error.softBorder',
							}}
						>
							<Typography variant='body2' sx={{ fontWeight: 600 }}>
								{t('auth.verify_link.failed')}
							</Typography>
							<Typography variant='body2' sx={{ mt: 0.5 }}>
								{t('auth.verify_link.invalid')}
							</Typography>
						</Alert>
					) : null}

					<Link
						component={RouterLink}
						to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)}
						sx={{ textDecoration: 'none' }}
					>
						<Button
							variant='outlined'
							fullWidth
							size='large'
							sx={{
								py: 1.5,
								borderRadius: 2,
								textTransform: 'none',
								fontSize: '1rem',
								fontWeight: 600,
							}}
						>
							{t('auth.back_to_login')}
						</Button>
					</Link>
				</Stack>
			</>
		)
	}

	return (
		<>
			<Box sx={{ mb: 3 }}>
				<Typography variant='h4' sx={{ mb: 1, fontWeight: 700 }}>
					{t('auth.verify_otp.title')}
				</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('auth.verify_otp.subtitle')}
				</Typography>
			</Box>
			<Box component='form' onSubmit={handleSubmit}>
				<Stack spacing={{ xs: 2, sm: 2.5 }}>
					{!identifierFromUrl ? (
						<ValidationTextField
							name='identifier'
							label={t('auth.field.identifier')}
							placeholder={t('auth.placeholder.identifier')}
							value={values.identifier}
							onChange={handleChange}
							ref={registerRef('identifier')}
							submitted={submitted}
							validate={[isPhoneOrEmail(), maxLen(255)]}
						/>
					) : null}

					<OtpCodeInput
						ref={otpInputRef}
						label={t('auth.field.otp')}
						value={otp}
						onChange={setOtp}
						invalidText={t('auth.verify_otp.invalid_length', {
							length: 6,
						})}
						autoFocus={true}
						disabled={loading}
					/>

					<Button
						type='submit'
						variant='contained'
						size='large'
						disabled={loading}
						startIcon={loading && <CircularProgress size={20} color='inherit' />}
						sx={{
							py: { xs: 1.2, sm: 1.5 },
							borderRadius: 2,
							textTransform: 'none',
							fontSize: { xs: '0.9rem', sm: '1rem' },
							fontWeight: 600,
						}}
					>
						{t('auth.verify_otp.submit')}
					</Button>

					{timer > 0 ? (
						<Button variant='outlined' size='large' fullWidth disabled sx={{ py: 1.5, borderRadius: 2 }}>
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
							sx={{ py: 1.5, borderRadius: 2, textTransform: 'none', fontWeight: 600 }}
						>
							{t('auth.verify_otp.resend')}
						</Button>
					)}
				</Stack>
			</Box>

			<Box sx={{ mt: 3, textAlign: 'center' }}>
				<Link
					component={RouterLink}
					to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)}
					variant='body2'
					sx={{ fontWeight: 600, textDecoration: 'none', '&:hover': { textDecoration: 'underline' } }}
				>
					{t('auth.back_to_login')}
				</Link>
			</Box>
		</>
	)
}

export default VerifyAccountPage

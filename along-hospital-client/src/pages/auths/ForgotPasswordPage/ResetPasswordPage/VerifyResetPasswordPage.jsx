import OtpCodeInput from '@/components/auth/OtpCodeInput'
import ValidationTextField from '@/components/textFields/ValidationTextField'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import { VerificationResendMs, VerificationStorageKey } from '@/constants/verificationConstants'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useForm from '@/hooks/useForm'
import useResendTimer from '@/hooks/useResendTimer'
import useTranslation from '@/hooks/useTranslation'
import { isPhoneOrEmail, maxLen } from '@/utils/validateUtil'
import { Alert, Box, Button, CircularProgress, Link, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { Link as RouterLink, useNavigate, useSearchParams } from 'react-router-dom'

const VerifyResetPasswordPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [params] = useSearchParams()
	const token = params.get('token') || ''
	const identifierFromUrl = params.get('identifier') || ''
	const deliveryMethodFromUrl =
		params.get('deliveryMethod') ||
		(token ? EnumConfig.VerificationDeliveryMethod.Email : EnumConfig.VerificationDeliveryMethod.Sms)
	const isSmsFlow = deliveryMethodFromUrl === EnumConfig.VerificationDeliveryMethod.Sms && !token
	const otpInputRef = useRef(null)
	const submitRef = useRef(null)
	const [submitted, setSubmitted] = useState(false)
	const [failed, setFailed] = useState(false)
	const [verifyingLink, setVerifyingLink] = useState(!isSmsFlow)
	const [otp, setOtp] = useState('')
	const { timer, start } = useResendTimer(VerificationStorageKey.FORGOT_PASSWORD)
	const { values, handleChange, registerRef, validateAll } = useForm({
		identifier: identifierFromUrl,
	})

	const { loading, submit } = useAxiosSubmit({
		url: ApiUrls.AUTH.FORGOT_PASSWORD_VERIFY,
		method: 'POST',
		onSuccess: async (resp) => {
			const nextResetToken = resp.data?.resetToken
			if (!nextResetToken) {
				if (isSmsFlow) {
					otpInputRef.current?.clear(t('auth.reset_otp.invalid'))
					return
				}

				setFailed(true)
				setVerifyingLink(false)
				return
			}

			setFailed(false)
			navigate(
				`${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.RESET_PASSWORD)}?resetToken=${encodeURIComponent(nextResetToken)}`,
				{ replace: true }
			)
		},
		onError: async (error) => {
			if (isSmsFlow) {
				const nextErrorText =
					error?.response?.data?.message || error?.response?.data?.title || t('auth.reset_otp.invalid')
				otpInputRef.current?.clear(nextErrorText)
				return
			}

			setFailed(true)
			setVerifyingLink(false)
		},
	})

	const resendRequest = useAxiosSubmit({
		url: ApiUrls.AUTH.FORGOT_PASSWORD,
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
			setVerifyingLink(false)
			return
		}

		if (!token) {
			setFailed(true)
			setVerifyingLink(false)
			return
		}

		setFailed(false)
		setVerifyingLink(true)
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
		setSubmitted(true)
		const ok = validateAll()
		if (!ok) return

		await resendRequest.submit({
			overrideData: {
				identifier: values.identifier,
				deliveryMethod: EnumConfig.VerificationDeliveryMethod.Sms,
			},
		})
	}

	if (!isSmsFlow) {
		if (verifyingLink) {
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
						{t('auth.reset.title')}
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

					<Button
						component={RouterLink}
						to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.FORGOT_PASSWORD)}
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
						{t('auth.verify_link.request_new')}
					</Button>
				</Stack>
			</>
		)
	}

	return (
		<>
			<Box sx={{ mb: 3 }}>
				<Typography variant='h4' sx={{ mb: 1, fontWeight: 700 }}>
					{t('auth.reset_otp.title')}
				</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('auth.reset_otp.subtitle')}
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
						invalidText={t('auth.reset_otp.invalid_length', {
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
						{t('auth.reset_otp.verify_submit')}
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
							{t('auth.reset_otp.resend')}
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

export default VerifyResetPasswordPage

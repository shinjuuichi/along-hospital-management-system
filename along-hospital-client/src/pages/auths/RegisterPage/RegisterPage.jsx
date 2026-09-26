import PasswordTextField from '@/components/textFields/PasswordTextField'
import ValidationTextField from '@/components/textFields/ValidationTextField'
import { VerificationResendMs, VerificationStorageKey } from '@/constants/verificationConstants'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useForm from '@/hooks/useForm'
import useResendTimer from '@/hooks/useResendTimer'
import useTranslation from '@/hooks/useTranslation'
import { compare } from '@/utils/validateUtil'
import { Box, Button, CircularProgress, Link, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { Link as RouterLink, useNavigate } from 'react-router-dom'

const RegisterPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [submitted, setSubmitted] = useState(false)
	const { start } = useResendTimer(VerificationStorageKey.REGISTER)
	const { values, handleChange, registerRef, validateAll } = useForm({
		email: '',
		phone: '',
		password: '',
		confirmPassword: '',
	})

	const { loading, submit } = useAxiosSubmit({
		url: ApiUrls.AUTH.REGISTER,
		method: 'POST',
		onSuccess: async () => {
			start(VerificationResendMs)

			navigate(
				`${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.RESEND)}?identifier=${encodeURIComponent(values.email.trim())}`,
				{ replace: true }
			)
		},
	})

	const onSubmit = async (e) => {
		e.preventDefault()
		setSubmitted(true)
		const ok = validateAll()
		if (!ok) return
		await submit({ overrideData: values })
	}

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
					{t('auth.register.title')}
				</Typography>
				<Typography
					variant='body2'
					color='text.secondary'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.register.subtitle')}
				</Typography>
			</Box>

			<Box component='form' onSubmit={onSubmit}>
				<Stack spacing={{ xs: 2, sm: 2.5 }}>
					<ValidationTextField
						name='email'
						label={t('auth.field.email')}
						type='email'
						placeholder={t('auth.placeholder.email')}
						value={values.email}
						onChange={handleChange}
						ref={registerRef('email')}
						submitted={submitted}
					/>
					<ValidationTextField
						name='phone'
						label={t('auth.field.phone')}
						type='tel'
						placeholder={t('auth.placeholder.phone')}
						value={values.phone}
						onChange={handleChange}
						ref={registerRef('phone')}
						submitted={submitted}
					/>
					<PasswordTextField
						name='password'
						label={t('auth.field.password')}
						placeholder={t('auth.placeholder.password')}
						value={values.password}
						onChange={handleChange}
						ref={registerRef('password')}
						submitted={submitted}
					/>
					<PasswordTextField
						name='confirmPassword'
						label={t('auth.field.confirm_password')}
						placeholder={t('auth.placeholder.confirm_password')}
						value={values.confirmPassword}
						onChange={handleChange}
						ref={registerRef('confirmPassword')}
						submitted={submitted}
						validate={[compare(values.password, t('error.password_not_match'))]}
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
							bgcolor: 'primary.main',
							color: 'primary.contrastText',
							'&:hover': {
								bgcolor: 'primary.dark',
							},
							'&.Mui-disabled': {
								bgcolor: 'action.disabledBackground',
								color: 'text.disabled',
							},
						}}
					>
						{t('auth.register.submit')}
					</Button>
				</Stack>
			</Box>

			<Box sx={{ mt: { xs: 2, sm: 3 }, textAlign: 'center' }}>
				<Typography
					variant='body2'
					color='text.secondary'
					component='span'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.register.already_have_account')}{' '}
				</Typography>
				<Link
					component={RouterLink}
					to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)}
					variant='body2'
					sx={{
						fontWeight: 600,
						textDecoration: 'none',
						fontSize: { xs: '0.8rem', sm: '0.875rem' },
						'&:hover': { textDecoration: 'underline' },
					}}
				>
					{t('auth.register.login')}
				</Link>
			</Box>
		</>
	)
}

export default RegisterPage

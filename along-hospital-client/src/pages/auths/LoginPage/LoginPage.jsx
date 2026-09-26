import PasswordTextField from '@/components/textFields/PasswordTextField'
import ValidationTextField from '@/components/textFields/ValidationTextField'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { getReturnUrlByRole } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { isPhoneOrEmail, maxLen } from '@/utils/validateUtil'
import {
	Box,
	Button,
	CircularProgress,
	Divider,
	Link,
	Stack,
	Typography,
	useMediaQuery,
} from '@mui/material'
import { GoogleLogin } from '@react-oauth/google'
import { useEffect, useState } from 'react'
import { Link as RouterLink, useNavigate } from 'react-router-dom'

const LoginPage = () => {
	const { t } = useTranslation()
	const isNarrow = useMediaQuery('(max-width:500px)')
	const navigate = useNavigate()
	const { auth, login } = useAuth()

	const [isLogin, setIsLogin] = useState(false)
	useEffect(() => {
		if (isLogin && auth?.role) {
			navigate(getReturnUrlByRole(auth.role), { replace: true })
		}
	}, [auth, isLogin])

	const { values, handleChange, registerRef, validateAll } = useForm({
		identifier: '',
		password: '',
	})

	const { loading, submit } = useAxiosSubmit({
		url: ApiUrls.AUTH.LOGIN,
		method: 'POST',
		onSuccess: async (resp) => {
			const { accessToken, stage } = resp.data
			if (!accessToken) return
			await login(accessToken)

			if (stage && stage !== EnumConfig.AuthStage.Done) {
				navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.COMPLETE_PROFILE), { replace: true })
			} else {
				setIsLogin(true)
			}
		},
		onError: async (err) => {
			const message = err?.response?.data?.message

			if (message.toLowerCase().includes('not verified')) {
				const identifier = values.identifier?.trim()
				const target = identifier
					? `${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.RESEND)}?identifier=${encodeURIComponent(identifier)}`
					: routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.RESEND)

				navigate(target, { replace: true })
			}
		},
	})

	const onSubmit = async (e) => {
		e.preventDefault()
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
					{t('auth.login.welcome')}
				</Typography>
				<Typography
					variant='body2'
					color='text.secondary'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.login.subtitle')}
				</Typography>
			</Box>

			<Box component='form' onSubmit={onSubmit}>
				<Stack spacing={{ xs: 2, sm: 2.5 }}>
					<ValidationTextField
						name='identifier'
						label={t('auth.field.identifier')}
						placeholder={t('auth.placeholder.identifier')}
						value={values.identifier}
						onChange={handleChange}
						type='text'
						validate={[isPhoneOrEmail(), maxLen(255)]}
						ref={registerRef('identifier')}
					/>
					<PasswordTextField
						name='password'
						label={t('auth.field.password')}
						placeholder={t('auth.placeholder.password')}
						value={values.password}
						onChange={handleChange}
						ref={registerRef('password')}
					/>

					<Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
						<Link
							component={RouterLink}
							to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.FORGOT_PASSWORD)}
							variant='body2'
							sx={{
								textDecoration: 'none',
								fontSize: { xs: '0.8rem', sm: '0.875rem' },
								maxWidth: '100%',
								whiteSpace: 'nowrap',
								overflow: 'hidden',
								textOverflow: 'ellipsis',
								'&:hover': { textDecoration: 'underline' },
							}}
						>
							{t('auth.login.forgot_password')}
						</Link>
					</Box>

					<Button
						type='submit'
						variant='contained'
						size='large'
						fullWidth
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
						{t('auth.login.submit')}
					</Button>
				</Stack>
			</Box>

			<Box sx={{ my: { xs: 2, sm: 3 } }}>
				<Divider>
					<Typography
						variant='body2'
						color='text.secondary'
						sx={{ fontSize: { xs: '0.75rem', sm: '0.875rem' } }}
					>
						{t('auth.or')}
					</Typography>
				</Divider>
			</Box>

			<Box
				sx={{
					display: 'flex',
					justifyContent: 'center',
					'& > div': {
						width: '100% !important',
						maxWidth: isNarrow ? '100%' : 400,
					},
					'& > div > div': {
						width: '100% !important',
						maxWidth: '100% !important',
					},
				}}
			>
				<GoogleLogin
					onSuccess={async (credentialResponse) => {
						const idToken = credentialResponse?.credential
						if (!idToken) return
						await submit({ overrideData: { idToken }, overrideUrl: ApiUrls.AUTH.LOGIN_GOOGLE })
					}}
					text='signin_with'
					shape='rectangular'
					size='large'
					width={isNarrow ? 280 : 400}
					locale={t('auth.google.locale')}
				/>
			</Box>

			<Box sx={{ mt: { xs: 2, sm: 3 }, textAlign: 'center' }}>
				<Typography
					variant='body2'
					color='text.secondary'
					component='span'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.login.no_account')}{' '}
				</Typography>
				<Link
					component={RouterLink}
					to={routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.REGISTER)}
					variant='body2'
					sx={{
						fontWeight: 600,
						textDecoration: 'none',
						fontSize: { xs: '0.8rem', sm: '0.875rem' },
						'&:hover': { textDecoration: 'underline' },
					}}
				>
					{t('auth.login.register')}
				</Link>
			</Box>
		</>
	)
}

export default LoginPage

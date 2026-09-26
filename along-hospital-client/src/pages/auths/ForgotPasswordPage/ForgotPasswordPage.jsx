import ValidationTextField from '@/components/textFields/ValidationTextField'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { isPhoneOrEmail, maxLen } from '@/utils/validateUtil'
import { Box, Button, CircularProgress, Link, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { Link as RouterLink, useNavigate } from 'react-router-dom'

const ForgotPasswordPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [submitted, setSubmitted] = useState(false)
	const { values, handleChange, registerRef, validateAll } = useForm({
		identifier: '',
	})

	const { loading, submit } = useAxiosSubmit({
		url: ApiUrls.AUTH.FORGOT_PASSWORD_OPTIONS,
		method: 'POST',
		onSuccess: async (resp) => {
			navigate(
				`${routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.FORGOT_PASSWORD_METHOD)}?identifier=${encodeURIComponent(values.identifier.trim())}`,
				{
					replace: true,
					state: {
						options: resp.data,
					},
				}
			)
		},
	})

	const onSubmit = async (e) => {
		e.preventDefault()
		setSubmitted(true)
		const ok = validateAll()
		if (!ok) return
		await submit({
			overrideData: {
				identifier: values.identifier,
			},
		})
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
					{t('auth.forgot.title')}
				</Typography>
				<Typography
					variant='body2'
					color='text.secondary'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.forgot.subtitle')}
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
						ref={registerRef('identifier')}
						submitted={submitted}
						validate={[isPhoneOrEmail(), maxLen(255)]}
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
						{t('button.next')}
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
					{t('auth.forgot.remember_password')}{' '}
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
					{t('auth.login.submit')}
				</Link>
			</Box>
		</>
	)
}

export default ForgotPasswordPage

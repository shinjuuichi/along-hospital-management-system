import { ApiUrls } from '@/configs/apiUrls'
import axiosConfig from '@/configs/axiosConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { maxLen, minLen } from '@/utils/validateUtil'
import { Box, Button, CircularProgress, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const CompleteProfilePage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { auth, login } = useAuth()
	const _enum = useEnum()
	const { values, handleChange, setField, registerRef, validateAll } = useForm({
		name: '',
		phone: '',
		dateOfBirth: '',
		gender: '',
		address: '',
	})
	const [submitted, setSubmitted] = useState(false)
	const [isCompleted, setIsCompleted] = useState(false)
	const { renderField } = useFieldRenderer(values, setField, handleChange, registerRef, submitted)

	const needsPhone = auth?.stage === EnumConfig.AuthStage.PatientProfilePendingWithoutPhone

	const fields = [
		{
			key: 'name',
			title: t('profile.field.name'),
			type: 'text',
			validate: [minLen(2), maxLen(255)],
			props: { placeholder: t('profile.placeholder.name') },
		},
		...(needsPhone
			? [
					{
						key: 'phone',
						title: t('profile.field.phone'),
						type: 'tel',
						required: true,
						props: { placeholder: t('auth.placeholder.phone') },
					},
				]
			: []),
		{
			key: 'dateOfBirth',
			title: t('profile.field.date_of_birth'),
			type: 'date',
			maxValue: new Date().toISOString().split('T')[0],
		},
		{
			key: 'gender',
			title: t('profile.field.gender'),
			type: 'select',
			options: _enum.genderOptions,
		},
		{
			key: 'address',
			title: t('profile.field.address'),
			type: 'text',
			validate: [maxLen(255)],
			props: { placeholder: t('profile.placeholder.address') },
		},
	]

	useEffect(() => {
		if (!auth) {
			navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN), { replace: true })
			return
		}
	}, [auth, navigate])

	useEffect(() => {
		if (isCompleted && auth.role) {
			navigate('/', { replace: true })
		}
	}, [isCompleted, auth, navigate])

	const { phone: _omitPhone, ...rest } = values
	const payload = needsPhone ? values : rest

	const postCompleteProfile = useAxiosSubmit({
		url: ApiUrls.AUTH.COMPLETE_PROFILE,
		method: 'POST',
		data: payload,
		onSuccess: async () => {
			const refreshResponse = await axiosConfig.post(ApiUrls.AUTH.REFRESH_TOKEN)

			const { accessToken } = refreshResponse.data

			await login(accessToken)

			setIsCompleted(true)
		},
	})

	const onSubmit = async (e) => {
		e.preventDefault()
		setSubmitted(true)
		const ok = validateAll()
		if (!ok) return
		await postCompleteProfile.submit()
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
					{t('auth.complete.title')}
				</Typography>
				<Typography
					variant='body2'
					color='text.secondary'
					sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
				>
					{t('auth.complete.subtitle')}
				</Typography>
			</Box>

			<Box component='form' onSubmit={onSubmit}>
				<Stack spacing={{ xs: 2, sm: 2.5 }}>
					{fields.map(renderField)}

					<Button
						type='submit'
						variant='contained'
						size='large'
						disabled={postCompleteProfile.loading}
						startIcon={postCompleteProfile.loading && <CircularProgress size={20} color='inherit' />}
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
						{t('button.submit')}
					</Button>
				</Stack>
			</Box>
		</>
	)
}

export default CompleteProfilePage

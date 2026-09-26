import { ApiUrls } from '@/configs/apiUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { compare, isPasswordStrong, maxLen, notEqual } from '@/utils/validateUtil'
import { CheckCircleOutline, Save, Security } from '@mui/icons-material'
import { Box, Button, Grid, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const ChangePasswordSection = () => {
	const { t } = useTranslation()
	const { logout } = useAuth()
	const [submitted, setSubmitted] = useState(false)

	const initialValues = {
		currentPassword: '',
		newPassword: '',
		confirmPassword: '',
	}

	const { values, handleChange, setField, reset, registerRef, validateAll } = useForm(initialValues)

	const changePassword = useAxiosSubmit({
		url: ApiUrls.AUTH.CHANGE_PASSWORD,
		method: 'POST',
		onSuccess: () => {
			setSubmitted(false)
			reset(initialValues)
			logout()
		},
	})

	const { renderField, hasRequiredMissing } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		submitted,
		'outlined',
		'medium'
	)

	const fields = [
		{
			key: 'currentPassword',
			title: t('profile.field.current_password'),
			type: 'password',
			validate: [maxLen(50)],
		},
		{
			key: 'newPassword',
			title: t('profile.field.new_password'),
			type: 'password',
			validate: [
				isPasswordStrong(),
				maxLen(50),
				notEqual(values.currentPassword, t('error.password_must_be_different')),
			],
		},
		{
			key: 'confirmPassword',
			title: t('profile.field.confirm_password'),
			type: 'password',
			validate: [
				isPasswordStrong(),
				maxLen(50),
				compare(values.newPassword, t('error.password_not_match')),
			],
		},
	]

	const handleSubmit = async () => {
		setSubmitted(true)

		const ok = validateAll()
		const isMissing = hasRequiredMissing(fields)

		if (!ok || isMissing) return

		await changePassword.submit({
			overrideData: {
				currentPassword: values.currentPassword,
				newPassword: values.newPassword,
				confirmPassword: values.confirmPassword,
			},
		})
	}

	const passwordTips = [
		t('profile.text.password_tip_length'),
		t('profile.text.password_tip_uppercase'),
		t('profile.text.password_tip_number'),
		t('profile.text.password_tip_special'),
	]

	return (
		<Paper sx={{ p: 3, borderRadius: 2, height: '100%', minHeight: 400 }}>
			<Grid container spacing={4}>
				<Grid size={{ xs: 12, md: 6 }}>
					<Stack spacing={3}>
						<Box>
							<Typography variant='h6' sx={{ fontWeight: 600, mb: 0.5 }}>
								{t('profile.title.change_password')}
							</Typography>
							<Typography variant='body2' color='text.secondary'>
								{t('profile.text.change_password_description')}
							</Typography>
						</Box>

						<Stack spacing={2.5}>{fields.map((field) => renderField(field))}</Stack>

						<Stack direction='row' justifyContent='flex-end' spacing={2}>
							<Button
								variant='outlined'
								color='error'
								onClick={() => {
									setSubmitted(false)
									reset(initialValues)
								}}
								disabled={changePassword.loading}
							>
								{t('button.cancel')}
							</Button>
							<Button
								variant='contained'
								startIcon={<Save />}
								onClick={handleSubmit}
								disabled={changePassword.loading}
							>
								{changePassword.loading ? t('button.submitting') : t('button.save')}
							</Button>
						</Stack>
					</Stack>
				</Grid>

				<Grid size={{ xs: 12, md: 6 }} sx={{ display: { xs: 'none', md: 'block' } }}>
					<Box
						sx={{
							p: 3,
							borderRadius: 2,
							bgcolor: 'primary.softBg',
							border: '1px solid',
							borderColor: 'primary.softBorder',
							height: '100%',
						}}
					>
						<Stack spacing={2.5}>
							<Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
								<Security color='primary' sx={{ fontSize: 28 }} />
								<Typography variant='subtitle1' sx={{ fontWeight: 600 }}>
									{t('profile.title.password_tips')}
								</Typography>
							</Box>
							<Stack spacing={1.5}>
								{passwordTips.map((tip, index) => (
									<Box key={index} sx={{ display: 'flex', alignItems: 'flex-start', gap: 1 }}>
										<CheckCircleOutline color='primary' sx={{ fontSize: 20, mt: 0.25 }} />
										<Typography variant='body2' color='text.secondary'>
											{tip}
										</Typography>
									</Box>
								))}
							</Stack>
						</Stack>
					</Box>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default ChangePasswordSection

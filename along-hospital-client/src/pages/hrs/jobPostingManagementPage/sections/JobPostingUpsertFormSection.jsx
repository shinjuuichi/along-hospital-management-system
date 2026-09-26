import TiptapEditor from '@/components/editors/TiptapEditor/TiptapEditor'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useTranslation from '@/hooks/useTranslation'
import { Save } from '@mui/icons-material'
import {
	Box,
	Button,
	Card,
	CardContent,
	CircularProgress,
	FormHelperText,
	Stack,
	Typography,
} from '@mui/material'

const JobPostingUpsertFormSection = ({
	detailLoading,
	onSubmit,
	formData,
	errors,
	onInputChange,
	onDescriptionChange,
	onContentImageUpload,
	submitButtonLabel,
	isFormDisabled,
	onBack,
	isSubmitting,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const handleChange = (e) => {
		const { name, value } = e.target
		if (name) onInputChange(name, value)
	}

	const { renderField } = useFieldRenderer(
		formData,
		onInputChange,
		handleChange,
		() => {},
		false,
		'outlined',
		'medium'
	)

	const fields = [
		{
			key: 'title',
			title: t('job_posting.field.title'),
			type: 'text',
			props: {
				disabled: isFormDisabled,
			},
		},
		{
			key: 'role',
			title: t('job_posting.field.role'),
			type: 'select',
			options: _enum.roleOptions.filter(opt => opt.value !== EnumConfig.Role.Patient),
			props: {
				disabled: isFormDisabled,
			},
		},
		{
			key: 'employmentType',
			title: t('job_posting.field.employment_type'),
			type: 'select',
			options: _enum.employmentTypeOptions,
			props: {
				disabled: isFormDisabled,
			},
		},
		{
			key: 'salaryMin',
			title: t('job_posting.field.salary_min'),
			type: 'number',
			required: false,
			props: {
				disabled: isFormDisabled,
			},
		},
		{
			key: 'closeDate',
			title: t('job_posting.field.close_date'),
			type: 'date',
			required: false,
			props: {
				disabled: isFormDisabled,
				InputLabelProps: { shrink: true },
				inputProps: { min: new Date().toISOString().split('T')[0] },
			},
		},
		{
			key: 'requirement',
			title: t('job_posting.field.requirement'),
			type: 'text',
			multiple: 6,
			required: false,
			props: {
				disabled: isFormDisabled,
				placeholder: t('job_posting.placeholder.enter_requirement'),
			},
		},
		{
			key: 'benefit',
			title: t('job_posting.field.benefit'),
			type: 'text',
			multiple: 6,
			required: false,
			props: {
				disabled: isFormDisabled,
				placeholder: t('job_posting.placeholder.enter_benefit'),
			},
		},
	]

	return (
		<Card>
			<CardContent>
				{detailLoading && (
					<Stack direction='row' spacing={1.5} alignItems='center' sx={{ mb: 2 }}>
						<CircularProgress size={20} />
						<Typography variant='body2' color='text.secondary'>
							{t('text.loading')}
						</Typography>
					</Stack>
				)}

				<Box
					component='form'
					onSubmit={onSubmit}
					sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}
				>
					{fields.map((field) => renderField(field))}

					<Box>
						<Typography variant='body2' color='text.secondary' sx={{ mb: 1 }}>
							{t('job_posting.field.description')} *
						</Typography>
						<TiptapEditor
							content={formData.description || ''}
							onChange={onDescriptionChange}
							onImageUpload={onContentImageUpload}
							placeholder={t('job_posting.placeholder.enter_description')}
							error={!!errors.description}
							minHeight={200}
							editable={!isFormDisabled}
						/>
						{errors.description && (
							<FormHelperText error sx={{ mt: 1 }}>
								{errors.description}
							</FormHelperText>
						)}
					</Box>

					<Stack direction='row' justifyContent='space-between' sx={{ mt: 2 }}>
						<Button variant='outlined' onClick={onBack} disabled={isSubmitting}>
							{t('button.back')}
						</Button>
						<Button
							type='submit'
							variant='contained'
							color='success'
							startIcon={<Save />}
							disabled={isFormDisabled}
						>
							{submitButtonLabel}
						</Button>
					</Stack>
				</Box>
			</CardContent>
		</Card>
	)
}

export default JobPostingUpsertFormSection

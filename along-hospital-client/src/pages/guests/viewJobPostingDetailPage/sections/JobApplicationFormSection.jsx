import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useTranslation from '@/hooks/useTranslation'
import { isEmail, isPhone } from '@/utils/validateUtil'
import { useCallback, useMemo } from 'react'

const JobApplicationFormSection = ({ open, onClose, jobPostingId }) => {
	if (!jobPostingId) return null
	const { t } = useTranslation()

	const initialValues = useMemo(
		() => ({
			name: '',
			email: '',
			phone: '',
			dateOfBirth: '',
			address: '',
			gender: '',
			cvUrl: null,
			jobPostingId: jobPostingId,
		}),
		[jobPostingId]
	)

	const { submit: submitApplication } = useAxiosSubmit({
		url: ApiUrls.JOB_APPLICATION.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const fields = useMemo(
		() => [
			{
				key: 'name',
				title: t('job_application.field.full_name'),
				type: 'text',
			},
			{
				key: 'email',
				title: t('job_application.field.email'),
				type: 'email',
				validate: [isEmail()],
			},
			{
				key: 'phone',
				title: t('job_application.field.phone'),
				type: 'tel',
				validate: [isPhone()],
			},
			{
				key: 'dateOfBirth',
				title: t('job_application.field.date_of_birth'),
				type: 'date',
			},
			{
				key: 'address',
				title: t('job_application.field.address'),
				type: 'text',
			},
			{
				key: 'gender',
				title: t('job_application.field.gender'),
				type: 'select',
				options: [
					{ value: EnumConfig.Gender.Male, label: t('enum.gender.male') },
					{ value: EnumConfig.Gender.Female, label: t('enum.gender.female') },
					{ value: EnumConfig.Gender.Other, label: t('enum.gender.other') },
				],
			},
			{
				key: 'cvUrl',
				title: t('job_application.field.cv_url'),
				type: 'file',
				accept: '.pdf',
				required: true,
			},
		],
		[t]
	)

	const handleSubmit = useCallback(
		async ({ values, closeDialog }) => {
			const { cvUrl, ...rest } = values
			const payload = { ...rest, CVFile: cvUrl }
			const response = await submitApplication({ overrideData: payload })
			if (response) closeDialog()
		},
		[submitApplication]
	)

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('job_application.dialog.apply_title')}
			fields={fields}
			initialValues={initialValues}
			submitLabel={t('job_application.button.submit_application')}
			onSubmit={handleSubmit}
			maxWidth='sm'
		/>
	)
}

export default JobApplicationFormSection

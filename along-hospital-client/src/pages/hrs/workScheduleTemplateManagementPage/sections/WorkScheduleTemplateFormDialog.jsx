import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const WorkScheduleTemplateManagementFormDialog = ({
	open,
	onClose,
	title,
	initialValues = {},
	submitLabel,
	submitButtonColor = 'primary',
	onSubmit,
	showActiveField = false,
}) => {
	const { t } = useTranslation()

	const fields = [
		{
			key: 'name',
			title: t('work_schedule_template.field.name'),
			validate: [maxLen(255)],
		},
		{
			key: 'description',
			title: t('work_schedule_template.field.description'),
			multiple: 3,
			required: false,
			validate: [maxLen(1000)],
		},
		...(showActiveField
			? [
					{
						key: 'isActive',
						title: t('work_schedule_template.field.active'),
						type: 'checkbox',
						required: false,
					},
				]
			: []),
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={title}
			fields={fields}
			initialValues={showActiveField ? initialValues : { ...initialValues, isActive: undefined }}
			onSubmit={onSubmit}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			textFieldVariant='outlined'
		/>
	)
}

export default WorkScheduleTemplateManagementFormDialog

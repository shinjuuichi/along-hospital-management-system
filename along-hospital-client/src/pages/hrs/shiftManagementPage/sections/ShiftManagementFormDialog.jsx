import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const ShiftManagementFormDialog = ({
	open,
	onClose,
	onSubmit,
	initialValues = {},
	submitLabel,
	title,
	submitButtonColor,
}) => {
	const { t } = useTranslation()

	const fields = [
		{
			key: 'name',
			title: t('shift.field.name'),
			validate: [maxLen(255)],
		},
		{
			key: 'timeRange',
			title: t('shift.field.time_range'),
			type: 'timerange',
			from: {
				key: 'startTime',
				label: t('shift.field.start_time'),
			},
			to: {
				key: 'endTime',
				label: t('shift.field.end_time'),
			},
		},
		{
			key: 'isOvertime',
			title: t('shift.field.is_overtime'),
			type: 'checkbox',
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={title}
			fields={fields}
			initialValues={initialValues}
			onSubmit={onSubmit}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			textFieldVariant='outlined'
		/>
	)
}

export default ShiftManagementFormDialog

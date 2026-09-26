import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const BuildingFormDialog = ({
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
			title: t('building.field.name'),
			validate: [maxLen(100)],
		},
		{
			key: 'location',
			title: t('building.field.location'),
			validate: [maxLen(255)],
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
		/>
	)
}

export default BuildingFormDialog

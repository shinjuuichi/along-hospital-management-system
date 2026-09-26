import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { numberRange } from '@/utils/validateUtil'

const FloorFormDialog = ({
	open,
	onClose,
	onSubmit,
	initialValues = {},
	buildingId,
	submitLabel,
	title,
	submitButtonColor,
}) => {
	const { t } = useTranslation()

	const mergedInitialValues = buildingId ? { ...initialValues, buildingId } : initialValues

	const fields = [
		{
			key: 'floorNumber',
			title: t('floor.field.floor_number'),
			type: 'number',
			minValue: 0,
			validate: [numberRange(0, null)],
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={title}
			fields={fields}
			initialValues={mergedInitialValues}
			onSubmit={onSubmit}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
		/>
	)
}

export default FloorFormDialog

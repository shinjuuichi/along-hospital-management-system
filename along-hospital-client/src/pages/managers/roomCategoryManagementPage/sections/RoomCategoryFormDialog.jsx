import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const RoomCategoryFormDialog = ({
	open,
	onClose,
	onSubmit,
	initialValues = {},
	submitLabel,
	title,
	submitButtonColor,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const normalizedInitialValues = {
		...initialValues,
		roles: initialValues.roles || [],
	}

	const fields = [
		{
			key: 'name',
			title: t('room_category.field.name'),
			validate: [maxLen(100)],
		},
		{
			key: 'description',
			title: t('room_category.field.description'),
			validate: [maxLen(1000)],
			multiple: 4,
			required: false,
		},
		{
			key: 'roles',
			title: t('room_category.field.roles'),
			type: 'select',
			multiple: true,
			required: false,
			options: _enum.roleOptions,
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={title}
			fields={fields}
			initialValues={normalizedInitialValues}
			onSubmit={onSubmit}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			textFieldVariant='outlined'
		/>
	)
}

export default RoomCategoryFormDialog

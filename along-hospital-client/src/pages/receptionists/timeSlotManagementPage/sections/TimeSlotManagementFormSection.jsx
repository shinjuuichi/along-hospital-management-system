import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { numberHigherThan } from '@/utils/validateUtil'

const TimeSlotManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
}) => {
	const { t } = useTranslation()

	const upsertFields = [
		{
			key: 'time',
			title: t('time_slot.field.time'),
			type: 'time',
		},
		{
			key: 'capacityPerDoctor',
			title: t('time_slot.field.capacity_per_doctor'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
	]

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={upsertFields}
				onSubmit={onCreateSubmit}
				title={t('time_slot.title.create_form')}
				submitButtonColor='primary'
				submitLabel={t('button.create')}
			/>
			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={upsertFields}
				initialValues={selectedItem}
				onSubmit={onUpdateSubmit}
				title={t('time_slot.title.update_form')}
				submitButtonColor='success'
				submitLabel={t('button.update')}
			/>
		</>
	)
}

export default TimeSlotManagementFormSection

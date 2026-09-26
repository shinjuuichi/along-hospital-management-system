import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const InterviewTypeManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
}) => {
	const { t } = useTranslation()

	const formFields = [
		{
			key: 'name',
			title: t('interview_type.field.name'),
			validate: [maxLen(255)],
		},
		{
			key: 'description',
			title: t('interview_type.field.description'),
			type: 'text',
			required: false,
			validate: [maxLen(1000)],
		},
	]

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={formFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('interview_type.dialog.create_title')}
				onSubmit={onCreateSubmit}
			/>
			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={formFields}
				initialValues={selectedItem ?? {}}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('interview_type.dialog.update_title')}
				onSubmit={onUpdateSubmit}
			/>
		</>
	)
}

export default InterviewTypeManagementFormSection

import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useCallback, useMemo } from 'react'

const StaffCertificateTypeManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
}) => {
	const { t } = useTranslation()

	const formFields = useMemo(
		() => [
			{
				key: 'name',
				title: t('staff_certificate_type.field.name'),
				type: 'text',
				required: true,
			},
			{
				key: 'scopeOfPractice',
				title: t('staff_certificate_type.field.scope_of_practice'),
				type: 'textarea',
				required: true,
			},
		],
		[t]
	)

	const updateInitialValues = useMemo(() => {
		if (!selectedItem) return {}
		return {
			id: selectedItem.id,
			name: selectedItem.name,
			scopeOfPractice: selectedItem.scopeOfPractice,
		}
	}, [selectedItem])

	const handleCreateSubmit = useCallback(
		async ({ values, closeDialog }) => {
			await onCreateSubmit({ values, closeDialog })
		},
		[onCreateSubmit]
	)

	const handleUpdateSubmit = useCallback(
		async ({ values, closeDialog }) => {
			await onUpdateSubmit({ values, closeDialog })
		},
		[onUpdateSubmit]
	)

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={formFields}
				initialValues={{}}
				submitLabel={t('button.create')}
				title={t('staff_certificate_type.dialog.create_title')}
				maxWidth='sm'
				onSubmit={handleCreateSubmit}
			/>

			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={formFields}
				initialValues={updateInitialValues}
				submitLabel={t('button.update')}
				title={t('staff_certificate_type.dialog.view_title')}
				maxWidth='sm'
				onSubmit={handleUpdateSubmit}
			/>
		</>
	)
}

export default StaffCertificateTypeManagementFormSection

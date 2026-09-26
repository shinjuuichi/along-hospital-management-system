import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'

const RespondComplaintDialog = ({
	open,
	onClose,
	initialResponse,
	onSaveDraft = (values) => Promise.resolve(values),
	onCloseComplaint = () => {},
	onSubmit = (values) => Promise.resolve(values),
}) => {
	const { t } = useTranslation()
	const fields = [{ key: 'response', title: t('complaint.field.response'), multiple: 2 }]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			fields={fields}
			title={t('medical_history.dialog.title.respond_complaint')}
			initialValues={{ response: initialResponse }}
			submitButtonColor='primary'
			submitLabel={t('button.submit')}
			onSubmit={({ values }) => onSubmit(values)}
			additionalButtons={[
				{
					label: t('medical_history.button.save_as_draft'),
					color: 'info',
					variant: 'outlined',
					onClick: ({ values }) => onSaveDraft(values),
				},
				{
					label: t('medical_history.button.close_complaint'),
					color: 'error',
					variant: 'outlined',
					onClick: onCloseComplaint,
				},
			]}
		/>
	)
}

export default RespondComplaintDialog

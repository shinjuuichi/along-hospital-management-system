import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'

const CreateComplaintDialog = ({
	open,
	onClose,
	onSubmit = (values) => Promise.resolve(values),
}) => {
	const { t } = useTranslation()
	const { complaintTopicOptions } = useEnum()

	const fields = [
		{
			key: 'complaintTopic',
			title: t('complaint.field.topic'),
			type: 'select',
			options: complaintTopicOptions,
		},
		{
			key: 'content',
			title: t('complaint.field.content'),
			type: 'textArea',
			multiple: 3,
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			fields={fields}
			title={t('medical_history.dialog.title.create_complaint')}
			submitLabel={t('button.create')}
			submitButtonColor='primary'
			onSubmit={({ values }) => onSubmit(values)}
		/>
	)
}

export default CreateComplaintDialog

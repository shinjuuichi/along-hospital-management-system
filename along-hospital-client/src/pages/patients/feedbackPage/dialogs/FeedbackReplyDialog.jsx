import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const FeedbackReplyDialog = ({ open, onClose, feedbackId, onSubmit, loading = false }) => {
	const { t } = useTranslation()

	const handleSubmit = async ({ values, closeDialog }) => {
		await onSubmit?.({ feedbackId, content: values.content })
		closeDialog()
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('feedback.button.reply')}
			fields={[
				{
					key: 'content',
					title: t('feedback.field.respond_content'),
					validate: [maxLen(1000)],
				},
			]}
			initialValues={{ content: '' }}
			submitLabel={t('feedback.button.send_reply')}
			submitButtonColor='primary'
			onSubmit={handleSubmit}
			isLoading={loading}
		/>
	)
}

export default FeedbackReplyDialog

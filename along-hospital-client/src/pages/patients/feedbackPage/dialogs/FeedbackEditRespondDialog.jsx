import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'

const FeedbackEditRespondDialog = ({ open, onClose, respond, onSubmit, loading = false }) => {
	const { t } = useTranslation()

	const handleSubmit = async ({ values, closeDialog }) => {
		await onSubmit?.(values)
		closeDialog()
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('feedback.title.edit_respond')}
			fields={[
				{
					key: 'content',
					title: t('feedback.field.respond_content'),
					validate: [maxLen(1000)],
				},
			]}
			initialValues={{ content: respond?.content || '' }}
			submitLabel={t('button.save')}
			submitButtonColor='primary'
			onSubmit={handleSubmit}
			isLoading={loading}
		/>
	)
}

export default FeedbackEditRespondDialog

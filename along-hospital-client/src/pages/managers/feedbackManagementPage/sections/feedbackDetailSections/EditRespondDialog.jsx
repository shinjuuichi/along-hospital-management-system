import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useMemo } from 'react'

const EditRespondDialog = ({ open, respond, onClose, onSubmit, loading = false }) => {
	const { t } = useTranslation()

	const fields = useMemo(
		() => [
			{
				key: 'content',
				title: t('feedback.field.respond_content'),
				multiple: 4,
			},
		],
		[t]
	)

	const initialValues = useMemo(() => ({ content: respond?.content || '' }), [respond])

	const handleSubmit = async ({ values, closeDialog }) => {
		await onSubmit(values)
		closeDialog()
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('feedback.title.edit_respond')}
			fields={fields}
			initialValues={initialValues}
			onSubmit={handleSubmit}
			isLoading={loading}
		/>
	)
}

export default EditRespondDialog

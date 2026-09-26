import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'

const ReportFeedbackDialog = ({ open, onClose, onSubmit, initialReason = '', loading = false }) => {
	const { t } = useTranslation()

	return (
		<GenericFormDialog
			open={!!open}
			onClose={onClose}
			title={t('feedback.dialog.report_title')}
			initialValues={{ reason: initialReason || '' }}
			fields={[
				{
					key: 'reason',
					title: t('feedback.dialog.report_reason'),
					multiple: 1,
					required: true,
				},
			]}
			submitLabel={t('button.report')}
			submitButtonColor='warning'
			isLoading={loading}
			onSubmit={async ({ values, closeDialog }) => {
				const result = await onSubmit?.(values.reason)
				if (result) closeDialog()
			}}
		/>
	)
}

export default ReportFeedbackDialog

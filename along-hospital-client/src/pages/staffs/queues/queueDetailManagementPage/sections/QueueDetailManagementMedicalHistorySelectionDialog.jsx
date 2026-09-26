import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Stack, Typography } from '@mui/material'
import { toast } from 'react-toastify'

const QueueDetailManagementMedicalHistorySelectionDialog = ({
	open,
	onClose,
	medicalHistories = [],
	onSubmit = async () => {},
}) => {
	const { t } = useTranslation()

	const fields = [
		{
			key: 'medicalHistoryId',
			title: t('queue.field.medical_history_selection'),
			type: 'select-dialog',
			options: medicalHistories.map((medicalHistory) => ({
				value: medicalHistory.id,
				label: medicalHistory,
				searchKey: medicalHistory.medicalHistoryNumber,
			})),
			renderOption: (_, label) => (
				<Stack sx={{ py: 0.25, width: '100%' }}>
					<Typography sx={{ fontWeight: 700, fontSize: '0.92rem' }}>
						{t('medical_history.field.medical_history_number')}{' '}
						{renderEmptyFallback(label?.medicalHistoryNumber)}
					</Typography>
					<Typography variant='caption' color='text.secondary'>
						{label?.patient?.name || t('queue.placeholder.patient_name_pending')}
					</Typography>
				</Stack>
			),
			renderOptionValue: (_, label) => label?.medicalHistoryNumber,
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('queue.dialog.assign_medical_history_title')}
			fields={fields}
			initialValues={{ medicalHistoryId: '' }}
			onSubmit={async ({ values, closeDialog }) => {
				const medicalHistoryId = Number(values.medicalHistoryId)
				if (!Number.isInteger(medicalHistoryId) || medicalHistoryId <= 0) {
					toast.error(t('queue.error.invalid_medical_history'))
					return
				}

				await onSubmit(medicalHistoryId)
				closeDialog()
			}}
			submitLabel={t('button.assign')}
			submitButtonColor='primary'
			maxWidth='sm'
		/>
	)
}

export default QueueDetailManagementMedicalHistorySelectionDialog

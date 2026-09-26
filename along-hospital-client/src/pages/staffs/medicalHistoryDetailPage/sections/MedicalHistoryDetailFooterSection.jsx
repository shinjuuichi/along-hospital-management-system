import ConfirmationButton from '@/components/generals/ConfirmationButton'
import useTranslation from '@/hooks/useTranslation'
import { Button, Paper, Stack } from '@mui/material'

const MedicalHistoryDetailFooterSection = ({
	canUpdateMedicalHistory,
	hasActiveInpatientOccupancy = false,
	onClickUpdateMedicalHistory,
	onClickCompleteMedicalHistory,
}) => {
	const { t } = useTranslation()

	const renderButtons = () => {
		if (!canUpdateMedicalHistory) {
			return null
		}

		return (
			<>
				<Button variant='outlined' color='success' onClick={onClickUpdateMedicalHistory}>
					{t('medical_history.button.update_medical_history')}
				</Button>
				<ConfirmationButton
					confirmationTitle={t('medical_history.dialog.confirm.complete_medical_history_title')}
					confirmationDescription={t(
						'medical_history.dialog.confirm.complete_medical_history_description'
					)}
					confirmButtonColor='secondary'
					confirmButtonText={t('button.complete')}
					variant='contained'
					color='secondary'
					disabled={hasActiveInpatientOccupancy}
					onConfirm={onClickCompleteMedicalHistory}
				>
					{t('medical_history.button.complete_medical_history')}
				</ConfirmationButton>
			</>
		)
	}

	if (renderButtons().props.children.every((child) => !child)) {
		return null
	}

	return (
		<Paper
			sx={{
				position: 'sticky',
				bottom: 15,
				p: 2,
				borderTop: '1px solid',
				borderColor: 'divider',
				backgroundColor: 'background.lightBlue',
			}}
			elevation={3}
		>
			<Stack
				direction={{ xs: 'column', sm: 'row' }}
				spacing={1.5}
				justifyContent='flex-start'
				alignItems='center'
			>
				{renderButtons()}
			</Stack>
		</Paper>
	)
}

export default MedicalHistoryDetailFooterSection

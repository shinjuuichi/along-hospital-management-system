import useTranslation from '@/hooks/useTranslation'
import { Button, Stack, Toolbar, Typography } from '@mui/material'

const CreateMedicalHistoryFooterSection = ({
	selectedPatient,
	createButtonDisabled,
	onCreateMedicalHistoryClick,
	onCancelClick,
	loading,
}) => {
	const { t } = useTranslation()

	return (
		<Toolbar
			sx={{
				position: 'sticky',
				bottom: 0,
				mt: 2,
				bgcolor: 'background.paper',
				borderTop: '1px solid',
				borderColor: 'divider',
				borderRadius: 2,
				boxShadow: 1,
				zIndex: 1,
			}}
		>
			<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ width: '100%' }}>
				<Typography variant='body2' color='text.secondary'>
					{selectedPatient
						? t('medical_history.title.chosen_patient', { name: selectedPatient.name })
						: t('medical_history.title.no_chosen_patient')}
				</Typography>
				<Stack direction='row' spacing={1}>
					<Button
						variant='contained'
						disabled={createButtonDisabled}
						onClick={onCreateMedicalHistoryClick}
						loading={loading}
					>
						{t('medical_history.button.create_medical_history')}
					</Button>
					<Button onClick={onCancelClick} variant='text'>
						{t('button.cancel')}
					</Button>
				</Stack>
			</Stack>
		</Toolbar>
	)
}

export default CreateMedicalHistoryFooterSection

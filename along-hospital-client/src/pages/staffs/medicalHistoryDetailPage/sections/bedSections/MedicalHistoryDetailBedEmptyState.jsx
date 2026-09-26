import { Button, Stack, Typography } from '@mui/material'

const MedicalHistoryDetailBedEmptyState = ({
	canShowAssignButton,
	loadingAction = false,
	onClickAssignBed,
	t,
}) => {
	return (
		<Stack alignItems='center' justifyContent='center' spacing={2} sx={{ py: 4 }}>
			<Typography color='text.secondary'>{t('bed_occupancy.placeholder.no_bed_assigned')}</Typography>
			{canShowAssignButton && (
				<Button variant='contained' onClick={onClickAssignBed} disabled={loadingAction}>
					{t('bed_occupancy.button.assign_bed')}
				</Button>
			)}
		</Stack>
	)
}

export default MedicalHistoryDetailBedEmptyState

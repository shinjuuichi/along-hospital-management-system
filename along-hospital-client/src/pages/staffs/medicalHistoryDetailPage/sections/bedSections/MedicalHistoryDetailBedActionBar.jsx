import ConfirmationButton from '@/components/generals/ConfirmationButton'
import { Button, Stack } from '@mui/material'

const MedicalHistoryDetailBedActionBar = ({
	canShowTransferButton,
	canShowDischargeButton,
	loadingAction = false,
	onClickTransferBed,
	onClickDischargeBed,
	t,
}) => {
	if (!canShowTransferButton && !canShowDischargeButton) {
		return null
	}

	return (
		<Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5}>
			{canShowTransferButton && (
				<Button
					variant='outlined'
					color='primary'
					onClick={onClickTransferBed}
					disabled={loadingAction}
				>
					{t('bed_occupancy.button.transfer_bed')}
				</Button>
			)}
			{canShowDischargeButton && (
				<ConfirmationButton
					variant='contained'
					color='warning'
					disabled={loadingAction}
					confirmationTitle={t('bed_occupancy.dialog.confirm.discharge_title')}
					confirmationDescription={t('bed_occupancy.dialog.confirm.discharge_description')}
					confirmButtonColor='warning'
					confirmButtonText={t('bed_occupancy.button.discharge_bed')}
					onConfirm={onClickDischargeBed}
				>
					{t('bed_occupancy.button.discharge_bed')}
				</ConfirmationButton>
			)}
		</Stack>
	)
}

export default MedicalHistoryDetailBedActionBar

import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'

const AppointmentInfo = ({
	appointmentTime = '2:30 PM (var)',
	durationText = '30 minutes (var)',
}) => {
	const { t } = useTranslation()
	return (
		<>
			<Stack direction='row' justifyContent='space-between' sx={{ mb: 1 }}>
				<Typography color='text.secondary'>{t('meeting_room.title.appointment_time')}</Typography>
				<Typography>{appointmentTime}</Typography>
			</Stack>
			<Stack direction='row' justifyContent='space-between' sx={{ mb: 2 }}>
				<Typography color='text.secondary'>{t('meeting_room.title.duration')}</Typography>
				<Typography>{durationText}</Typography>
			</Stack>
		</>
	)
}

export default AppointmentInfo

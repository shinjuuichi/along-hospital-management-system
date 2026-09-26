import useTranslation from '@/hooks/useTranslation'
import { Circle } from '@mui/icons-material'
import { Avatar, Box, Stack, Typography } from '@mui/material'

const DoctorInfoHeader = ({
	avatarUrl = '',
	name = 'Doctor Name (Var)',
	specialty = 'Doctor Specialty (Var)',
}) => {
	const { t } = useTranslation()
	return (
		<>
			<Avatar
				src={avatarUrl || t('meeting_room.doctor_avatar_url')}
				alt='Doctor'
				sx={{ width: 96, height: 96 }}
			/>
			<Box sx={{ textAlign: 'center' }}>
				<Typography variant='h5' fontWeight={700}>
					{name}
				</Typography>
				<Typography color='text.secondary'>{specialty}</Typography>
			</Box>
			<Stack direction='row' alignItems='center' spacing={1}>
				<Circle color='success' sx={{ fontSize: 12 }} />
				<Typography color='text.secondary'>{t('meeting_room.message.waiting_for_doctor')}</Typography>
			</Stack>
		</>
	)
}

export default DoctorInfoHeader

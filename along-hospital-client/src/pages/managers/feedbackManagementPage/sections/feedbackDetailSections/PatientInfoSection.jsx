import useTranslation from '@/hooks/useTranslation'
import PersonIcon from '@mui/icons-material/Person'
import { Avatar, Box, Card, CardContent, Divider, Stack, Typography } from '@mui/material'

const PatientInfoSection = ({ feedback }) => {
	const { t } = useTranslation()

	return (
		<Card variant='outlined'>
			<CardContent>
				<Typography variant='subtitle1' fontWeight='bold' gutterBottom color='primary'>
					{t('feedback.title.patient_info')}
				</Typography>
				<Divider sx={{ mb: 2 }} />
				<Stack direction='row' spacing={2} alignItems='center'>
					<Avatar sx={{ bgcolor: 'primary.main' }}>
						<PersonIcon />
					</Avatar>
					<Box>
						<Typography variant='body1' fontWeight='medium'>
							{feedback.patientName}
						</Typography>
						<Typography variant='caption' color='text.secondary'>
							ID: {feedback.patientId}
						</Typography>
					</Box>
				</Stack>
			</CardContent>
		</Card>
	)
}

export default PatientInfoSection

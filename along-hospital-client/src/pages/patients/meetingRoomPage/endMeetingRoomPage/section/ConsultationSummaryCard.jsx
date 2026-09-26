import useTranslation from '@/hooks/useTranslation'
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline'
import { Box, Typography } from '@mui/material'

const ConsultationSummaryCard = () => {
	const { t } = useTranslation()
	return (
		<Box
			sx={{
				display: 'flex',
				flexDirection: 'column',
				alignItems: 'center',
				justifyContent: 'center',
				textAlign: 'center',
			}}
		>
			<CheckCircleOutlineIcon
				sx={{
					fontSize: 80,
					color: 'success.main',
					mb: 3,
				}}
			/>
			<Typography variant='h4' fontWeight={700} sx={{ mb: 2 }}>
				{t('meeting_room.thank_you_title')}
			</Typography>
			<Typography variant='body1' color='text.secondary' sx={{ mb: 1, maxWidth: 600 }}>
				{t('meeting_room.thank_you_message')}
			</Typography>
		</Box>
	)
}

export default ConsultationSummaryCard

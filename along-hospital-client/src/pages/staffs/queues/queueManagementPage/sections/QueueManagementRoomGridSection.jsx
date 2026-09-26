import useTranslation from '@/hooks/useTranslation'
import { Box, Grid, Paper, Typography } from '@mui/material'
import QueueManagementRoomCardSection from './QueueManagementRoomCardSection'

const QueueManagementRoomGridSection = ({ roomSnapshots }) => {
	const { t } = useTranslation()

	if (!roomSnapshots.length) {
		return (
			<Paper sx={{ p: 4, borderRadius: 3 }}>
				<Box textAlign='center'>
					<Typography variant='h6'>{t('queue.placeholder.no_room_result')}</Typography>
				</Box>
			</Paper>
		)
	}

	return (
		<Grid container spacing={2}>
			{roomSnapshots.map((snapshot) => (
				<Grid key={`${snapshot.roomId ?? 'reception'}`} size={{ xs: 12, sm: 6, md: 3 }}>
					<QueueManagementRoomCardSection snapshot={snapshot} />
				</Grid>
			))}
		</Grid>
	)
}

export default QueueManagementRoomGridSection

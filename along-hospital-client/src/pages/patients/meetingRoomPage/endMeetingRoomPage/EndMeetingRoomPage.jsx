import { Box, Container, Paper } from '@mui/material'
import ConsultationSummaryCard from './section/ConsultationSummaryCard'
import EndConsultationActions from './section/EndConsultationActions'
import SummaryNoticeText from './section/SummaryNoticeText'

const EndMeetingRoomPage = () => {
	return (
		<Box
			sx={{
				minHeight: '100vh',
				display: 'flex',
				alignItems: 'center',
				justifyContent: 'center',
				bgcolor: (t) => t.palette.background.default,
				py: { xs: 4, md: 6 },
			}}
		>
			<Container maxWidth='md'>
				<Paper variant='outlined' sx={{ p: { xs: 2.5, md: 3 }, borderRadius: 3 }}>
					<ConsultationSummaryCard />
					<EndConsultationActions />
					<SummaryNoticeText />
				</Paper>
			</Container>
		</Box>
	)
}

export default EndMeetingRoomPage

import { Box, Container } from '@mui/material'
import DoctorMeetingRoomTeleSessionSection from './sections/DoctorMeetingRoomTeleSessionSection'

const DoctorMeetingRoomPage = () => {
	return (
		<Box
			sx={{ py: { xs: 2, md: 3 }, px: { xs: 1, md: 0 }, bgcolor: (t) => t.palette.background.default }}
		>
			<Container maxWidth='lg'>
				<DoctorMeetingRoomTeleSessionSection />
			</Container>
		</Box>
	)
}

export default DoctorMeetingRoomPage

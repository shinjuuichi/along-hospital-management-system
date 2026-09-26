import { Box, Container, Paper, Stack } from '@mui/material'
import AppointmentInfo from './section/AppointmentInfo'
import DoctorInfoHeader from './section/DoctorInfoHeader'
import WaitingNote from './section/WaitingNote'

const WaitingRoomPage = () => {
	return (
		<Box
			sx={{
				py: { xs: 4, md: 8 },
				background: (theme) =>
					`linear-gradient(180deg, ${theme.palette.background.default} 0%, ${
						theme.palette.success.softBg || theme.palette.background.paper
					} 100%)`,
				minHeight: 'calc(100vh - 72px - 200px)',
			}}
		>
			<Container maxWidth='sm'>
				<Paper
					elevation={0}
					sx={{ p: { xs: 3, md: 4 }, borderRadius: 4, border: '1px solid', borderColor: 'divider' }}
				>
					<Stack alignItems='center' spacing={2.5}>
						<DoctorInfoHeader />
						<Box sx={{ alignSelf: 'stretch', mt: 2 }}>
							<AppointmentInfo />
						</Box>
						<WaitingNote />
					</Stack>
				</Paper>
			</Container>
		</Box>
	)
}

export default WaitingRoomPage

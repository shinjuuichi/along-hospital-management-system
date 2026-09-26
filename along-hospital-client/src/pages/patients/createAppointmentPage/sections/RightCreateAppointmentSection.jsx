import { Box } from '@mui/material'

const RightCreateAppointmentSection = () => {
	return (
		<Box sx={{ height: '100%', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
			<Box
				component='img'
				src='/appointment-banner.png'
				alt=''
				sx={{
					width: '100%',
					height: '100%',
					objectFit: 'cover',
					boxShadow: 2,
					bgcolor: (t) => t.palette.primary?.softBg || t.palette.background.paper,
				}}
			/>
		</Box>
	)
}

export default RightCreateAppointmentSection

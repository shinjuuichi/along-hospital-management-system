import { Stack, Typography } from '@mui/material'

const PatientInfoRow = ({ icon, value, spacing = 1.5, noWrap = false }) => (
	<Stack direction='row' spacing={spacing} alignItems='center' sx={{ color: 'text.secondary' }}>
		{icon}
		<Typography variant='body2' noWrap={noWrap}>
			{value}
		</Typography>
	</Stack>
)

export default PatientInfoRow

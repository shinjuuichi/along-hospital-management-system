import { CheckCircleOutline } from '@mui/icons-material'
import { Box, Typography } from '@mui/material'

const AuthSuccessState = ({ title, description }) => {
	return (
		<Box sx={{ textAlign: 'center', py: { xs: 3, sm: 4 } }}>
			<CheckCircleOutline
				sx={{
					fontSize: { xs: '3rem', sm: '4rem' },
					mb: 2,
					color: 'success.main',
				}}
			/>
			<Typography
				variant='h6'
				sx={{
					fontWeight: 600,
					mb: 1,
					fontSize: { xs: '1rem', sm: '1.25rem' },
				}}
			>
				{title}
			</Typography>
			<Typography
				variant='body2'
				color='text.secondary'
				sx={{ fontSize: { xs: '0.8rem', sm: '0.875rem' } }}
			>
				{description}
			</Typography>
		</Box>
	)
}

export default AuthSuccessState

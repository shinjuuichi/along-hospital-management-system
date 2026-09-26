import { Box, Typography } from '@mui/material'

const VerificationIdentifierSummary = ({ title, value, description = '' }) => {
	return (
		<Box
			sx={{
				p: { xs: 1.75, sm: 2 },
				borderRadius: 3,
				border: 1,
				borderColor: 'divider',
				bgcolor: 'grey.50',
			}}
		>
			<Typography variant='overline' color='text.secondary' sx={{ letterSpacing: 1 }}>
				{title}
			</Typography>
			<Typography
				variant='body1'
				sx={{
					fontWeight: 700,
					wordBreak: 'break-word',
				}}
			>
				{value}
			</Typography>
			{description ? (
				<Typography variant='body2' color='text.secondary' sx={{ mt: 0.5 }}>
					{description}
				</Typography>
			) : null}
		</Box>
	)
}

export default VerificationIdentifierSummary

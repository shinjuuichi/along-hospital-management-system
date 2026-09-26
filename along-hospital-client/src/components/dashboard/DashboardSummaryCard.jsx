import DashboardWidgetCard from './DashboardWidgetCard'
import { Box, Typography } from '@mui/material'

const DashboardSummaryCard = ({
	title,
	value,
	formatValue = (nextValue) => nextValue,
	loading = false,
	error = null,
	accent = '#1976d2',
}) => {
	return (
		<DashboardWidgetCard title={title} loading={loading} error={error} minHeight={164}>
			<Box
				sx={{
					height: '100%',
					display: 'flex',
					flexDirection: 'column',
					justifyContent: 'space-between',
					borderRadius: 2.5,
					p: 2,
					background: `linear-gradient(135deg, ${accent} 0%, ${accent}22 100%)`,
				}}
			>
				<Typography variant='body2' sx={{ color: 'text.secondary', fontWeight: 600 }}>
					{title}
				</Typography>
				<Typography
					variant='h4'
					sx={{
						fontWeight: 800,
						lineHeight: 1.1,
						wordBreak: 'break-word',
					}}
				>
					{formatValue(value)}
				</Typography>
			</Box>
		</DashboardWidgetCard>
	)
}

export default DashboardSummaryCard

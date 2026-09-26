import DashboardWidgetCard from './DashboardWidgetCard'
import { Box, Stack, Typography } from '@mui/material'
import { PieChart } from '@mui/x-charts'

const DashboardDistributionChartCard = ({
	title,
	subtitle,
	labels = [],
	data = [],
	loading = false,
	error = null,
	height = 320,
	valueFormatter = (value) => value,
}) => {
	const rows = labels.map((label, index) => ({
		id: `${label}-${index}`,
		label,
		value: data[index] || 0,
	}))
	const hasData = rows.some((row) => row.value > 0)

	return (
		<DashboardWidgetCard
			title={title}
			subtitle={subtitle}
			loading={loading}
			error={error}
			empty={!rows.length || !hasData}
			minHeight={height + 96}
		>
			<Stack direction={{ xs: 'column', lg: 'row' }} spacing={2} alignItems='center'>
				<Box sx={{ flex: 1, minWidth: 0 }}>
					<PieChart
						height={height}
						series={[
							{
								data: rows,
								valueFormatter: (item) => valueFormatter(item.value),
								innerRadius: 52,
								paddingAngle: 2,
							},
						]}
						margin={{ top: 16, bottom: 16, left: 16, right: 16 }}
					/>
				</Box>
				<Stack spacing={1} sx={{ width: { xs: '100%', lg: 220 } }}>
					{rows.map((row) => (
						<Stack
							key={row.id}
							direction='row'
							justifyContent='space-between'
							alignItems='center'
							sx={{
								px: 1.25,
								py: 0.9,
								borderRadius: 2,
								bgcolor: 'action.hover',
							}}
						>
							<Typography variant='body2'>{row.label}</Typography>
							<Typography variant='body2' sx={{ fontWeight: 700 }}>
								{valueFormatter(row.value)}
							</Typography>
						</Stack>
					))}
				</Stack>
			</Stack>
		</DashboardWidgetCard>
	)
}

export default DashboardDistributionChartCard

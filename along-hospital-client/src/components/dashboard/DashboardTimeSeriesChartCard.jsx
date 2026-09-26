import DashboardWidgetCard from './DashboardWidgetCard'
import { BarChart, LineChart } from '@mui/x-charts'

const DashboardTimeSeriesChartCard = ({
	title,
	subtitle,
	chartType = 'line',
	labels = [],
	datasets = [],
	loading = false,
	error = null,
	height = 320,
	valueFormatter = (value) => value,
}) => {
	const empty = !labels.length || !datasets.length
	const series = datasets.map((dataset) => ({
		data: dataset.data || [],
		label: dataset.label,
		valueFormatter,
	}))

	return (
		<DashboardWidgetCard
			title={title}
			subtitle={subtitle}
			loading={loading}
			error={error}
			empty={empty}
			minHeight={height + 96}
		>
			{chartType === 'bar' ? (
				<BarChart
					height={height}
					xAxis={[{ data: labels, scaleType: 'band' }]}
					series={series}
					margin={{ top: 24, bottom: 40, left: 64, right: 20 }}
				/>
			) : (
				<LineChart
					height={height}
					xAxis={[{ data: labels, scaleType: 'point' }]}
					series={series}
					margin={{ top: 24, bottom: 40, left: 64, right: 20 }}
				/>
			)}
		</DashboardWidgetCard>
	)
}

export default DashboardTimeSeriesChartCard

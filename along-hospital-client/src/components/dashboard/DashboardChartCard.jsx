import useTranslation from '@/hooks/useTranslation'
import { Box, Typography } from '@mui/material'
import { BarChart, Gauge, LineChart, PieChart, RadarChart, SparkLineChart } from '@mui/x-charts'
import DashboardWidgetCard from './DashboardWidgetCard'

const DEFAULT_MARGIN = { top: 24, bottom: 40, left: 56, right: 20 }
const PIE_MARGIN = { top: 16, bottom: 16, left: 16, right: 16 }

const DashboardChartCard = ({
	widget,
	title,
	subtitle,
	loading = false,
	error = null,
	height = 320,
	formatValue = (value) => value,
	colors = [],
	showLegend = false,
	translateSegmentKey = (key) => key,
	translateMetricKey = (key) => key,
	resolveSeriesLabel = (label) => label,
}) => {
	const { t } = useTranslation()

	if (!widget?.chartKind) return null

	const empty = widget.chartKind === 'pie' ? false : isChartEmpty(widget)
	const minHeight = getCardMinHeight(widget.chartKind, height, showLegend)

	return (
		<DashboardWidgetCard
			title={title}
			subtitle={subtitle}
			loading={loading}
			error={error}
			empty={empty}
			minHeight={minHeight}
		>
			{renderChart({
				widget,
				height,
				formatValue,
				colors,
				showLegend,
				translateSegmentKey,
				translateMetricKey,
				resolveSeriesLabel,
				t,
			})}
		</DashboardWidgetCard>
	)
}

function renderChart({
	widget,
	height,
	formatValue,
	colors,
	showLegend,
	translateSegmentKey,
	translateMetricKey,
	resolveSeriesLabel,
	t,
}) {
	switch (widget.chartKind) {
		case 'gauge':
			return renderGaugeChart(widget, height, formatValue, colors)
		case 'sparkline':
			return renderSparklineChart(widget, height, formatValue, colors)
		case 'line':
			return renderLineChart(widget, height, formatValue, colors, showLegend, resolveSeriesLabel)
		case 'bar':
			return renderBarChart(widget, height, formatValue, colors, showLegend, resolveSeriesLabel)
		case 'pie':
			return renderPieChart(widget, height, formatValue, colors, showLegend, translateSegmentKey, t)
		case 'radar':
			return renderRadarChart(widget, height, colors, showLegend, translateMetricKey, resolveSeriesLabel)
		default:
			return null
	}
}

function renderGaugeChart(widget, height, formatValue, colors) {
	return (
		<Gauge
			height={height}
			value={widget.value ?? null}
			valueMin={widget.min ?? 0}
			valueMax={widget.max ?? 100}
			startAngle={widget.gaugeOptions?.startAngle ?? 0}
			endAngle={widget.gaugeOptions?.endAngle ?? 360}
			innerRadius={widget.gaugeOptions?.innerRadius ?? '80%'}
			outerRadius={widget.gaugeOptions?.outerRadius ?? '100%'}
			text={({ value }) => (value == null ? null : formatValue(value))}
			sx={{
				'& .MuiGauge-valueArc': {
					fill: colors[0] ?? '#1976d2',
				},
				'& .MuiGauge-referenceArc': {
					fill: 'rgba(15, 23, 42, 0.12)',
				},
				'& .MuiGauge-valueText': {
					fontSize: 28,
					fontWeight: 700,
					fill: '#0f172a',
				},
			}}
		/>
	)
}

function renderSparklineChart(widget, height, formatValue, colors) {
	const sparklineSeries = widget.series?.[0]

	return (
		<Box sx={{ flexGrow: 1 }}>
			<SparkLineChart
				data={sparklineSeries?.data ?? []}
				plotType={widget.plotType === 'bar' ? 'bar' : 'line'}
				height={height}
				color={colors[0]}
				showTooltip
				showHighlight
				valueFormatter={(value) => formatValue(value ?? 0)}
			/>
		</Box>
	)
}

function renderLineChart(widget, height, formatValue, colors, showLegend, resolveSeriesLabel) {
	return (
		<LineChart
			height={height}
			xAxis={[{ scaleType: 'point', data: widget.xLabels ?? [], height: 28 }]}
			yAxis={[{ width: 50 }]}
			series={createCartesianSeries(widget.series, formatValue, resolveSeriesLabel)}
			margin={DEFAULT_MARGIN}
			grid={{ horizontal: true }}
			colors={colors}
			hideLegend={!showLegend}
		/>
	)
}

function renderBarChart(widget, height, formatValue, colors, showLegend, resolveSeriesLabel) {
	return (
		<BarChart
			height={height}
			xAxis={[{ scaleType: 'band', data: widget.xLabels ?? [] }]}
			yAxis={[{ width: 50 }]}
			series={createCartesianSeries(widget.series, formatValue, resolveSeriesLabel)}
			margin={DEFAULT_MARGIN}
			grid={{ horizontal: true }}
			colors={colors}
			hideLegend={!showLegend}
		/>
	)
}

function renderPieChart(widget, height, formatValue, colors, showLegend, translateSegmentKey, t) {
	const hasData = hasPieChartData(widget)
	const data = hasData
		? (widget.segments ?? []).map((segment, index) => ({
				id: `${segment.key}-${index}`,
				label: translateSegmentKey(segment.key),
				value: segment.value ?? 0,
			}))
		: [{ id: '__placeholder__', label: '', value: 1 }]

	return (
		<Box sx={{ position: 'relative', width: '100%' }}>
			<PieChart
				height={height}
				series={[
					{
						data,
						innerRadius: 52,
						paddingAngle: hasData ? 2 : 0,
						valueFormatter: (item) =>
							item.id === '__placeholder__' ? formatValue(0) : formatValue(item.value ?? 0),
					},
				]}
				margin={PIE_MARGIN}
				colors={hasData ? colors : ['rgba(148, 163, 184, 0.45)']}
				hideLegend={hasData ? !showLegend : true}
			/>
			{!hasData ? (
				<Box
					sx={{
						position: 'absolute',
						inset: 0,
						display: 'flex',
						flexDirection: 'column',
						alignItems: 'center',
						justifyContent: 'center',
						pointerEvents: 'none',
					}}
				>
					<Typography variant='h4' sx={{ fontWeight: 800, color: 'text.primary', lineHeight: 1 }}>
						0
					</Typography>
					<Typography variant='body2' color='text.secondary'>
						{t('text.none')}
					</Typography>
				</Box>
			) : null}
		</Box>
	)
}

function renderRadarChart(widget, height, colors, showLegend, translateMetricKey, resolveSeriesLabel) {
	const radarProps = {
		height,
		series: (widget.series ?? []).map((dataset) => ({
			data: dataset?.data ?? [],
			label: dataset?.label ? resolveSeriesLabel(dataset.label) : undefined,
			fillArea: true,
		})),
		radar: {
			metrics: widget.metrics ?? [],
			labelFormatter: (metric) => translateMetricKey(metric),
		},
		colors,
		hideLegend: !showLegend,
	}

	if (widget.max != null) {
		radarProps.radar.max = widget.max
	}

	return <RadarChart {...radarProps} />
}

function createCartesianSeries(series, formatValue, resolveSeriesLabel) {
	return (series ?? []).map((dataset) => ({
		data: dataset?.data ?? [],
		label: dataset?.label ? resolveSeriesLabel(dataset.label) : undefined,
		valueFormatter: (value) => formatValue(value ?? 0),
	}))
}

function hasPieChartData(widget) {
	return !!widget?.segments?.length && widget.segments.some((segment) => Number(segment?.value ?? 0) > 0)
}

function isChartEmpty(widget) {
	switch (widget?.chartKind) {
		case 'gauge':
			return widget.value == null || Number.isNaN(Number(widget.value))
		case 'sparkline':
			return !widget.series?.[0]?.data?.length
		case 'line':
		case 'bar':
			return !widget.xLabels?.length || !(widget.series ?? []).some((dataset) => dataset?.data?.length)
		case 'pie':
			return !hasPieChartData(widget)
		case 'radar':
			return !widget.metrics?.length || !(widget.series ?? []).some((dataset) => dataset?.data?.length)
		default:
			return true
	}
}

function getCardMinHeight(chartKind, height, showLegend) {
	switch (chartKind) {
		case 'sparkline':
			return 216
		case 'gauge':
			return height + 80
		case 'pie':
		case 'radar':
			return height + (showLegend ? 132 : 96)
		case 'line':
		case 'bar':
		default:
			return height + (showLegend ? 116 : 96)
	}
}

export default DashboardChartCard

import DashboardChartCard from './DashboardChartCard'
import DashboardSummaryCard from './DashboardSummaryCard'

const DashboardWidgetRenderer = ({ widget, meta, loading = false, error = null }) => {
	if (!widget || !meta) return null

	switch (widget.kind) {
		case 'summary':
			return (
				<DashboardSummaryCard
					title={meta.title}
					value={widget.value}
					formatValue={meta.formatValue}
					loading={loading}
					error={error}
					accent={meta.accent}
				/>
			)
		case 'chart':
			return (
				<DashboardChartCard
					widget={widget}
					title={meta.title}
					subtitle={meta.subtitle}
					loading={loading}
					error={error}
					height={meta.height}
					formatValue={meta.formatValue}
					colors={meta.colors}
					showLegend={meta.showLegend}
					translateSegmentKey={meta.translateSegmentKey}
					translateMetricKey={meta.translateMetricKey}
					resolveSeriesLabel={meta.resolveSeriesLabel}
				/>
			)
		default:
			return null
	}
}

export default DashboardWidgetRenderer

import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
	formatNumberWithCommas,
} from '@/utils/formatNumberUtil'

const DEFAULT_COLORS = ['#0f766e', '#0369a1', '#f97316', '#16a34a', '#dc2626']
const SUMMARY_GRID = { xs: 12, md: 6, xl: 4 }
const CHART_GRID = { xs: 12, xl: 6 }

export const createSummaryWidgetItem = ({
	key,
	title,
	value,
	format = 'number',
	accent = '#1976d2',
	gridSize = SUMMARY_GRID,
}) => ({
	gridSize,
	widget: {
		widgetKey: key,
		kind: 'summary',
		value,
	},
	meta: {
		title,
		accent,
		formatValue: getValueFormatter(format),
	},
})

export const createChartWidgetItemFromChart = ({
	key,
	title,
	chartKind,
	chart,
	format = 'number',
	colors = DEFAULT_COLORS,
	showLegend,
	gridSize = CHART_GRID,
}) => ({
	gridSize,
	widget: {
		widgetKey: key,
		kind: 'chart',
		chartKind,
		xLabels: chart?.labels ?? [],
		series: (chart?.datasets ?? []).map((dataset) => ({
			label: dataset?.label || undefined,
			data: dataset?.data ?? [],
		})),
	},
	meta: {
		title,
		colors,
		showLegend: showLegend ?? ((chart?.datasets?.length ?? 0) > 1),
		height: chartKind === 'bar' || chartKind === 'line' ? 320 : 340,
		formatValue: getValueFormatter(format),
		resolveSeriesLabel: (label) => humanizeLabel(label),
	},
})

export const createChartWidgetItemFromDistribution = ({
	key,
	title,
	distribution,
	format = 'number',
	colors = DEFAULT_COLORS,
	gridSize = CHART_GRID,
}) => ({
	gridSize,
	widget: {
		widgetKey: key,
		kind: 'chart',
		chartKind: 'pie',
		segments: (distribution?.labels ?? []).map((label, index) => ({
			key: label,
			value: distribution?.data?.[index] ?? 0,
		})),
	},
	meta: {
		title,
		colors,
		showLegend: true,
		height: 340,
		formatValue: getValueFormatter(format),
		translateSegmentKey: (label) => humanizeLabel(label),
	},
})

function getValueFormatter(format) {
	switch (format) {
		case 'currency':
			return formatCurrencyBasedOnCurrentLanguage
		case 'percent':
			return formatPercentValue
		case 'number':
		default:
			return (value) => formatNumberWithCommas(value ?? 0)
	}
}

function formatPercentValue(value) {
	const numericValue = Number(value ?? 0)
	if (Number.isNaN(numericValue)) return '0%'
	if (numericValue > 1) return `${formatNumberWithCommas(numericValue)}%`
	return formatNumberToPercent(numericValue)
}

function humanizeLabel(value) {
	return String(value ?? '')
		.replace(/([a-z0-9])([A-Z])/g, '$1 $2')
		.replace(/[_-]+/g, ' ')
		.replace(/\s+/g, ' ')
		.trim()
		.replace(/\b\w/g, (char) => char.toUpperCase())
}

import {
	createChartWidgetItemFromChart,
	createChartWidgetItemFromDistribution,
	createSummaryWidgetItem,
} from '@/components/dashboard/dashboardWidgetFactory'

export const buildPharmacistDashboardTabWidgets = (statistics, activeTab, t) => {
	const overview = statistics?.overview
	const order = statistics?.order

	const tabWidgets = {
		overview: [
			createSummaryWidgetItem({
				key: 'overviewTotalOrders',
				title: t('statistic_dashboard_pharmacist.widget.total_orders'),
				value: overview?.totalOrders,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'overviewPendingOrders',
				title: t('statistic_dashboard_pharmacist.widget.pending_orders'),
				value: overview?.pendingOrders,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'overviewCompletedOrders',
				title: t('statistic_dashboard_pharmacist.widget.completed_orders'),
				value: overview?.completedOrders,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'overviewRevenue',
				title: t('statistic_dashboard_pharmacist.widget.revenue'),
				value: overview?.revenue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createChartWidgetItemFromChart({
				key: 'overviewRevenueOverTime',
				title: t('statistic_dashboard_pharmacist.widget.revenue_over_time'),
				chartKind: 'line',
				chart: order?.revenueOverTime,
				format: 'currency',
				colors: ['#0f766e', '#14b8a6'],
			}),
			createChartWidgetItemFromChart({
				key: 'overviewOrdersOverTime',
				title: t('statistic_dashboard_pharmacist.widget.orders_over_time'),
				chartKind: 'bar',
				chart: order?.ordersOverTime,
				colors: ['#0369a1', '#38bdf8'],
			}),
			createChartWidgetItemFromDistribution({
				key: 'overviewOrderStatus',
				title: t('statistic_dashboard_pharmacist.widget.order_status'),
				distribution: order?.orderStatus,
				colors: ['#f59e0b', '#0369a1', '#0ea5e9', '#16a34a', '#dc2626'],
			}),
		],
		order: [
			createSummaryWidgetItem({
				key: 'orderRevenue',
				title: t('statistic_dashboard_pharmacist.widget.revenue'),
				value: order?.revenue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createSummaryWidgetItem({
				key: 'orderOrders',
				title: t('statistic_dashboard_pharmacist.widget.total_orders'),
				value: order?.orders,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'orderPendingOrders',
				title: t('statistic_dashboard_pharmacist.widget.pending_orders'),
				value: order?.pendingOrders,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'orderCompletedOrders',
				title: t('statistic_dashboard_pharmacist.widget.completed_orders'),
				value: order?.completedOrders,
				accent: '#16a34a',
			}),
			createChartWidgetItemFromChart({
				key: 'orderRevenueOverTime',
				title: t('statistic_dashboard_pharmacist.widget.revenue_over_time'),
				chartKind: 'line',
				chart: order?.revenueOverTime,
				format: 'currency',
				colors: ['#0f766e', '#14b8a6'],
			}),
			createChartWidgetItemFromChart({
				key: 'orderOrdersOverTime',
				title: t('statistic_dashboard_pharmacist.widget.orders_over_time'),
				chartKind: 'bar',
				chart: order?.ordersOverTime,
				colors: ['#0369a1', '#38bdf8'],
			}),
			createChartWidgetItemFromDistribution({
				key: 'orderOrderStatus',
				title: t('statistic_dashboard_pharmacist.widget.order_status'),
				distribution: order?.orderStatus,
				colors: ['#f59e0b', '#0369a1', '#0ea5e9', '#16a34a', '#dc2626'],
			}),
		],
		topMedicines: [],
	}

	return tabWidgets[activeTab] ?? tabWidgets.overview
}

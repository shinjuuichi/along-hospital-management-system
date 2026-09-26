import {
	createChartWidgetItemFromChart,
	createChartWidgetItemFromDistribution,
	createSummaryWidgetItem,
} from '@/components/dashboard/dashboardWidgetFactory'

export const buildManagerDashboardTabWidgets = (statistics, activeTab, t) => {
	const overview = statistics?.overview
	const order = statistics?.order
	const medicalHistory = statistics?.medicalHistory
	const appointment = statistics?.appointment
	const userGrowth = statistics?.userGrowth

	const tabWidgets = {
		overview: [
			createSummaryWidgetItem({
				key: 'overviewOrderRevenue',
				title: t('statistic_dashboard_manager.widget.order_revenue'),
				value: overview?.orderRevenue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createSummaryWidgetItem({
				key: 'overviewNewPatients',
				title: t('statistic_dashboard_manager.widget.new_patients'),
				value: overview?.newPatients,
				accent: '#7c3aed',
			}),
			createSummaryWidgetItem({
				key: 'overviewNewStaff',
				title: t('statistic_dashboard_manager.widget.new_staff'),
				value: overview?.newStaff,
				accent: '#ea580c',
			}),
			createChartWidgetItemFromChart({
				key: 'overviewRevenueOverTime',
				title: t('statistic_dashboard_manager.widget.revenue_over_time'),
				chartKind: 'line',
				chart: order?.revenueOverTime,
				format: 'currency',
				colors: ['#0f766e', '#14b8a6'],
			}),
			createChartWidgetItemFromChart({
				key: 'overviewNewUsersOverTime',
				title: t('statistic_dashboard_manager.widget.new_users_over_time'),
				chartKind: 'bar',
				chart: userGrowth?.newUsersOverTime,
				colors: ['#7c3aed', '#22c55e'],
				showLegend: true,
			}),
			createChartWidgetItemFromDistribution({
				key: 'overviewOrderStatus',
				title: t('statistic_dashboard_manager.widget.order_status'),
				distribution: order?.orderStatus,
				colors: ['#0369a1', '#0ea5e9', '#f97316', '#16a34a', '#dc2626'],
			}),
		],
		commercial: [
			createSummaryWidgetItem({
				key: 'commercialOrders',
				title: t('statistic_dashboard_manager.widget.orders'),
				value: order?.orders,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'commercialRevenue',
				title: t('statistic_dashboard_manager.widget.revenue'),
				value: order?.revenue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createChartWidgetItemFromChart({
				key: 'commercialOrdersOverTime',
				title: t('statistic_dashboard_manager.widget.orders_over_time'),
				chartKind: 'bar',
				chart: order?.ordersOverTime,
				colors: ['#0369a1', '#38bdf8'],
			}),
		],
		clinical: [
			createSummaryWidgetItem({
				key: 'clinicalMedicalHistories',
				title: t('statistic_dashboard_manager.widget.total_medical_histories'),
				value: medicalHistory?.totalMedicalHistories,
				accent: '#7c3aed',
			}),
			createSummaryWidgetItem({
				key: 'clinicalAppointments',
				title: t('statistic_dashboard_manager.widget.appointments'),
				value: appointment?.appointments,
				accent: '#0f766e',
			}),
			createChartWidgetItemFromChart({
				key: 'clinicalAppointmentsOverTime',
				title: t('statistic_dashboard_manager.widget.appointments_over_time'),
				chartKind: 'bar',
				chart: appointment?.appointmentsOverTime,
				colors: ['#0369a1', '#60a5fa'],
			}),
			createChartWidgetItemFromDistribution({
				key: 'clinicalAppointmentStatus',
				title: t('statistic_dashboard_manager.widget.appointment_status'),
				distribution: appointment?.appointmentStatus,
				colors: ['#f59e0b', '#16a34a', '#dc2626'],
			}),
		],
	}

	return tabWidgets[activeTab] ?? tabWidgets.overview
}

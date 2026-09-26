import {
	createChartWidgetItemFromChart,
	createChartWidgetItemFromDistribution,
	createSummaryWidgetItem,
} from '@/components/dashboard/dashboardWidgetFactory'

export const buildAccountantDashboardTabWidgets = (statistics, activeTab, t) => {
	const overview = statistics?.overview
	const invoice = statistics?.invoice
	const collection = statistics?.collection
	const refund = statistics?.refund

	const tabWidgets = {
		overview: [
			createSummaryWidgetItem({
				key: 'accountantOverviewTotalInvoices',
				title: t('statistic_dashboard_accountant.widget.total_invoices'),
				value: overview?.totalInvoices,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'accountantOverviewPendingInvoices',
				title: t('statistic_dashboard_accountant.widget.pending_invoices'),
				value: invoice?.pendingInvoices,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'accountantOverviewCompletedInvoices',
				title: t('statistic_dashboard_accountant.widget.completed_invoices'),
				value: invoice?.completedInvoices,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'accountantOverviewNetCollectedAmount',
				title: t('statistic_dashboard_accountant.widget.net_collected_amount'),
				value: overview?.netCollectedAmount,
				format: 'currency',
				accent: '#0f766e',
			}),
			createSummaryWidgetItem({
				key: 'accountantOverviewRefundAmount',
				title: t('statistic_dashboard_accountant.widget.refund_amount'),
				value: overview?.refundAmount,
				format: 'currency',
				accent: '#ea580c',
			}),
			createSummaryWidgetItem({
				key: 'accountantOverviewPendingRefunds',
				title: t('statistic_dashboard_accountant.widget.pending_refunds'),
				value: overview?.pendingRefunds,
				accent: '#dc2626',
			}),
			createChartWidgetItemFromDistribution({
				key: 'accountantOverviewInvoiceStatus',
				title: t('statistic_dashboard_accountant.widget.invoice_status'),
				distribution: invoice?.invoiceStatus,
				colors: ['#0369a1', '#16a34a', '#dc2626'],
			}),
			createChartWidgetItemFromChart({
				key: 'accountantOverviewCollectionsOverTime',
				title: t('statistic_dashboard_accountant.widget.collections_over_time'),
				chartKind: 'line',
				chart: collection?.collectionsOverTime,
				format: 'currency',
				colors: ['#0f766e', '#14b8a6'],
			}),
			createChartWidgetItemFromChart({
				key: 'accountantOverviewRefundsOverTime',
				title: t('statistic_dashboard_accountant.widget.refunds_over_time'),
				chartKind: 'line',
				chart: refund?.refundsOverTime,
				format: 'currency',
				colors: ['#ea580c', '#fb7185'],
			}),
		],
		invoices: [
			createSummaryWidgetItem({
				key: 'accountantInvoiceTotalInvoices',
				title: t('statistic_dashboard_accountant.widget.total_invoices'),
				value: invoice?.totalInvoices,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'accountantInvoiceGrossInvoiceAmount',
				title: t('statistic_dashboard_accountant.widget.gross_invoice_amount'),
				value: invoice?.grossInvoiceAmount,
				format: 'currency',
				accent: '#0f766e',
			}),
			createSummaryWidgetItem({
				key: 'accountantInvoicePendingInvoices',
				title: t('statistic_dashboard_accountant.widget.pending_invoices'),
				value: invoice?.pendingInvoices,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'accountantInvoiceCompletedInvoices',
				title: t('statistic_dashboard_accountant.widget.completed_invoices'),
				value: invoice?.completedInvoices,
				accent: '#16a34a',
			}),
			createChartWidgetItemFromChart({
				key: 'accountantInvoicesOverTime',
				title: t('statistic_dashboard_accountant.widget.invoices_over_time'),
				chartKind: 'bar',
				chart: invoice?.invoicesOverTime,
				colors: ['#0369a1', '#60a5fa'],
			}),
			createChartWidgetItemFromDistribution({
				key: 'accountantInvoicesStatus',
				title: t('statistic_dashboard_accountant.widget.invoice_status'),
				distribution: invoice?.invoiceStatus,
				colors: ['#0369a1', '#16a34a', '#dc2626'],
			}),
		],
		refunds: [
			createSummaryWidgetItem({
				key: 'accountantRefundAmount',
				title: t('statistic_dashboard_accountant.widget.refund_amount'),
				value: refund?.refundAmount,
				format: 'currency',
				accent: '#ea580c',
			}),
			createSummaryWidgetItem({
				key: 'accountantPendingRefunds',
				title: t('statistic_dashboard_accountant.widget.pending_refunds'),
				value: refund?.pendingRefunds,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'accountantApprovedRefunds',
				title: t('statistic_dashboard_accountant.widget.approved_refunds'),
				value: refund?.approvedRefunds,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'accountantCancelledRefunds',
				title: t('statistic_dashboard_accountant.widget.cancelled_refunds'),
				value: refund?.cancelledRefunds,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'accountantCollectedAmount',
				title: t('statistic_dashboard_accountant.widget.collected_amount'),
				value: collection?.collectedAmount,
				format: 'currency',
				accent: '#0f766e',
			}),
			createSummaryWidgetItem({
				key: 'accountantNetCollectedAmount',
				title: t('statistic_dashboard_accountant.widget.net_collected_amount'),
				value: collection?.netCollectedAmount,
				format: 'currency',
				accent: '#0369a1',
			}),
			createChartWidgetItemFromDistribution({
				key: 'accountantRefundStatus',
				title: t('statistic_dashboard_accountant.widget.refund_status'),
				distribution: refund?.refundStatus,
				colors: ['#f59e0b', '#16a34a', '#dc2626'],
			}),
			createChartWidgetItemFromChart({
				key: 'accountantRefundsOverTime',
				title: t('statistic_dashboard_accountant.widget.refunds_over_time'),
				chartKind: 'line',
				chart: refund?.refundsOverTime,
				format: 'currency',
				colors: ['#ea580c', '#fb7185'],
			}),
			createChartWidgetItemFromChart({
				key: 'accountantCollectionsOverTime',
				title: t('statistic_dashboard_accountant.widget.collections_over_time'),
				chartKind: 'line',
				chart: collection?.collectionsOverTime,
				format: 'currency',
				colors: ['#0f766e', '#14b8a6'],
			}),
		],
	}

	return tabWidgets[activeTab] ?? tabWidgets.overview
}

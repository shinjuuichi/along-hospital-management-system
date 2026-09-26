import {
	createChartWidgetItemFromChart,
	createChartWidgetItemFromDistribution,
	createSummaryWidgetItem,
} from '@/components/dashboard/dashboardWidgetFactory'

export const buildInventoryClerkDashboardTabWidgets = (statistics, activeTab, t) => {
	const overview = statistics?.overview
	const inventory = statistics?.inventory
	const supplier = statistics?.supplier
	const importStats = statistics?.import

	const tabWidgets = {
		overview: [
			createSummaryWidgetItem({
				key: 'overviewTotalInventoryItems',
				title: t('statistic_dashboard_inventory_clerk.widget.total_inventory_items'),
				value: overview?.totalInventoryItems,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'overviewLowStockItems',
				title: t('statistic_dashboard_inventory_clerk.widget.low_stock_items'),
				value: overview?.lowStockItems,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'overviewOutOfStockItems',
				title: t('statistic_dashboard_inventory_clerk.widget.out_of_stock_items'),
				value: overview?.outOfStockItems,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'overviewTotalSuppliers',
				title: t('statistic_dashboard_inventory_clerk.widget.total_suppliers'),
				value: overview?.totalSuppliers,
				accent: '#7c3aed',
			}),
			createSummaryWidgetItem({
				key: 'overviewTotalImports',
				title: t('statistic_dashboard_inventory_clerk.widget.total_imports'),
				value: overview?.totalImports,
				accent: '#0891b2',
			}),
			createSummaryWidgetItem({
				key: 'overviewTotalImportValue',
				title: t('statistic_dashboard_inventory_clerk.widget.total_import_value'),
				value: overview?.totalImportValue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createChartWidgetItemFromDistribution({
				key: 'overviewStockStatus',
				title: t('statistic_dashboard_inventory_clerk.widget.stock_status'),
				distribution: inventory?.stockStatus,
				colors: ['#dc2626', '#f59e0b', '#16a34a'],
			}),
			createChartWidgetItemFromChart({
				key: 'overviewInventoryOverTime',
				title: t('statistic_dashboard_inventory_clerk.widget.inventory_over_time'),
				chartKind: 'line',
				chart: inventory?.inventoryOverTime,
				colors: ['#0369a1', '#38bdf8'],
			}),
		],
		inventory: [
			createSummaryWidgetItem({
				key: 'inventoryTotalInventoryItems',
				title: t('statistic_dashboard_inventory_clerk.widget.total_inventory_items'),
				value: inventory?.totalInventoryItems,
				accent: '#0369a1',
			}),
			createSummaryWidgetItem({
				key: 'inventoryLowStockItems',
				title: t('statistic_dashboard_inventory_clerk.widget.low_stock_items'),
				value: inventory?.lowStockItems,
				accent: '#f59e0b',
			}),
			createSummaryWidgetItem({
				key: 'inventoryOutOfStockItems',
				title: t('statistic_dashboard_inventory_clerk.widget.out_of_stock_items'),
				value: inventory?.outOfStockItems,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'inventoryTotalQuantity',
				title: t('statistic_dashboard_inventory_clerk.widget.total_quantity'),
				value: inventory?.totalQuantity,
				accent: '#2563eb',
			}),
			createChartWidgetItemFromDistribution({
				key: 'inventoryStockStatus',
				title: t('statistic_dashboard_inventory_clerk.widget.stock_status'),
				distribution: inventory?.stockStatus,
				colors: ['#dc2626', '#f59e0b', '#16a34a'],
			}),
			createChartWidgetItemFromChart({
				key: 'inventoryOverTime',
				title: t('statistic_dashboard_inventory_clerk.widget.inventory_over_time'),
				chartKind: 'line',
				chart: inventory?.inventoryOverTime,
				colors: ['#0369a1', '#38bdf8'],
			}),
		],
		supplier: [
			createSummaryWidgetItem({
				key: 'supplierTotalSuppliers',
				title: t('statistic_dashboard_inventory_clerk.widget.total_suppliers'),
				value: supplier?.totalSuppliers,
				accent: '#7c3aed',
			}),
			createChartWidgetItemFromChart({
				key: 'suppliersOverTime',
				title: t('statistic_dashboard_inventory_clerk.widget.suppliers_over_time'),
				chartKind: 'line',
				chart: supplier?.suppliersOverTime,
				colors: ['#7c3aed', '#a855f7'],
			}),
		],
		import: [
			createSummaryWidgetItem({
				key: 'importTotalImports',
				title: t('statistic_dashboard_inventory_clerk.widget.total_imports'),
				value: importStats?.totalImports,
				accent: '#0891b2',
			}),
			createSummaryWidgetItem({
				key: 'importTotalImportValue',
				title: t('statistic_dashboard_inventory_clerk.widget.total_import_value'),
				value: importStats?.totalImportValue,
				format: 'currency',
				accent: '#0f766e',
			}),
			createChartWidgetItemFromChart({
				key: 'importsOverTime',
				title: t('statistic_dashboard_inventory_clerk.widget.imports_over_time'),
				chartKind: 'line',
				chart: importStats?.importsOverTime,
				colors: ['#0891b2', '#22d3ee'],
			}),
			createChartWidgetItemFromDistribution({
				key: 'importBySupplier',
				title: t('statistic_dashboard_inventory_clerk.widget.import_by_supplier'),
				distribution: importStats?.importBySupplier,
				colors: ['#0369a1', '#0ea5e9', '#f97316', '#16a34a', '#dc2626'],
			}),
		],
	}

	return tabWidgets[activeTab] ?? tabWidgets.overview
}

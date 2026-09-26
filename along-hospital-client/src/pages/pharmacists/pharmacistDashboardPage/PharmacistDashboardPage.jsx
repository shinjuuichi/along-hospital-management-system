import ExportButton from '@/components/buttons/ExportButton'
import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import DashboardFilterDialog from '@/components/dashboard/DashboardFilterDialog'
import DashboardWidgetCard from '@/components/dashboard/DashboardWidgetCard'
import DashboardWidgetRenderer from '@/components/dashboard/DashboardWidgetRenderer'
import GenericTabs from '@/components/generals/GenericTabs'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDashboardRangeLabel, getDefaultDashboardFilter } from '@/utils/dashboardUtils'
import { downloadFile } from '@/utils/downloadFile'
import { Chip, Grid, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import { buildPharmacistDashboardTabWidgets } from './helpers/pharmacistDashboardTabWidgets'

const PharmacistDashboardPage = () => {
	const { t } = useTranslation()
	const [filterDialogOpen, setFilterDialogOpen] = useState(false)
	const [filters, setFilters] = useState(getDefaultDashboardFilter())
	const [exportLoading, setExportLoading] = useState(false)

	const handleExport = async () => {
		setExportLoading(true)
		try {
			await downloadFile(ApiUrls.REPORT.PHARMACIST_DASHBOARD_EXPORT, {
				FromDate: filters.fromDate,
				ToDate: filters.toDate,
			})
		} finally {
			setExportLoading(false)
		}
	}

	const tabs = useMemo(
		() => [
			{
				key: 'overview',
				title: t('statistic_dashboard_pharmacist.tab.overview'),
			},
			{
				key: 'order',
				title: t('statistic_dashboard_pharmacist.tab.order'),
			},
			{
				key: 'topMedicines',
				title: t('statistic_dashboard_pharmacist.tab.top_medicines'),
			},
		],
		[t]
	)
	const [currentTab, setCurrentTab] = useState(tabs[0])

	const statisticsQuery = useFetch(
		ApiUrls.REPORT.PHARMACIST_DASHBOARD_STATISTICS,
		{
			fromDate: filters.fromDate,
			toDate: filters.toDate,
		},
		[filters.fromDate, filters.toDate]
	)

	const widgets = useMemo(
		() => buildPharmacistDashboardTabWidgets(statisticsQuery.data, currentTab?.key, t),
		[currentTab?.key, statisticsQuery.data, t]
	)

	const topMedicinesItems = statisticsQuery.data?.topMedicines?.items ?? []

	const renderTopMedicinesTable = () => (
		<DashboardWidgetCard
			title={t('statistic_dashboard_pharmacist.widget.top_medicines_title')}
			loading={statisticsQuery.loading}
			error={statisticsQuery.error}
			empty={!topMedicinesItems.length}
			minHeight={320}
		>
			<Stack spacing={0}>
				<Stack
					direction='row'
					sx={{
						px: 2,
						py: 1.5,
						bgcolor: 'action.hover',
						borderRadius: 1,
					}}
				>
					<Typography variant='body2' sx={{ fontWeight: 700, flex: 1 }}>
						{t('statistic_dashboard_pharmacist.widget.sku_code')}
					</Typography>
					<Typography variant='body2' sx={{ fontWeight: 700, flex: 3 }}>
						{t('statistic_dashboard_pharmacist.widget.medicine_name')}
					</Typography>
					<Typography variant='body2' sx={{ fontWeight: 700, flex: 1, textAlign: 'right' }}>
						{t('statistic_dashboard_pharmacist.widget.quantity_sold')}
					</Typography>
				</Stack>
				{topMedicinesItems.map((item, index) => (
					<Stack
						key={item.skuCode ?? index}
						direction='row'
						sx={{
							px: 2,
							py: 1.5,
							borderBottom: '1px solid',
							borderColor: 'divider',
							'&:last-child': { borderBottom: 'none' },
						}}
					>
						<Typography variant='body2' sx={{ flex: 1 }}>
							{item.skuCode}
						</Typography>
						<Typography variant='body2' sx={{ flex: 3 }}>
							{item.medicineName}
						</Typography>
						<Typography variant='body2' sx={{ flex: 1, textAlign: 'right', fontWeight: 700 }}>
							{item.quantitySold}
						</Typography>
					</Stack>
				))}
			</Stack>
		</DashboardWidgetCard>
	)

	return (
		<>
			<Stack spacing={3}>
				<Stack
					spacing={2}
					sx={{
						p: 3,
						borderRadius: 4,
						background:
							'linear-gradient(135deg, rgba(8,145,178,0.14) 0%, rgba(14,116,144,0.04) 55%, rgba(15,23,42,0.02) 100%)',
						border: (theme) => `1px solid ${theme.palette.divider}`,
					}}
				>
					<Stack
						direction={{ xs: 'column', md: 'row' }}
						justifyContent='space-between'
						alignItems={{ xs: 'flex-start', md: 'center' }}
						spacing={2}
					>
						<Stack spacing={1}>
							<Typography variant='h4' sx={{ fontWeight: 800 }}>
								{t('statistic_dashboard_pharmacist.title.page')}
							</Typography>
							<Typography variant='body1' color='text.secondary'>
								{t('statistic_dashboard_pharmacist.text.description')}
							</Typography>
						</Stack>
						<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
							<FilterButton
								loading={statisticsQuery.loading}
								onFilterClick={() => setFilterDialogOpen(true)}
							/>
							<ExportButton
								loading={exportLoading}
								onExportClick={handleExport}
								translationPrefix='statistic_dashboard_pharmacist'
							/>
							<ResetFilterButton
								loading={statisticsQuery.loading}
								onResetFilterClick={() => setFilters(getDefaultDashboardFilter())}
							/>
						</Stack>
					</Stack>

					<Stack direction={{ xs: 'column', md: 'row' }} spacing={1.25} alignItems={{ md: 'center' }}>
						<Typography variant='subtitle2' color='text.secondary'>
							{t('statistic_dashboard_pharmacist.title.current_range')}
						</Typography>
						<Chip
							color='primary'
							variant='outlined'
							label={formatDashboardRangeLabel(filters.fromDate, filters.toDate)}
						/>
					</Stack>

					<GenericTabs
						tabs={tabs}
						currentTab={currentTab}
						setCurrentTab={setCurrentTab}
						loading={statisticsQuery.loading}
					/>
				</Stack>

				{currentTab?.key === 'topMedicines' ? (
					<Grid container spacing={2.25}>
						<Grid size={{ xs: 12, xl: 8 }}>{renderTopMedicinesTable()}</Grid>
					</Grid>
				) : (
					<Grid container spacing={2.25}>
						{widgets.map(({ widget, meta, gridSize }) => (
							<Grid key={widget.widgetKey} size={gridSize}>
								<DashboardWidgetRenderer
									widget={widget}
									meta={meta}
									loading={statisticsQuery.loading}
									error={statisticsQuery.error}
								/>
							</Grid>
						))}
					</Grid>
				)}
			</Stack>

			<DashboardFilterDialog
				open={filterDialogOpen}
				onClose={() => setFilterDialogOpen(false)}
				filterValues={filters}
				onApply={setFilters}
				translationPrefix='statistic_dashboard_pharmacist'
			/>
		</>
	)
}

export default PharmacistDashboardPage

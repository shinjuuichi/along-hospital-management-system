import ExportButton from '@/components/buttons/ExportButton'
import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import DashboardFilterDialog from '@/components/dashboard/DashboardFilterDialog'
import DashboardWidgetRenderer from '@/components/dashboard/DashboardWidgetRenderer'
import GenericTabs from '@/components/generals/GenericTabs'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDashboardRangeLabel, getDefaultDashboardFilter } from '@/utils/dashboardUtils'
import { downloadFile } from '@/utils/downloadFile'
import { Chip, Grid, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import { buildHRDashboardTabWidgets } from './helpers/hrDashboardTabWidgets'

const HRDashboardPage = () => {
	const { t } = useTranslation()
	const [filterDialogOpen, setFilterDialogOpen] = useState(false)
	const [filters, setFilters] = useState(getDefaultDashboardFilter())
	const [exportLoading, setExportLoading] = useState(false)

	const handleExport = async () => {
		setExportLoading(true)
		try {
			await downloadFile(ApiUrls.REPORT.HR_DASHBOARD_EXPORT, {
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
				title: t('statistic_dashboard_hr.tab.overview'),
			},
			{
				key: 'recruitment',
				title: t('statistic_dashboard_hr.tab.recruitment'),
			},
			{
				key: 'leave',
				title: t('statistic_dashboard_hr.tab.leave'),
			},
		],
		[t]
	)
	const [currentTab, setCurrentTab] = useState(tabs[0])

	const statisticsQuery = useFetch(
		ApiUrls.REPORT.HR_DASHBOARD_STATISTICS,
		{
			fromDate: filters.fromDate,
			toDate: filters.toDate,
		},
		[filters.fromDate, filters.toDate]
	)

	const widgets = useMemo(
		() => buildHRDashboardTabWidgets(statisticsQuery.data, currentTab?.key, t),
		[currentTab?.key, statisticsQuery.data, t]
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
								{t('statistic_dashboard_hr.title.page')}
							</Typography>
							<Typography variant='body1' color='text.secondary'>
								{t('statistic_dashboard_hr.text.description')}
							</Typography>
						</Stack>
						<Stack
							direction='row'
							spacing={1}
							flexWrap='nowrap'
							useFlexGap
							sx={{
								display: 'inline-flex',
								width: 'fit-content',
								whiteSpace: 'nowrap',
								alignSelf: 'flex-start',
							}}
						>
							<FilterButton
								loading={statisticsQuery.loading}
								onFilterClick={() => setFilterDialogOpen(true)}
							/>
							<ExportButton
								loading={exportLoading}
								onExportClick={handleExport}
								translationPrefix='statistic_dashboard_hr'
							/>
							<ResetFilterButton
								loading={statisticsQuery.loading}
								onResetFilterClick={() => setFilters(getDefaultDashboardFilter())}
							/>
						</Stack>
					</Stack>

					<Stack direction={{ xs: 'column', md: 'row' }} spacing={1.25} alignItems={{ md: 'center' }}>
						<Typography variant='subtitle2' color='text.secondary'>
							{t('statistic_dashboard_hr.title.current_range')}
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
			</Stack>

			<DashboardFilterDialog
				open={filterDialogOpen}
				onClose={() => setFilterDialogOpen(false)}
				filterValues={filters}
				onApply={setFilters}
				translationPrefix='statistic_dashboard_hr'
			/>
		</>
	)
}

export default HRDashboardPage

import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import FeedbackReportManagementFilterSection from './sections/FeedbackReportManagementFilterSection'
import FeedbackReportManagementTableSection from './sections/FeedbackReportManagementTableSection'

const FeedbackReportManagementPage = () => {
	const [filters, setFilters] = useState({ status: '' })
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const { t } = useTranslation()

	const getReports = useFetch(
		ApiUrls.FEEDBACK_REPORT.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const resolveReport = useAxiosSubmit({
		url: ApiUrls.FEEDBACK_REPORT.MANAGEMENT.RESOLVE(''),
		method: 'PUT',
	})
	const rejectReport = useAxiosSubmit({
		url: ApiUrls.FEEDBACK_REPORT.MANAGEMENT.REJECT(''),
		method: 'PUT',
	})
	const [openReason, setOpenReason] = useState(false)
	const [selectedReport, setSelectedReport] = useState(null)

	const handleResolve = async (row) => {
		if (!row?.id) return
		const res = await resolveReport.submit({ overrideParam: row.id })
		if (res) getReports.fetch()
	}

	const handleReject = async (row) => {
		if (!row?.id) return
		const res = await rejectReport.submit({ overrideParam: row.id })
		if (res) getReports.fetch()
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('feedback_report.title.management')}</Typography>
				<FeedbackReportManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getReports.loading}
				/>
				<FeedbackReportManagementTableSection
					reports={getReports.data?.collection}
					loading={getReports.loading}
					sort={sort}
					setSort={setSort}
					onViewReason={(row) => {
						setSelectedReport(row)
						setOpenReason(true)
					}}
					onResolve={handleResolve}
					onReject={handleReject}
				/>
				<GenericTablePagination
					totalPage={getReports.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getReports.loading}
				/>
			</Stack>
			<GenericFormDialog
				open={openReason}
				onClose={() => {
					setOpenReason(false)
					setSelectedReport(null)
				}}
				title={t('feedback_report.dialog.reason_title')}
				initialValues={{
					reason: selectedReport?.reason,
					feedbackContent: selectedReport?.feedbackContent || selectedReport?.content || '',
				}}
				fields={[
					{
						key: 'reason',
						title: t('feedback_report.field.reason'),
						multiple: 4,
						required: false,
						props: { readOnly: true },
					},
					{
						key: 'feedbackContent',
						title: t('feedback.field.content'),
						multiple: 6,
						required: false,
						props: { readOnly: true },
					},
				]}
				submitLabel={t('button.close')}
				onSubmit={({ closeDialog }) => closeDialog()}
				maxWidth='md'
			/>
		</Paper>
	)
}

export default FeedbackReportManagementPage

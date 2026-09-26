import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import ComplaintManagementFilterSection from './sections/ComplaintManagementFilterSection'
import ComplaintManagementSummaryDialog from './sections/ComplaintManagementSummaryDialog'
import ComplaintManagementTableSection from './sections/ComplaintManagementTableSection'

const ComplaintManagementPage = () => {
	const [filters, setFilters] = useState({
		complaintTopic: '',
		complaintType: '',
		complaintResolveStatus: '',
		content: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [summaryYearWeek, setSummaryYearWeek] = useState('')
	const [openSummaryDialog, setOpenSummaryDialog] = useState(false)

	const { t } = useTranslation()

	const getComplaints = useFetch(
		ApiUrls.COMPLAINT.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const getComplaintSummary = useFetch(
		ApiUrls.COMPLAINT.MANAGEMENT.SUMMARY,
		{ yearWeek: summaryYearWeek },
		[summaryYearWeek],
		false
	)

	const classifyComplaint = useAxiosSubmit({
		method: 'PUT',
	})

	return (
		<>
			<Paper sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Typography variant='h5'>{t('complaint.title.complaint_management')}</Typography>
					<ComplaintManagementFilterSection
						filters={filters}
						setFilters={setFilters}
						loading={getComplaints.loading}
					/>
					<Box>
						<Button variant='contained' color='warning' onClick={() => setOpenSummaryDialog(true)}>
							{t('complaint.button.view_summary')}
						</Button>
					</Box>
					<ComplaintManagementTableSection
						complaints={getComplaints.data?.collection}
						loading={getComplaints.loading}
						sort={sort}
						setSort={setSort}
						onClassifyComplaint={async (type, id) => {
							const response = await classifyComplaint.submit({
								overrideData: { type },
								overrideUrl: ApiUrls.COMPLAINT.MANAGEMENT.CLASSIFY(id),
							})

							if (response) {
								getComplaints.setData((prevData) => {
									const updatedCollection = prevData.collection.map((complaint) => {
										if (complaint.id === id) {
											return { ...complaint, complaintType: type }
										}
										return complaint
									})
									return { ...prevData, collection: updatedCollection }
								})
							}
						}}
					/>
					<GenericTablePagination
						totalPage={getComplaints.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						loading={getComplaints.loading}
					/>
				</Stack>
			</Paper>
			<ComplaintManagementSummaryDialog
				open={openSummaryDialog}
				onClose={() => setOpenSummaryDialog(false)}
				summary={getComplaintSummary.data || ''}
				onFilterWeek={(yearWeek) => setSummaryYearWeek(yearWeek)}
			/>
		</>
	)
}

export default ComplaintManagementPage

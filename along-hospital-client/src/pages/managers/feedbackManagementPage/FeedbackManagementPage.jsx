import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import FeedbackDetailDialog from './sections/FeedbackDetailDialog'
import FeedbackManagementFilterSection from './sections/FeedbackManagementFilterSection'
import FeedbackManagementTableSection from './sections/FeedbackManagementTableSection'

const FeedbackManagementPage = () => {
	const { t } = useTranslation()

	const [filters, setFilters] = useState({
		feedbackReplyStatus: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openDetail, setOpenDetail] = useState(false)
	const [selectedRow, setSelectedRow] = useState(null)

	const getFeedbacks = useFetch(
		ApiUrls.FEEDBACK.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const createRespondSubmit = useAxiosSubmit({
		url: ApiUrls.FEEDBACK_RESPOND.INDEX,
		method: 'POST',
	})

	const editRespondSubmit = useAxiosSubmit({
		method: 'PUT',
	})

	const handleViewDetail = (row) => {
		setSelectedRow(row)
		setOpenDetail(true)
	}

	const handleCloseDetail = () => {
		setOpenDetail(false)
		setSelectedRow(null)
	}

	const handleReplySuccess = async () => {
		const res = await getFeedbacks.fetch()
		if (res?.data?.collection && selectedRow) {
			const updatedRow = res.data.collection.find((item) => item.id === selectedRow.id)
			if (updatedRow) {
				setSelectedRow(updatedRow)
			}
		}
	}

	const handleCreateRespond = async (data) => {
		const res = await createRespondSubmit.submit({ overrideData: data })
		if (res) {
			await handleReplySuccess()
		}
		return res
	}

	const handleEditRespond = async (respondId, data) => {
		const res = await editRespondSubmit.submit({
			overrideUrl: ApiUrls.FEEDBACK_RESPOND.DETAIL(respondId),
			overrideData: data,
		})
		if (res) {
			await handleReplySuccess()
		}
		return res
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('feedback.title.feedback_management')}</Typography>
				<FeedbackManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getFeedbacks.loading}
				/>
				<FeedbackManagementTableSection
					feedbacks={getFeedbacks.data?.collection}
					loading={getFeedbacks.loading}
					sort={sort}
					setSort={setSort}
					onViewDetail={handleViewDetail}
				/>
				<GenericTablePagination
					totalPage={getFeedbacks.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getFeedbacks.loading}
				/>
			</Stack>
			<FeedbackDetailDialog
				open={openDetail}
				onClose={handleCloseDetail}
				feedback={selectedRow}
				onReplySuccess={handleReplySuccess}
				onCreateRespond={handleCreateRespond}
				onEditRespond={handleEditRespond}
				createRespondLoading={createRespondSubmit.loading}
				editRespondLoading={editRespondSubmit.loading}
			/>
		</Paper>
	)
}

export default FeedbackManagementPage

import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import LeaveRequestPendingDialog from '@/pages/hrs/leaveRequestManagementPage/sections/LeaveRequestPendingDialog'
import LeaveRequestDetailDialog from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestDetailDialog'
import LeaveRequestFilterSection from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestFilterSection'
import LeaveRequestTableSection from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestTableSection'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { toast } from 'react-toastify'

const initialLeaveFilters = {
	createdBy: '',
	leaveType: '',
	status: '',
	fromDate: '',
	toDate: '',
}

const initialSort = { key: 'id', direction: 'desc' }

const LeaveRequestManagementPage = () => {
	const { auth } = useAuth()
	const confirm = useConfirm()
	const { t } = useTranslation()

	const [openPendingDialog, setOpenPendingDialog] = useState(false)
	const [detailRow, setDetailRow] = useState(null)
	const [openDetailDialog, setOpenDetailDialog] = useState(false)
	const detailRequestIdRef = useRef(0)

	const [leaveFilters, setLeaveFilters] = useState(initialLeaveFilters)
	const [leaveSort, setLeaveSort] = useState(initialSort)
	const [leavePage, setLeavePage] = useState(1)
	const [leavePageSize, setLeavePageSize] = useState(10)

	const [pendingLeavePage, setPendingLeavePage] = useState(1)
	const [pendingLeavePageSize, setPendingLeavePageSize] = useState(10)

	const getLeaveManagementRequests = useFetch(
		ApiUrls.LEAVE_REQUEST.MANAGEMENT.INDEX,
		{
			sort: `${leaveSort.key} ${leaveSort.direction}`,
			...leaveFilters,
			page: leavePage,
			pageSize: leavePageSize,
		},
		[leaveSort, leaveFilters, leavePage, leavePageSize]
	)

	const getPendingLeaveRequests = useFetch(
		ApiUrls.LEAVE_REQUEST.MANAGEMENT.INDEX,
		{
			status: EnumConfig.LeaveRequestStatus.Pending,
			page: pendingLeavePage,
			pageSize: pendingLeavePageSize,
		},
		[pendingLeavePage, pendingLeavePageSize],
		false
	)

	const approveLeaveRequest = useAxiosSubmit({ method: 'PUT' })
	const rejectLeaveRequest = useAxiosSubmit({ method: 'PUT' })
	const getLeaveRequestDetail = useAxiosSubmit({ method: 'GET' })

	const fetchPendingLeaveRequests = getPendingLeaveRequests.fetch

	const getRequesterId = (row) =>
		row?.createdBy ??
		row?.staffId ??
		row?.staff?.id ??
		row?.staff?.staffId ??
		row?.requesterId ??
		null

	const isOwnRequest = (row) => {
		const requesterId = getRequesterId(row)
		if (!requesterId) return false
		return String(requesterId) === String(auth?.userId ?? '')
	}

	const canReviewRequest = (row) => !isOwnRequest(row)
	const showCannotReviewOwnRequestError = () =>
		toast.error(t('leave_request.error.cannot_review_own_request'))

	const refreshCurrentData = async () => {
		await getLeaveManagementRequests.fetch()

		if (openPendingDialog) {
			await fetchPendingLeaveRequests()
		}
	}

	const handleApproveLeaveRequest = async (row) => {
		if (!row?.id) return
		if (!canReviewRequest(row)) {
			showCannotReviewOwnRequestError()
			return
		}

		const confirmed = await confirm({
			title: t('leave_request.confirm.approve_leave_title'),
			description: t('leave_request.confirm.approve_leave_description', { id: row.id }),
			confirmColor: 'success',
			confirmText: t('leave_request.button.approve'),
		})
		if (!confirmed) return

		const response = await approveLeaveRequest.submit({
			overrideUrl: ApiUrls.LEAVE_REQUEST.MANAGEMENT.APPROVE(row.id),
			overrideData: null,
		})
		if (response) {
			await refreshCurrentData()
		}
	}

	const handleRejectLeaveRequest = async (row) => {
		if (!row?.id) return
		if (!canReviewRequest(row)) {
			showCannotReviewOwnRequestError()
			return
		}

		const confirmed = await confirm({
			title: t('leave_request.confirm.reject_leave_title'),
			description: t('leave_request.confirm.reject_leave_description', { id: row.id }),
			confirmColor: 'error',
			confirmText: t('leave_request.button.reject'),
		})
		if (!confirmed) return

		const response = await rejectLeaveRequest.submit({
			overrideUrl: ApiUrls.LEAVE_REQUEST.MANAGEMENT.REJECT(row.id),
			overrideData: null,
		})
		if (response) {
			await refreshCurrentData()
		}
	}

	const handleOpenPendingDialog = () => {
		setOpenPendingDialog(true)
	}

	const handleOpenDetailDialog = async (row) => {
		if (!row?.id) return

		const requestId = Date.now()
		detailRequestIdRef.current = requestId
		setOpenDetailDialog(true)
		setDetailRow(null)

		const response = await getLeaveRequestDetail.submit({
			overrideUrl: ApiUrls.LEAVE_REQUEST.MANAGEMENT.DETAIL(row.id),
		})

		if (detailRequestIdRef.current !== requestId) return

		setDetailRow(response?.data || row)
	}

	const handleCloseDetailDialog = () => {
		detailRequestIdRef.current += 1
		setOpenDetailDialog(false)
		setDetailRow(null)
	}

	useEffect(() => {
		if (!openPendingDialog) return
		fetchPendingLeaveRequests()
	}, [fetchPendingLeaveRequests, openPendingDialog, pendingLeavePage, pendingLeavePageSize])

	const actionLoading = approveLeaveRequest.loading || rejectLeaveRequest.loading

	const currentItems = getLeaveManagementRequests.data?.collection || []
	const currentTotalPage = getLeaveManagementRequests.data?.totalPage
	const currentPendingItems = getPendingLeaveRequests.data?.collection || []
	const currentPendingTotalPage = getPendingLeaveRequests.data?.totalPage

	return (
		<>
			<Paper sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Typography variant='h5'>{t('leave_request.title.leave_request_management')}</Typography>

					<LeaveRequestFilterSection
						filters={leaveFilters}
						setFilters={(nextFilters) => {
							setLeaveFilters(nextFilters)
							setLeavePage(1)
						}}
						loading={getLeaveManagementRequests.loading}
						isManagement
					/>

					<Stack direction='row' justifyContent='flex-end'>
						<Button
							variant='contained'
							color='warning'
							onClick={handleOpenPendingDialog}
							disabled={getLeaveManagementRequests.loading || actionLoading}
						>
							{t('leave_request.title.pending_requests')}
						</Button>
					</Stack>

					<LeaveRequestTableSection
						data={currentItems}
						loading={getLeaveManagementRequests.loading}
						sort={leaveSort}
						setSort={setLeaveSort}
						isManagement
						canManageRequest={canReviewRequest}
						onDetail={handleOpenDetailDialog}
						onApprove={handleApproveLeaveRequest}
						onReject={handleRejectLeaveRequest}
						actionLoading={actionLoading}
					/>

					<GenericTablePagination
						totalPage={currentTotalPage}
						page={leavePage}
						setPage={setLeavePage}
						pageSize={leavePageSize}
						setPageSize={setLeavePageSize}
						loading={getLeaveManagementRequests.loading}
					/>
				</Stack>
			</Paper>

			<LeaveRequestPendingDialog
				open={openPendingDialog}
				onClose={() => setOpenPendingDialog(false)}
				data={currentPendingItems}
				loading={getPendingLeaveRequests.loading}
				totalPage={currentPendingTotalPage}
				page={pendingLeavePage}
				setPage={setPendingLeavePage}
				pageSize={pendingLeavePageSize}
				setPageSize={setPendingLeavePageSize}
				canManageRequest={canReviewRequest}
				onApprove={handleApproveLeaveRequest}
				onReject={handleRejectLeaveRequest}
				actionLoading={actionLoading}
			/>

			<LeaveRequestDetailDialog
				open={openDetailDialog}
				onClose={handleCloseDetailDialog}
				data={detailRow}
				loading={getLeaveRequestDetail.loading}
			/>
		</>
	)
}

export default LeaveRequestManagementPage

import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	LEAVE_REQUEST_TABS,
	default as StaffLeaveRequestDialog,
} from '@/pages/staffs/staffWorkSchedulePage/sections/StaffLeaveRequestDialog'
import StaffWorkScheduleContentSection from '@/pages/staffs/staffWorkSchedulePage/sections/StaffWorkScheduleContentSection'
import { setShiftsStore } from '@/redux/reducers/managementReducer'
import { getCurrentMonthRange } from '@/utils/formatDateUtil'
import { Stack } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const buildLeaveRequestPayload = (values = {}) => {
	const isShiftUnit = values?.leaveUnit === EnumConfig.LeaveUnit.Shift

	return isShiftUnit
		? {
				leaveType: values?.leaveType,
				leaveUnit: values?.leaveUnit,
				fromDate: values?.shiftDate,
				toDate: values?.shiftDate,
				shiftId: values?.shiftId ? Number(values.shiftId) : undefined,
				reason: values?.reason,
			}
		: {
				leaveType: values?.leaveType,
				leaveUnit: values?.leaveUnit,
				fromDate: values?.fromDate,
				toDate: values?.toDate,
				reason: values?.reason,
			}
}

const StaffWorkSchedulePage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const [dateRange, setDateRange] = useState(getCurrentMonthRange())
	const [isLeaveRequestDialogOpen, setIsLeaveRequestDialogOpen] = useState(false)
	const [leaveRequestTab, setLeaveRequestTab] = useState(LEAVE_REQUEST_TABS.CREATE)
	const [leaveRequestDetailRow, setLeaveRequestDetailRow] = useState(null)
	const [leaveSort, setLeaveSort] = useState({ key: 'id', direction: 'desc' })
	const [leavePage, setLeavePage] = useState(1)
	const [leavePageSize, setLeavePageSize] = useState(10)
	const getMyWorkSchedules = useFetch(
		ApiUrls.WORK_SCHEDULE.STAFF,
		{
			fromDate: dateRange.fromDate,
			toDate: dateRange.toDate,
		},
		[dateRange.fromDate, dateRange.toDate]
	)
	const shiftStore = useReduxStore({
		selector: (state) => state.management.shifts,
		setStore: setShiftsStore,
	})
	const getLeaveRequests = useFetch(
		ApiUrls.LEAVE_REQUEST.INDEX,
		{
			sort: `${leaveSort.key} ${leaveSort.direction}`,
			pageNumber: leavePage,
			pageSize: leavePageSize,
		},
		[],
		false
	)
	const {
		data: leaveRequestData,
		fetch: fetchLeaveRequests,
		loading: leaveRequestLoading,
	} = getLeaveRequests
	const createLeaveRequest = useAxiosSubmit({
		url: ApiUrls.LEAVE_REQUEST.INDEX,
		method: 'POST',
	})
	const cancelLeaveRequest = useAxiosSubmit({ method: 'PUT' })

	const scheduleAssignments = useMemo(() => {
		const schedules = Array.isArray(getMyWorkSchedules.data) ? getMyWorkSchedules.data : []
		const items = []

		for (const schedule of schedules) {
			const assignments = schedule?.workScheduleAssignments || []
			if (!assignments.length) continue

			for (const assignment of assignments) {
				items.push({
					key: `assignment-${assignment?.id || `${schedule?.id}-${assignment?.locationId}`}`,
					schedule,
					assignment,
				})
			}
		}

		return items.sort((a, b) => {
			const dateA = `${a?.schedule?.workDate || ''} ${a?.schedule?.shift?.startTime || ''}`
			const dateB = `${b?.schedule?.workDate || ''} ${b?.schedule?.shift?.startTime || ''}`
			return dateA.localeCompare(dateB)
		})
	}, [getMyWorkSchedules.data])
	const leaveRequests = leaveRequestData?.collection || []
	const leaveTotalPage = leaveRequestData?.totalPage || 0
	const leaveActionLoading = createLeaveRequest.loading || cancelLeaveRequest.loading

	useEffect(() => {
		if (!isLeaveRequestDialogOpen || leaveRequestTab !== LEAVE_REQUEST_TABS.HISTORY) return

		fetchLeaveRequests()
	}, [
		fetchLeaveRequests,
		isLeaveRequestDialogOpen,
		leavePage,
		leavePageSize,
		leaveRequestTab,
		leaveSort,
	])

	const handleOpenLeaveRequestDialog = () => {
		setLeaveRequestDetailRow(null)
		setLeaveRequestTab(LEAVE_REQUEST_TABS.CREATE)
		setIsLeaveRequestDialogOpen(true)
	}

	const handleCloseLeaveRequestDialog = () => {
		setLeaveRequestDetailRow(null)
		setIsLeaveRequestDialogOpen(false)
	}

	const handleLeaveRequestTabChange = (nextTab) => {
		const resolvedTab =
			nextTab === LEAVE_REQUEST_TABS.HISTORY ? LEAVE_REQUEST_TABS.HISTORY : LEAVE_REQUEST_TABS.CREATE

		setLeaveRequestTab(resolvedTab)
		setLeaveRequestDetailRow(null)
	}

	const handleCreateLeaveRequest = async (values) => {
		const response = await createLeaveRequest.submit({
			overrideData: buildLeaveRequestPayload(values),
		})
		if (!response) return undefined

		setLeavePage(1)
		setLeaveRequestTab(LEAVE_REQUEST_TABS.HISTORY)
		return response
	}

	const handleCancelLeaveRequest = async (row) => {
		if (!row?.id) return

		const confirmed = await confirm({
			title: t('leave_request.confirm.cancel_leave_title'),
			description: t('leave_request.confirm.cancel_leave_description', { id: row.id }),
			confirmColor: 'error',
			confirmText: t('leave_request.button.cancel_request'),
		})
		if (!confirmed) return

		const response = await cancelLeaveRequest.submit({
			overrideUrl: ApiUrls.LEAVE_REQUEST.CANCEL(row.id),
			overrideData: null,
		})
		if (!response) return

		setLeaveRequestDetailRow((prev) => (prev?.id === row.id ? null : prev))
		await fetchLeaveRequests()
	}

	const handleVisibleRangeChange = useCallback((nextDateRange) => {
		if (!nextDateRange?.fromDate || !nextDateRange?.toDate) return

		setDateRange((prev) => {
			if (prev.fromDate === nextDateRange.fromDate && prev.toDate === nextDateRange.toDate) {
				return prev
			}

			return nextDateRange
		})
	}, [])

	return (
		<>
			<Stack spacing={2}>
				<StaffWorkScheduleContentSection
					scheduleAssignments={scheduleAssignments}
					onVisibleRangeChange={handleVisibleRangeChange}
					onOpenLeaveRequest={handleOpenLeaveRequestDialog}
				/>
			</Stack>

			{isLeaveRequestDialogOpen && (
				<StaffLeaveRequestDialog
					open={isLeaveRequestDialogOpen}
					onClose={handleCloseLeaveRequestDialog}
					currentTab={leaveRequestTab}
					onTabChange={handleLeaveRequestTabChange}
					detailRow={leaveRequestDetailRow}
					onDetailClose={() => setLeaveRequestDetailRow(null)}
					leaveRequests={leaveRequests}
					leaveLoading={leaveRequestLoading}
					leaveSort={leaveSort}
					setLeaveSort={setLeaveSort}
					leavePage={leavePage}
					setLeavePage={setLeavePage}
					leavePageSize={leavePageSize}
					setLeavePageSize={setLeavePageSize}
					leaveTotalPage={leaveTotalPage}
					shifts={shiftStore.data}
					createLoading={createLeaveRequest.loading}
					actionLoading={leaveActionLoading}
					onDetail={setLeaveRequestDetailRow}
					onCancelLeaveRequest={handleCancelLeaveRequest}
					onSubmitLeaveRequest={handleCreateLeaveRequest}
				/>
			)}
		</>
	)
}

export default StaffWorkSchedulePage

import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTabs from '@/components/generals/GenericTabs'
import useTranslation from '@/hooks/useTranslation'
import LeaveRequestDetailDialog from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestDetailDialog'
import LeaveRequestFormDialog from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestFormDialog'
import LeaveRequestTableSection from '@/pages/staffs/leaveRequestPage/sections/LeaveRequestTableSection'
import CloseRoundedIcon from '@mui/icons-material/CloseRounded'
import HistoryRoundedIcon from '@mui/icons-material/HistoryRounded'
import PlaylistAddRoundedIcon from '@mui/icons-material/PlaylistAddRounded'
import { Dialog, DialogContent, DialogTitle, IconButton, Stack, Typography } from '@mui/material'
import { useMemo } from 'react'

const LEAVE_REQUEST_TABS = {
	CREATE: 'create',
	HISTORY: 'history',
}

const StaffLeaveRequestDialog = ({
	open = false,
	onClose = () => {},
	currentTab = LEAVE_REQUEST_TABS.CREATE,
	onTabChange = (tabKey) => tabKey,
	detailRow = null,
	onDetailClose = () => {},
	leaveRequests = [],
	leaveLoading = false,
	leaveSort = { key: 'id', direction: 'desc' },
	setLeaveSort = (nextSort) => nextSort,
	leavePage = 1,
	setLeavePage = (nextPage) => nextPage,
	leavePageSize = 10,
	setLeavePageSize = (nextPageSize) => nextPageSize,
	leaveTotalPage = 0,
	shifts = [],
	createLoading = false,
	actionLoading = false,
	onDetail = (row) => row,
	onCancelLeaveRequest = (row) => row,
	onSubmitLeaveRequest = async (values) => values,
}) => {
	const { t } = useTranslation()
	const tabs = useMemo(
		() => [
			{
				key: LEAVE_REQUEST_TABS.CREATE,
				title: t('leave_request.title.create_leave_request'),
				icon: <PlaylistAddRoundedIcon />,
			},
			{
				key: LEAVE_REQUEST_TABS.HISTORY,
				title: t('leave_request.title.history'),
				icon: <HistoryRoundedIcon />,
			},
		],
		[t]
	)

	return (
		<>
			<Dialog open={open} onClose={onClose} fullWidth maxWidth='lg'>
				<DialogTitle>
					<Stack direction='row' justifyContent='space-between' alignItems='center' spacing={1}>
						<Typography variant='h6'>{t('leave_request.title.leave_request')}</Typography>
						<IconButton onClick={onClose} size='small'>
							<CloseRoundedIcon />
						</IconButton>
					</Stack>
				</DialogTitle>

				<DialogContent dividers>
					<Stack spacing={2.5}>
						<GenericTabs
							tabs={tabs}
							currentTab={currentTab}
							setCurrentTab={(tab) => onTabChange(tab?.key || LEAVE_REQUEST_TABS.CREATE)}
						/>

						{currentTab === LEAVE_REQUEST_TABS.HISTORY ? (
							<Stack spacing={2}>
								<LeaveRequestTableSection
									data={leaveRequests}
									loading={leaveLoading}
									sort={leaveSort}
									setSort={setLeaveSort}
									stickyHeader={false}
									onDetail={onDetail}
									onCancel={onCancelLeaveRequest}
									actionLoading={actionLoading}
								/>

								<GenericTablePagination
									totalPage={leaveTotalPage}
									page={leavePage}
									setPage={setLeavePage}
									pageSize={leavePageSize}
									setPageSize={setLeavePageSize}
									loading={leaveLoading}
								/>
							</Stack>
						) : (
							<LeaveRequestFormDialog
								open={open && currentTab === LEAVE_REQUEST_TABS.CREATE}
								loading={createLoading}
								shifts={shifts}
								onCancel={onClose}
								onSubmit={onSubmitLeaveRequest}
							/>
						)}
					</Stack>
				</DialogContent>
			</Dialog>

			<LeaveRequestDetailDialog open={Boolean(detailRow)} onClose={onDetailClose} data={detailRow} />
		</>
	)
}

export { LEAVE_REQUEST_TABS }
export default StaffLeaveRequestDialog

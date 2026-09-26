import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { defaultLeaveRequestStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatLeaveShiftValue } from '@/pages/staffs/leaveRequestPage/helpers/leaveRequestHelper'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Button,
	Chip,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
	Paper,
	Stack,
	Typography,
} from '@mui/material'

const PendingRequestInfoRow = ({ label, value }) => (
	<Stack direction='row' alignItems='flex-start' justifyContent='space-between' spacing={2}>
		<Typography variant='body2' sx={{ color: 'text.secondary' }}>
			{label}
		</Typography>
		<Typography
			variant='body2'
			sx={{
				fontWeight: 500,
				textAlign: 'right',
				wordBreak: 'break-word',
				overflowWrap: 'anywhere',
			}}
		>
			{renderEmptyFallback(value)}
		</Typography>
	</Stack>
)

const LeaveRequestPendingDialog = ({
	open = false,
	onClose = () => {},
	data = [],
	loading = false,
	totalPage = 0,
	page = 1,
	setPage = (nextPage) => nextPage,
	pageSize = 10,
	setPageSize = (nextPageSize) => nextPageSize,
	canManageRequest = () => true,
	onApprove = (row) => row,
	onReject = (row) => row,
	actionLoading = false,
}) => {
	const _enum = useEnum()
	const { t } = useTranslation()

	return (
		<Dialog open={open} onClose={onClose} fullWidth maxWidth='xl'>
			<DialogTitle>{t('leave_request.title.pending_requests')}</DialogTitle>
			<DialogContent dividers>
				<Stack spacing={2}>
					{loading && data.length === 0 ? (
						<Typography variant='body2' sx={{ color: 'text.secondary' }}>
							{t('text.loading')}
						</Typography>
					) : data.length === 0 ? (
						<Typography variant='body2' sx={{ color: 'text.secondary' }}>
							{t('text.placeholder.no_data')}
						</Typography>
					) : (
						data.map((row) => {
							const canManageCurrentRow = canManageRequest(row)
							const statusLabel =
								getEnumLabelByValue(_enum.leaveRequestStatusOptions, row?.status) || row?.status
							const leaveTypeLabel =
								getEnumLabelByValue(_enum.leaveTypeOptions, row?.leaveType) || row?.leaveType
							const leaveUnitLabel =
								getEnumLabelByValue(_enum.leaveUnitOptions, row?.leaveUnit) || row?.leaveUnit

							return (
								<Paper key={row?.id} variant='outlined' sx={{ p: 2 }}>
									<Stack spacing={1.25}>
										<Stack direction='row' alignItems='center' justifyContent='space-between'>
											<Typography variant='subtitle1' sx={{ fontWeight: 700 }}>
												#{renderEmptyFallback(row?.id)}
											</Typography>
											<Chip
												label={renderEmptyFallback(statusLabel)}
												color={defaultLeaveRequestStatusStyle(row?.status)}
												size='small'
											/>
										</Stack>

										<Typography variant='subtitle2' sx={{ fontWeight: 700 }}>
											{t('leave_request.text.important_information')}
										</Typography>

										<PendingRequestInfoRow
											label={t('leave_request.field.staff_name')}
											value={row?.staff?.name}
										/>

										<PendingRequestInfoRow
											label={t('leave_request.field.leave_type')}
											value={leaveTypeLabel}
										/>
										<PendingRequestInfoRow
											label={t('leave_request.field.leave_unit')}
											value={leaveUnitLabel}
										/>
										{row?.leaveUnit === EnumConfig.LeaveUnit.Shift && (
											<PendingRequestInfoRow
												label={t('leave_request.field.shift')}
												value={formatLeaveShiftValue(row)}
											/>
										)}
										<PendingRequestInfoRow
											label={t('leave_request.field.from')}
											value={renderEmptyFallback(formatDateBasedOnCurrentLanguage(row?.fromDate))}
										/>
										<PendingRequestInfoRow
											label={t('leave_request.field.to')}
											value={renderEmptyFallback(formatDateBasedOnCurrentLanguage(row?.toDate))}
										/>

										<PendingRequestInfoRow label={t('leave_request.field.reason')} value={row?.reason} />

										<Divider />

										<Stack direction='row' justifyContent='flex-end' spacing={1}>
											<Button
												size='small'
												variant='contained'
												color='success'
												onClick={() => onApprove(row)}
												disabled={actionLoading || !canManageCurrentRow}
											>
												{t('leave_request.button.approve')}
											</Button>
											<Button
												size='small'
												variant='contained'
												color='error'
												onClick={() => onReject(row)}
												disabled={actionLoading || !canManageCurrentRow}
											>
												{t('leave_request.button.reject')}
											</Button>
										</Stack>
									</Stack>
								</Paper>
							)
						})
					)}

					<GenericTablePagination
						totalPage={totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						loading={loading}
					/>
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose} color='inherit'>
					{t('button.close')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default LeaveRequestPendingDialog

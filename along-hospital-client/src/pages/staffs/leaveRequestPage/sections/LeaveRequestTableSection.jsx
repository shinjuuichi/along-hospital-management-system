import ActionMenu from '@/components/generals/ActionMenu'
import SearchBar from '@/components/generals/SearchBar'
import GenericTable from '@/components/tables/GenericTable'
import { defaultLeaveRequestStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatLeaveShiftValue } from '@/pages/staffs/leaveRequestPage/helpers/leaveRequestHelper'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Avatar, Chip, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const isPendingRequest = (status) =>
	String(status || '').toLowerCase() === EnumConfig.LeaveRequestStatus.Pending.toLowerCase()

const matchSearch = (row, keyword) => {
	if (!keyword) return true
	const lowerKeyword = keyword.toLowerCase()
	const searchableValues = [
		row?.staff?.name,
		row?.staff?.email,
		row?.decider?.name,
		row?.reason,
		row?.shiftName,
	]
	return searchableValues.some((val) =>
		String(val || '')
			.toLowerCase()
			.includes(lowerKeyword)
	)
}

const renderLeaveDateRangeCell = (row, t) => {
	const isShiftUnit = row?.leaveUnit === EnumConfig.LeaveUnit.Shift
	const fromLabel = isShiftUnit ? t('leave_request.field.date') : t('leave_request.field.from')
	const fromValue = formatDateBasedOnCurrentLanguage(row?.fromDate || row?.shiftDate)
	const toValue = formatDateBasedOnCurrentLanguage(row?.toDate)

	return (
		<Stack spacing={0.4} sx={{ minWidth: 0, width: '100%', whiteSpace: 'normal' }}>
			<Typography
				component='div'
				variant='body2'
				sx={{ whiteSpace: 'normal', overflowWrap: 'anywhere', lineHeight: 1.35 }}
			>
				<Typography component='span' variant='caption' sx={{ color: 'text.secondary', mr: 0.75 }}>
					{`${fromLabel}:`}
				</Typography>
				<Typography component='span' sx={{ fontWeight: 600 }}>
					{fromValue}
				</Typography>
			</Typography>

			{!isShiftUnit && (
				<Typography
					component='div'
					variant='body2'
					sx={{ whiteSpace: 'normal', overflowWrap: 'anywhere', lineHeight: 1.35 }}
				>
					<Typography component='span' variant='caption' sx={{ color: 'text.secondary', mr: 0.75 }}>
						{`${t('leave_request.field.to')}:`}
					</Typography>
					<Typography component='span' sx={{ fontWeight: 600 }}>
						{toValue}
					</Typography>
				</Typography>
			)}
		</Stack>
	)
}

const LeaveRequestTableSection = ({
	data = [],
	loading = false,
	sort = { key: 'id', direction: 'desc' },
	setSort = (nextSort) => nextSort,
	showIdColumn = false,
	stickyHeader = true,
	isManagement = false,
	canManageRequest = () => true,
	onDetail = (row) => row,
	onCancel = (row) => row,
	onApprove = (row) => row,
	onReject = (row) => row,
	actionLoading = false,
}) => {
	const _enum = useEnum()
	const { t } = useTranslation()
	const [searchKeyword, setSearchKeyword] = useState('')

	const filteredData = useMemo(
		() => data.filter((row) => matchSearch(row, searchKeyword)),
		[data, searchKeyword]
	)

	const fields = useMemo(() => {
		const statusField = {
			key: 'status',
			title: t('leave_request.field.status'),
			width: 10,
			sortable: true,
			render: (value) => (
				<Chip
					label={renderEmptyFallback(
						getEnumLabelByValue(_enum.leaveRequestStatusOptions, value) || value
					)}
					color={defaultLeaveRequestStatusStyle(value)}
					size='small'
				/>
			),
		}

		const managementStaffFields = isManagement
			? [
					{
						key: 'staff.name',
						title: t('leave_request.field.staff_name'),
						width: 12,
						sortable: false,
						render: (_, row) => (
							<Stack direction='row' spacing={1} alignItems='center'>
								<Avatar src={getImageFromCloud(row?.staff?.image)} sx={{ width: 28, height: 28 }} />
								<Typography variant='body2'>{renderEmptyFallback(row?.staff?.name)}</Typography>
							</Stack>
						),
					},
					{
						key: 'staff.email',
						title: t('leave_request.field.staff_email'),
						width: 14,
						sortable: false,
						render: (value) => renderEmptyFallback(value),
					},
				]
			: []

		const actionField = {
			key: '',
			title: t('leave_request.field.actions'),
			width: 8,
			render: (_, row) => {
				const detailAction = {
					title: t('button.detail'),
					onClick: () => onDetail(row),
				}

				const cancelAction = isPendingRequest(row?.status)
					? {
							title: t('button.cancel'),
							onClick: () => onCancel(row),
							disabled: actionLoading,
						}
					: null

				const canManageCurrentRow = canManageRequest(row)

				const managementActions =
					isPendingRequest(row?.status) && canManageCurrentRow
						? [
								{
									title: t('leave_request.button.approve'),
									onClick: () => onApprove(row),
									disabled: actionLoading,
								},
								{
									title: t('leave_request.button.reject'),
									onClick: () => onReject(row),
									disabled: actionLoading,
								},
							]
						: []

				const actions = (
					isManagement ? [detailAction, ...managementActions] : [detailAction, cancelAction]
				).filter(Boolean)

				return <ActionMenu actions={actions} />
			},
		}

		return [
			...(showIdColumn
				? [{ key: 'id', title: 'ID', width: 6, sortable: true, fixedColumn: true }]
				: []),
			...managementStaffFields,
			{
				key: 'leaveType',
				title: t('leave_request.field.leave_type'),
				width: 10,
				sortable: true,
				render: (value) =>
					renderEmptyFallback(getEnumLabelByValue(_enum.leaveTypeOptions, value) || value),
			},
			{
				key: 'leaveUnit',
				title: t('leave_request.field.leave_unit'),
				width: 10,
				sortable: true,
				render: (value) =>
					renderEmptyFallback(getEnumLabelByValue(_enum.leaveUnitOptions, value) || value),
			},
			{
				key: 'shiftName',
				title: t('leave_request.field.shift'),
				width: 14,
				sortable: false,
				render: (_, row) => formatLeaveShiftValue(row),
			},
			{
				key: 'fromDate',
				title: t('leave_request.field.leave_dates'),
				width: 20,
				sortable: true,
				render: (_, row) => renderLeaveDateRangeCell(row, t),
			},
			statusField,
			actionField,
		]
	}, [
		actionLoading,
		canManageRequest,
		_enum.leaveTypeOptions,
		_enum.leaveUnitOptions,
		_enum.leaveRequestStatusOptions,
		isManagement,
		onApprove,
		onCancel,
		onDetail,
		onReject,
		showIdColumn,
		t,
	])

	return (
		<Stack spacing={1.5}>
			<SearchBar
				value={searchKeyword}
				setValue={setSearchKeyword}
				placeholder={t('leave_request.placeholder.search')}
				widthPercent={0}
			/>
			<GenericTable
				data={filteredData}
				fields={fields}
				rowKey='id'
				loading={loading}
				sort={sort}
				setSort={setSort}
				stickyHeader={stickyHeader}
			/>
		</Stack>
	)
}

export default LeaveRequestTableSection

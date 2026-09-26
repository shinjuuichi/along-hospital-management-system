import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { defaultAttendanceLogTypeStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Avatar, Chip, Paper, Stack, Typography } from '@mui/material'
import React from 'react'
import ManageAttendanceFilterSection from './sections/ManageAttendanceFilterSection'

const ManageAttendanceBasePage = ({
	filters,
	setFilters,
	sort,
	setSort,
	page,
	setPage,
	pageSize,
	setPageSize,
	attendances,
	totalPage,
	loading,
	buttons = <React.Fragment />,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = [
		{
			key: 'staff.name',
			title: t('profile.field.name'),
			width: 20,
			render: (_, row) => (
				<Stack direction={'row'} spacing={2} alignItems='center'>
					<Avatar src={getImageFromCloud(row.staff?.image)} />
					<Typography>{row.staff?.name}</Typography>
				</Stack>
			),
		},
		{ key: 'staff.phone', title: t('profile.field.phone'), width: 20 },
		{ key: 'staff.email', title: t('profile.field.email'), width: 20 },
		{
			key: 'logType',
			title: t('attendance.field.log_type'),
			width: 20,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.attendanceLogTypeOptions, value)}
					color={defaultAttendanceLogTypeStyle(value)}
					size='small'
				/>
			),
		},
		{
			key: 'logTime',
			title: t('attendance.field.log_time'),
			width: 20,
			sortable: true,
			render: (value) => formatDatetimeStringBasedOnCurrentLanguage(value),
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('attendance.title.manage_attendance')}</Typography>
				<ManageAttendanceFilterSection filters={filters} setFilters={setFilters} loading={loading} />
				{buttons}
				<GenericTable
					data={attendances}
					fields={fields}
					rowKey='id'
					loading={loading}
					sort={sort}
					setSort={setSort}
				/>
				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={loading}
				/>
			</Stack>
		</Paper>
	)
}

export default ManageAttendanceBasePage

import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultBooleanStyle, defaultWorkScheduleStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const WorkScheduleManagementTableSection = ({
	schedules,
	loading,
	sort,
	setSort,
	selectedScheduleIds = [],
	setSelectedScheduleIds,
	onCreate,
	onEdit,
	onDelete,
	onPublish,
	onLock,
	onFinalize,
}) => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const _enum = useEnum()
	const hasSelectedSchedules = selectedScheduleIds.length > 0
	const canManageSchedule = (row) =>
		row?.workScheduleStatus === EnumConfig.WorkScheduleStatus.Draft ||
		row?.workScheduleStatus === EnumConfig.WorkScheduleStatus.Published

	const fields = [
		{
			key: 'workDate',
			title: t('work_schedule.field.work_date'),
			width: 20,
			sortable: true,
			render: (value) => formatDateBasedOnCurrentLanguage(value),
		},
		{
			key: 'shift.name',
			title: t('work_schedule.field.shift'),
			width: 25,
			render: (_, row) => {
				const shiftName = row.shift?.name
				const startTime = row.shift?.startTime
				const endTime = row.shift?.endTime

				if (!shiftName && !startTime && !endTime) return renderEmptyFallback(null)
				if (!startTime || !endTime) return renderEmptyFallback(shiftName)

				return `${shiftName} (${startTime} - ${endTime})`
			},
		},
		{
			key: 'shift.isOvertime',
			title: t('work_schedule.field.is_overtime'),
			width: 15,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.booleanOptions, value)}
					color={defaultBooleanStyle(value)}
					size='small'
					variant='outlined'
				/>
			),
		},
		{
			key: 'workScheduleStatus',
			title: t('work_schedule.field.status'),
			width: 15,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.workScheduleStatusOptions, value) || value}
					color={defaultWorkScheduleStatusStyle(value)}
					size='small'
				/>
			),
		},
		{
			key: 'workScheduleTemplateId',
			title: t('work_schedule.field.template_id'),
			width: 15,
			render: (value) => renderEmptyFallback(value),
		},
		{
			key: '',
			title: t('work_schedule.table.actions'),
			width: 10,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('work_schedule.button.open'),
							onClick: () =>
								navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_MANAGEMENT.DETAIL(row.id))),
						},
						{
							title: t('button.edit'),
							disabled: !canManageSchedule(row),
							onClick: () => onEdit?.(row),
						},
						{
							title: t('button.delete'),
							disabled: !canManageSchedule(row),
							onClick: () => onDelete?.(row),
						},
					]}
				/>
			),
		},
	]

	return (
		<Stack spacing={2}>
			<Stack direction='row' alignItems='center' justifyContent='flex-end' spacing={1}>
				<Button
					variant='contained'
					color='success'
					onClick={onPublish}
					disabled={!hasSelectedSchedules}
				>
					{t('work_schedule.button.publish')}
				</Button>
				<Button variant='contained' color='warning' onClick={onLock} disabled={!hasSelectedSchedules}>
					{t('work_schedule.button.lock')}
				</Button>
				<Button
					variant='contained'
					color='secondary'
					onClick={onFinalize}
					disabled={!hasSelectedSchedules}
				>
					{t('work_schedule.button.finalize')}
				</Button>
				<Button variant='contained' color='primary' onClick={onCreate}>
					{t('work_schedule.button.create')}
				</Button>
			</Stack>

			<GenericTable
				data={schedules}
				fields={fields}
				rowKey='id'
				canSelectRows
				selectedRows={selectedScheduleIds}
				setSelectedRows={setSelectedScheduleIds}
				loading={loading}
				sort={sort}
				setSort={setSort}
				stickyHeader
			/>
		</Stack>
	)
}

export default WorkScheduleManagementTableSection

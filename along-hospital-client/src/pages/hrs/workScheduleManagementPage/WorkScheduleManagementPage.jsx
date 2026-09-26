import { ApiUrls } from '@/configs/apiUrls'
import { defaultWorkScheduleStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setShiftsStore } from '@/redux/reducers/managementReducer'
import { resolveDateTime, resolveEventPalette } from '@/utils/commons'
import {
	formatDateBasedOnCurrentLanguage,
	formatDateToSqlDate,
	getCurrentMonthRange,
} from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import viLocale from '@fullcalendar/core/locales/vi'
import dayGridPlugin from '@fullcalendar/daygrid'
import interactionPlugin from '@fullcalendar/interaction'
import FullCalendar from '@fullcalendar/react'
import CloseRoundedIcon from '@mui/icons-material/CloseRounded'
import {
	Button,
	Checkbox,
	Chip,
	Drawer,
	IconButton,
	Paper,
	Stack,
	Typography,
	useTheme,
} from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import WorkScheduleManagementFormSection from './sections/WorkScheduleManagementFormSection'

const WorkScheduleManagementPage = () => {
	const { t, language } = useTranslation()
	const confirm = useConfirm()
	const theme = useTheme()
	const navigate = useNavigate()
	const _enum = useEnum()

	const [visibleRange, setVisibleRange] = useState(getCurrentMonthRange())
	const [selectedRow, setSelectedRow] = useState(null)
	const [selectedScheduleIds, setSelectedScheduleIds] = useState([])
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [drawerSchedule, setDrawerSchedule] = useState(null)

	const getSchedules = useFetch(
		ApiUrls.WORK_SCHEDULE.MANAGEMENT.DATE_RANGE,
		{
			fromDate: visibleRange.fromDate,
			toDate: visibleRange.toDate,
		},
		[visibleRange.fromDate, visibleRange.toDate]
	)

	const getShifts = useReduxStore({
		selector: (state) => state.management.shifts,
		setStore: setShiftsStore,
	})

	const getTemplates = useFetch(ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.GET_ALL)

	const shiftOptions = useMemo(
		() =>
			(getShifts.data || []).map((shift) => ({
				value: shift.id,
				label: `${shift.name} (${shift.startTime} – ${shift.endTime})`,
			})),
		[getShifts.data]
	)

	const templateOptions = useMemo(
		() =>
			(getTemplates.data || [])
				.filter((tpl) => tpl.isActive !== false)
				.map((tpl) => ({
					value: tpl.id,
					label: tpl.name || `#${tpl.id}`,
				})),
		[getTemplates.data]
	)

	const createSchedule = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const updateSchedule = useAxiosSubmit({
		method: 'PUT',
	})

	const deleteSchedule = useAxiosSubmit({
		method: 'DELETE',
	})

	const publishSchedule = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE.MANAGEMENT.PUBLISH,
		method: 'PUT',
	})

	const lockSchedule = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE.MANAGEMENT.LOCK,
		method: 'PUT',
	})

	const finalizeSchedule = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE.MANAGEMENT.FINALIZE,
		method: 'PUT',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const payload = {
			fromDate: values.fromDate,
			toDate: values.toDate,
			shiftId: values.shiftId,
			workScheduleTemplateId: values.workScheduleTemplateId,
		}

		if (values.scheduleType === 'TEMPLATE') {
			payload.shiftId = null
		} else {
			payload.workScheduleTemplateId = null
		}

		const response = await createSchedule.submit({ overrideData: payload })
		if (!response) return

		closeDialog()
		await getSchedules.fetch()
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		if (!selectedRow?.id) return

		const payload = {
			workDate: values.workDate,
			shiftId: values.shiftId,
		}

		const response = await updateSchedule.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE.MANAGEMENT.DETAIL(selectedRow.id),
			overrideData: payload,
		})
		if (!response) return

		closeDialog()
		setSelectedRow(null)
		await getSchedules.fetch()
	}

	const handleDelete = async (item) => {
		if (!item?.id) return

		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('work_schedule.dialog.delete_title'),
			description: t('work_schedule.dialog.delete_description', { id: item.id }),
		})
		if (!isConfirmed) return

		const response = await deleteSchedule.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE.MANAGEMENT.DETAIL(item.id),
		})
		if (response) {
			setDrawerSchedule((prev) => (prev?.id === item.id ? null : prev))
			await getSchedules.fetch()
		}
	}

	const schedules = useMemo(() => getSchedules.data || [], [getSchedules.data])
	const canManageSchedule = (row) =>
		row?.workScheduleStatus === EnumConfig.WorkScheduleStatus.Draft ||
		row?.workScheduleStatus === EnumConfig.WorkScheduleStatus.Published

	const calendarEvents = useMemo(
		() =>
			schedules
				.map((item) => {
					const start = resolveDateTime(item?.workDate, item?.shift?.startTime)
					const end = resolveDateTime(item?.workDate, item?.shift?.endTime)
					if (!start) return null

					const statusColor = defaultWorkScheduleStatusStyle(item?.workScheduleStatus)
					const palette = resolveEventPalette(theme, statusColor)
					const isSelected = selectedScheduleIds.includes(item.id)

					return {
						id: String(item.id),
						title: item?.shift?.name || `#${item.id}`,
						start,
						end: end || undefined,
						allDay: false,
						backgroundColor: palette.backgroundColor,
						borderColor: isSelected ? theme.palette.warning.main : palette.borderColor,
						textColor: palette.textColor,
						extendedProps: { schedule: item },
					}
				})
				.filter(Boolean),
		[schedules, selectedScheduleIds, theme]
	)

	const handleDatesSet = ({ view, start, end }) => {
		const monthStart = view?.currentStart || start
		const monthEndExclusive = view?.currentEnd || end
		if (!monthStart || !monthEndExclusive) return

		const fromDate = formatDateToSqlDate(monthStart)
		const toDateRaw = new Date(monthEndExclusive)
		toDateRaw.setDate(toDateRaw.getDate() - 1)
		const toDate = formatDateToSqlDate(toDateRaw)

		setVisibleRange((prev) => {
			if (prev.fromDate === fromDate && prev.toDate === toDate) {
				return prev
			}

			return { fromDate, toDate }
		})
	}

	const handleEventClick = (eventClickInfo) => {
		setDrawerSchedule(eventClickInfo?.event?.extendedProps?.schedule || null)
	}

	const handleToggleSelected = (scheduleId) => {
		if (!scheduleId) return

		setSelectedScheduleIds((prev) =>
			prev.includes(scheduleId) ? prev.filter((id) => id !== scheduleId) : [...prev, scheduleId]
		)
	}

	const scheduleIdsByDate = useMemo(() => {
		return schedules.reduce((acc, item) => {
			const workDate = item?.workDate
			const id = item?.id
			if (!workDate || !id) return acc

			if (!acc[workDate]) {
				acc[workDate] = []
			}

			acc[workDate].push(id)
			return acc
		}, {})
	}, [schedules])

	const getDaySelectionState = (dateString) => {
		const dayScheduleIds = scheduleIdsByDate[dateString] || []
		const selectedCount = dayScheduleIds.filter((id) => selectedScheduleIds.includes(id)).length

		return {
			count: dayScheduleIds.length,
			checked: dayScheduleIds.length > 0 && selectedCount === dayScheduleIds.length,
			indeterminate: selectedCount > 0 && selectedCount < dayScheduleIds.length,
		}
	}

	const handleToggleDaySelected = (dateString, checked) => {
		const dayScheduleIds = scheduleIdsByDate[dateString] || []
		if (!dayScheduleIds.length) return

		setSelectedScheduleIds((prev) => {
			if (checked) {
				const next = new Set([...prev, ...dayScheduleIds])
				return Array.from(next)
			}

			return prev.filter((id) => !dayScheduleIds.includes(id))
		})
	}

	const handlePublish = async () => {
		if (!selectedScheduleIds.length) return
		const isConfirmed = await confirm({
			confirmText: t('work_schedule.button.publish'),
			confirmColor: 'success',
			title: t('work_schedule.dialog.publish_title'),
			description: t('work_schedule.dialog.publish_description'),
		})
		if (!isConfirmed) return

		const response = await publishSchedule.submit({
			overrideData: {
				workScheduleIds: selectedScheduleIds,
			},
		})
		if (!response) return

		setSelectedScheduleIds([])
		await getSchedules.fetch()
	}

	const handleLock = async () => {
		if (!selectedScheduleIds.length) return
		const isConfirmed = await confirm({
			confirmText: t('work_schedule.button.lock'),
			confirmColor: 'warning',
			title: t('work_schedule.dialog.lock_title'),
			description: t('work_schedule.dialog.lock_description'),
		})
		if (!isConfirmed) return

		const response = await lockSchedule.submit({
			overrideData: {
				workScheduleIds: selectedScheduleIds,
			},
		})
		if (!response) return

		setSelectedScheduleIds([])
		await getSchedules.fetch()
	}

	useEffect(() => {
		const currentIds = new Set(schedules.map((item) => item?.id).filter(Boolean))
		setSelectedScheduleIds((prev) => prev.filter((id) => currentIds.has(id)))
	}, [schedules])

	const handleFinalize = async () => {
		if (!selectedScheduleIds.length) return
		const isConfirmed = await confirm({
			confirmText: t('work_schedule.button.finalize'),
			confirmColor: 'secondary',
			title: t('work_schedule.dialog.finalize_title'),
			description: t('work_schedule.dialog.finalize_description'),
		})
		if (!isConfirmed) return

		const response = await finalizeSchedule.submit({
			overrideData: {
				workScheduleIds: selectedScheduleIds,
			},
		})
		if (!response) return

		setSelectedScheduleIds([])
		await getSchedules.fetch()
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('work_schedule.title.management')}</Typography>

				<Stack direction='row' alignItems='center' justifyContent='flex-end' spacing={1}>
					<Button
						variant='contained'
						color='success'
						onClick={handlePublish}
						disabled={!selectedScheduleIds.length}
					>
						{t('work_schedule.button.publish')}
					</Button>
					<Button
						variant='contained'
						color='warning'
						onClick={handleLock}
						disabled={!selectedScheduleIds.length}
					>
						{t('work_schedule.button.lock')}
					</Button>
					<Button
						variant='contained'
						color='secondary'
						onClick={handleFinalize}
						disabled={!selectedScheduleIds.length}
					>
						{t('work_schedule.button.finalize')}
					</Button>
					<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
						{t('work_schedule.button.create')}
					</Button>
				</Stack>

				<FullCalendar
					plugins={[dayGridPlugin, interactionPlugin]}
					locales={[viLocale]}
					locale={String(language || '').includes('vi') ? 'vi' : 'en'}
					initialView='dayGridMonth'
					headerToolbar={{
						left: 'prev,next today',
						center: 'title',
						right: '',
					}}
					buttonText={{
						today: t('work_schedule.button.today'),
					}}
					height='auto'
					events={calendarEvents}
					eventContent={(eventInfo) => {
						const schedule = eventInfo.event.extendedProps?.schedule
						const scheduleId = schedule?.id
						const isChecked = selectedScheduleIds.includes(scheduleId)
						const statusColor = defaultWorkScheduleStatusStyle(schedule?.workScheduleStatus)
						const palette = resolveEventPalette(theme, statusColor)

						return (
							<Stack
								direction='row'
								spacing={0.5}
								alignItems='center'
								sx={{
									px: 0.25,
									py: 0.125,
									borderRadius: 1,
									border: `1px solid ${palette.borderColor}`,
									bgcolor: palette.backgroundColor,
									color: palette.textColor,
									opacity: isChecked ? 1 : 0.92,
								}}
							>
								<Checkbox
									size='small'
									checked={isChecked}
									sx={{ p: 0.25, color: palette.textColor }}
									onClick={(event) => event.stopPropagation()}
									onChange={(event) => {
										event.stopPropagation()
										handleToggleSelected(scheduleId)
									}}
								/>
								<Stack spacing={0} sx={{ minWidth: 0 }}>
									<Typography variant='caption' sx={{ lineHeight: 1.1 }} noWrap>
										{eventInfo.timeText ? `${eventInfo.timeText} ` : ''}
										{eventInfo.event.title}
									</Typography>
								</Stack>
							</Stack>
						)
					}}
					dayCellContent={(dayCellArg) => {
						const dateString = formatDateToSqlDate(dayCellArg.date)
						const daySelection = getDaySelectionState(dateString)

						return (
							<Stack
								direction='row'
								alignItems='center'
								justifyContent='space-between'
								sx={{ width: '100%' }}
							>
								<Typography variant='caption'>{dayCellArg.dayNumberText}</Typography>
								<Checkbox
									size='small'
									disabled={!daySelection.count}
									checked={daySelection.checked}
									indeterminate={daySelection.indeterminate}
									onClick={(event) => event.stopPropagation()}
									onChange={(event) => {
										event.stopPropagation()
										handleToggleDaySelected(dateString, event.target.checked)
									}}
								/>
							</Stack>
						)
					}}
					datesSet={handleDatesSet}
					eventClick={handleEventClick}
					eventTimeFormat={{ hour: '2-digit', minute: '2-digit', hour12: false }}
					nowIndicator
					dayMaxEvents
				/>
			</Stack>

			<Drawer
				anchor='right'
				open={Boolean(drawerSchedule)}
				onClose={() => setDrawerSchedule(null)}
				PaperProps={{ sx: { width: { xs: '100%', sm: 520 }, p: 2 } }}
			>
				<Stack spacing={2}>
					<Stack direction='row' justifyContent='space-between' alignItems='center'>
						<Typography variant='h6'>{t('work_schedule.title.detail')}</Typography>
						<IconButton onClick={() => setDrawerSchedule(null)}>
							<CloseRoundedIcon />
						</IconButton>
					</Stack>

					{drawerSchedule ? (
						<Stack spacing={1.5}>
							<Typography variant='body2'>
								{t('work_schedule.field.work_date')}:{' '}
								{formatDateBasedOnCurrentLanguage(drawerSchedule.workDate)}
							</Typography>
							<Typography variant='body2'>
								{t('work_schedule.field.shift')}:{' '}
								{drawerSchedule.shift?.name
									? `${drawerSchedule.shift?.name} (${drawerSchedule.shift?.startTime} - ${drawerSchedule.shift?.endTime})`
									: renderEmptyFallback(null)}
							</Typography>
							<Stack direction='row' spacing={1} alignItems='center'>
								<Typography variant='body2'>{t('work_schedule.field.status')}:</Typography>
								<Chip
									size='small'
									label={
										getEnumLabelByValue(_enum.workScheduleStatusOptions, drawerSchedule.workScheduleStatus) ||
										drawerSchedule.workScheduleStatus
									}
									color={defaultWorkScheduleStatusStyle(drawerSchedule.workScheduleStatus)}
								/>
							</Stack>

							<Stack direction='row' spacing={1} flexWrap='wrap'>
								<Button
									variant='outlined'
									onClick={() =>
										navigate(
											routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_MANAGEMENT.DETAIL(drawerSchedule.id))
										)
									}
								>
									{t('work_schedule.button.open')}
								</Button>
								<Button
									variant='outlined'
									disabled={!canManageSchedule(drawerSchedule)}
									onClick={() => {
										setSelectedRow(drawerSchedule)
										setOpenUpdate(true)
									}}
								>
									{t('button.edit')}
								</Button>
								<Button
									variant='outlined'
									color='error'
									disabled={!canManageSchedule(drawerSchedule)}
									onClick={() => handleDelete(drawerSchedule)}
								>
									{t('button.delete')}
								</Button>
							</Stack>
						</Stack>
					) : null}

					<Button variant='outlined' onClick={() => setDrawerSchedule(null)}>
						{t('button.close')}
					</Button>
				</Stack>
			</Drawer>

			<WorkScheduleManagementFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onCreate={handleCreate}
				onUpdate={handleUpdate}
				shiftOptions={shiftOptions}
				templateOptions={templateOptions}
			/>
		</Paper>
	)
}

export default WorkScheduleManagementPage

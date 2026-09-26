import { defaultWorkScheduleStatusStyle } from '@/configs/defaultStylesConfig'
import useTranslation from '@/hooks/useTranslation'
import StaffWorkScheduleAssignmentCardSection from '@/pages/staffs/staffWorkSchedulePage/sections/StaffWorkScheduleAssignmentCardSection'
import { resolveDateTime, resolveEventPalette } from '@/utils/commons'
import { formatDateToSqlDate } from '@/utils/formatDateUtil'
import viLocale from '@fullcalendar/core/locales/vi'
import dayGridPlugin from '@fullcalendar/daygrid'
import interactionPlugin from '@fullcalendar/interaction'
import FullCalendar from '@fullcalendar/react'
import CloseRoundedIcon from '@mui/icons-material/CloseRounded'
import { Box, Button, Drawer, IconButton, Paper, Stack, Typography, useTheme } from '@mui/material'
import { useMemo, useState } from 'react'

const resolveEventRange = (item) => {
	const shiftStart = resolveDateTime(item?.schedule?.workDate, item?.schedule?.shift?.startTime)
	const shiftEnd = resolveDateTime(item?.schedule?.workDate, item?.schedule?.shift?.endTime)

	if (shiftStart) {
		return { start: shiftStart, end: shiftEnd }
	}

	const segments = (item?.assignment?.workSegments || [])
		.filter((segment) => segment?.startTime)
		.sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime())

	if (segments.length) {
		const start = segments[0]?.startTime || null
		const end = segments[segments.length - 1]?.endTime || null
		return { start, end }
	}

	return { start: null, end: null }
}

const StaffWorkScheduleContentSection = ({
	scheduleAssignments = [],
	onVisibleRangeChange = (nextDateRange) => nextDateRange,
	onOpenLeaveRequest = () => {},
}) => {
	const { language, t } = useTranslation()
	const theme = useTheme()
	const [selectedItem, setSelectedItem] = useState(null)

	const calendarEvents = useMemo(
		() =>
			scheduleAssignments
				.map((item) => {
					const { start, end } = resolveEventRange(item)
					if (!start) return null

					const color = defaultWorkScheduleStatusStyle(item?.schedule?.workScheduleStatus)
					const palette = resolveEventPalette(theme, color)

					return {
						id: item.key,
						title: `${item?.schedule?.shift?.name}`,
						start,
						end: end || undefined,
						allDay: false,
						backgroundColor: palette.backgroundColor,
						borderColor: palette.borderColor,
						textColor: palette.textColor,
						extendedProps: { item },
					}
				})
				.filter(Boolean),
		[scheduleAssignments, theme]
	)

	const handleDatesSet = ({ view, start, end }) => {
		const monthStart = view?.currentStart || start
		const monthEndExclusive = view?.currentEnd || end
		if (!monthStart || !monthEndExclusive) return

		const fromDate = formatDateToSqlDate(monthStart)
		const toDateRaw = new Date(monthEndExclusive)
		toDateRaw.setDate(toDateRaw.getDate() - 1)
		const toDate = formatDateToSqlDate(toDateRaw)

		onVisibleRangeChange({ fromDate, toDate })
	}

	const handleEventClick = (eventClickInfo) => {
		setSelectedItem(eventClickInfo?.event?.extendedProps?.item || null)
	}

	return (
		<>
			<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
				<Stack spacing={2}>
					<Stack
						direction={{ xs: 'column', md: 'row' }}
						justifyContent='space-between'
						alignItems={{ xs: 'stretch', md: 'center' }}
						spacing={1.5}
					>
						<Box>
							<Typography variant='h6'>{t('work_schedule.title.calendar')}</Typography>
						</Box>

						<Button variant='contained' onClick={onOpenLeaveRequest}>
							{t('leave_request.button.new_request')}
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
						datesSet={handleDatesSet}
						eventClick={handleEventClick}
						eventTimeFormat={{ hour: '2-digit', minute: '2-digit', hour12: false }}
						nowIndicator
						dayMaxEvents
					/>
				</Stack>
			</Paper>

			<Drawer
				anchor='right'
				open={Boolean(selectedItem)}
				onClose={() => setSelectedItem(null)}
				PaperProps={{ sx: { width: { xs: '100%', sm: 520 }, p: 2 } }}
			>
				<Stack spacing={2}>
					<Stack direction='row' justifyContent='space-between' alignItems='center'>
						<Typography variant='h6'>{t('work_schedule.title.shift_detail')}</Typography>
						<IconButton onClick={() => setSelectedItem(null)}>
							<CloseRoundedIcon />
						</IconButton>
					</Stack>

					{selectedItem ? (
						<StaffWorkScheduleAssignmentCardSection
							schedule={selectedItem.schedule}
							assignment={selectedItem.assignment}
						/>
					) : null}

					<Button variant='outlined' onClick={() => setSelectedItem(null)}>
						{t('button.close')}
					</Button>
				</Stack>
			</Drawer>
		</>
	)
}

export default StaffWorkScheduleContentSection

import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import {
	formatDateBasedOnCurrentLanguage,
	formatDateKey,
	formatTimeToHourMinute,
	parseDateKey,
} from '@/utils/formatDateUtil'
import { ChevronLeft, ChevronRight } from '@mui/icons-material'
import {
	Box,
	Button,
	Dialog,
	DialogContent,
	DialogTitle,
	IconButton,
	Skeleton,
	Stack,
	Typography,
} from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const getDisplayCapacity = (timeSlot, appointmentMeetingType) => {
	if (appointmentMeetingType === EnumConfig.AppointmentMeetingType.InPerson) {
		return {
			available: timeSlot.availableCapacityForInPerson,
			max: timeSlot.maxCapacityForInPerson,
		}
	} else if (appointmentMeetingType === EnumConfig.AppointmentMeetingType.Telehealth) {
		return {
			available: timeSlot.availableCapacityForTeleHealth,
			max: timeSlot.maxCapacityForTeleHealth,
		}
	} else {
		return {
			available: 0,
			max: 0,
		}
	}
}

const CreateAppointmentTimeSlotChoiceSection = ({
	date,
	timeSlotId,
	timeSlots,
	appointmentMeetingType,
	onDateChange,
	onTimeSlotChange,
	disabled,
	loading,
}) => {
	const { t } = useTranslation()
	const [open, setOpen] = useState(false)

	const [viewMonth, setViewMonth] = useState(() => {
		const today = new Date()
		return new Date(today.getFullYear(), today.getMonth(), 1)
	})

	useEffect(() => {
		if (!date) return
		const selectedDate = parseDateKey(date)
		setViewMonth(new Date(selectedDate.getFullYear(), selectedDate.getMonth(), 1))
	}, [date])

	const slotButtons = useMemo(
		() =>
			(timeSlots || []).map((slot) => {
				const capacity = getDisplayCapacity(slot, appointmentMeetingType)
				return {
					...slot,
					available: capacity.available,
					max: capacity.max,
				}
			}),
		[timeSlots, appointmentMeetingType]
	)

	const firstDayIndex = useMemo(() => {
		const day = new Date(viewMonth.getFullYear(), viewMonth.getMonth(), 1).getDay()
		return day
	}, [viewMonth])

	const daysInMonth = useMemo(() => {
		return new Date(viewMonth.getFullYear(), viewMonth.getMonth() + 1, 0).getDate()
	}, [viewMonth])

	const today = useMemo(() => {
		const now = new Date()
		return new Date(now.getFullYear(), now.getMonth(), now.getDate())
	}, [])

	const monthLabel = viewMonth.toLocaleDateString(undefined, {
		year: 'numeric',
		month: 'long',
	})

	const selectedSlot = slotButtons.find((slot) => String(slot.id) === String(timeSlotId))

	return (
		<>
			<Stack spacing={0.5}>
				<Typography variant='body2' sx={{ color: 'text.secondary' }}>
					{t('appointment.field.time_slot')}
				</Typography>
				<Button
					onClick={() => setOpen(true)}
					variant='outlined'
					disabled={disabled}
					sx={{ justifyContent: 'space-between', py: 1.2 }}
				>
					<Stack alignItems='flex-start' spacing={0.25}>
						<Typography variant='body2'>{t('appointment.button.choose_time_slot')}</Typography>
						<Typography variant='caption' sx={{ color: 'text.secondary' }}>
							{formatDateBasedOnCurrentLanguage(date) || t('appointment.text.no_date_selected')}
							{selectedSlot ? ` • ${formatTimeToHourMinute(selectedSlot.time)}` : ''}
						</Typography>
					</Stack>
				</Button>
			</Stack>

			<Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth='md'>
				<DialogTitle>{t('appointment.dialog.select_time_slot_title')}</DialogTitle>
				<DialogContent>
					<Stack spacing={2}>
						<Stack direction='row' alignItems='center' justifyContent='space-between'>
							<IconButton
								onClick={() => setViewMonth((prev) => new Date(prev.getFullYear(), prev.getMonth() - 1, 1))}
							>
								<ChevronLeft />
							</IconButton>
							<Typography variant='h6'>{monthLabel}</Typography>
							<IconButton
								onClick={() => setViewMonth((prev) => new Date(prev.getFullYear(), prev.getMonth() + 1, 1))}
							>
								<ChevronRight />
							</IconButton>
						</Stack>

						<Box
							sx={{
								display: 'grid',
								gridTemplateColumns: 'repeat(7, minmax(0, 1fr))',
								gap: 1,
							}}
						>
							{[0, 1, 2, 3, 4, 5, 6].map((idx) => (
								<Box key={`weekday-${idx}`}>
									<Typography
										variant='caption'
										sx={{ color: 'text.secondary', display: 'block', textAlign: 'center' }}
									>
										{new Date(2026, 0, idx + 4).toLocaleDateString(undefined, {
											weekday: 'short',
										})}
									</Typography>
								</Box>
							))}

							{Array.from({ length: firstDayIndex }).map((_, idx) => (
								<Box key={`pad-${idx}`} />
							))}

							{Array.from({ length: daysInMonth }).map((_, idx) => {
								const current = new Date(viewMonth.getFullYear(), viewMonth.getMonth(), idx + 1)
								const dateKey = formatDateKey(current)
								const active = dateKey === date
								const disabledDate = current < today

								return (
									<Box key={dateKey}>
										<Button
											fullWidth
											variant={active ? 'contained' : 'text'}
											disabled={disabledDate}
											onClick={() => onDateChange(dateKey)}
										>
											{idx + 1}
										</Button>
									</Box>
								)
							})}
						</Box>

						<Stack spacing={1}>
							<Typography variant='subtitle1'>{t('appointment.title.available_time_slots')}</Typography>
							{loading ? (
								<Box
									sx={{
										display: 'grid',
										gridTemplateColumns: 'repeat(auto-fill, minmax(100px, 1fr))',
										gap: 0.75,
									}}
								>
									{Array.from({ length: 10 }).map((_, idx) => (
										<Skeleton key={idx} height={50} variant='rounded' />
									))}
								</Box>
							) : slotButtons.length === 0 ? (
								<Typography variant='body2' sx={{ color: 'text.secondary' }}>
									{t('appointment.text.no_time_slot_available')}
								</Typography>
							) : (
								<Box
									sx={{
										display: 'grid',
										gridTemplateColumns: 'repeat(auto-fill, minmax(100px, 1fr))',
										gap: 0.75,
									}}
								>
									{slotButtons.map((slot) => {
										const selected = String(timeSlotId) === String(slot.id)
										const disabledSlot = slot.available <= 0

										return (
											<Button
												key={slot.id}
												fullWidth
												size='small'
												variant={selected ? 'contained' : 'outlined'}
												disabled={disabledSlot}
												onClick={() => onTimeSlotChange(String(slot.id))}
												sx={{ py: 0.5, minHeight: 50, textTransform: 'none' }}
											>
												<Stack spacing={0.1} sx={{ width: '100%' }}>
													<Typography variant='caption' fontWeight={600}>
														{formatTimeToHourMinute(slot.time)}
													</Typography>
													<Typography variant='caption'>
														{t('appointment.text.time_slot_booked', {
															booked: Math.max(0, slot.max - slot.available),
															max: slot.max,
														})}
													</Typography>
												</Stack>
											</Button>
										)
									})}
								</Box>
							)}
						</Stack>

						<Stack direction='row' justifyContent='flex-end'>
							<Button variant='contained' onClick={() => setOpen(false)}>
								{t('button.confirm')}
							</Button>
						</Stack>
					</Stack>
				</DialogContent>
			</Dialog>
		</>
	)
}

export default CreateAppointmentTimeSlotChoiceSection

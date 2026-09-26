import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import {
	Button,
	Checkbox,
	Paper,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Typography,
} from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const WeeklyShiftRuleSection = ({ dayShiftsData, shifts, onSave, loading = false }) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const dayColumns = _enum.dayOfWeekOptions
	const mondayValue = EnumConfig.DayOfWeek.Monday

	const [weeklyState, setWeeklyState] = useState({})
	const [initialState, setInitialState] = useState({})

	useEffect(() => {
		const dayValues = Object.values(EnumConfig.DayOfWeek)
		const shiftIdSet = new Set((shifts || []).map((s) => s.id))
		const next = {}
		for (const day of dayValues) {
			next[day] = []
		}
		for (const item of dayShiftsData || []) {
			if (!shiftIdSet.has(item.shiftId) || !next[item.dayOfWeek]) continue
			if (!next[item.dayOfWeek].includes(item.shiftId)) {
				next[item.dayOfWeek].push(item.shiftId)
			}
		}
		setWeeklyState(next)
		setInitialState(next)
	}, [dayShiftsData, shifts])

	const hasChanges = useMemo(() => {
		const days = Object.keys(initialState)
		if (days.length !== Object.keys(weeklyState).length) return true
		for (const day of days) {
			const a = [...(initialState[day] || [])].sort()
			const b = [...(weeklyState[day] || [])].sort()
			if (a.length !== b.length || a.some((v, i) => v !== b[i])) return true
		}
		return false
	}, [weeklyState, initialState])

	const dayShiftPayload = useMemo(() => {
		const payload = []
		for (const day of dayColumns) {
			for (const shiftId of weeklyState[day.value] || []) {
				payload.push({ dayOfWeek: day.value, shiftId })
			}
		}
		return payload
	}, [dayColumns, weeklyState])

	const toggleShiftForDay = (dayValue, shiftId) => {
		setWeeklyState((prev) => {
			const current = prev[dayValue] || []
			const isChecked = current.includes(shiftId)
			return {
				...prev,
				[dayValue]: isChecked ? current.filter((id) => id !== shiftId) : [...current, shiftId],
			}
		})
	}

	const copyMondayToDay = (targetDayValue) => {
		setWeeklyState((prev) => ({
			...prev,
			[targetDayValue]: [...(prev[mondayValue] || [])],
		}))
	}

	const selectAll = () => {
		const allShiftIds = (shifts || []).map((s) => s.id)
		const next = {}
		for (const day of dayColumns) {
			next[day.value] = allShiftIds
		}
		setWeeklyState(next)
	}

	const clearAll = () => {
		const next = {}
		for (const day of dayColumns) {
			next[day.value] = []
		}
		setWeeklyState(next)
	}

	if (loading) {
		return (
			<Paper variant='outlined' sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Typography variant='h6'>{t('work_schedule_template.title.weekly_shift_rule')}</Typography>
					<SkeletonBox numberOfBoxes={4} heights={[40, 40, 40, 40]} rounded />
				</Stack>
			</Paper>
		)
	}

	return (
		<Paper variant='outlined' sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h6'>{t('work_schedule_template.title.weekly_shift_rule')}</Typography>

				<TableContainer>
					<Table size='small'>
						<TableHead>
							<TableRow>
								<TableCell>{t('work_schedule_template.field.shift')}</TableCell>
								<TableCell>{t('work_schedule_template.field.time')}</TableCell>
								{dayColumns.map((day) => (
									<TableCell key={day.value} align='center'>
										{day.label}
									</TableCell>
								))}
							</TableRow>
						</TableHead>
						<TableBody>
							{(shifts || []).map((shift) => (
								<TableRow key={shift.id}>
									<TableCell>{shift.name}</TableCell>
									<TableCell sx={{ whiteSpace: 'nowrap' }}>
										{shift.startTime} – {shift.endTime}
									</TableCell>
									{dayColumns.map((day) => (
										<TableCell key={`${shift.id}-${day.value}`} align='center'>
											<Checkbox
												size='small'
												checked={(weeklyState[day.value] || []).includes(shift.id)}
												onChange={() => toggleShiftForDay(day.value, shift.id)}
											/>
										</TableCell>
									))}
								</TableRow>
							))}
						</TableBody>
					</Table>
				</TableContainer>

				<Stack direction='row' gap={1} flexWrap='wrap' alignItems='center'>
					<Typography variant='body2'>{t('work_schedule_template.text.copy_monday_to')}</Typography>
					{dayColumns
						.filter((day) => day.value !== mondayValue)
						.map((day) => (
							<Button
								key={day.value}
								size='small'
								variant='outlined'
								onClick={() => copyMondayToDay(day.value)}
							>
								{day.label}
							</Button>
						))}
					<Button size='small' variant='outlined' color='inherit' onClick={selectAll}>
						{t('work_schedule_template.button.select_all')}
					</Button>
					<Button size='small' variant='outlined' color='inherit' onClick={clearAll}>
						{t('work_schedule_template.button.clear_all')}
					</Button>
				</Stack>

				<Button variant='contained' disabled={!hasChanges} onClick={() => onSave?.(dayShiftPayload)}>
					{t('work_schedule_template.button.save_weekly_rules')}
				</Button>
			</Stack>
		</Paper>
	)
}

export default WeeklyShiftRuleSection

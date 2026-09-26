import { defaultWorkScheduleStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import StaffWorkScheduleSegmentRowSection from '@/pages/staffs/staffWorkSchedulePage/sections/StaffWorkScheduleSegmentRowSection'
import { formatDateBasedOnCurrentLanguage, formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import CalendarTodayRoundedIcon from '@mui/icons-material/CalendarTodayRounded'
import PlaceRoundedIcon from '@mui/icons-material/PlaceRounded'
import { Chip, Divider, Paper, Stack, Typography } from '@mui/material'
import { useMemo } from 'react'

const getLocationLabel = (assignment) => {
	const roomCode = assignment?.room?.code
	const teleCode = assignment?.teleRoom?.roomCode
	return roomCode || teleCode || `#${assignment?.locationId || '--'}`
}

const StaffWorkScheduleAssignmentCardSection = ({ assignment, schedule }) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const segments = useMemo(
		() =>
			(assignment?.workSegments || []).sort(
				(a, b) => new Date(a?.startTime || 0).getTime() - new Date(b?.startTime || 0).getTime()
			),
		[assignment]
	)

	return (
		<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
			<Stack spacing={1.5}>
				<Stack
					direction='row'
					justifyContent='space-between'
					alignItems='center'
					flexWrap='wrap'
					gap={1}
				>
					<Stack direction='row' spacing={1} alignItems='center'>
						<CalendarTodayRoundedIcon fontSize='small' color='action' />
						<Typography fontWeight={700}>{formatDateBasedOnCurrentLanguage(schedule?.workDate)}</Typography>
						<Typography color='text.secondary'>
							{formatTimeToHourMinute(schedule?.shift?.startTime)} -{' '}
							{formatTimeToHourMinute(schedule?.shift?.endTime)}
						</Typography>
					</Stack>
					<Chip
						size='small'
						color={defaultWorkScheduleStatusStyle(schedule?.workScheduleStatus)}
						label={
							getEnumLabelByValue(_enum.workScheduleStatusOptions, schedule?.workScheduleStatus) ||
							schedule?.workScheduleStatus ||
							'--'
						}
					/>
				</Stack>

				<Stack direction='row' spacing={1} alignItems='center'>
					<PlaceRoundedIcon fontSize='small' color='action' />
					<Typography variant='body2' color='text.secondary'>
						{t('work_schedule.field.location_type')}:{' '}
						{getEnumLabelByValue(_enum.locationTypeOptions, assignment?.locationType) ||
							assignment?.locationType ||
							'--'}{' '}
						- {getLocationLabel(assignment)}
					</Typography>
				</Stack>

				<Divider />

				<Stack spacing={1}>
					{segments.length > 0 ? (
						segments.map((segment) => (
							<StaffWorkScheduleSegmentRowSection key={segment?.id} segment={segment} />
						))
					) : (
						<Typography variant='body2' color='text.secondary'>
							{t('work_schedule.text.no_work_segments')}
						</Typography>
					)}
				</Stack>
			</Stack>
		</Paper>
	)
}

export default StaffWorkScheduleAssignmentCardSection

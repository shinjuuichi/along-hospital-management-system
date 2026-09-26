import { defaultWorkStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import { formatDateTimeToTimeOnly } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Chip, Paper, Stack, Typography } from '@mui/material'

const StaffWorkScheduleSegmentRowSection = ({ segment }) => {
	const _enum = useEnum()

	return (
		<Paper
			variant='outlined'
			sx={{
				px: 1.5,
				py: 1.25,
				borderRadius: 2,
			}}
		>
			<Stack direction={{ xs: 'column', md: 'row' }} spacing={1.5} alignItems={{ md: 'center' }}>
				<Typography fontWeight={700} sx={{ minWidth: 130 }}>
					{formatDateTimeToTimeOnly(segment?.startTime)} -{' '}
					{formatDateTimeToTimeOnly(segment?.endTime)}
				</Typography>
				<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
					<Chip
						size='small'
						color={defaultWorkStatusStyle(segment?.workStatus)}
						label={
							renderEmptyFallback(
								getEnumLabelByValue(_enum.workStatusOptions, segment?.workStatus) || segment?.workStatus
							)
						}
					/>
				</Stack>
				<Typography color='text.secondary' sx={{ flex: 1 }}>
					{renderEmptyFallback(
						getEnumLabelByValue(_enum.workStatusReasonOptions, segment?.workStatusReason) ||
							segment?.workStatusReason
					)}
				</Typography>
			</Stack>
		</Paper>
	)
}

export default StaffWorkScheduleSegmentRowSection

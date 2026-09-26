import { EnumConfig } from '@/configs/enumConfig'
import { formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

export const formatLeaveShiftValue = (row) => {
	const isShiftLeaveUnit = row?.leaveUnit === EnumConfig.LeaveUnit.Shift
	if (!row || !isShiftLeaveUnit) return renderEmptyFallback(null)

	const shiftName = row.shiftName || (row.shiftId ? `#${row.shiftId}` : '')
	const shiftStartTime = formatTimeToHourMinute(row.shiftStartTime)
	const shiftEndTime = formatTimeToHourMinute(row.shiftEndTime)

	if (!shiftName && !shiftStartTime && !shiftEndTime) {
		return renderEmptyFallback(null)
	}

	if (shiftName && shiftStartTime && shiftEndTime) {
		return `${shiftName} (${shiftStartTime} - ${shiftEndTime})`
	}

	return shiftName || `${shiftStartTime} - ${shiftEndTime}`
}

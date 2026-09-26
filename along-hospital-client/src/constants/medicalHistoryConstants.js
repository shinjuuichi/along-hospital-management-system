import { EnumConfig } from '@/configs/enumConfig'

export const AssignBedDialogMode = Object.freeze({
	Assign: 'assign',
	Transfer: 'transfer',
})

export const BedOccupancyStoreRoles = Object.freeze([
	EnumConfig.Role.Receptionist,
	EnumConfig.Role.Nurse,
])

export const BedOccupancyManagementRoles = Object.freeze([
	EnumConfig.Role.Manager,
	EnumConfig.Role.Doctor,
	EnumConfig.Role.Nurse,
	EnumConfig.Role.Accountant,
	EnumConfig.Role.Receptionist,
])

export const BedOccupancyTimelineViewMode = Object.freeze({
	Compact: 'compact',
	Detailed: 'detailed',
})

export const BedOccupancyCompactTimelineScrollItemWidth = 220
export const BedOccupancyDetailedTimelineScrollItemWidth = 240
export const BedOccupancySingleTimelineItemMaxWidth = 380
export const BedOccupancyCompactStackPreviewCount = 3
export const BedOccupancyCompactStackOffsetX = 18
export const BedOccupancyCompactStackOffsetY = 12
export const BedOccupancyDetailHighlightDurationMs = 1800
export const BedOccupancyMinuteInMs = 60 * 1000
export const BedOccupancyHourInMs = 60 * BedOccupancyMinuteInMs
export const BedOccupancyDayInMs = 24 * BedOccupancyHourInMs

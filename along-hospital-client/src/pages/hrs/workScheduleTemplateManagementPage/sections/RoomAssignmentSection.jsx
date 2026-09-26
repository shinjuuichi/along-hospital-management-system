import AssignmentGridSection from '@/components/generals/AssignmentGridSection'
import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { useCallback, useMemo } from 'react'
import {
	getEntitySpecialtyName,
	getLocationSpecialtyId,
	getStaffRoles,
	getStaffSpecialtyId,
	hasAnyMatchingRole,
	hasRole,
} from '../helper/workScheduleTemplateHelper'

const RoomAssignmentSection = ({
	assignments,
	otherShiftAssignments,
	shifts,
	rooms,
	specialties,
	staffs,
	onAdd,
	onRemove,
	loading = false,
}) => {
	const { t } = useTranslation()
	const specialtyMap = useMemo(
		() => new Map((specialties || []).map((specialty) => [specialty?.id, specialty?.name])),
		[specialties]
	)

	const getRoomLabel = useCallback((room) => room?.code, [])
	const getRoomSubLabel = useCallback((room) => room?.roomCategoryName || room?.categoryName, [])
	const getRoomRoles = useCallback(
		(room) => (room?.roles || []).filter((role) => role !== EnumConfig.Role.Patient),
		[]
	)
	const getRoomMetaLabel = useCallback(
		(room) => {
			const specialtyName = getEntitySpecialtyName(room, specialtyMap)
			return specialtyName ? `${t('room.field.specialty')}: ${specialtyName}` : ''
		},
		[specialtyMap, t]
	)
	const getStaffMetaLabel = useCallback(
		(staff) => {
			const specialtyName = getEntitySpecialtyName(staff, specialtyMap)
			return `${t('staff.field.specialty')}: ${specialtyName || t('text.none')}`
		},
		[specialtyMap, t]
	)
	const isStaffAssignableToRoom = useCallback(
		(staff, room) => {
			if (!staff || !room) return false

			const allowedRoles = getRoomRoles(room)
			if (!allowedRoles.length || !hasAnyMatchingRole(staff, allowedRoles)) return false
			if (!hasRole(staff, EnumConfig.Role.Doctor)) return true

			const staffSpecialtyId = getStaffSpecialtyId(staff)
			const roomSpecialtyId = getLocationSpecialtyId(room)
			return (
				staffSpecialtyId != null &&
				roomSpecialtyId != null &&
				staffSpecialtyId === roomSpecialtyId
			)
		},
		[getRoomRoles]
	)

	return (
		<AssignmentGridSection
			title={t('work_schedule_template.title.assign_staff_to_inpatient_room')}
			assignments={assignments}
			otherShiftAssignments={otherShiftAssignments}
			shifts={shifts}
			locations={rooms}
			staffs={staffs}
			locationIdKey='roomId'
			getLocationLabel={getRoomLabel}
			getLocationSubLabel={getRoomSubLabel}
			getLocationMetaLabel={getRoomMetaLabel}
			getLocationRoles={getRoomRoles}
			getStaffRoles={getStaffRoles}
			getStaffMetaLabel={getStaffMetaLabel}
			isStaffAssignable={isStaffAssignableToRoom}
			onAdd={onAdd}
			onRemove={onRemove}
			assignLabel={t('work_schedule_template.button.assign_staff')}
			loading={loading}
		/>
	)
}

export default RoomAssignmentSection

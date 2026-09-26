import AssignmentGridSection from '@/components/generals/AssignmentGridSection'
import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { useCallback, useMemo } from 'react'
import {
	getEntitySpecialtyName,
	getLocationSpecialtyId,
	getStaffRoles,
	getStaffSpecialtyId,
	hasRole,
} from '../helper/workScheduleTemplateHelper'

const DOCTOR_ROLES = [EnumConfig.Role.Doctor]

const TeleRoomAssignmentSection = ({
	assignments,
	otherShiftAssignments,
	shifts,
	teleRooms,
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

	const getRoomLabel = useCallback((teleRoom) => teleRoom?.roomCode, [])
	const getRoomSubLabel = useCallback((teleRoom) => teleRoom?.roomDisplayName, [])
	const getRoomRoles = useCallback(() => DOCTOR_ROLES, [])
	const getRoomMetaLabel = useCallback(
		(teleRoom) => {
			const specialtyName = getEntitySpecialtyName(teleRoom, specialtyMap)
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
	const isStaffAssignableToTeleRoom = useCallback((staff, teleRoom) => {
		if (!staff || !teleRoom) return false
		if (!hasRole(staff, EnumConfig.Role.Doctor)) return false

		const staffSpecialtyId = getStaffSpecialtyId(staff)
		const teleRoomSpecialtyId = getLocationSpecialtyId(teleRoom)
		return (
			staffSpecialtyId != null &&
			teleRoomSpecialtyId != null &&
			staffSpecialtyId === teleRoomSpecialtyId
		)
	}, [])

	return (
		<AssignmentGridSection
			title={t('work_schedule_template.title.assign_doctor_to_tele_room')}
			assignments={assignments}
			otherShiftAssignments={otherShiftAssignments}
			shifts={shifts}
			locations={teleRooms}
			staffs={staffs}
			locationIdKey='teleRoomId'
			getLocationLabel={getRoomLabel}
			getLocationSubLabel={getRoomSubLabel}
			getLocationMetaLabel={getRoomMetaLabel}
			getLocationRoles={getRoomRoles}
			getStaffRoles={getStaffRoles}
			getStaffMetaLabel={getStaffMetaLabel}
			isStaffAssignable={isStaffAssignableToTeleRoom}
			onAdd={onAdd}
			onRemove={onRemove}
			assignLabel={t('work_schedule_template.button.assign_doctor')}
			loading={loading}
		/>
	)
}

export default TeleRoomAssignmentSection

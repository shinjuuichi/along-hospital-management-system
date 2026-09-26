import MultipleSelectDialog from '@/components/dialogs/commons/MultipleSelectDialog'
import GenericTabs from '@/components/generals/GenericTabs'
import StaffRenderOption from '@/components/renderOptions/StaffRenderOption'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setRoomsStore,
	setStaffsStore,
	setTeleRoomsStore,
} from '@/redux/reducers/managementReducer'
import { mergeDateAndTime } from '@/utils/formatDateUtil'
import { Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'
import WorkScheduleAssignmentTableSection from './sections/WorkScheduleAssignmentTableSection'
import WorkScheduleInfoSection from './sections/WorkScheduleInfoSection'

const AssignmentTab = {
	ROOM: 'room',
	TELE: 'tele',
}
const DOCTOR_ROLES = [EnumConfig.Role.Doctor]

const getStaffRoles = (staff) => {
	if (!staff) return []
	const roles = Array.isArray(staff.roles) ? staff.roles : staff.role ? [staff.role] : []
	return roles.map((role) => `${role || ''}`.trim()).filter(Boolean)
}

const hasAnyMatchingRole = (staff, allowedRoles = []) => {
	const allowedRoleSet = new Set(allowedRoles.map((role) => `${role || ''}`.trim()).filter(Boolean))
	if (!allowedRoleSet.size) return false

	return getStaffRoles(staff).some((staffRole) => allowedRoleSet.has(`${staffRole || ''}`.trim()))
}

const WorkScheduleManagementDetailPage = () => {
	const { t } = useTranslation()
	const { id } = useParams()
	const scheduleId = Number(id)
	const confirm = useConfirm()

	const [currentTab, setCurrentTab] = useState(AssignmentTab.ROOM)
	const [openCreate, setOpenCreate] = useState(false)
	const [selectedLocationId, setSelectedLocationId] = useState(null)

	const {
		data: schedule,
		loading,
		fetch: refetch,
	} = useFetch(ApiUrls.WORK_SCHEDULE.MANAGEMENT.DETAIL(scheduleId), {}, [scheduleId])

	const getRooms = useReduxStore({ selector: (s) => s.management.rooms, setStore: setRoomsStore })
	const getTeleRooms = useReduxStore({
		selector: (s) => s.management.teleRooms,
		setStore: setTeleRoomsStore,
	})
	const getStaffs = useReduxStore({ selector: (s) => s.management.staffs, setStore: setStaffsStore })

	const createAssignment = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE_ASSIGNMENT.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const deleteAssignment = useAxiosSubmit({ method: 'DELETE' })
	const updateWorkSegment = useAxiosSubmit({ method: 'PUT' })

	const assignments = useMemo(
		() => schedule?.workScheduleAssignments || [],
		[schedule?.workScheduleAssignments]
	)

	const assignmentsGrouped = useMemo(
		() => ({
			[AssignmentTab.ROOM]: assignments.filter((a) => a.locationType === EnumConfig.LocationType.Room),
			[AssignmentTab.TELE]: assignments.filter(
				(a) => a.locationType === EnumConfig.LocationType.TeleRoom
			),
		}),
		[assignments]
	)

	const assignmentTabs = useMemo(
		() => [
			{
				key: AssignmentTab.ROOM,
				title: `${t('work_schedule.tab.room_assignments')} (${assignmentsGrouped[AssignmentTab.ROOM].length})`,
				locationType: EnumConfig.LocationType.Room,
			},
			{
				key: AssignmentTab.TELE,
				title: `${t('work_schedule.tab.tele_room_assignments')} (${assignmentsGrouped[AssignmentTab.TELE].length})`,
				locationType: EnumConfig.LocationType.TeleRoom,
			},
		],
		[t, assignmentsGrouped]
	)

	const activeTabData = useMemo(() => {
		return assignmentTabs.find((tab) => tab.key === currentTab) || assignmentTabs[0]
	}, [assignmentTabs, currentTab])

	const canManageAssignments = useMemo(() => {
		const status = schedule?.workScheduleStatus
		return (
			status === EnumConfig.WorkScheduleStatus.Draft ||
			status === EnumConfig.WorkScheduleStatus.Published
		)
	}, [schedule?.workScheduleStatus])

	const canUpdateSegments = useMemo(
		() => schedule?.workScheduleStatus !== EnumConfig.WorkScheduleStatus.Finalized,
		[schedule?.workScheduleStatus]
	)

	const handleTabChange = useCallback((tab) => {
		if (tab?.key) setCurrentTab(tab.key)
	}, [])

	const handleAdd = useCallback(
		(locationId) => {
			if (!canManageAssignments) return
			setSelectedLocationId(locationId)
			setOpenCreate(true)
		},
		[canManageAssignments]
	)

	const handleDelete = useCallback(
		async (row) => {
			if (!canManageAssignments) return

			const isConfirmed = await confirm({
				confirmText: t('button.delete'),
				confirmColor: 'error',
				title: t('work_schedule.dialog.delete_assignment_title'),
				description: t('work_schedule.dialog.delete_assignment_description', { id: row.id }),
			})
			if (!isConfirmed) return

			const response = await deleteAssignment.submit({
				overrideUrl: ApiUrls.WORK_SCHEDULE_ASSIGNMENT.MANAGEMENT.DETAIL(row.id),
			})
			if (response) await refetch()
		},
		[confirm, t, deleteAssignment, refetch, canManageAssignments]
	)

	const selectedLocation = useMemo(() => {
		if (!selectedLocationId) return null
		const isRoom = activeTabData.locationType === EnumConfig.LocationType.Room
		const locations = isRoom ? getRooms.data || [] : getTeleRooms.data || []
		return locations.find((loc) => loc.id === selectedLocationId)
	}, [selectedLocationId, activeTabData.locationType, getRooms.data, getTeleRooms.data])

	const assignedStaffIds = useMemo(
		() => new Set((assignments || []).map((a) => a.staffId)),
		[assignments]
	)

	const allowedRoles = useMemo(() => {
		if (!selectedLocation) return []

		return activeTabData.locationType === EnumConfig.LocationType.TeleRoom
			? DOCTOR_ROLES
			: (selectedLocation.roles || []).filter((role) => role !== EnumConfig.Role.Patient)
	}, [selectedLocation, activeTabData.locationType])

	const staffOptions = useMemo(
		() =>
			(getStaffs.data || []).map((staff) => ({
				value: staff.id,
				label: staff,
				searchKey: [staff.name, staff.phone, staff.email, getStaffRoles(staff).join(' ')]
					.filter(Boolean)
					.join(' '),
				disabled: assignedStaffIds.has(staff.id) || !hasAnyMatchingRole(staff, allowedRoles),
			})),
		[getStaffs.data, assignedStaffIds, allowedRoles]
	)

	const handleStaffSelect = useCallback(
		async (staffIds) => {
			if (!canManageAssignments || !staffIds?.length || !selectedLocationId) return

			await createAssignment.submit({
				overrideData: {
					workScheduleId: scheduleId,
					staffIds,
					locationType: activeTabData.locationType,
					locationId: selectedLocationId,
				},
			})
			setOpenCreate(false)
			setSelectedLocationId(null)
			await refetch()
		},
		[
			canManageAssignments,
			selectedLocationId,
			activeTabData.locationType,
			createAssignment,
			scheduleId,
			refetch,
		]
	)

	const handleUpdateWorkSegment = useCallback(
		async ({ workSegment, values, closeDialog }) => {
			if (!workSegment?.id) return

			const payload = {
				StartTime: mergeDateAndTime(workSegment.startTime, values?.startTime),
				EndTime: mergeDateAndTime(workSegment.endTime, values?.endTime),
			}

			if (!payload.StartTime || !payload.EndTime) return

			const response = await updateWorkSegment.submit({
				overrideUrl: ApiUrls.WORK_SEGMENT.MANAGEMENT.DETAIL(workSegment.id),
				overrideData: payload,
			})
			if (response) {
				closeDialog?.()
				await refetch()
			}
		},
		[updateWorkSegment, refetch]
	)

	if (loading) {
		return (
			<Paper sx={{ p: 2 }}>
				<SkeletonBox numberOfBoxes={3} heights={[36, 80, 42]} rounded />
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('work_schedule.title.detail')}</Typography>

				<WorkScheduleInfoSection schedule={schedule} />

				<Typography variant='h6'>
					{t('work_schedule.title.assignments')} ({assignments.length})
				</Typography>

				<GenericTabs
					tabs={assignmentTabs}
					currentTab={activeTabData.key}
					setCurrentTab={handleTabChange}
				/>

				<WorkScheduleAssignmentTableSection
					assignments={assignmentsGrouped[activeTabData.key] || []}
					locationType={activeTabData.locationType}
					loading={loading}
					rooms={getRooms.data || []}
					teleRooms={getTeleRooms.data || []}
					canManageAssignments={canManageAssignments}
					canUpdateSegments={canUpdateSegments}
					onAdd={handleAdd}
					onDelete={handleDelete}
					onUpdateWorkSegment={canUpdateSegments ? handleUpdateWorkSegment : undefined}
				/>
			</Stack>

			{canManageAssignments && (
				<MultipleSelectDialog
					open={openCreate}
					onClose={() => {
						setOpenCreate(false)
						setSelectedLocationId(null)
					}}
					title={
						selectedLocation
							? `${t('work_schedule.dialog.create_assignment_title')} — ${selectedLocation.code || selectedLocation.roomCode}`
							: t('work_schedule.dialog.create_assignment_title')
					}
					options={staffOptions}
					value={[]}
					onChange={handleStaffSelect}
					renderOption={(_, staff) => <StaffRenderOption staff={staff} />}
				/>
			)}
		</Paper>
	)
}

export default WorkScheduleManagementDetailPage

import GenericTabs from '@/components/generals/GenericTabs'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import RoomAssignmentSection from '@/pages/hrs/workScheduleTemplateManagementPage/sections/RoomAssignmentSection'
import TeleRoomAssignmentSection from '@/pages/hrs/workScheduleTemplateManagementPage/sections/TeleRoomAssignmentSection'
import WeeklyShiftRuleSection from '@/pages/hrs/workScheduleTemplateManagementPage/sections/WeeklyShiftRuleSection'
import {
	setRoomsStore,
	setShiftsStore,
	setSpecialtiesStore,
	setStaffsStore,
	setTeleRoomsStore,
} from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'

const AssignmentTab = {
	ROOM: 'room',
	TELE: 'tele',
}

const WorkScheduleTemplateManagementDetailPage = () => {
	const { t } = useTranslation()
	const { id } = useParams()
	const templateId = +id
	const [currentAssignmentTab, setCurrentAssignmentTab] = useState(AssignmentTab.ROOM)

	const getTemplate = useFetch(ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DETAIL(templateId), {}, [
		templateId,
	])
	const getDayShifts = useFetch(
		ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DAY_SHIFTS(templateId),
		{},
		[templateId]
	)
	const getRoomAssignments = useFetch(
		ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.ROOM_ASSIGNMENTS(templateId),
		{},
		[templateId]
	)
	const getTeleAssignments = useFetch(
		ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.TELE_ROOM_ASSIGNMENTS(templateId),
		{},
		[templateId]
	)

	const templateLoading = getTemplate.loading
	const dayShiftsLoading = getDayShifts.loading
	const roomAssignmentsLoading = getRoomAssignments.loading
	const teleAssignmentsLoading = getTeleAssignments.loading

	const getShifts = useReduxStore({
		selector: (state) => state.management.shifts,
		setStore: setShiftsStore,
	})
	const getRooms = useReduxStore({
		selector: (state) => state.management.rooms,
		setStore: setRoomsStore,
	})
	const getTeleRooms = useReduxStore({
		selector: (state) => state.management.teleRooms,
		setStore: setTeleRoomsStore,
	})
	const getSpecialties = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})
	const getStaffs = useReduxStore({
		selector: (state) => state.management.staffs,
		setStore: setStaffsStore,
	})

	const shifts = getShifts.data || []
	const rooms = getRooms.data || []
	const teleRooms = getTeleRooms.data || []
	const specialties = getSpecialties.data || []
	const staffs = getStaffs.data || []

	const assignmentTabs = useMemo(
		() => [
			{
				key: AssignmentTab.ROOM,
				title: t('work_schedule_template.tab.room_assignments'),
			},
			{
				key: AssignmentTab.TELE,
				title: t('work_schedule_template.tab.tele_room_assignments'),
			},
		],
		[t]
	)

	const activeAssignmentTab = useMemo(
		() =>
			assignmentTabs.some((tab) => tab.key === currentAssignmentTab)
				? currentAssignmentTab
				: assignmentTabs[0]?.key || '',
		[assignmentTabs, currentAssignmentTab]
	)

	const handleAssignmentTabChange = useCallback((tab) => {
		if (tab?.key) {
			setCurrentAssignmentTab(tab.key)
		}
	}, [])

	const saveDayShifts = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			await getDayShifts.fetch()
		},
	})

	const createRoomAssignment = useAxiosSubmit({
		method: 'POST',
		onSuccess: async () => {
			await getRoomAssignments.fetch()
		},
	})

	const deleteRoomAssignment = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			await getRoomAssignments.fetch()
		},
	})

	const createTeleAssignment = useAxiosSubmit({
		method: 'POST',
		onSuccess: async () => {
			await getTeleAssignments.fetch()
		},
	})

	const deleteTeleAssignment = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			await getTeleAssignments.fetch()
		},
	})

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				{templateLoading ? (
					<SkeletonBox numberOfBoxes={2} heights={[36, 20]} rounded />
				) : (
					<>
						<Typography variant='h5'>{t('work_schedule_template.title.template_detail')}</Typography>
						<Typography variant='subtitle1'>{getTemplate.data?.name}</Typography>
						{getTemplate.data?.description && (
							<Typography variant='body2' color='text.secondary'>
								{getTemplate.data.description}
							</Typography>
						)}
					</>
				)}

				<WeeklyShiftRuleSection
					dayShiftsData={getDayShifts.data}
					shifts={shifts}
					loading={dayShiftsLoading}
					onSave={async (payload) => {
						await saveDayShifts.submit({
							overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DAY_SHIFTS(templateId),
							overrideData: payload,
						})
					}}
				/>

				<GenericTabs
					tabs={assignmentTabs}
					currentTab={activeAssignmentTab}
					setCurrentTab={handleAssignmentTabChange}
				/>

				{activeAssignmentTab === AssignmentTab.ROOM && (
					<RoomAssignmentSection
						assignments={getRoomAssignments.data}
						otherShiftAssignments={getTeleAssignments.data}
						shifts={shifts}
						rooms={rooms}
						specialties={specialties}
						staffs={staffs}
						loading={roomAssignmentsLoading}
						onAdd={async (data, resetForm) => {
							const response = await createRoomAssignment.submit({
								overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.ROOM_ASSIGNMENTS(templateId),
								overrideData: data,
							})
							if (response) {
								resetForm?.()
							}
							return response
						}}
						onRemove={async (item) => {
							await deleteRoomAssignment.submit({
								overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.ROOM_ASSIGNMENTS(templateId),
								overrideParam: {
									shiftId: item.shiftId,
									roomId: item.roomId,
									staffId: item.staffId,
								},
							})
						}}
					/>
				)}

				{activeAssignmentTab === AssignmentTab.TELE && (
					<TeleRoomAssignmentSection
						assignments={getTeleAssignments.data}
						otherShiftAssignments={getRoomAssignments.data}
						shifts={shifts}
						teleRooms={teleRooms}
						specialties={specialties}
						staffs={staffs}
						loading={teleAssignmentsLoading}
						onAdd={async (data, resetForm) => {
							const response = await createTeleAssignment.submit({
								overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.TELE_ROOM_ASSIGNMENTS(templateId),
								overrideData: data,
							})
							if (response) {
								resetForm?.()
							}
							return response
						}}
						onRemove={async (item) => {
							await deleteTeleAssignment.submit({
								overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.TELE_ROOM_ASSIGNMENTS(templateId),
								overrideParam: {
									shiftId: item.shiftId,
									teleRoomId: item.teleRoomId,
									staffId: item.staffId,
								},
							})
						}}
					/>
				)}
			</Stack>
		</Paper>
	)
}

export default WorkScheduleTemplateManagementDetailPage

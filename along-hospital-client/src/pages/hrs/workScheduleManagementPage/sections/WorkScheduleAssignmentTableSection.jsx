import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import AssignmentCardSection from '@/components/generals/AssignmentCardSection'
import SearchBar from '@/components/generals/SearchBar'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import GenericTable from '@/components/tables/GenericTable'
import { defaultWorkStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateTimeToTimeOnly, formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Dialog, Grid, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const DOCTOR_ROLES = [EnumConfig.Role.Doctor]

const WorkScheduleAssignmentTableSection = ({
	assignments,
	locationType,
	loading,
	rooms = [],
	teleRooms = [],
	canManageAssignments = true,
	canUpdateSegments = true,
	onAdd,
	onDelete,
	onUpdateWorkSegment,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [searchTerm, setSearchTerm] = useState('')
	const [selectedAssignment, setSelectedAssignment] = useState(null)
	const [selectedWorkSegment, setSelectedWorkSegment] = useState(null)
	const [segmentsDialogOpen, setSegmentsDialogOpen] = useState(false)
	const [openUpdateSegment, setOpenUpdateSegment] = useState(false)
	const canUpdateSegment = Boolean(onUpdateWorkSegment)

	const isRoom = locationType === EnumConfig.LocationType.Room
	const locations = isRoom ? rooms : teleRooms

	const filteredLocations = useMemo(() => {
		if (!searchTerm.trim()) return locations
		const term = searchTerm.toLowerCase()
		return locations.filter((loc) => {
			const label = isRoom ? (loc.code || '').toLowerCase() : (loc.roomCode || '').toLowerCase()
			const subLabel = isRoom
				? `${loc.roomCategoryName || ''} ${loc.specialtyName || ''}`.toLowerCase()
				: (loc.roomDisplayName || '').toLowerCase()
			return label.includes(term) || subLabel.includes(term)
		})
	}, [locations, searchTerm, isRoom])

	const assignmentsByLocation = useMemo(() => {
		const map = {}
		for (const a of assignments || []) {
			const locId = a.locationId
			if (!map[locId]) map[locId] = []
			map[locId].push(a)
		}
		return map
	}, [assignments])

	const getLocationLabel = useCallback((loc) => (isRoom ? loc.code : loc.roomCode), [isRoom])

	const getLocationSubLabel = useCallback((loc) => {
		if (!isRoom) return loc.roomDisplayName
		return [loc.roomCategoryName, loc.specialtyName].filter(Boolean).join(' • ')
	}, [isRoom])

	const getLocationRoles = useCallback(
		(loc) => (isRoom ? (loc.roles || []).filter((r) => r !== EnumConfig.Role.Patient) : DOCTOR_ROLES),
		[isRoom]
	)

	const handleViewSegments = useCallback((assignment) => {
		setSelectedAssignment(assignment)
		setSegmentsDialogOpen(true)
	}, [])

	const handleOpenUpdateSegment = useCallback(
		(segment) => {
			if (!canUpdateSegment || !canUpdateSegments) return
			setSelectedWorkSegment(segment)
			setOpenUpdateSegment(true)
		},
		[canUpdateSegment, canUpdateSegments]
	)

	const handleCloseUpdateSegment = useCallback(() => {
		setOpenUpdateSegment(false)
		setSelectedWorkSegment(null)
	}, [])

	const updateSegmentFields = useMemo(
		() => [
			{
				key: 'timeRange',
				title: t('work_schedule.field.work_segments'),
				type: 'timerange',
				from: {
					key: 'startTime',
					label: t('work_schedule.field.start_time'),
				},
				to: {
					key: 'endTime',
					label: t('work_schedule.field.end_time'),
				},
			},
		],
		[t]
	)

	const updateSegmentInitialValues = useMemo(
		() => ({
			startTime: formatDateTimeToTimeOnly(selectedWorkSegment?.startTime),
			endTime: formatDateTimeToTimeOnly(selectedWorkSegment?.endTime),
		}),
		[selectedWorkSegment]
	)

	const handleUpdateSegment = useCallback(
		async ({ values, closeDialog }) => {
			if (!canUpdateSegment) {
				closeDialog?.()
				return
			}
			if (!selectedWorkSegment?.id) return
			await onUpdateWorkSegment?.({
				workSegment: selectedWorkSegment,
				values,
				closeDialog,
			})
		},
		[selectedWorkSegment, onUpdateWorkSegment, canUpdateSegment]
	)

	useEffect(() => {
		if (!selectedAssignment?.id) return

		const nextAssignment = assignments.find((assignment) => assignment.id === selectedAssignment.id)
		if (!nextAssignment) {
			setSelectedAssignment(null)
			setSelectedWorkSegment(null)
			setSegmentsDialogOpen(false)
			setOpenUpdateSegment(false)
			return
		}

		setSelectedAssignment(nextAssignment)
	}, [assignments, selectedAssignment?.id])

	useEffect(() => {
		if (!selectedWorkSegment?.id || !selectedAssignment?.workSegments?.length) return

		const nextSegment = selectedAssignment.workSegments.find(
			(segment) => segment.id === selectedWorkSegment.id
		)
		if (!nextSegment) {
			setSelectedWorkSegment(null)
			setOpenUpdateSegment(false)
			return
		}

		setSelectedWorkSegment(nextSegment)
	}, [selectedAssignment, selectedWorkSegment?.id])

	const fields = useMemo(
		() => [
			{
				key: 'startTime',
				title: t('work_schedule.field.start_time'),
				minWidthPx: 110,
				render: (value) => formatTimeToHourMinute(value ? new Date(value).toTimeString() : ''),
			},
			{
				key: 'endTime',
				title: t('work_schedule.field.end_time'),
				minWidthPx: 110,
				render: (value) => formatTimeToHourMinute(value ? new Date(value).toTimeString() : ''),
			},
			{
				key: 'workStatus',
				title: t('work_schedule.field.work_status'),
				minWidthPx: 110,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.workStatusOptions, value) || value}
						color={defaultWorkStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'workStatusReason',
				title: t('work_schedule.field.work_status_reason'),
				minWidthPx: 200,
				render: (value) => renderEmptyFallback(getEnumLabelByValue(_enum.workStatusReasonOptions, value) || value),
			},
			{
				key: 'actions',
				title: t('work_schedule.table.actions'),
				minWidthPx: 120,
				render: (_, row) => (
					<Button
						size='small'
						variant='outlined'
						fullWidth
						onClick={() => handleOpenUpdateSegment(row)}
						disabled={!canUpdateSegment || !canUpdateSegments}
						sx={{ whiteSpace: 'nowrap' }}
					>
						{t('work_schedule.button.update')}
					</Button>
				),
			},
		],
		[
			t,
			_enum.workStatusOptions,
			_enum.workStatusReasonOptions,
			canUpdateSegment,
			canUpdateSegments,
			handleOpenUpdateSegment,
		]
	)

	if (loading) {
		return (
			<Paper variant='outlined' sx={{ p: 2 }}>
				<Stack spacing={2}>
					<SkeletonBox numberOfBoxes={1} heights={[42]} rounded />
					<Grid container spacing={2}>
						{[1, 2].map((i) => (
							<Grid size={{ xs: 12, sm: 6, lg: 4 }} key={i}>
								<SkeletonBox numberOfBoxes={1} heights={[200]} rounded />
							</Grid>
						))}
					</Grid>
				</Stack>
			</Paper>
		)
	}

	return (
		<Paper variant='outlined' sx={{ p: 2 }}>
			<Stack spacing={2}>
				<SearchBar value={searchTerm} setValue={setSearchTerm} />

				{filteredLocations.length > 0 ? (
					<Grid container spacing={2}>
						{filteredLocations.map((loc) => (
							<Grid size={{ xs: 12, sm: 6, lg: 4 }} key={loc.id}>
								<AssignmentCardSection
									location={loc}
									locationLabel={getLocationLabel(loc)}
									locationSubLabel={getLocationSubLabel(loc)}
									locationRoles={getLocationRoles(loc)}
									assignments={assignmentsByLocation[loc.id] || []}
									canManageAssignments={canManageAssignments}
									onAdd={onAdd}
									onRemove={onDelete}
									onViewSegments={handleViewSegments}
								/>
							</Grid>
						))}
					</Grid>
				) : (
					<Typography variant='body2' color='text.disabled' sx={{ py: 3, textAlign: 'center' }}>
						{searchTerm
							? t('work_schedule.text.no_results_found')
							: t('work_schedule.text.no_locations_found')}
					</Typography>
				)}
			</Stack>

			<Dialog
				open={segmentsDialogOpen}
				onClose={() => setSegmentsDialogOpen(false)}
				maxWidth={false}
				fullWidth
				PaperProps={{
					sx: {
						width: '95vw',
						maxWidth: 1200,
					},
				}}
			>
				<Box sx={{ p: 3 }}>
					<Stack spacing={2}>
						<Typography variant='h6'>
							{selectedAssignment?.staff?.name &&
								`${t('work_schedule.title.segments_for')} ${selectedAssignment.staff.name}`}
						</Typography>

						{selectedAssignment?.workSegments?.length > 0 ? (
							<GenericTable
								data={selectedAssignment.workSegments}
								fields={fields}
								rowKey='id'
								loading={false}
								stickyHeader
							/>
						) : (
							<Typography variant='body2' color='text.secondary'>
								{t('work_schedule.text.no_work_segments')}
							</Typography>
						)}
					</Stack>
				</Box>
			</Dialog>
			{canUpdateSegment && (
				<GenericFormDialog
					open={openUpdateSegment}
					onClose={handleCloseUpdateSegment}
					title={t('work_schedule.dialog.edit_work_segment_title')}
					fields={updateSegmentFields}
					initialValues={updateSegmentInitialValues}
					onSubmit={handleUpdateSegment}
					submitLabel={t('work_schedule.button.update')}
					submitButtonColor='info'
					textFieldVariant='outlined'
				/>
			)}
		</Paper>
	)
}

export default WorkScheduleAssignmentTableSection

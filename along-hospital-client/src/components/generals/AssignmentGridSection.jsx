import MultipleSelectDialog from '@/components/dialogs/commons/MultipleSelectDialog'
import GenericTabs from '@/components/generals/GenericTabs'
import SearchBar from '@/components/generals/SearchBar'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Avatar, Grid, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import AssignmentCardSection from './AssignmentCardSection'

const AssignmentGridSection = ({
	title,
	assignments,
	otherShiftAssignments,
	shifts,
	locations,
	staffs,
	locationIdKey = 'locationId',
	getLocationLabel,
	getLocationSubLabel,
	getLocationMetaLabel,
	getLocationRoles,
	getStaffRoles = (staff) => staff?.roles || [],
	getStaffMetaLabel,
	isStaffAssignable = () => true,
	onAdd,
	onRemove,
	assignLabel,
	loading = false,
}) => {
	const _enum = useEnum()
	const { t } = useTranslation()

	const [selectedShiftId, setSelectedShiftId] = useState('')
	const [dialogOpen, setDialogOpen] = useState(false)
	const [dialogLocationId, setDialogLocationId] = useState(null)
	const [searchTerm, setSearchTerm] = useState('')

	const filteredLocations = useMemo(() => {
		if (!searchTerm.trim()) return locations || []
		const term = searchTerm.trim().toLowerCase()
		return (locations || []).filter((location) => {
			const label = (getLocationLabel(location) || '').toLowerCase()
			const subLabel = (getLocationSubLabel?.(location) || '').toLowerCase()
			const metaLabel = (getLocationMetaLabel?.(location) || '').toLowerCase()
			return label.includes(term) || subLabel.includes(term) || metaLabel.includes(term)
		})
	}, [locations, searchTerm, getLocationLabel, getLocationMetaLabel, getLocationSubLabel])

	const roleOptions = _enum.roleOptions

	const shiftTabs = useMemo(
		() =>
			(shifts || []).map((shift) => ({
				key: `${shift.id}`,
				title: shift.name,
			})),
		[shifts]
	)

	const activeShiftId = useMemo(() => {
		if (!shiftTabs.length) return ''
		if (shiftTabs.some((tab) => tab.key === selectedShiftId)) return selectedShiftId
		return shiftTabs[0].key
	}, [selectedShiftId, shiftTabs])

	const activeShiftIdNum = Number(activeShiftId)

	const assignmentsByLocation = useMemo(() => {
		const map = {}
		for (const a of assignments || []) {
			if (a.shiftId !== activeShiftIdNum) continue
			const locId = a[locationIdKey]
			if (!map[locId]) map[locId] = []
			map[locId].push(a)
		}
		return map
	}, [activeShiftIdNum, assignments, locationIdKey])

	const blockedStaffIds = useMemo(() => {
		const ids = new Set()
		for (const a of assignments || []) {
			if (a.shiftId === activeShiftIdNum) ids.add(a.staffId)
		}
		for (const a of otherShiftAssignments || []) {
			if (a.shiftId === activeShiftIdNum) ids.add(a.staffId)
		}
		return ids
	}, [activeShiftIdNum, assignments, otherShiftAssignments])

	const dialogLocation = useMemo(
		() => (locations || []).find((location) => location.id === dialogLocationId),
		[locations, dialogLocationId]
	)
	const dialogLocationRoles = useMemo(
		() => (dialogLocation ? getLocationRoles(dialogLocation) : []),
		[dialogLocation, getLocationRoles]
	)
	const getStaffLabel = useCallback((staff) => staff?.fullName || staff?.name || `${staff?.id || ''}`, [])

	const dialogStaffOptions = useMemo(() => {
		if (!dialogLocation || !dialogLocationRoles.length) return []
		return [...(staffs || [])]
			.map((staff) => {
				const specialtyName = staff?.specialtyName || staff?.specialty?.name || ''
				const metaLabel =
					getStaffMetaLabel?.(staff, dialogLocation) ||
					(specialtyName ? `${t('staff.field.specialty')}: ${specialtyName}` : '')
				const disabled =
					blockedStaffIds.has(staff.id) ||
					!isStaffAssignable(staff, dialogLocation, {
						locationRoles: dialogLocationRoles,
					})
				const roleLabel = getStaffRoles(staff)
					.filter((role) => role !== EnumConfig.Role.Patient)
					.map((role) => getEnumLabelByValue(roleOptions, role) || role)
					.join(', ')

				return {
					value: staff.id,
					label: {
						staff,
						metaLabel,
					},
					searchKey: [getStaffLabel(staff), staff?.phone, staff?.email, roleLabel, metaLabel]
						.filter(Boolean)
						.join(' '),
					disabled,
				}
			})
			.sort((a, b) => getStaffLabel(a.label.staff).localeCompare(getStaffLabel(b.label.staff)))
	}, [
		blockedStaffIds,
		dialogLocation,
		dialogLocationRoles,
		getStaffLabel,
		getStaffMetaLabel,
		getStaffRoles,
		isStaffAssignable,
		roleOptions,
		staffs,
		t,
	])

	const renderStaffOption = useCallback(
		(_, optionLabel) => {
			const staff = optionLabel?.staff || optionLabel
			const metaLabel = optionLabel?.metaLabel || ''

			return (
				<Stack direction='row' alignItems='center' gap={2}>
					<Avatar src={getImageFromCloud(staff?.image)} alt={getStaffLabel(staff)} />
					<Stack gap={0.2}>
						<Stack direction='row' gap={1} alignItems='center'>
							<Typography>{getStaffLabel(staff)}</Typography>
							{staff?.phone && (
								<Typography lineHeight='normal' variant='caption' color='text.secondary'>
									{staff.phone}
								</Typography>
							)}
							{staff?.email && (
								<Typography lineHeight='normal' variant='caption' color='text.secondary'>
									{staff.email}
								</Typography>
							)}
						</Stack>
						<Typography variant='caption' color='text.secondary'>
							{getStaffRoles(staff)
								.filter((role) => role !== EnumConfig.Role.Patient)
								.map((role) => getEnumLabelByValue(roleOptions, role) || role)
								.join(', ')}
						</Typography>
						{metaLabel && (
							<Typography variant='caption' color='text.secondary'>
								{metaLabel}
							</Typography>
						)}
					</Stack>
				</Stack>
			)
		},
		[getStaffLabel, getStaffRoles, roleOptions]
	)

	const handleStaffSelect = useCallback(
		async (staffIds) => {
			if (activeShiftId === '' || dialogLocationId == null || !staffIds?.length) return
			await onAdd?.(
				{
					shiftId: activeShiftIdNum,
					[locationIdKey]: dialogLocationId,
					staffIds,
				},
				() => setDialogOpen(false)
			)
		},
		[activeShiftId, activeShiftIdNum, dialogLocationId, onAdd, locationIdKey]
	)

	const handleOpenDialog = useCallback((locationId) => {
		setDialogLocationId(locationId)
		setDialogOpen(true)
	}, [])

	const handleShiftTabChange = useCallback((tab) => {
		if (tab?.key) {
			setSelectedShiftId(tab.key)
		}
	}, [])

	const dialogTitle = useMemo(() => {
		const locationLabel = dialogLocation ? getLocationLabel(dialogLocation) : ''
		return locationLabel ? `${assignLabel} - ${locationLabel}` : assignLabel
	}, [assignLabel, dialogLocation, getLocationLabel])

	if (loading) {
		return (
			<Paper variant='outlined' sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Typography variant='h6'>{title}</Typography>
					<SkeletonBox numberOfBoxes={1} heights={[42]} rounded />
					<Grid container spacing={2}>
						{[1, 2, 3].map((i) => (
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
				<Typography variant='h6'>{title}</Typography>

				{shiftTabs.length > 0 && (
					<GenericTabs
						tabs={shiftTabs}
						currentTab={activeShiftId}
						setCurrentTab={handleShiftTabChange}
					/>
				)}

				<SearchBar value={searchTerm} setValue={setSearchTerm} />

				<Grid container spacing={2}>
					{filteredLocations.map((location) => (
						<Grid size={{ xs: 12, sm: 6, lg: 4 }} key={location.id}>
							<AssignmentCardSection
								location={location}
								locationLabel={getLocationLabel(location)}
								locationSubLabel={getLocationSubLabel?.(location)}
								locationMetaLabel={getLocationMetaLabel?.(location)}
								locationRoles={getLocationRoles(location)}
								assignments={assignmentsByLocation[location.id] || []}
								getStaffRoles={getStaffRoles}
								onAdd={handleOpenDialog}
								onRemove={onRemove}
							/>
						</Grid>
					))}
				</Grid>

				<MultipleSelectDialog
					open={dialogOpen}
					onClose={() => setDialogOpen(false)}
					title={dialogTitle}
					options={dialogStaffOptions}
					value={[]}
					onChange={handleStaffSelect}
					renderOption={renderStaffOption}
				/>
			</Stack>
		</Paper>
	)
}

export default AssignmentGridSection

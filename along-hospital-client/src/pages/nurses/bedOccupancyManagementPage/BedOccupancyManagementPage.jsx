import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Grid, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import BedOccupancyFiltersSection from './sections/BedOccupancyFiltersSection'
import BedOccupancyRoomBoardSection from './sections/BedOccupancyRoomBoardSection'
import BedOccupancyRoomListSection from './sections/BedOccupancyRoomListSection'
import MedicalHistoryPreviewDrawer from './sections/MedicalHistoryPreviewDrawer'

const defaultFilters = {
	search: '',
	building: '',
	floor: '',
	specialtyId: '',
	bedStatus: '',
}

const normalizeFilters = (filters) => ({
	...defaultFilters,
	...(filters ?? {}),
})

const normalizeValue = (value) =>
	String(value || '')
		.trim()
		.toLowerCase()

const matchesSearchValue = (value, search) => normalizeValue(value).includes(search)
const contentPanelHeight = { xs: 'auto', lg: 'clamp(480px, 68vh, 760px)' }

const SummaryCard = ({ label, value, color = 'default' }) => (
	<Paper
		variant='outlined'
		sx={{
			p: 2,
			borderRadius: 2,
			height: '100%',
		}}
	>
		<Stack spacing={0.5}>
			<Typography variant='caption' color='text.secondary'>
				{label}
			</Typography>
			<Typography variant='h6' color={color === 'default' ? 'text.primary' : `${color}.main`}>
				{value}
			</Typography>
		</Stack>
	</Paper>
)

const BedOccupancyManagementPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const { loading, data } = useFetch(ApiUrls.BED_OCCUPANCY.ROOMS, {}, [])

	const previewFetcher = useAxiosSubmit({ method: 'GET' })

	const previewRequestRef = useRef(null)

	const [filters, setFilters] = useState(() => normalizeFilters(defaultFilters))
	const [selectedRoomId, setSelectedRoomId] = useState(null)
	const [medicalHistoryPreviewOpen, setMedicalHistoryPreviewOpen] = useState(false)
	const [medicalHistoryPreview, setMedicalHistoryPreview] = useState(null)

	const safeFilters = useMemo(() => normalizeFilters(filters), [filters])

	const handleSetFilters = (nextFilters) => {
		setFilters((prevFilters) => {
			const resolvedFilters =
				typeof nextFilters === 'function' ? nextFilters(normalizeFilters(prevFilters)) : nextFilters

			return normalizeFilters(resolvedFilters)
		})
	}

	const roomBoard = useMemo(() => (Array.isArray(data) ? data : []), [data])

	const buildingOptions = useMemo(
		() =>
			[...new Set(roomBoard.map((room) => room.buildingName).filter(Boolean))].sort((left, right) =>
				left.localeCompare(right)
			),
		[roomBoard]
	)

	const floorOptions = useMemo(
		() =>
			[
				...new Set(roomBoard.map((room) => room.floorNumber).filter((value) => value !== undefined)),
			].sort((left, right) => left - right),
		[roomBoard]
	)

	const specialtyOptions = useMemo(
		() =>
			Array.from(
				roomBoard.reduce((specialtiesMap, room) => {
					if (room?.specialtyId == null) return specialtiesMap

					specialtiesMap.set(room.specialtyId, {
						value: room.specialtyId,
						label: room.specialtyName || String(room.specialtyId),
					})

					return specialtiesMap
				}, new Map()).values()
			)
				.sort((left, right) => left.label.localeCompare(right.label)),
		[roomBoard]
	)

	const filteredRooms = useMemo(() => {
		const search = normalizeValue(safeFilters.search)

		return roomBoard
			.filter((room) => {
				if (safeFilters.building && room.buildingName !== safeFilters.building) return false
				if (safeFilters.floor && String(room.floorNumber) !== String(safeFilters.floor)) return false
				if (safeFilters.specialtyId && String(room.specialtyId) !== String(safeFilters.specialtyId))
					return false
				return true
			})
			.map((room) => {
				const bedStatusFiltered = safeFilters.bedStatus
					? room.beds.filter((bed) => bed.status === safeFilters.bedStatus)
					: room.beds

				if (!search) {
					return { ...room, beds: bedStatusFiltered }
				}

				const roomMatchesSearch = matchesSearchValue(room.code, search)
				const searchedBeds = roomMatchesSearch
					? bedStatusFiltered
					: bedStatusFiltered.filter(
							(bed) =>
								matchesSearchValue(bed.code, search) ||
								matchesSearchValue(bed.currentOccupancy?.patientName, search) ||
								matchesSearchValue(bed.currentOccupancy?.medicalHistoryNumber, search)
						)

				return { ...room, beds: searchedBeds }
			})
			.filter((room) => room.beds.length > 0 || (!safeFilters.bedStatus && !search))
	}, [roomBoard, safeFilters])

	const selectedRoom = useMemo(
		() => filteredRooms.find((room) => room.id === selectedRoomId) || null,
		[filteredRooms, selectedRoomId]
	)

	const summary = useMemo(
		() =>
			filteredRooms.reduce(
				(result, room) => {
					result.rooms += 1
					result.beds += room.beds.length
					result.occupied += room.beds.filter(
						(bed) => bed.status === EnumConfig.BedStatus.Occupied
					).length
					result.available += room.beds.filter(
						(bed) => bed.status === EnumConfig.BedStatus.Active
					).length
					result.maintenance += room.beds.filter(
						(bed) => bed.status === EnumConfig.BedStatus.Maintenance
					).length
					return result
				},
				{ rooms: 0, beds: 0, occupied: 0, available: 0, maintenance: 0 }
			),
		[filteredRooms]
	)

	useEffect(() => {
		if (filteredRooms.length === 0) {
			setSelectedRoomId(null)
			return
		}

		if (!filteredRooms.some((room) => room.id === selectedRoomId)) {
			setSelectedRoomId(filteredRooms[0].id)
		}
	}, [filteredRooms, selectedRoomId])

	const fetchMedicalHistoryPreview = async (medicalHistoryId, shouldOpen = true) => {
		previewRequestRef.current = medicalHistoryId
		if (shouldOpen) setMedicalHistoryPreviewOpen(true)
		setMedicalHistoryPreview(null)

		const response = await previewFetcher.submit({
			overrideUrl: ApiUrls.MEDICAL_HISTORY.DETAIL(medicalHistoryId),
		})

		if (previewRequestRef.current === medicalHistoryId && response?.data) {
			setMedicalHistoryPreview(response.data)
		}
	}

	if (loading && roomBoard.length === 0) {
		return (
			<Paper sx={{ p: 3, borderRadius: 3 }}>
				<SkeletonBox numberOfBoxes={4} heights={[60, 90, 220, 360]} />
			</Paper>
		)
	}

	return (
		<Box>
			<Stack spacing={3}>
				<Paper sx={{ p: 3, borderRadius: 3 }}>
					<Stack spacing={2.5}>
						<Stack direction={{ xs: 'column', md: 'row' }} justifyContent='space-between' spacing={2}>
							<Stack spacing={0.5}>
								<Typography variant='h5'>{t('bed_occupancy.title.management')}</Typography>
							</Stack>
						</Stack>

						<Grid container spacing={2}>
							<Grid size={{ xs: 12, sm: 6, lg: 2.4 }}>
								<SummaryCard
									label={t('bed_occupancy.summary.rooms')}
									value={summary.rooms}
									color='primary'
								/>
							</Grid>
							<Grid size={{ xs: 12, sm: 6, lg: 2.4 }}>
								<SummaryCard label={t('bed_occupancy.summary.beds')} value={summary.beds} />
							</Grid>
							<Grid size={{ xs: 12, sm: 6, lg: 2.4 }}>
								<SummaryCard
									label={t('bed_occupancy.summary.occupied')}
									value={summary.occupied}
									color='info'
								/>
							</Grid>
							<Grid size={{ xs: 12, sm: 6, lg: 2.4 }}>
								<SummaryCard
									label={t('bed_occupancy.summary.available')}
									value={summary.available}
									color='success'
								/>
							</Grid>
							<Grid size={{ xs: 12, sm: 6, lg: 2.4 }}>
								<SummaryCard
									label={t('bed_occupancy.summary.maintenance')}
									value={summary.maintenance}
									color='warning'
								/>
							</Grid>
						</Grid>
					</Stack>
				</Paper>

				<BedOccupancyFiltersSection
					filters={safeFilters}
					setFilters={handleSetFilters}
					defaultFilters={defaultFilters}
					buildingOptions={buildingOptions}
					floorOptions={floorOptions}
					specialtyOptions={specialtyOptions}
				/>

				<Grid container spacing={2.5} sx={{ minHeight: 0 }}>
					<Grid size={{ xs: 12, xl: 4 }} sx={{ minHeight: 0 }}>
						<BedOccupancyRoomListSection
							rooms={filteredRooms}
							selectedRoomId={selectedRoomId}
							onSelectRoom={setSelectedRoomId}
							panelHeight={contentPanelHeight}
						/>
					</Grid>
					<Grid size={{ xs: 12, xl: 8 }} sx={{ minHeight: 0 }}>
						<BedOccupancyRoomBoardSection
							room={selectedRoom}
							onViewInpatient={(medicalHistoryId) => fetchMedicalHistoryPreview(medicalHistoryId)}
							onOpenDetail={(medicalHistoryId) =>
								navigate(
									routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(medicalHistoryId))
								)
							}
							panelHeight={contentPanelHeight}
						/>
					</Grid>
				</Grid>
			</Stack>

			<MedicalHistoryPreviewDrawer
				open={medicalHistoryPreviewOpen}
				onClose={() => {
					setMedicalHistoryPreviewOpen(false)
					setMedicalHistoryPreview(null)
				}}
				loading={previewFetcher.loading}
				medicalHistory={medicalHistoryPreview}
			/>
		</Box>
	)
}

export default BedOccupancyManagementPage

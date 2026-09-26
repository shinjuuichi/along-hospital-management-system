import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { defaultOccupancyStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import {
	BedOccupancyDetailHighlightDurationMs,
	BedOccupancyManagementRoles,
} from '@/constants/medicalHistoryConstants'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import MedicalHistoryDetailBedActionBar from '@/pages/staffs/medicalHistoryDetailPage/sections/bedSections/MedicalHistoryDetailBedActionBar'
import MedicalHistoryDetailBedEmptyState from '@/pages/staffs/medicalHistoryDetailPage/sections/bedSections/MedicalHistoryDetailBedEmptyState'
import MedicalHistoryDetailBedOccupancyTimeline from '@/pages/staffs/medicalHistoryDetailPage/sections/bedSections/MedicalHistoryDetailBedOccupancyTimeline'
import MedicalHistoryDetailBedSummarySection from '@/pages/staffs/medicalHistoryDetailPage/sections/bedSections/MedicalHistoryDetailBedSummarySection'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { HotelOutlined } from '@mui/icons-material'
import { Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'

const getDateValue = (value) => {
	if (!value) {
		return null
	}

	const dateValue = new Date(value)
	return Number.isNaN(dateValue.getTime()) ? null : dateValue.getTime()
}

const getSortedBedOccupancies = (bedOccupancies) =>
	[...bedOccupancies].sort((firstOccupancy, secondOccupancy) => {
		const firstDateValue = getDateValue(firstOccupancy?.fromDateTime) ?? 0
		const secondDateValue = getDateValue(secondOccupancy?.fromDateTime) ?? 0

		return firstDateValue - secondDateValue
	})

const getDefaultSelectedOccupancyId = (bedOccupancies, currentBedOccupancy) => {
	if (currentBedOccupancy?.id) {
		return currentBedOccupancy.id
	}

	return bedOccupancies.at(-1)?.id ?? null
}

const getSelectedOccupancy = (bedOccupancies, currentBedOccupancy, selectedOccupancyId) => {
	return (
		bedOccupancies.find((occupancy) => occupancy.id === selectedOccupancyId) ||
		currentBedOccupancy ||
		bedOccupancies.at(-1) ||
		null
	)
}

const getTransferNoteDisplay = (selectedOccupancy, currentBedOccupancy) => {
	if (selectedOccupancy?.transferNote) {
		return {
			labelKey: 'bed_occupancy.field.transfer_note',
			value: selectedOccupancy.transferNote,
		}
	}

	if (selectedOccupancy?.id === currentBedOccupancy?.id && currentBedOccupancy?.latestTransferNote) {
		return {
			labelKey: 'bed_occupancy.field.latest_transfer_note',
			value: currentBedOccupancy.latestTransferNote,
		}
	}

	return null
}

const isActiveOccupancy = (occupancy) =>
	occupancy?.occupancyStatus === EnumConfig.OccupancyStatus.Active

const getCurrentLanguage = () => {
	let language = localStorage.getItem('language') || 'en'
	try {
		language = JSON.parse(language)
	} catch {
		/* empty */
	}

	return language === 'vi' ? 'vi-VN' : 'en-US'
}

const formatLocalizedNumber = (value, maximumFractionDigits = 2) =>
	new Intl.NumberFormat(getCurrentLanguage(), {
		minimumFractionDigits: 0,
		maximumFractionDigits,
	}).format(Number(value) || 0)

const formatDurationInDays = (value, t) =>
	`${formatLocalizedNumber(value)} ${t('bed_occupancy.text.day_unit')}`

const getEstimatedBedChargeAmount = (bedOccupancies) =>
	bedOccupancies.reduce((sum, occupancy) => sum + (Number(occupancy?.totalAmount) || 0), 0)

const MedicalHistoryDetailBedSection = ({
	bedOccupancy,
	bedOccupancies = [],
	medicalHistoryStatus,
	dischargeDate,
	role,
	loading = false,
	loadingAction = false,
	onClickAssignBed,
	onClickDischargeBed,
	onClickTransferBed,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const detailHighlightTimeoutRef = useRef(null)
	const occupancyHistory = getSortedBedOccupancies(
		bedOccupancies.length > 0 ? bedOccupancies : bedOccupancy ? [bedOccupancy] : []
	)
	const [selectedOccupancyId, setSelectedOccupancyId] = useState(() =>
		getDefaultSelectedOccupancyId(occupancyHistory, bedOccupancy)
	)
	const [highlightedOccupancyId, setHighlightedOccupancyId] = useState(null)

	const isDraftStatus = medicalHistoryStatus === EnumConfig.MedicalHistoryStatus.Draft
	const hasBeenDischarged = Boolean(dischargeDate)
	const canAssignBed = isDraftStatus && !hasBeenDischarged
	const hasBedOccupancy = occupancyHistory.length > 0
	const selectedOccupancy = getSelectedOccupancy(
		occupancyHistory,
		bedOccupancy,
		selectedOccupancyId
	)
	const estimatedBedChargeAmount = getEstimatedBedChargeAmount(occupancyHistory)
	const transferNoteDisplay = getTransferNoteDisplay(selectedOccupancy, bedOccupancy)
	const canManageBed = BedOccupancyManagementRoles.includes(role)
	const hasActiveCurrentOccupancy = isActiveOccupancy(bedOccupancy)
	const canTransferBed = canAssignBed && hasActiveCurrentOccupancy
	const canDischargeBed =
		canManageBed && isDraftStatus && !hasBeenDischarged && hasActiveCurrentOccupancy
	const isSelectedBedInfoHighlighted = highlightedOccupancyId === selectedOccupancy?.id
	const canShowAssignButton = canManageBed && canAssignBed
	const canShowTransferButton = canManageBed && canTransferBed

	const handleSelectOccupancyFromTimeline = (occupancyId) => {
		setSelectedOccupancyId(occupancyId)
		setHighlightedOccupancyId(occupancyId)

		if (detailHighlightTimeoutRef.current) {
			clearTimeout(detailHighlightTimeoutRef.current)
		}

		detailHighlightTimeoutRef.current = setTimeout(() => {
			setHighlightedOccupancyId(null)
		}, BedOccupancyDetailHighlightDurationMs)
	}

	useEffect(() => {
		const nextOccupancyHistory = getSortedBedOccupancies(
			bedOccupancies.length > 0 ? bedOccupancies : bedOccupancy ? [bedOccupancy] : []
		)

		setSelectedOccupancyId((previousSelectedOccupancyId) => {
			if (
				previousSelectedOccupancyId &&
				nextOccupancyHistory.some((occupancy) => occupancy.id === previousSelectedOccupancyId)
			) {
				return previousSelectedOccupancyId
			}

			return getDefaultSelectedOccupancyId(nextOccupancyHistory, bedOccupancy)
		})
	}, [bedOccupancy, bedOccupancies])

	useEffect(() => {
		return () => {
			if (detailHighlightTimeoutRef.current) {
				clearTimeout(detailHighlightTimeoutRef.current)
			}
		}
	}, [])

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2, height: '100%' }}>
				<Typography variant='h6' gutterBottom>
					{t('bed_occupancy.title.bed_assignment')}
				</Typography>
				<SkeletonBox numberOfBoxes={4} heights={[30, 30, 30, 40]} />
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 3, borderRadius: 2, height: '100%' }}>
			<Stack direction='row' alignItems='center' justifyContent='space-between' mb={3}>
				<Stack direction='row' alignItems='center' spacing={1}>
					<HotelOutlined color='primary' />
					<Typography variant='h6'>{t('bed_occupancy.title.bed_assignment')}</Typography>
				</Stack>
				{selectedOccupancy && (
					<Chip
						label={getEnumLabelByValue(
							_enum.occupancyStatusOptions,
							selectedOccupancy?.occupancyStatus
						)}
						color={defaultOccupancyStatusStyle(selectedOccupancy?.occupancyStatus)}
						size='small'
					/>
				)}
			</Stack>

			{!hasBedOccupancy ? (
				<MedicalHistoryDetailBedEmptyState
					canShowAssignButton={canShowAssignButton}
					loadingAction={loadingAction}
					onClickAssignBed={onClickAssignBed}
					t={t}
				/>
			) : (
				<Stack spacing={3}>
					<MedicalHistoryDetailBedOccupancyTimeline
						bedOccupancies={occupancyHistory}
						currentBedOccupancy={bedOccupancy}
						selectedOccupancyId={selectedOccupancyId}
						onSelectOccupancy={handleSelectOccupancyFromTimeline}
					/>

					<MedicalHistoryDetailBedSummarySection
						selectedOccupancy={selectedOccupancy}
						isSelectedBedInfoHighlighted={isSelectedBedInfoHighlighted}
						transferNoteDisplay={transferNoteDisplay}
						estimatedBedChargeAmount={estimatedBedChargeAmount}
						formatDurationInDays={formatDurationInDays}
						occupancyStatusOptions={_enum.occupancyStatusOptions}
						t={t}
					/>

					<MedicalHistoryDetailBedActionBar
						canShowTransferButton={canShowTransferButton}
						canShowDischargeButton={canDischargeBed}
						loadingAction={loadingAction}
						onClickTransferBed={onClickTransferBed}
						onClickDischargeBed={onClickDischargeBed}
						t={t}
					/>
				</Stack>
			)}
		</Paper>
	)
}

export default MedicalHistoryDetailBedSection

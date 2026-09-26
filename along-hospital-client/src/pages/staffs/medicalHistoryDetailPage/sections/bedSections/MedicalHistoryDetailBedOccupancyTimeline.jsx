import { defaultOccupancyStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import {
	BedOccupancyCompactStackPreviewCount,
	BedOccupancyCompactTimelineScrollItemWidth,
	BedOccupancyDayInMs,
	BedOccupancyDetailedTimelineScrollItemWidth,
	BedOccupancyHourInMs,
	BedOccupancyMinuteInMs,
	BedOccupancySingleTimelineItemMaxWidth,
	BedOccupancyTimelineViewMode,
} from '@/constants/medicalHistoryConstants'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import CompactOccupancyTimeline from '@/pages/staffs/medicalHistoryDetailPage/sections/bedSections/CompactOccupancyTimeline'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, ButtonBase, Chip, Paper, Stack, Tooltip, Typography } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'
import { useEffect, useState } from 'react'

const getMaxVisibleTimelineSegments = (isSmallScreen, isLargeScreenDown) => {
	if (isSmallScreen) {
		return 2
	}

	if (isLargeScreenDown) {
		return 3
	}

	return 4
}

const getDefaultTimelineViewMode = (bedOccupancies) =>
	bedOccupancies.length > 4
		? BedOccupancyTimelineViewMode.Compact
		: BedOccupancyTimelineViewMode.Detailed

const getTimelineItemWidth = (timelineViewMode) =>
	timelineViewMode === BedOccupancyTimelineViewMode.Compact
		? BedOccupancyCompactTimelineScrollItemWidth
		: BedOccupancyDetailedTimelineScrollItemWidth

const getDateValue = (value) => {
	if (!value) {
		return null
	}

	const dateValue = new Date(value)
	return Number.isNaN(dateValue.getTime()) ? null : dateValue.getTime()
}

const isActiveOccupancy = (occupancy) =>
	occupancy?.occupancyStatus === EnumConfig.OccupancyStatus.Active

const isDischargedOccupancy = (occupancy) =>
	occupancy?.occupancyStatus === EnumConfig.OccupancyStatus.Discharged

const getBedCode = (occupancy) => renderEmptyFallback(occupancy?.bed?.code)

const getCompactGroupLabel = (occupancy) => {
	const roomCode = occupancy?.bed?.room?.code
	if (roomCode) {
		return roomCode
	}

	const bedCode = getBedCode(occupancy)
	const lastDashIndex = bedCode.lastIndexOf('-')
	return lastDashIndex > 0 ? bedCode.slice(0, lastDashIndex) : bedCode
}

const getRoomCode = (occupancy) => occupancy?.bed?.room?.code || getCompactGroupLabel(occupancy)

const getOccupancyTimelinePalette = (theme, occupancyStatus, isEmphasized) => {
	const paletteKey = defaultOccupancyStatusStyle(occupancyStatus)
	const palette = theme.palette[paletteKey] || theme.palette.primary

	return {
		backgroundColor: isEmphasized ? alpha(palette.main, 0.14) : alpha(palette.main, 0.06),
		borderColor: isEmphasized ? palette.main : alpha(palette.main, 0.24),
		dotColor: palette.main,
		titleColor: isEmphasized ? palette.main : theme.palette.text.primary,
		shadowColor: alpha(palette.main, 0.18),
	}
}

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

const getTimelineDurationMs = (startDateTime, endDateTime) => {
	const startDateValue = getDateValue(startDateTime)
	const endDateValue = getDateValue(endDateTime) ?? Date.now()

	if (startDateValue === null) {
		return 0
	}

	return Math.max(endDateValue - startDateValue, 0)
}

const formatTimelineDuration = (durationMs) => {
	if (durationMs < BedOccupancyHourInMs) {
		return `${formatLocalizedNumber(Math.max(durationMs / BedOccupancyMinuteInMs, 1), 0)}m`
	}

	if (durationMs < BedOccupancyDayInMs) {
		return `${formatLocalizedNumber(durationMs / BedOccupancyHourInMs, 1)}h`
	}

	return `${formatLocalizedNumber(durationMs / BedOccupancyDayInMs, 1)}d`
}

const getTimelineDurationByOccupancy = (occupancy) =>
	formatTimelineDuration(getTimelineDurationMs(occupancy?.fromDateTime, occupancy?.toDateTime))

const getTimelineDurationByRange = (occupancies) => {
	const firstOccupancy = occupancies[0]
	const lastOccupancy = occupancies.at(-1)

	return formatTimelineDuration(
		getTimelineDurationMs(firstOccupancy?.fromDateTime, lastOccupancy?.toDateTime)
	)
}

const getTimelineItems = (bedOccupancies, timelineViewMode) => {
	if (timelineViewMode === BedOccupancyTimelineViewMode.Detailed) {
		return bedOccupancies.map((occupancy) => ({
			id: `occupancy-${occupancy.id}`,
			type: 'occupancy',
			occupancies: [occupancy],
			representative: occupancy,
			containsActive: isActiveOccupancy(occupancy),
		}))
	}

	const compactGroups = []

	bedOccupancies.forEach((occupancy) => {
		const groupLabel = getCompactGroupLabel(occupancy)
		const previousGroup = compactGroups.at(-1)

		if (previousGroup && previousGroup.groupLabel === groupLabel) {
			previousGroup.occupancies.push(occupancy)
			return
		}

		compactGroups.push({
			groupLabel,
			occupancies: [occupancy],
		})
	})

	return compactGroups.map((group) => {
		const representative =
			group.occupancies.find((occupancy) => isActiveOccupancy(occupancy)) || group.occupancies.at(-1)

		return {
			id: `group-${group.occupancies[0].id}-${representative.id}`,
			type: group.occupancies.length > 1 ? 'group' : 'occupancy',
			occupancies: group.occupancies,
			representative,
			containsActive: group.occupancies.some((occupancy) => isActiveOccupancy(occupancy)),
		}
	})
}

const getCompactPrimaryOccupancy = (bedOccupancies, currentBedOccupancy) => {
	if (isActiveOccupancy(currentBedOccupancy) || isDischargedOccupancy(currentBedOccupancy)) {
		return currentBedOccupancy
	}

	return (
		bedOccupancies.find((occupancy) => isActiveOccupancy(occupancy)) ||
		bedOccupancies.find((occupancy) => isDischargedOccupancy(occupancy)) ||
		null
	)
}

const getCompactTransferOccupancies = (bedOccupancies, primaryOccupancy) =>
	bedOccupancies.filter(
		(occupancy) =>
			occupancy?.id !== primaryOccupancy?.id &&
			!isActiveOccupancy(occupancy) &&
			!isDischargedOccupancy(occupancy)
	)

const getCompactTransferPreviewOccupancies = (occupancies) =>
	occupancies.slice(-BedOccupancyCompactStackPreviewCount)

const getCompactTransferTrail = (occupancies) =>
	occupancies.map((occupancy) => getBedCode(occupancy)).join(' -> ')

const isTimelineItemSelected = (timelineItem, selectedOccupancyId) =>
	timelineItem.occupancies.some((occupancy) => occupancy.id === selectedOccupancyId)

const TimelineTooltipContent = ({ timelineItem, previousTimelineItem, t }) => {
	const firstOccupancy = timelineItem.occupancies[0]
	const latestOccupancy = timelineItem.representative
	const previousOccupancy = previousTimelineItem?.occupancies.at(-1) || null
	const timelineTitle =
		timelineItem.type === 'group' && timelineItem.occupancies.length > 1
			? `${getCompactGroupLabel(latestOccupancy)} (${timelineItem.occupancies.length})`
			: getBedCode(latestOccupancy)
	const transferSummary = previousOccupancy
		? `${t('bed_occupancy.text.transferred')}: ${getBedCode(previousOccupancy)} -> ${getBedCode(latestOccupancy)}`
		: `${t('bed_occupancy.text.assigned')}: ${getBedCode(latestOccupancy)}`
	const durationLabel =
		timelineItem.type === 'group' && timelineItem.occupancies.length > 1
			? getTimelineDurationByRange(timelineItem.occupancies)
			: getTimelineDurationByOccupancy(latestOccupancy)

	return (
		<Stack spacing={0.5} sx={{ py: 0.5 }}>
			<Typography variant='subtitle2'>{timelineTitle}</Typography>
			<Typography variant='caption'>{transferSummary}</Typography>
			<Typography variant='caption'>
				{`${t('bed_occupancy.text.duration_label')}: ${durationLabel}`}
			</Typography>
			{timelineItem.type === 'group' && timelineItem.occupancies.length > 1 && (
				<Typography variant='caption'>
					{`${t('bed_occupancy.text.latest_bed')}: ${getBedCode(firstOccupancy)} -> ${getBedCode(latestOccupancy)}`}
				</Typography>
			)}
			<Typography variant='caption'>{t('bed_occupancy.text.click_to_focus_detail')}</Typography>
		</Stack>
	)
}

const MedicalHistoryDetailBedOccupancyTimeline = ({
	bedOccupancies,
	currentBedOccupancy,
	selectedOccupancyId,
	onSelectOccupancy,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const theme = useTheme()
	const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'))
	const isLargeScreenDown = useMediaQuery(theme.breakpoints.down('lg'))
	const [hoveredTimelineItemId, setHoveredTimelineItemId] = useState(null)
	const [timelineViewMode, setTimelineViewMode] = useState(() =>
		bedOccupancies.length > 0 ? getDefaultTimelineViewMode(bedOccupancies) : null
	)
	const maxVisibleTimelineSegments = getMaxVisibleTimelineSegments(
		isSmallScreen,
		isLargeScreenDown
	)
	const compactPrimaryOccupancy = getCompactPrimaryOccupancy(
		bedOccupancies,
		currentBedOccupancy
	)
	const compactTransferOccupancies = getCompactTransferOccupancies(
		bedOccupancies,
		compactPrimaryOccupancy
	)
	const resolvedTimelineViewMode =
		timelineViewMode || getDefaultTimelineViewMode(bedOccupancies)
	const timelineItems = getTimelineItems(bedOccupancies, resolvedTimelineViewMode)
	const timelineItemWidth = getTimelineItemWidth(resolvedTimelineViewMode)
	const shouldScrollTimeline = timelineItems.length > maxVisibleTimelineSegments
	const timelineGap = Number.parseFloat(theme.spacing(2))
	const timelineContainerPadding = Number.parseFloat(theme.spacing(1)) * 2
	const timelineContentMinWidth = shouldScrollTimeline
		? `${timelineItems.length * timelineItemWidth +
				Math.max(timelineItems.length - 1, 0) * timelineGap +
				timelineContainerPadding}px`
		: '100%'
	const timelineSummaryOccupancy =
		currentBedOccupancy ||
		bedOccupancies.find((occupancy) => isActiveOccupancy(occupancy)) ||
		bedOccupancies.at(-1) ||
		null

	const handleTimelineItemClick = (timelineItem) => {
		onSelectOccupancy(timelineItem.representative.id)
	}

	useEffect(() => {
		if (!timelineViewMode && bedOccupancies.length > 0) {
			setTimelineViewMode(getDefaultTimelineViewMode(bedOccupancies))
		}
	}, [bedOccupancies, timelineViewMode])

	useEffect(() => {
		setHoveredTimelineItemId(null)
	}, [bedOccupancies, resolvedTimelineViewMode])

	return (
		<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
			<Stack spacing={2}>
				<Stack
					direction={{ xs: 'column', lg: 'row' }}
					alignItems={{ xs: 'flex-start', lg: 'center' }}
					justifyContent='space-between'
					spacing={2}
				>
					<Stack spacing={0.75}>
						<Typography variant='subtitle2' color='text.secondary'>
							{t('bed_occupancy.title.occupancy_timeline')}
						</Typography>
						<Typography variant='caption' color='text.secondary'>
							{t(
								resolvedTimelineViewMode === BedOccupancyTimelineViewMode.Compact
									? 'bed_occupancy.text.compact_view_hint'
									: 'bed_occupancy.text.detailed_view_hint'
							)}
						</Typography>
					</Stack>

					<Stack
						direction={{ xs: 'column', sm: 'row' }}
						alignItems={{ xs: 'stretch', sm: 'center' }}
						spacing={1.5}
						sx={{ width: { xs: '100%', lg: 'auto' } }}
					>
						{timelineSummaryOccupancy &&
							resolvedTimelineViewMode !== BedOccupancyTimelineViewMode.Compact && (
								<Paper
									variant='outlined'
									sx={{
										px: 1.5,
										py: 1,
										borderRadius: 999,
										bgcolor: alpha(theme.palette.success.main, 0.08),
										borderColor: alpha(theme.palette.success.main, 0.24),
									}}
								>
									<Stack direction='row' spacing={1} alignItems='center' useFlexGap flexWrap='wrap'>
										<Typography variant='caption' color='text.secondary'>
											{`${t('bed_occupancy.text.current_bed')}:`}
										</Typography>
										<Typography variant='subtitle2'>{getBedCode(timelineSummaryOccupancy)}</Typography>
										<Chip
											label={getEnumLabelByValue(
												_enum.occupancyStatusOptions,
												timelineSummaryOccupancy?.occupancyStatus
											)}
											color={defaultOccupancyStatusStyle(
												timelineSummaryOccupancy?.occupancyStatus
											)}
											size='small'
										/>
									</Stack>
								</Paper>
							)}

						<Stack direction='row' spacing={1}>
							<Button
								size='small'
								variant={
									resolvedTimelineViewMode === BedOccupancyTimelineViewMode.Compact
										? 'contained'
										: 'outlined'
								}
								onClick={() => setTimelineViewMode(BedOccupancyTimelineViewMode.Compact)}
							>
								{t('bed_occupancy.button.compact_view')}
							</Button>
							<Button
								size='small'
								variant={
									resolvedTimelineViewMode === BedOccupancyTimelineViewMode.Detailed
										? 'contained'
										: 'outlined'
								}
								onClick={() => setTimelineViewMode(BedOccupancyTimelineViewMode.Detailed)}
							>
								{t('bed_occupancy.button.detailed_view')}
							</Button>
						</Stack>
					</Stack>
				</Stack>

				<Typography variant='caption' color='text.secondary'>
					{t('bed_occupancy.text.timeline_click_hint')}
				</Typography>

				{resolvedTimelineViewMode === BedOccupancyTimelineViewMode.Compact ? (
					<CompactOccupancyTimeline
						transferOccupancies={compactTransferOccupancies}
						primaryOccupancy={compactPrimaryOccupancy}
						selectedOccupancyId={selectedOccupancyId}
						onSelectOccupancy={onSelectOccupancy}
						getCompactTransferPreviewOccupancies={getCompactTransferPreviewOccupancies}
						getCompactTransferTrail={getCompactTransferTrail}
						getOccupancyTimelinePalette={getOccupancyTimelinePalette}
						getBedCode={getBedCode}
						getTimelineDurationMs={getTimelineDurationMs}
						formatTimelineDuration={formatTimelineDuration}
						getTimelineDurationByOccupancy={getTimelineDurationByOccupancy}
						getTimelineDurationByRange={getTimelineDurationByRange}
						isDischargedOccupancy={isDischargedOccupancy}
						t={t}
						_enum={_enum}
					/>
				) : (
					<Box
						sx={{
							overflowX: shouldScrollTimeline ? 'auto' : 'hidden',
							pb: shouldScrollTimeline ? 1 : 0,
							scrollSnapType: shouldScrollTimeline ? 'x proximity' : 'none',
							'&::-webkit-scrollbar': {
								height: 8,
							},
							'&::-webkit-scrollbar-thumb': {
								bgcolor: 'divider',
								borderRadius: 999,
							},
						}}
					>
						<Box
							sx={{
								minWidth: timelineContentMinWidth,
								width: shouldScrollTimeline ? 'auto' : '100%',
								px: 1,
								py: 1,
							}}
						>
							<Stack direction='row' spacing={2} sx={{ width: '100%', alignItems: 'stretch' }}>
								{timelineItems.map((timelineItem, timelineIndex) => {
									const isSelected = isTimelineItemSelected(timelineItem, selectedOccupancyId)
									const isHovered = hoveredTimelineItemId === timelineItem.id
									const isEmphasized = isSelected || isHovered
									const isStickyActiveItem =
										shouldScrollTimeline && timelineItem.containsActive
									const palette = getOccupancyTimelinePalette(
										theme,
										timelineItem.representative?.occupancyStatus,
										isEmphasized || isStickyActiveItem
									)
									const lineColor = isEmphasized ? palette.dotColor : theme.palette.divider
									const timelineTitle =
										timelineItem.type === 'group'
											? getCompactGroupLabel(timelineItem.representative)
											: getBedCode(timelineItem.representative)
									const timelineSubtitle =
										timelineItem.type === 'group' && timelineItem.occupancies.length > 1
											? `${timelineItem.occupancies.length} ${t('bed_occupancy.text.transfer_events')}`
											: getRoomCode(timelineItem.representative)
									const timelineDescription =
										timelineItem.type === 'group' && timelineItem.occupancies.length > 1
											? `${getBedCode(timelineItem.occupancies[0])} -> ${getBedCode(timelineItem.representative)}`
											: renderEmptyFallback(
													timelineItem.representative?.bed?.bedCategoryName
												)
									const timelineFooterPrimary =
										timelineItem.type === 'group' && timelineItem.occupancies.length > 1
											? `${t('bed_occupancy.text.duration_label')}: ${getTimelineDurationByRange(
													timelineItem.occupancies
												)}`
											: renderEmptyFallback(
													formatDatetimeStringBasedOnCurrentLanguage(
														timelineItem.representative?.fromDateTime
													)
												)
									const timelineFooterSecondary =
										timelineItem.type === 'group' && timelineItem.occupancies.length > 1
											? renderEmptyFallback(
													formatDatetimeStringBasedOnCurrentLanguage(
														timelineItem.representative?.fromDateTime
													)
												)
											: timelineItem.representative?.toDateTime
												? formatDatetimeStringBasedOnCurrentLanguage(
														timelineItem.representative.toDateTime
													)
												: t('bed_occupancy.text.now')

									return (
										<Box
											key={timelineItem.id}
											sx={{
												flex: shouldScrollTimeline
													? `0 0 ${timelineItemWidth}px`
													: '1 1 0',
												width: shouldScrollTimeline ? timelineItemWidth : 'auto',
												minWidth: 0,
												maxWidth:
													!shouldScrollTimeline && timelineItems.length === 1
														? BedOccupancySingleTimelineItemMaxWidth
														: 'none',
												mx:
													!shouldScrollTimeline && timelineItems.length === 1
														? 'auto'
														: 0,
												position: isStickyActiveItem ? 'sticky' : 'relative',
												right: isStickyActiveItem ? 0 : 'auto',
												zIndex: isStickyActiveItem ? 4 : 1,
												scrollSnapAlign: shouldScrollTimeline ? 'start' : 'none',
											}}
										>
											<Tooltip
												arrow
												placement='top'
												title={
													<TimelineTooltipContent
														timelineItem={timelineItem}
														previousTimelineItem={timelineItems[timelineIndex - 1]}
														t={t}
													/>
												}
											>
												<ButtonBase
													onClick={() => handleTimelineItemClick(timelineItem)}
													onMouseEnter={() => setHoveredTimelineItemId(timelineItem.id)}
													onMouseLeave={() => setHoveredTimelineItemId(null)}
													sx={{
														width: '100%',
														display: 'block',
														textAlign: 'left',
														borderRadius: 3,
													}}
												>
													<Stack spacing={1.5}>
														<Box sx={{ position: 'relative', height: 28 }}>
															<Box
																sx={{
																	position: 'absolute',
																	left: timelineIndex === 0 ? '50%' : 0,
																	right:
																		timelineIndex === timelineItems.length - 1
																			? '50%'
																			: 0,
																	top: '50%',
																	transform: 'translateY(-50%)',
																	height: isEmphasized ? 4 : 3,
																	bgcolor: lineColor,
																	borderRadius: 999,
																	transition: 'all 0.2s ease',
																}}
															/>
															<Box
																sx={{
																	position: 'absolute',
																	left: '50%',
																	top: '50%',
																	transform: 'translate(-50%, -50%)',
																	width: timelineItem.containsActive ? 18 : 14,
																	height: timelineItem.containsActive ? 18 : 14,
																	borderRadius: '50%',
																	bgcolor: isEmphasized
																		? palette.dotColor
																		: theme.palette.background.paper,
																	border: `3px solid ${palette.dotColor}`,
																	boxShadow:
																		isEmphasized || isStickyActiveItem
																			? `0 8px 20px ${palette.shadowColor}`
																			: 'none',
																	transition:
																		'background-color 0.2s ease, box-shadow 0.2s ease, transform 0.2s ease',
																}}
															/>
														</Box>

														<Paper
															variant='outlined'
															sx={{
																p: 1.5,
																borderRadius: 3,
																borderColor: palette.borderColor,
																bgcolor: isStickyActiveItem
																	? theme.palette.background.paper
																	: palette.backgroundColor,
																boxShadow:
																	isSelected || isStickyActiveItem
																		? `0 10px 24px ${palette.shadowColor}`
																		: isHovered
																			? `0 8px 18px ${palette.shadowColor}`
																			: 'none',
																transform: isSelected
																	? 'translateY(-6px)'
																	: isHovered
																		? 'translateY(-3px)'
																		: 'none',
																transition:
																	'transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease',
																minHeight: 148,
															}}
														>
															<Stack spacing={1.25}>
																<Stack
																	direction='row'
																	justifyContent='space-between'
																	spacing={1}
																	alignItems='flex-start'
																>
																	<Box sx={{ minWidth: 0 }}>
																		<Typography
																			variant='subtitle2'
																			noWrap
																			sx={{ color: palette.titleColor }}
																		>
																			{timelineTitle}
																		</Typography>
																		<Typography
																			variant='caption'
																			color='text.secondary'
																			noWrap
																		>
																			{timelineSubtitle}
																		</Typography>
																	</Box>
																	<Chip
																		label={getEnumLabelByValue(
																			_enum.occupancyStatusOptions,
																			timelineItem.representative?.occupancyStatus
																		)}
																		color={defaultOccupancyStatusStyle(
																			timelineItem.representative?.occupancyStatus
																		)}
																		size='small'
																	/>
																</Stack>

																<Typography variant='body2' fontWeight={500} noWrap>
																	{timelineDescription}
																</Typography>

																<Stack spacing={0.5}>
																	<Typography variant='caption' color='text.secondary' noWrap>
																		{timelineFooterPrimary}
																	</Typography>
																	<Typography variant='caption' color='text.secondary' noWrap>
																		{timelineFooterSecondary}
																	</Typography>
																</Stack>
															</Stack>
														</Paper>
													</Stack>
												</ButtonBase>
											</Tooltip>
										</Box>
									)
								})}
							</Stack>
						</Box>
					</Box>
				)}
			</Stack>
		</Paper>
	)
}

export default MedicalHistoryDetailBedOccupancyTimeline

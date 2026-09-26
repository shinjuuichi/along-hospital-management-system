import { defaultOccupancyStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import {
	BedOccupancyCompactStackOffsetX,
	BedOccupancyCompactStackOffsetY,
} from '@/constants/medicalHistoryConstants'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, ButtonBase, Chip, Grid, Paper, Stack, Typography } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'

const CompactOccupancyTimeline = ({
	transferOccupancies,
	primaryOccupancy,
	selectedOccupancyId,
	onSelectOccupancy,
	getCompactTransferPreviewOccupancies,
	getCompactTransferTrail,
	getOccupancyTimelinePalette,
	getBedCode,
	getTimelineDurationMs,
	formatTimelineDuration,
	getTimelineDurationByOccupancy,
	getTimelineDurationByRange,
	isDischargedOccupancy,
	t,
	_enum,
}) => {
	const theme = useTheme()
	const transferRepresentative = transferOccupancies.at(-1) || null
	const transferPreviewOccupancies = getCompactTransferPreviewOccupancies(transferOccupancies)
	const isTransferSelected = transferOccupancies.some(
		(occupancy) => occupancy.id === selectedOccupancyId
	)
	const isPrimarySelected = primaryOccupancy?.id === selectedOccupancyId
	const isPrimaryDischarged = isDischargedOccupancy(primaryOccupancy)
	const transferPalette = getOccupancyTimelinePalette(
		theme,
		transferRepresentative?.occupancyStatus || EnumConfig.OccupancyStatus.Transferred,
		isTransferSelected
	)
	const primaryPalette = getOccupancyTimelinePalette(
		theme,
		primaryOccupancy?.occupancyStatus || EnumConfig.OccupancyStatus.Active,
		isPrimarySelected
	)

	return (
		<Grid container spacing={2}>
			{transferOccupancies.length > 0 && transferRepresentative && (
				<Grid size={{ xs: 12, md: primaryOccupancy ? 6 : 12 }}>
					<ButtonBase
						onClick={() => onSelectOccupancy(transferRepresentative.id)}
						sx={{ width: '100%', display: 'block', textAlign: 'left', borderRadius: 3 }}
					>
						<Paper
							variant='outlined'
							sx={{
								p: 2,
								borderRadius: 3,
								borderColor: transferPalette.borderColor,
								bgcolor: transferPalette.backgroundColor,
								boxShadow: isTransferSelected
									? `0 12px 28px ${transferPalette.shadowColor}`
									: `0 6px 16px ${alpha(transferPalette.dotColor, 0.1)}`,
								transition: 'box-shadow 0.2s ease, border-color 0.2s ease',
								height: '100%',
							}}
						>
							<Stack spacing={2}>
								<Stack direction='row' justifyContent='space-between' spacing={1.5}>
									<Box sx={{ minWidth: 0 }}>
										<Typography variant='subtitle2' sx={{ color: transferPalette.titleColor }}>
											{t('bed_occupancy.title.occupancy_history')}
										</Typography>
										<Typography variant='caption' color='text.secondary'>
											{`${transferOccupancies.length} ${t('bed_occupancy.text.transfer_events')}`}
										</Typography>
									</Box>
									<Chip
										label={getEnumLabelByValue(
											_enum.occupancyStatusOptions,
											transferRepresentative.occupancyStatus
										)}
										color={defaultOccupancyStatusStyle(transferRepresentative.occupancyStatus)}
										size='small'
									/>
								</Stack>

								<Typography variant='body2' fontWeight={600} noWrap>
									{getCompactTransferTrail(transferOccupancies)}
								</Typography>

								<Box
									sx={{
										position: 'relative',
										height:
											124 +
											Math.max(transferPreviewOccupancies.length - 1, 0) *
												BedOccupancyCompactStackOffsetY,
									}}
								>
									{transferPreviewOccupancies.map((occupancy, index) => {
										const depth = transferPreviewOccupancies.length - index - 1
										const isTopCard = depth === 0
										const previewPalette = getOccupancyTimelinePalette(
											theme,
											occupancy.occupancyStatus,
											isTopCard
										)

										return (
											<Paper
												key={occupancy.id}
												variant='outlined'
												sx={{
													position: 'absolute',
													top: depth * BedOccupancyCompactStackOffsetY,
													left: depth * BedOccupancyCompactStackOffsetX,
													right: 0,
													p: 1.5,
													borderRadius: 2.5,
													borderColor: isTopCard
														? previewPalette.borderColor
														: alpha(previewPalette.dotColor, 0.18),
													bgcolor: isTopCard
														? theme.palette.background.paper
														: alpha(previewPalette.dotColor, 0.06),
													boxShadow: isTopCard
														? `0 10px 24px ${previewPalette.shadowColor}`
														: 'none',
													transform: `scale(${1 - depth * 0.03})`,
													transformOrigin: 'top left',
													zIndex: index + 1,
													overflow: 'hidden',
												}}
											>
												<Stack
													direction='row'
													justifyContent='space-between'
													alignItems='flex-start'
													spacing={1}
												>
													<Box sx={{ minWidth: 0 }}>
														<Typography variant='subtitle2' noWrap>
															{getBedCode(occupancy)}
														</Typography>
														<Typography variant='caption' color='text.secondary' noWrap>
															{renderEmptyFallback(
																occupancy?.bed?.room?.code || occupancy?.bed?.bedCategoryName || null
															)}
														</Typography>
													</Box>
													<Typography variant='caption' color='text.secondary' noWrap>
														{formatTimelineDuration(
															getTimelineDurationMs(
																occupancy?.fromDateTime,
																occupancy?.toDateTime
															)
														)}
													</Typography>
												</Stack>
											</Paper>
										)
									})}
								</Box>

								<Stack direction='row' justifyContent='space-between' spacing={2}>
									<Typography variant='caption' color='text.secondary'>
										{`${t('bed_occupancy.text.duration_label')}: ${getTimelineDurationByRange(
											transferOccupancies
										)}`}
									</Typography>
									<Typography variant='caption' color='text.secondary' noWrap>
										{renderEmptyFallback(
											formatDatetimeStringBasedOnCurrentLanguage(
												transferRepresentative?.toDateTime ||
													transferRepresentative?.fromDateTime
											)
										)}
									</Typography>
								</Stack>
							</Stack>
						</Paper>
					</ButtonBase>
				</Grid>
			)}

			{primaryOccupancy && (
				<Grid size={{ xs: 12, md: transferOccupancies.length > 0 ? 6 : 12 }}>
					<ButtonBase
						onClick={() => onSelectOccupancy(primaryOccupancy.id)}
						sx={{ width: '100%', display: 'block', textAlign: 'left', borderRadius: 3 }}
					>
						<Paper
							variant='outlined'
							sx={{
								p: 2,
								borderRadius: 3,
								borderColor: primaryPalette.borderColor,
								bgcolor: primaryPalette.backgroundColor,
								boxShadow: isPrimarySelected
									? `0 12px 28px ${primaryPalette.shadowColor}`
									: `0 6px 16px ${alpha(primaryPalette.dotColor, 0.12)}`,
								transition: 'box-shadow 0.2s ease, border-color 0.2s ease',
								height: '100%',
							}}
						>
							<Stack spacing={2}>
								<Stack direction='row' justifyContent='space-between' spacing={1.5}>
									<Box sx={{ minWidth: 0 }}>
										<Typography variant='subtitle2' sx={{ color: primaryPalette.titleColor }}>
											{isPrimaryDischarged
												? getEnumLabelByValue(
														_enum.occupancyStatusOptions,
														primaryOccupancy.occupancyStatus
													)
												: t('bed_occupancy.text.current_bed')}
										</Typography>
										<Typography variant='caption' color='text.secondary'>
											{renderEmptyFallback(primaryOccupancy?.bed?.room?.code)}
										</Typography>
									</Box>
									<Chip
										label={getEnumLabelByValue(
											_enum.occupancyStatusOptions,
											primaryOccupancy.occupancyStatus
										)}
										color={defaultOccupancyStatusStyle(primaryOccupancy.occupancyStatus)}
										size='small'
									/>
								</Stack>

								<Box>
									<Typography variant='h5'>{getBedCode(primaryOccupancy)}</Typography>
									<Typography variant='body2' color='text.secondary'>
										{renderEmptyFallback(primaryOccupancy?.bed?.bedCategoryName)}
									</Typography>
								</Box>

								<Stack direction='row' spacing={1} useFlexGap flexWrap='wrap'>
									<Chip
										size='small'
										variant='outlined'
										label={renderEmptyFallback(
											formatDatetimeStringBasedOnCurrentLanguage(primaryOccupancy?.fromDateTime)
										)}
									/>
									{isPrimaryDischarged ? (
										<Chip
											size='small'
											variant='outlined'
										color='info'
										label={`${getEnumLabelByValue(
											_enum.occupancyStatusOptions,
											primaryOccupancy.occupancyStatus
										)}: ${renderEmptyFallback(
											formatDatetimeStringBasedOnCurrentLanguage(primaryOccupancy?.toDateTime)
										)}`}
									/>
									) : (
										<Chip
											size='small'
											variant='outlined'
											color='success'
											label={`${t('bed_occupancy.text.duration_label')}: ${getTimelineDurationByOccupancy(
												primaryOccupancy
											)}`}
										/>
									)}
								</Stack>
							</Stack>
						</Paper>
					</ButtonBase>
				</Grid>
			)}
		</Grid>
	)
}

export default CompactOccupancyTimeline

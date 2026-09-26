import { defaultOccupancyStatusStyle } from '@/configs/defaultStylesConfig'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { BedOutlined, MeetingRoomOutlined } from '@mui/icons-material'
import { Chip, Grid, Paper, Stack, Typography } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'

const MedicalHistoryDetailBedSummaryRow = ({ label, children }) => {
	return (
		<Stack direction='row' justifyContent='space-between' alignItems='center'>
			<Typography variant='body2' color='text.secondary'>
				{label}
			</Typography>
			{children}
		</Stack>
	)
}

const MedicalHistoryDetailBedSummaryValue = ({ value, fontWeight = 500 }) => {
	return (
		<Typography variant='body2' fontWeight={fontWeight}>
			{value}
		</Typography>
	)
}

const MedicalHistoryDetailBedSummarySection = ({
	selectedOccupancy,
	isSelectedBedInfoHighlighted,
	transferNoteDisplay,
	estimatedBedChargeAmount,
	formatDurationInDays,
	occupancyStatusOptions,
	t,
}) => {
	const theme = useTheme()

	return (
		<>
			<Grid container spacing={2}>
				<Grid size={{ xs: 12, md: 6 }}>
					<Paper
						variant='outlined'
						sx={{
							p: 2,
							borderRadius: 2,
							height: '100%',
							bgcolor: 'background.default',
							borderColor: isSelectedBedInfoHighlighted
								? alpha(theme.palette.primary.main, 0.45)
								: undefined,
							boxShadow: isSelectedBedInfoHighlighted
								? `0 0 0 1px ${alpha(theme.palette.primary.main, 0.16)}`
								: 'none',
							transition: 'border-color 0.2s ease, box-shadow 0.2s ease',
						}}
					>
						<Stack direction='row' alignItems='center' spacing={1} mb={2}>
							<BedOutlined color='primary' sx={{ fontSize: 20 }} />
							<Typography variant='subtitle2' fontWeight={600}>
								{t('bed_occupancy.title.bed_info')}
							</Typography>
						</Stack>
						<Stack spacing={1.5}>
							<MedicalHistoryDetailBedSummaryRow label={t('bed.field.code')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(selectedOccupancy?.bed?.code)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed.field.bed_category')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(selectedOccupancy?.bed?.bedCategoryName)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.occupancy_status')}>
								<Chip
									label={getEnumLabelByValue(
										occupancyStatusOptions,
										selectedOccupancy?.occupancyStatus
									)}
									color={defaultOccupancyStatusStyle(selectedOccupancy?.occupancyStatus)}
									size='small'
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.from_date')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(
										formatDatetimeStringBasedOnCurrentLanguage(selectedOccupancy?.fromDateTime)
									)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.to_date')}>
								<MedicalHistoryDetailBedSummaryValue
									value={
										selectedOccupancy?.toDateTime
											? formatDatetimeStringBasedOnCurrentLanguage(selectedOccupancy.toDateTime)
											: t('bed_occupancy.text.now')
									}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.duration_days')}>
								<MedicalHistoryDetailBedSummaryValue
									value={formatDurationInDays(selectedOccupancy?.durationInDays, t)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.daily_rate')}>
								<MedicalHistoryDetailBedSummaryValue
									value={formatCurrencyBasedOnCurrentLanguage(selectedOccupancy?.unitPrice)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('bed_occupancy.field.total_amount')}>
								<MedicalHistoryDetailBedSummaryValue
									value={formatCurrencyBasedOnCurrentLanguage(selectedOccupancy?.totalAmount)}
									fontWeight={600}
								/>
							</MedicalHistoryDetailBedSummaryRow>
						</Stack>
					</Paper>
				</Grid>
				<Grid size={{ xs: 12, md: 6 }}>
					<Paper
						variant='outlined'
						sx={{
							p: 2,
							borderRadius: 2,
							height: '100%',
							bgcolor: 'background.default',
						}}
					>
						<Stack direction='row' alignItems='center' spacing={1} mb={2}>
							<MeetingRoomOutlined color='primary' sx={{ fontSize: 20 }} />
							<Typography variant='subtitle2' fontWeight={600}>
								{t('bed_occupancy.title.room_info')}
							</Typography>
						</Stack>
						<Stack spacing={1.5}>
							<MedicalHistoryDetailBedSummaryRow label={t('room.field.room_code')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(selectedOccupancy?.bed?.room?.code)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('room.field.building')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(selectedOccupancy?.bed?.room?.buildingName)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
							<MedicalHistoryDetailBedSummaryRow label={t('room.field.floor')}>
								<MedicalHistoryDetailBedSummaryValue
									value={renderEmptyFallback(selectedOccupancy?.bed?.room?.floorNumber)}
								/>
							</MedicalHistoryDetailBedSummaryRow>
						</Stack>
					</Paper>
				</Grid>
			</Grid>

			{transferNoteDisplay && (
				<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
					<Stack spacing={0.75}>
						<Typography variant='subtitle2' color='text.secondary'>
							{t(transferNoteDisplay.labelKey)}
						</Typography>
						<Typography variant='body2'>{transferNoteDisplay.value}</Typography>
					</Stack>
				</Paper>
			)}

			<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
				<Stack spacing={0.75}>
					<Typography variant='subtitle2' color='text.secondary'>
						{t('bed_occupancy.title.billing_summary')}
					</Typography>
					<Stack direction='row' justifyContent='space-between' alignItems='center'>
						<Typography variant='body2' color='text.secondary'>
							{t('bed_occupancy.field.total_bed_charge')}
						</Typography>
						<Typography variant='body2' fontWeight={700}>
							{formatCurrencyBasedOnCurrentLanguage(estimatedBedChargeAmount)}
						</Typography>
					</Stack>
					<Typography variant='caption' color='text.secondary'>
						{t('bed_occupancy.text.estimated_billing_hint')}
					</Typography>
				</Stack>
			</Paper>
		</>
	)
}

export default MedicalHistoryDetailBedSummarySection

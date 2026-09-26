import {
	defaultBedStatusStyle,
	defaultMedicalHistoryStatusStyle,
} from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import {
	MeetingRoomOutlined,
	OpenInNewRounded,
	PersonOutlined,
	VisibilityOutlined,
} from '@mui/icons-material'
import { Box, Button, Chip, Divider, Grid, Paper, Stack, Typography } from '@mui/material'

const BedOccupancyCard = ({ bed, onViewInpatient, onOpenDetail }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const currentOccupancy = bed.currentOccupancy
	const hasCurrentOccupancy = Boolean(currentOccupancy?.medicalHistoryId)

	return (
		<Paper
			variant='outlined'
			sx={{
				p: 2,
				borderRadius: 2.5,
				height: '100%',
			}}
		>
			<Stack spacing={2} sx={{ height: '100%' }}>
				<Stack direction='row' justifyContent='space-between' alignItems='flex-start' spacing={1}>
					<Stack spacing={0.5}>
						<Typography variant='subtitle1' fontWeight={700}>
							{bed.code}
						</Typography>
						<Typography variant='body2' color='text.secondary'>
							{bed.bedCategoryName || '-'}
						</Typography>
					</Stack>
					<Chip
						size='small'
						label={getEnumLabelByValue(_enum.bedStatusOptions, bed.status) || bed.status}
						color={defaultBedStatusStyle(bed.status)}
					/>
				</Stack>

				{hasCurrentOccupancy ? (
					<Stack spacing={1.25}>
						<Stack direction='row' spacing={1} alignItems='center'>
							<PersonOutlined fontSize='small' color='action' />
							<Typography variant='body2' fontWeight={600}>
								{currentOccupancy.patientName || '-'}
							</Typography>
						</Stack>
						<Stack direction='row' justifyContent='space-between' spacing={2}>
							<Typography variant='body2' color='text.secondary'>
								{t('medical_history.field.medical_history_number')}
							</Typography>
							<Typography variant='body2' fontWeight={500}>
								{currentOccupancy.medicalHistoryNumber || '-'}
							</Typography>
						</Stack>
						<Stack direction='row' justifyContent='space-between' spacing={2}>
							<Typography variant='body2' color='text.secondary'>
								{t('medical_history.field.admission_date')}
							</Typography>
							<Typography variant='body2' fontWeight={500}>
								{formatDatetimeStringBasedOnCurrentLanguage(
									currentOccupancy.admissionDate || currentOccupancy.fromDateTime
								) || '-'}
							</Typography>
						</Stack>
						<Chip
							size='small'
							variant='outlined'
							label={
								getEnumLabelByValue(
									_enum.medicalHistoryStatusOptions,
									currentOccupancy.medicalHistoryStatus
								) || currentOccupancy.medicalHistoryStatus
							}
							color={defaultMedicalHistoryStatusStyle(currentOccupancy.medicalHistoryStatus)}
							sx={{ width: 'fit-content' }}
						/>
					</Stack>
				) : (
					<Typography color='text.secondary'>
						{bed.status === EnumConfig.BedStatus.Active
							? t('bed_occupancy.text.available')
							: t('bed_occupancy.text.no_current_inpatient')}
					</Typography>
				)}

				<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap sx={{ mt: 'auto' }}>
					{hasCurrentOccupancy && (
						<Button
							size='small'
							variant='contained'
							startIcon={<VisibilityOutlined />}
							onClick={() => onViewInpatient(currentOccupancy.medicalHistoryId)}
						>
							{t('bed_occupancy.button.view_inpatient')}
						</Button>
					)}
					{hasCurrentOccupancy && (
						<Button
							size='small'
							variant='outlined'
							startIcon={<OpenInNewRounded />}
							onClick={() => onOpenDetail(currentOccupancy.medicalHistoryId)}
						>
							{t('button.detail')}
						</Button>
					)}
				</Stack>
			</Stack>
		</Paper>
	)
}

const BedOccupancyRoomBoardSection = ({
	room,
	onViewInpatient,
	onOpenDetail,
	panelHeight,
}) => {
	const { t } = useTranslation()

	if (!room) {
		return (
			<Paper
				sx={{
					p: 3,
					borderRadius: 3,
					height: panelHeight || '100%',
					minHeight: 0,
				}}
			>
				<Stack alignItems='center' justifyContent='center' sx={{ height: '100%', minHeight: 320 }}>
					<Typography color='text.secondary'>
						{t('bed_occupancy.placeholder.no_room_selected')}
					</Typography>
				</Stack>
			</Paper>
		)
	}

	return (
		<Paper
			sx={{
				p: 3,
				borderRadius: 3,
				height: panelHeight || '100%',
				minHeight: 0,
				overflow: 'hidden',
			}}
		>
			<Stack spacing={3} sx={{ height: '100%', minHeight: 0 }}>
				<Stack spacing={1.5}>
					<Stack direction='row' justifyContent='space-between' alignItems='flex-start' spacing={2}>
						<Stack spacing={0.5}>
							<Typography variant='h6'>{room.code}</Typography>
							<Typography color='text.secondary'>
								{room.buildingName} · {t('room.field.floor')} {room.floorNumber} ·{' '}
								{room.specialtyName || '-'}
							</Typography>
						</Stack>
						<Chip
							icon={<MeetingRoomOutlined />}
							label={`${room.totalBeds} ${t('bed_occupancy.summary.beds').toLowerCase()}`}
							color='primary'
							variant='outlined'
						/>
					</Stack>
					<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
						<Chip
							size='small'
							label={`${t('bed_occupancy.summary.occupied')}: ${room.occupiedBeds}`}
							color='info'
						/>
						<Chip
							size='small'
							label={`${t('bed_occupancy.summary.available')}: ${room.availableBeds}`}
							color='success'
						/>
						<Chip
							size='small'
							label={`${t('bed_occupancy.summary.maintenance')}: ${room.maintenanceBeds}`}
							color='warning'
						/>
					</Stack>
				</Stack>

				<Divider />

				<Box
					sx={{
						flex: 1,
						minHeight: 0,
						overflowY: { xs: 'visible', lg: 'auto' },
						pr: { xs: 0, lg: 0.5 },
					}}
				>
					{room.beds.length === 0 ? (
						<Typography color='text.secondary'>
							{t('bed_occupancy.placeholder.no_matching_beds')}
						</Typography>
					) : (
						<Grid container spacing={2}>
							{room.beds.map((bed) => (
								<Grid key={bed.id} size={{ xs: 12, md: 6, xl: 4 }}>
									<BedOccupancyCard
										bed={bed}
										onViewInpatient={onViewInpatient}
										onOpenDetail={onOpenDetail}
									/>
								</Grid>
							))}
						</Grid>
					)}
				</Box>
			</Stack>
		</Paper>
	)
}

export default BedOccupancyRoomBoardSection

import EmptyBox from '@/components/placeholders/EmptyBox'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import {
	defaultComplaintResolveStatusStyle,
	defaultMedicalHistoryStatusStyle,
	defaultMedicalHistoryTypeStyle,
} from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import {
	formatDateBasedOnCurrentLanguage,
	formatDatetimeStringBasedOnCurrentLanguage,
} from '@/utils/formatDateUtil'
import { getObjectValueFromStringPath } from '@/utils/handleObjectUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	alpha,
	Avatar,
	Card,
	CardActionArea,
	CardContent,
	Chip,
	Grid,
	Stack,
	Typography,
	useTheme,
} from '@mui/material'

const ManagementMedicalHistoryCardSection = ({ items, onOpen, loading }) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const theme = useTheme()

	if (loading) {
		return <SkeletonBox numberOfBoxes={3} heights={[150]} />
	}

	if (!items || items.length === 0) {
		return <EmptyBox />
	}

	const fields1st = [
		{
			label: t('medical_history.field.id'),
			value: 'id',
		},
		{
			label: t('medical_history.field.medical_history_number'),
			value: 'medicalHistoryNumber',
		},
		{
			label: t('medical_history.field.diagnosis'),
			value: 'diagnosis',
		},
		{
			label: t('medical_history.field.specialty'),
			value: 'specialty.name',
		},
		{
			label: t('medical_history.field.status'),
			value: (item) => (
				<Chip
					size='small'
					label={getEnumLabelByValue(_enum.medicalHistoryStatusOptions, item.medicalHistoryStatus)}
					color={defaultMedicalHistoryStatusStyle(item.medicalHistoryStatus)}
				/>
			),
		},
		{
			label: t('medical_history.field.type'),
			value: (item) => (
				<Chip
					size='small'
					variant='outlined'
					label={getEnumLabelByValue(_enum.medicalHistoryTypeOptions, item.medicalHistoryType)}
					color={defaultMedicalHistoryTypeStyle(item.medicalHistoryType)}
				/>
			),
		},
	]

	const fields2nd = [
		{
			label: t('medical_history.field.admission_date'),
			value: (item) => (
				<Typography variant='body2'>
					{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(item.admissionDate))}
				</Typography>
			),
		},
		{
			label: t('medical_history.field.discharge_date'),
			value: (item) => (
				<Typography variant='body2'>
					{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(item.dischargeDate))}
				</Typography>
			),
		},
		{
			label: t('medical_history.field.follow_up_appointment_date'),
			value: (item) => (
				<Typography variant='body2'>
					{renderEmptyFallback(formatDateBasedOnCurrentLanguage(item.followUpAppointmentDate))}
				</Typography>
			),
		},
		{
			label: t('medical_history.field.prescription.prescription'),
			value: (item) => {
				return (
					<Typography variant='body2'>
						{t('medical_history.field.prescription.medication_days')} (
						{renderEmptyFallback(item.prescription?.medicationDays)} {t('text.day')})
					</Typography>
				)
			},
		},
		{
			label: t('complaint.field.resolve_status'),
			value: (item) =>
				item.complaint ? (
					<Chip
						size='small'
						label={getEnumLabelByValue(
							_enum.complaintResolveStatusOptions,
							item.complaint.complaintResolveStatus
						)}
						color={defaultComplaintResolveStatusStyle(item.complaint.complaintResolveStatus)}
					/>
				) : (
					<Typography variant='body2'>{renderEmptyFallback(null)}</Typography>
				),
		},
	]

	return items.map((item, index) => (
		<Card
			key={index}
			sx={{
				bgcolor: alpha(theme.palette.background.lightBlue, 0.9),
				borderRadius: 2,
				'&:hover': { boxShadow: 6 },
			}}
		>
			<CardActionArea onClick={() => onOpen(item)}>
				<CardContent>
					<Grid container spacing={2} alignItems='center'>
						<Grid size={{ xs: 12, md: 3 }}>
							<Stack spacing={2}>
								<Stack direction='row' spacing={2} alignItems='start'>
									<Avatar src={getImageFromCloud(item.patient?.image)} />
									<Stack>
										<Typography variant='subtitle1'>
											{t('medical_history.field.patient')}: {item.patient?.name}
										</Typography>
										<Typography variant='body2' color='text.secondary'>
											{item.patient?.gender}
										</Typography>
										<Typography variant='body2' color='text.secondary'>
											{item.patient?.dateOfBirth}
										</Typography>
									</Stack>
								</Stack>
								<Stack direction='row' spacing={2} alignItems='start'>
									<Avatar src={getImageFromCloud(item.doctor?.image)} />
									<Stack>
										<Typography variant='subtitle1'>
											{t('medical_history.field.doctor')}: {item.doctor?.name}
										</Typography>
										{item.doctor ? (
											<>
												<Typography variant='body2' color='text.secondary'>
													{item.doctor.gender}
												</Typography>
												<Typography variant='body2' color='text.secondary'>
													{item.doctor.specialtyName}
												</Typography>
											</>
										) : (
											<Typography variant='body2' color='text.secondary'>
												{t('medical_history.placeholder.no_assigned_doctor')}
											</Typography>
										)}
									</Stack>
								</Stack>
							</Stack>
						</Grid>
						<Grid size={{ xs: 12, md: 9 }}>
							<Stack spacing={1}>
								<Grid container spacing={2}>
									<Grid size={{ xs: 12, md: 6 }}>
										<Stack spacing={1}>
											{fields1st.map((field, idx) => (
												<Stack key={idx} direction='row' spacing={1} alignItems='center'>
													<Typography variant='subtitle2' fontWeight={600}>
														{field.label}:
													</Typography>
													{typeof field.value === 'function' ? (
														field.value(item)
													) : (
														<Typography variant='body2'>
															{renderEmptyFallback(getObjectValueFromStringPath(item, field.value))}
														</Typography>
													)}
												</Stack>
											))}
										</Stack>
									</Grid>
									<Grid size={{ xs: 12, md: 6 }}>
										<Stack spacing={1}>
											{fields2nd.map((field, idx) => (
												<Stack key={idx} direction='row' spacing={1} alignItems='center'>
													<Typography variant='subtitle2' fontWeight={600}>
														{field.label}:
													</Typography>
													{typeof field.value === 'function' ? (
														field.value(item)
													) : (
														<Typography variant='body2'>
															{renderEmptyFallback(getObjectValueFromStringPath(item, field.value))}
														</Typography>
													)}
												</Stack>
											))}
										</Stack>
									</Grid>
								</Grid>
							</Stack>
						</Grid>
					</Grid>
				</CardContent>
			</CardActionArea>
		</Card>
	))
}

export default ManagementMedicalHistoryCardSection

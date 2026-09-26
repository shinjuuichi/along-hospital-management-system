import SkeletonBox from '@/components/skeletons/SkeletonBox'
import {
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
import { Avatar, Button, Chip, Grid, Paper, Stack, Typography, useTheme } from '@mui/material'

const MedicalHistoryDetailHeaderInfoSection = ({
	medicalHistory,
	onClickPatientInfo,
	onClickDoctorInfo,
	loading = false,
}) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const _enum = useEnum()

	const userInfoMappingFields = [
		{ key: 'phone', title: t('profile.field.phone') },
		{ key: 'email', title: t('profile.field.email') },
		{ key: 'gender', title: t('profile.field.gender') },
		{ key: 'dateOfBirth', title: t('profile.field.date_of_birth') },
	]

	const medicalHistoryMappingFields = [
		{ label: t('medical_history.field.medical_history_number'), value: 'medicalHistoryNumber' },
		{
			label: t('medical_history.field.status'),
			value: (item) => (
				<Chip
					label={renderEmptyFallback(
						getEnumLabelByValue(_enum.medicalHistoryStatusOptions, item?.medicalHistoryStatus)
					)}
					size='small'
					color={defaultMedicalHistoryStatusStyle(item?.medicalHistoryStatus)}
					sx={{ px: 1.25, borderRadius: 2 }}
				/>
			),
		},
		{
			label: t('medical_history.field.type'),
			value: (item) => (
				<Chip
					label={renderEmptyFallback(
						getEnumLabelByValue(_enum.medicalHistoryTypeOptions, item?.medicalHistoryType)
					)}
					size='small'
					variant='outlined'
					color={defaultMedicalHistoryTypeStyle(item?.medicalHistoryType)}
					sx={{ px: 1.25, borderRadius: 2 }}
				/>
			),
		},
		{ label: t('medical_history.field.specialty'), value: 'specialty.name' },
		{ label: t('medical_history.field.diagnosis'), value: 'diagnosis' },
		{
			label: t('medical_history.field.admission_date'),
			value: (item) => (
				<Typography variant='body2' color='text.secondary' textAlign={'right'}>
					{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(item?.admissionDate))}
				</Typography>
			),
		},
		{
			label: t('medical_history.field.discharge_date'),
			value: (item) => (
				<Typography variant='body2' color='text.secondary' textAlign={'right'}>
					{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(item?.dischargeDate))}
				</Typography>
			),
		},
		{
			label: t('medical_history.field.follow_up_appointment_date'),
			value: (item) => (
				<Typography variant='body2' color='text.secondary' textAlign={'right'}>
					{renderEmptyFallback(formatDateBasedOnCurrentLanguage(item?.followUpAppointmentDate))}
				</Typography>
			),
		},
	]

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2 }}>
				<Typography variant='h6' gutterBottom>
					{t('medical_history.title.medical_history_information')}
				</Typography>
				<Grid container alignItems='stretch' spacing={2}>
					{[1, 2, 3].map(() => (
						<Grid size={{ xs: 12, md: 4 }} key={Math.random()}>
							<SkeletonBox numberOfBoxes={1} heights={[200]} rounded />
						</Grid>
					))}
				</Grid>
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 3, borderRadius: 2 }}>
			<Typography variant='h6' gutterBottom>
				{t('medical_history.title.medical_history_information')}
			</Typography>
			<Grid container alignItems='stretch' spacing={2}>
				<Grid size={{ xs: 12, md: 4 }}>
					<Paper
						sx={{
							display: 'flex',
							gap: 1,
							height: '100%',
							flexDirection: 'column',
							p: 2,
							bgcolor: theme.palette.background.default,
						}}
					>
						<Stack direction='row' spacing={1} alignItems='center'>
							<Avatar src={getImageFromCloud(medicalHistory?.patient?.image)} />
							<Button variant='text' onClick={onClickPatientInfo}>
								{medicalHistory?.patient?.name}
							</Button>
						</Stack>
						{userInfoMappingFields.map((field) => (
							<Stack direction={'row'} width='100%' justifyContent={'space-between'} key={field.key}>
								<Typography variant='body2' color='text.secondary' whiteSpace={'nowrap'}>
									{field.title}:
								</Typography>
								<Typography variant='body2' color='text.secondary' textAlign={'right'}>
									{renderEmptyFallback(medicalHistory?.patient?.[field.key])}
								</Typography>
							</Stack>
						))}
					</Paper>
				</Grid>
				<Grid size={{ xs: 12, md: 4 }}>
					<Paper
						sx={{
							display: 'flex',
							gap: 1,
							height: '100%',
							flexDirection: 'column',
							p: 2,
							bgcolor: theme.palette.background.default,
						}}
					>
						<Stack direction='row' spacing={1} alignItems='center'>
							<Avatar src={getImageFromCloud(medicalHistory?.doctor?.image)} />
							<Button variant='text' onClick={onClickDoctorInfo}>
								{medicalHistory?.doctor?.name}
							</Button>
						</Stack>
						{userInfoMappingFields.map((field) => (
							<Stack direction={'row'} width='100%' justifyContent={'space-between'} key={field.key}>
								<Typography variant='body2' color='text.secondary' whiteSpace={'nowrap'}>
									{field.title}:
								</Typography>
								<Typography variant='body2' color='text.secondary' textAlign={'right'}>
									{renderEmptyFallback(medicalHistory?.doctor?.[field.key])}
								</Typography>
							</Stack>
						))}
					</Paper>
				</Grid>
				<Grid size={{ xs: 12, md: 4 }}>
					<Paper
						sx={{
							p: 2,
							bgcolor: theme.palette.background.default,
							height: '100%',
							display: 'flex',
							justifyContent: 'center',
							alignItems: 'center',
						}}
					>
						<Stack spacing={1} width='100%' alignItems={{ xs: 'flex-start', md: 'flex-end' }}>
							{medicalHistoryMappingFields.map((field, idx) => (
								<Stack direction={'row'} width='100%' justifyContent={'space-between'} key={idx}>
									<Typography variant='body2' color='text.secondary' whiteSpace={'nowrap'}>
										{field.label}:
									</Typography>
									{typeof field.value === 'function' ? (
										field.value(medicalHistory)
									) : (
										<Typography variant='body2' color='text.secondary' textAlign={'right'}>
											{renderEmptyFallback(getObjectValueFromStringPath(medicalHistory, field.value))}
										</Typography>
									)}
								</Stack>
							))}
						</Stack>
					</Paper>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default MedicalHistoryDetailHeaderInfoSection

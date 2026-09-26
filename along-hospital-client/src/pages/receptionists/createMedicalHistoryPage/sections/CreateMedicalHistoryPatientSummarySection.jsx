import PatientInfoRow from '@/components/infoRows/PatientInfoRow'
import SkeletonPatientCard from '@/components/skeletons/SkeletonPatientCard'
import { defaultAllergySeverityStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import {
	Bloodtype,
	CalendarMonth,
	Email,
	Female,
	FitnessCenter,
	LocationOn,
	Male,
	Phone,
	Straighten,
	Transgender,
	WarningAmber,
} from '@mui/icons-material'
import { Avatar, Box, Chip, Paper, Stack, Typography } from '@mui/material'

const CreateMedicalHistoryPatientSummarySection = ({ patient }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	if (!patient) {
		return (
			<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
				<Typography variant='subtitle1' sx={{ mb: 1.5 }}>
					{t('medical_history.title.patient_summary')}
				</Typography>
				<SkeletonPatientCard />
			</Paper>
		)
	}

	const chipFields = [
		{
			icon:
				patient.gender === EnumConfig.Gender.Male ? (
					<Male color='primary' />
				) : patient.gender === EnumConfig.Gender.Female ? (
					<Female color='error' />
				) : (
					<Transgender color='warning' />
				),
			label: getEnumLabelByValue(_enum.genderOptions, patient.gender),
		},
		{ icon: <Bloodtype color='error' />, label: patient.bloodType },
		patient.height && { icon: <Straighten color='action' />, label: `${patient.height} cm` },
		patient.weight && { icon: <FitnessCenter color='action' />, label: `${patient.weight} kg` },
	].filter(Boolean)

	const patientDataFields = [
		{
			icon: <CalendarMonth />,
			value: patient.dateOfBirth || t('profile.placeholder.no_date_of_birth'),
			isDob: true,
		},
		{ icon: <Phone />, value: patient.phone || t('profile.placeholder.no_phone') },
		{ icon: <Email />, value: patient.email || t('profile.placeholder.no_email') },
		{ icon: <LocationOn />, value: patient.address || t('profile.placeholder.no_address') },
	]

	return (
		<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
			<Typography variant='subtitle1' sx={{ mb: 1.5 }}>
				{t('medical_history.title.patient_summary')}
			</Typography>
			<Stack direction='row' spacing={1.5} alignItems='center' sx={{ mb: 2 }}>
				<Avatar src={patient.image} sx={{ width: 56, height: 56 }} />
				<Box sx={{ flex: 1, minWidth: 0 }}>
					<Stack direction='row' spacing={1} alignItems='center'>
						<Typography variant='h6' noWrap>
							{patient.name}
						</Typography>
						{!!patient.medicalNumber && <Chip size='small' color='info' label={patient.medicalNumber} />}
					</Stack>
					{patientDataFields
						.filter((f) => f.isDob)
						.map((field, index) => (
							<PatientInfoRow key={index} icon={field.icon} value={field.value} />
						))}
				</Box>
			</Stack>
			<Stack container spacing={1.2}>
				<Stack direction='row' spacing={1}>
					{chipFields.map((field, index) => (
						<Chip key={index} size='small' icon={field.icon} label={field.label} />
					))}
				</Stack>

				{patientDataFields
					.filter((f) => !f.isDob)
					.map((field, index) => (
						<PatientInfoRow key={index} icon={field.icon} value={field.value} />
					))}

				<Typography variant='subtitle2' sx={{ mb: 0.5 }}>
					{t('profile.field.allergy.allergies')}
				</Typography>
				{Array.isArray(patient.allergies) && patient.allergies.length > 0 ? (
					<Stack direction='row' gap={1} flexWrap='wrap'>
						{patient.allergies.slice(0, 3).map((al) => (
							<Chip
								key={al.id}
								size='small'
								icon={<WarningAmber color='inherit' />}
								label={`${al.name}${
									al.severityLevel
										? ` (${getEnumLabelByValue(_enum.severityLevelOptions, al.severityLevel)})`
										: ''
								}`}
								sx={{
									bgcolor: defaultAllergySeverityStyle(al.severityLevel),
									color: 'primary.contrastText',
								}}
							/>
						))}
						{patient.allergies.length > 3 && (
							<Chip size='small' label={`+${patient.allergies.length - 3} ${t('text.more')}`} />
						)}
					</Stack>
				) : (
					<Typography variant='body2' color='text.secondary'>
						{t('profile.placeholder.no_allergies')}
					</Typography>
				)}
			</Stack>
		</Paper>
	)
}

export default CreateMedicalHistoryPatientSummarySection

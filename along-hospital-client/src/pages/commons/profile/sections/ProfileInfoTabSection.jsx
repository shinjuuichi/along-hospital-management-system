import DrawerInfoRow from '@/components/infoRows/DrawerInfoRow'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useTranslation from '@/hooks/useTranslation'
import { formatDateToDDMMYYYY } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { ContactEmergency, Info, MedicalInformation } from '@mui/icons-material'
import { Box, Divider, Grid, Paper, Stack, Typography } from '@mui/material'

const ProfileInfoTabSection = ({
	profile,
	role,
	editMode = false,
	formValues = {},
	onFieldChange,
	onFieldHandleChange,
	onFieldRegisterRef,
	submitted = false,
	loading = false,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const isDoctor = role === EnumConfig.Role.Doctor
	const isPatient = role === EnumConfig.Role.Patient
	const specialtyLabel = profile?.specialtyName
	const qualificationLabel = profile?.qualificationName

	const { renderField } = useFieldRenderer(
		formValues,
		onFieldChange,
		onFieldHandleChange,
		onFieldRegisterRef,
		submitted,
		'outlined',
		'medium'
	)

	const genderLabel = editMode
		? renderEmptyFallback(
				formValues.gender
					? getEnumLabelByValue(_enum.genderOptions, formValues.gender) || formValues.gender
					: null
			)
		: renderEmptyFallback(
				profile?.gender
					? getEnumLabelByValue(_enum.genderOptions, profile.gender) || profile.gender
					: null
			)

	const fields = [
		{
			key: 'name',
			title: t('profile.field.name'),
			type: 'text',
			required: false,
		},
		{
			key: 'dateOfBirth',
			title: t('profile.field.date_of_birth'),
			type: 'date',
			required: false,
			maxValue: new Date().toISOString().split('T')[0],
		},
		{
			key: 'gender',
			title: t('profile.field.gender'),
			type: 'select',
			options: _enum.genderOptions,
			required: false,
		},
		{
			key: 'address',
			title: t('profile.field.address'),
			type: 'text',
			required: false,
		},
	]

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2, height: '100%', minHeight: 400 }}>
				<SkeletonBox numberOfBoxes={4} heights={[50, 50, 50, 50]} />
			</Paper>
		)
	}

	const SectionHeader = ({ icon, title }) => (
		<Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 2 }}>
			<Box
				sx={{
					p: 1,
					borderRadius: 1.5,
					bgcolor: 'primary.softBg',
					color: 'primary.main',
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
				}}
			>
				{icon}
			</Box>
			<Typography variant='subtitle1' sx={{ fontWeight: 600 }}>
				{title}
			</Typography>
		</Box>
	)

	return (
		<Paper sx={{ p: 3, borderRadius: 2, height: '100%', minHeight: 400 }}>
			<Stack spacing={3}>
				{/* Personal Information Section */}
				<Box>
					<SectionHeader icon={<Info />} title={t('profile.title.personal_information')} />
					<Grid container spacing={3}>
						<Grid size={{ xs: 12, md: 6 }}>
							<Stack spacing={2.5}>
								{editMode ? (
									renderField(fields.find((f) => f.key === 'name'))
								) : (
									<DrawerInfoRow label={t('profile.field.name')} value={renderEmptyFallback(profile?.name)} />
								)}
								{editMode ? (
									renderField(fields.find((f) => f.key === 'dateOfBirth'))
								) : (
									<DrawerInfoRow
										label={t('profile.field.date_of_birth')}
										value={renderEmptyFallback(formatDateToDDMMYYYY(profile?.dateOfBirth))}
									/>
								)}
								{editMode ? (
									renderField(fields.find((f) => f.key === 'gender'))
								) : (
									<DrawerInfoRow label={t('profile.field.gender')} value={genderLabel} />
								)}
							</Stack>
						</Grid>
						<Grid size={{ xs: 12, md: 6 }}>
							<Stack spacing={2.5}>
								<DrawerInfoRow label={t('profile.field.email')} value={renderEmptyFallback(profile?.email)} />
								<DrawerInfoRow label={t('profile.field.phone')} value={renderEmptyFallback(profile?.phone)} />
								{editMode ? (
									renderField(fields.find((f) => f.key === 'address'))
								) : (
									<DrawerInfoRow label={t('profile.field.address')} value={renderEmptyFallback(profile?.address)} />
								)}
							</Stack>
						</Grid>
					</Grid>
				</Box>
				{isDoctor && (
					<Box>
						<Divider sx={{ my: 1 }} />
						<SectionHeader
							icon={<MedicalInformation />}
							title={t('profile.title.professional_information')}
						/>
						<Grid container spacing={3}>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={2.5}>
									<DrawerInfoRow
										label={t('profile.field.qualification')}
										value={renderEmptyFallback(qualificationLabel)}
									/>
								</Stack>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={2.5}>
									<DrawerInfoRow
										label={t('profile.field.specialty')}
										value={renderEmptyFallback(specialtyLabel)}
									/>
								</Stack>
							</Grid>
						</Grid>
					</Box>
				)}
				{isPatient && (
					<Box>
						<Divider sx={{ my: 1 }} />
						<SectionHeader icon={<ContactEmergency />} title={t('profile.title.health_information')} />
						<Grid container spacing={3}>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={2.5}>
									<DrawerInfoRow
										label={t('profile.field.medical_number')}
										value={renderEmptyFallback(profile?.medicalNumber)}
									/>
									<DrawerInfoRow
										label={t('profile.field.height')}
										value={renderEmptyFallback(profile?.height > 0 ? `${profile.height} cm` : null)}
									/>
								</Stack>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={2.5}>
									<DrawerInfoRow
										label={t('profile.field.weight')}
										value={renderEmptyFallback(profile?.weight > 0 ? `${profile.weight} kg` : null)}
									/>
									<DrawerInfoRow label={t('profile.field.blood_type')} value={renderEmptyFallback(profile?.bloodType)} />
								</Stack>
							</Grid>
						</Grid>
					</Box>
				)}
			</Stack>
		</Paper>
	)
}

export default ProfileInfoTabSection

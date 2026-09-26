import GenericDrawer from '@/components/generals/GenericDrawer'
import {
	defaultComplaintResolveStatusStyle,
	defaultMedicalHistoryStatusStyle,
	defaultMedicalHistoryTypeStyle,
} from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import {
	formatDateAndTimeBasedOnCurrentLanguage,
	formatDatetimeStringBasedOnCurrentLanguage,
} from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { LocalPharmacy } from '@mui/icons-material'
import { Avatar, Box, Button, Chip, Stack, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const ManagementMedicalHistoryDetailDrawerSection = ({ open, onClose, item }) => {
	const { t } = useTranslation()
	const { auth } = useAuth()
	const _enum = useEnum()
	const navigate = useNavigate()

	const fields = !item
		? []
		: [
				{
					title: (
						<Stack direction='row' justifyContent='space-between' alignItems='center'>
							<Typography variant='subtitle1'>
								{t('medical_history.title.medical_history_information')}
							</Typography>
							<Chip
								size='small'
								label={getEnumLabelByValue(_enum.medicalHistoryStatusOptions, item.medicalHistoryStatus)}
								color={defaultMedicalHistoryStatusStyle(item.medicalHistoryStatus)}
							/>
						</Stack>
					),
					of: [
						{ label: t('medical_history.field.id'), value: item.id },
						{
							label: t('medical_history.field.medical_history_number'),
							value: item.medicalHistoryNumber,
						},
						{ label: t('medical_history.field.diagnosis'), value: item.diagnosis },
						{
							label: t('medical_history.field.type'),
							value: (
								<Chip
									size='small'
									variant='outlined'
									label={getEnumLabelByValue(_enum.medicalHistoryTypeOptions, item.medicalHistoryType)}
									color={defaultMedicalHistoryTypeStyle(item.medicalHistoryType)}
								/>
							),
						},
						{ label: t('medical_history.field.specialty'), value: item.specialty?.name },
						{
							label: t('medical_history.field.admission_date'),
							value: formatDatetimeStringBasedOnCurrentLanguage(item.admissionDate),
						},
						{
							label: t('medical_history.field.discharge_date'),
							value: formatDatetimeStringBasedOnCurrentLanguage(item.dischargeDate),
						},
						{
							label: t('medical_history.field.follow_up_appointment_date'),
							value: formatDateAndTimeBasedOnCurrentLanguage(item.followUpAppointmentDate),
						},
						{
							label: t('medical_history.field.prescription.prescription'),
							value: item.prescription ? (
								<Stack direction='row' spacing={1} alignItems='center'>
									<LocalPharmacy fontSize='small' />
									<Typography variant='body2'>
										{t('medical_history.field.prescription.medication_days')} (
										{renderEmptyFallback(item.prescription?.medicationDays)} {t('text.day')})
									</Typography>
								</Stack>
							) : (
								renderEmptyFallback(null)
							),
						},
						{
							label: t('complaint.field.resolve_status'),
							value: item.complaint ? (
								<Chip
									size='small'
									label={getEnumLabelByValue(
										_enum.complaintResolveStatusOptions,
										item.complaint.complaintResolveStatus
									)}
									color={defaultComplaintResolveStatusStyle(item.complaint.complaintResolveStatus)}
								/>
							) : (
								renderEmptyFallback(null)
							),
						},
					],
				},
				{
					title: (
						<Box>
							<Typography variant='subtitle1' sx={{ mb: 1.25 }}>
								{t('appointment.title.patient_info')}
							</Typography>
							<Stack direction='row' spacing={2} alignItems='center'>
								<Avatar src={getImageFromCloud(item.patient?.image)} />
								<Stack>
									<Typography variant='body1' sx={{ fontWeight: 600 }}>
										{item.patient?.name}
									</Typography>
									<Typography variant='body2' sx={{ color: 'text.secondary' }}>
										{item.patient?.email}
									</Typography>
								</Stack>
							</Stack>
						</Box>
					),
					of: [
						{ label: t('profile.field.phone'), value: item.patient?.phone },
						{
							label: t('profile.field.date_of_birth'),
							value: formatDateAndTimeBasedOnCurrentLanguage(item.patient?.dateOfBirth),
						},
						{ label: t('profile.field.gender'), value: item.patient?.gender },
						{ label: t('profile.field.address'), value: item.patient?.address },
						{ label: t('profile.field.medical_number'), value: item.patient?.medicalNumber },
						{
							label: t('profile.field.height_weight'),
							value: `${renderEmptyFallback(item.patient?.height)} cm / ${renderEmptyFallback(item.patient?.weight)} kg`,
						},
						{ label: t('profile.field.blood_type'), value: item.patient?.bloodType },
					],
				},
				{
					title: (
						<Box>
							<Typography variant='subtitle1' sx={{ mb: 1.25 }}>
								{t('appointment.title.doctor_info')}
							</Typography>
							<Stack direction='row' spacing={2} alignItems='center'>
								<Avatar src={getImageFromCloud(item.doctor?.image)} />
								<Stack>
									<Typography variant='body1' sx={{ fontWeight: 600 }}>
										{item.doctor?.name}
									</Typography>
									<Typography variant='body2' sx={{ color: 'text.secondary' }}>
										{item.doctor?.email}
									</Typography>
								</Stack>
							</Stack>
						</Box>
					),
					of: [
						{ label: t('profile.field.phone'), value: item.doctor?.phone },
						{
							label: t('profile.field.specialty'),
							value: item.doctor?.specialtyName || item.doctor?.specialty,
						},
					],
				},
			]

	const navigateToDetailBasedOnRole = () => {
		if (!item) return

		const isPatient = auth?.role === EnumConfig.Role.Patient
		navigate(
			isPatient
				? routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.MEDICAL_HISTORY.DETAIL(item.id))
				: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(item.id))
		)
	}

	const buttons = (
		<Stack direction='row' spacing={2}>
			<Button variant='contained' color='primary' onClick={navigateToDetailBasedOnRole} fullWidth>
				{t('medical_history.button.view_full_detail')}
			</Button>
		</Stack>
	)

	return (
		<GenericDrawer
			open={open}
			onClose={onClose}
			title={t('medical_history.title.medical_history_detail')}
			fields={fields}
			buttons={buttons}
		/>
	)
}

export default ManagementMedicalHistoryDetailDrawerSection

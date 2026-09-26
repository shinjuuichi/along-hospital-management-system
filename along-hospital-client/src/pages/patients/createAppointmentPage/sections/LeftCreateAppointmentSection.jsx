import ConfirmationButton from '@/components/generals/ConfirmationButton'
import SkeletonTextField from '@/components/skeletons/SkeletonTextField'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import CreateAppointmentTimeSlotChoiceSection from '@/pages/patients/createAppointmentPage/sections/CreateAppointmentTimeSlotChoiceSection'
import { maxLen } from '@/utils/validateUtil'
import { Divider, Grid, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'

const LeftCreateAppointmentSection = ({
	specialties,
	timeSlots,
	userProfile,
	onSubmit,
	onValuesChange,
	loadingGet,
	loadingTimeSlots,
	loadingSubmit,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [submitted, setSubmitted] = useState(false)

	const { values, handleChange, setField, registerRef, validateAll } = useForm({})
	const { renderField, hasRequiredMissing } = useFieldRenderer(
		{ ...userProfile, ...values },
		setField,
		handleChange,
		registerRef,
		submitted,
		'outlined',
		'medium'
	)

	useEffect(() => {
		onValuesChange(values)
	}, [values, onValuesChange])

	useEffect(() => {
		if (!values.timeSlotId) {
			return
		}

		const selectedSlot = timeSlots.find((s) => String(s.id) === String(values.timeSlotId))

		const availableCapacity =
			values.appointmentMeetingType === EnumConfig.AppointmentMeetingType.InPerson
				? selectedSlot?.availableCapacityForInPerson
				: values.appointmentMeetingType === EnumConfig.AppointmentMeetingType.Telehealth
					? selectedSlot?.availableCapacityForTeleHealth
					: 0

		if (!selectedSlot || availableCapacity <= 0) {
			setField('timeSlotId', '')
		}
	}, [values.timeSlotId, values.appointmentMeetingType, timeSlots])

	const patientFields = [
		{
			key: 'name',
			title: t('profile.field.name'),
			required: false,
		},
		{
			key: 'email',
			title: t('profile.field.email'),
			type: 'email',
			required: false,
		},
		{
			key: 'phone',
			title: t('profile.field.phone'),
			required: false,
		},
		{
			key: 'dateOfBirth',
			title: t('profile.field.date_of_birth'),
			type: 'date',
			required: false,
		},
		{
			key: 'address',
			title: t('profile.field.address'),
			multiple: 3,
			required: false,
		},
		{
			key: 'gender',
			title: t('profile.field.gender'),
			required: false,
		},
	]

	const appointmentFields = [
		{
			key: 'specialtyId',
			title: t('appointment.field.specialty'),
			type: 'select',
			options: specialties.map((s) => ({ label: s.name, value: s.id })),
		},
		{
			key: 'date',
			title: t('appointment.field.date'),
			required: false,
		},
		{
			key: 'timeSlotId',
			title: t('appointment.field.time_slot'),
			required: false,
		},
		{
			key: 'appointmentMeetingType',
			title: t('appointment.field.meeting_type'),
			type: 'select',
			options: _enum.appointmentMeetingTypeOptions,
		},
		{
			key: 'purpose',
			title: t('appointment.field.purpose'),
			multiple: 3,
			validate: [maxLen(1000)],
			required: false,
		},
	]

	const fields = [...patientFields, ...appointmentFields]

	const handleSubmit = async () => {
		setSubmitted(true)
		const ok = validateAll()
		const isMissing = hasRequiredMissing(fields)
		const missingDateOrTimeSlot = !values.date || !values.timeSlotId

		if (!ok || isMissing || missingDateOrTimeSlot) {
			toast.warn(t('error.fill_all_required'))
		} else {
			onSubmit(values)
		}
	}

	return (
		<Stack spacing={2}>
			<Typography variant='h4' sx={{ color: 'text.primary' }}>
				{t('appointment.title.create_appointment')}
			</Typography>
			<Typography variant='body2' sx={{ color: 'text.secondary' }}>
				* {t('appointment.title.create_appointment_note')}
			</Typography>
			<Grid container spacing={2} px={2} flexWrap={'nowrap'} maxHeight={'100%'} overflow={'auto'}>
				<Grid size={{ sm: 12, md: 6 }}>
					{loadingGet ? (
						<SkeletonTextField numberOfRow={patientFields?.length || 0} withTitle withLabel />
					) : (
						<>
							<Typography variant='h6' sx={{ mb: 2, color: 'text.primary' }}>
								{t('appointment.title.patient_info')}
							</Typography>
							<Stack spacing={2}>
								{patientFields.map((f) => renderField({ ...f, props: { disabled: true } }))}
							</Stack>
						</>
					)}
				</Grid>

				<Grid size={'auto'}>
					<Divider />
				</Grid>

				<Grid size={{ sm: 12, md: 6 }}>
					{loadingGet ? (
						<SkeletonTextField numberOfRow={appointmentFields?.length || 0} withLabel withTitle />
					) : (
						<>
							<Typography variant='h6' sx={{ mb: 2, color: 'text.primary' }}>
								{t('appointment.title.appointment_info')}
							</Typography>
							<Stack spacing={2}>
								{appointmentFields
									.filter((f) => !['date', 'timeSlotId'].includes(f.key))
									.map((f) => renderField(f))}
								<CreateAppointmentTimeSlotChoiceSection
									date={values.date}
									timeSlotId={values.timeSlotId}
									timeSlots={timeSlots}
									appointmentMeetingType={values.appointmentMeetingType}
									onDateChange={(selectedDate) => {
										setField('date', selectedDate)
										setField('timeSlotId', '')
									}}
									onTimeSlotChange={(id) => setField('timeSlotId', id)}
									disabled={!values.specialtyId || !values.appointmentMeetingType}
									loading={loadingTimeSlots}
								/>
							</Stack>
						</>
					)}
				</Grid>
			</Grid>
			<ConfirmationButton
				confirmationTitle={t('appointment.dialog.confirm_create_title')}
				confirmationDescription={t('appointment.dialog.confirm_create_description')}
				confirmButtonText={t('button.create')}
				confirmButtonColor='primary'
				onConfirm={handleSubmit}
				variant='contained'
				sx={{ width: '50%' }}
				loading={loadingSubmit}
			>
				{t('button.create')}
			</ConfirmationButton>
		</Stack>
	)
}

export default LeftCreateAppointmentSection

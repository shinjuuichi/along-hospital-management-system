import StaffRenderOption from '@/components/renderOptions/StaffRenderOption'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo } from 'react'

const CreateMedicalHistoryDataSection = ({
	specialtyOptions = [],
	showDoctorField = false,
	availableDoctors = [],
	disableSpecialtyField = false,
	disableDoctorField = false,
	medicalHistoryTypeOptions = [],
	defaultType = null,
	onChange,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const { values, handleChange, setField, registerRef } = useForm({
		medicalHistoryType: defaultType,
		specialtyId: null,
		assignedDoctorId: null,
	})
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'medium'
	)

	const doctorOptions = useMemo(
		() =>
			(availableDoctors || []).map((doctor) => ({
				value: doctor.userId,
				label: doctor,
				searchKey: `${doctor.name || ''} ${doctor.phone || ''}`.trim(),
			})),
		[availableDoctors]
	)

	useEffect(() => {
		onChange?.(values)
	}, [values, onChange])

	const fields = [
		{
			key: 'medicalHistoryType',
			title: t('medical_history.field.type'),
			type: 'radio',
			options:
				medicalHistoryTypeOptions.length > 0
					? medicalHistoryTypeOptions
					: _enum.medicalHistoryTypeOptions,
			props: { disabled: Boolean(defaultType) },
		},
		{
			key: 'specialtyId',
			title: t('appointment.field.specialty'),
			type: 'select',
			options: specialtyOptions.map((s) => ({ label: s.name, value: s.id })),
			props: { disabled: disableSpecialtyField },
		},
		...(showDoctorField
			? [
					{
						key: 'assignedDoctorId',
						title: t('medical_history.title.assign_doctor_to_medical_history'),
						type: 'select-dialog',
						options: doctorOptions,
						renderOption: (_, doctor) => <StaffRenderOption staff={doctor} />,
						props: { disabled: disableDoctorField },
					},
				]
			: []),
	]

	return (
		<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
			<Typography variant='subtitle1' sx={{ mb: 1.5 }}>
				{t('medical_history.title.medical_record_info')}
			</Typography>
			<Stack gap={2}>{fields.map((field) => renderField(field))}</Stack>

			<Typography variant='caption' sx={{ mt: 1.5, display: 'block', color: 'text.secondary' }}>
				{t('medical_history.placeholder.fill_all_before_create')}
			</Typography>
		</Paper>
	)
}

export default CreateMedicalHistoryDataSection

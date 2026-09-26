import FilterButton from '@/components/buttons/FilterButton'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Typography } from '@mui/material'
import { Grid, Stack } from '@mui/system'

const ManagementMedicalHistoryFilterSection = ({
	filters,
	setFilters,
	patientNames,
	doctorNames,
	loading,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const { values, handleChange, setField, registerRef } = useForm(filters)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const { auth } = useAuth()
	const isPatient = auth?.role === EnumConfig.Role.Patient

	const fields1st = [
		!isPatient && {
			key: 'medicalHistoryStatus',
			title: t('medical_history.field.status'),
			type: 'select',
			options: [{ label: t('text.all'), value: '' }, ..._enum.medicalHistoryStatusOptions],
			required: false,
		},
		{
			key: 'startDate',
			title: t('text.start_date'),
			type: 'date',
			required: false,
		},
		{
			key: 'endDate',
			title: t('text.end_date'),
			type: 'date',
			required: false,
		},
	].filter(Boolean)

	const fields2nd = [
		{
			key: 'medicalHistoryNumber',
			title: t('medical_history.placeholder.search_medical_history_number'),
			type: 'search',
			required: false,
		},
		patientNames && {
			key: 'patientName',
			title: t('medical_history.placeholder.search_patient_name'),
			type: 'search',
			options: patientNames,
			required: false,
		},
		doctorNames && {
			key: 'doctorName',
			title: t('medical_history.placeholder.search_doctor_name'),
			options: doctorNames,
			type: 'search',
			required: false,
		},
	].filter(Boolean)

	return (
		<Paper sx={{ p: 2, display: 'flex', flexWrap: 'wrap', gap: 2, bgcolor: 'background.default' }}>
			<Stack spacing={2} width={'100%'}>
				<Typography variant='caption'>{t('text.filters')}</Typography>
				<Grid container spacing={2}>
					{fields1st.map((field) => (
						<Grid size={{ xs: 12, md: Math.floor(12 / fields1st.length) }} key={field.key}>
							{renderField(field)}
						</Grid>
					))}
				</Grid>
				<Grid container spacing={2}>
					{fields2nd.map((field) => (
						<Grid
							size={{ xs: 12, md: fields2nd.length !== 0 ? Math.floor(10 / fields2nd.length) : 0 }}
							key={field.key}
						>
							{renderField(field)}
						</Grid>
					))}
					<Grid size={{ xs: 12, md: fields2nd.length !== 0 ? 2 : 12 }}>
						<FilterButton onFilterClick={() => setFilters(values)} loading={loading} fullWidth />
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default ManagementMedicalHistoryFilterSection

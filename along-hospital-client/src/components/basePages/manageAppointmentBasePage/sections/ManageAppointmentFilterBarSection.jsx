import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'

const ManageAppointmentFilterBarSection = ({
	filters,
	setFilters,
	specialties,
	loading = false,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const { auth } = useAuth()
	const isPatient = auth?.role === EnumConfig.Role.Patient

	const { values, handleChange, setField, registerRef, reset } = useForm(filters)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const fields1st = [
		{ key: 'startDate', title: t('text.start_date'), type: 'date', required: false },
		{ key: 'endDate', title: t('text.end_date'), type: 'date', required: false },
	]

	const fields2nd = [
		{
			key: 'specialtyId',
			title: t('appointment.field.specialty'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				...specialties.map((spec) => ({
					value: spec.id,
					label: spec.name,
				})),
			],
			required: false,
		},
		{
			key: 'meetingType',
			title: t('appointment.field.meeting_type'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.appointmentMeetingTypeOptions],
			required: false,
		},
	]

	return (
		<Stack
			spacing={1.5}
			sx={{
				pt: 1,
				pb: 2,
				px: 2,
				bgcolor: 'background.paper',
				border: (theme) => `1px solid ${theme.palette.divider}`,
				borderRadius: 1,
			}}
		>
			<Typography variant='caption'>{t('text.filters')}</Typography>

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields1st.map(renderField)}
			</Stack>
			{isPatient && (
				<Stack direction='row' spacing={2} alignItems='center'>
					{fields2nd.map(renderField)}
				</Stack>
			)}
			<Grid container spacing={2}>
				<Grid size={{ xs: 6, md: 2 }}>
					<FilterButton
						onFilterClick={() => setFilters(values)}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
				<Grid size={{ xs: 6, md: 2 }}>
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default ManageAppointmentFilterBarSection

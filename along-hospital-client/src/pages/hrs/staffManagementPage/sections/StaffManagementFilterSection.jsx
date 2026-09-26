import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'
import { useMemo } from 'react'

const StaffManagementFilterSection = ({
	filters = {},
	setFilters,
	loading = false,
	specialties = [],
	qualifications = [],
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const roleOptions = useMemo(
		() => _enum.roleOptions.filter((opt) => opt.value !== EnumConfig.Role.Patient),
		[_enum.roleOptions]
	)

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

	const filterFieldsRow1 = [
		{
			key: 'role',
			title: t('staff.field.role'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...roleOptions],
			required: false,
		},
		{
			key: 'status',
			title: t('staff.field.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.staffStatusOptions],
			required: false,
		},
		{
			key: 'gender',
			title: t('staff.field.gender'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.genderOptions],
			required: false,
		},
	]

	const filterFieldsRow2 = [
		{
			key: 'specialtyId',
			title: t('staff.field.specialty'),
			type: 'select-dialog',
			options: [
				{ value: '', label: t('text.all') },
				...(specialties || [])
					.map((s) => ({ value: s?.id, label: s?.name }))
					.filter((o) => o.value != null),
			],
			required: false,
		},
		{
			key: 'qualificationId',
			title: t('staff.field.qualification'),
			type: 'select-dialog',
			options: [
				{ value: '', label: t('text.all') },
				...(qualifications || [])
					.map((q) => ({ value: q?.id, label: q?.name }))
					.filter((o) => o.value != null),
			],
			required: false,
		},
	]

	const filterFieldsRow3 = [
		{
			key: 'name',
			title: t('staff.placeholder.search_staff'),
			type: 'search',
			required: false,
		},
		{
			key: 'phone',
			title: t('staff.placeholder.search_phone'),
			type: 'search',
			required: false,
		},
		{
			key: 'email',
			title: t('staff.placeholder.search_email'),
			type: 'search',
			required: false,
		},
	]

	return (
		<Paper
			sx={{
				bgcolor: 'background.default',
				p: 2,
			}}
		>
			<Stack spacing={2}>
				<Grid container spacing={2} columns={12} alignItems='center'>
					{filterFieldsRow1.map((field) => (
						<Grid key={field.key} size={4}>
							{renderField(field)}
						</Grid>
					))}
				</Grid>

				<Grid container spacing={2} columns={12} alignItems='center'>
					{filterFieldsRow2.map((field) => (
						<Grid key={field.key} size={4}>
							{renderField(field)}
						</Grid>
					))}
				</Grid>

				<Grid container spacing={2} columns={12} alignItems='center'>
					{filterFieldsRow3.map((field) => (
						<Grid key={field.key} size={4}>
							{renderField(field)}
						</Grid>
					))}
				</Grid>

				<Grid container spacing={2} columns={12} alignItems='center' justifyContent='flex-end'>
					<Grid size={2}>
						<FilterButton fullWidth loading={loading} onFilterClick={() => setFilters(values)} />
					</Grid>
					<Grid size={2}>
						<ResetFilterButton
							fullWidth
							loading={loading}
							onResetFilterClick={() => {
								reset({})
								setFilters({})
							}}
						/>
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default StaffManagementFilterSection

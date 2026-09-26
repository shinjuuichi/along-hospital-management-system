import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'
import { useEffect, useMemo } from 'react'

const JobPostingFilterBarSection = ({ filters = {}, setFilters = () => {}, loading = false }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const initialValues = {
		status: filters.status || '',
		employmentType: filters.employmentType || '',
		role: filters.role || '',
	}

	const { values, setField, handleChange, reset, registerRef } = useForm(initialValues)

	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	useEffect(() => {
		reset(initialValues)
	}, [filters])

	const withAllOption = (options = []) => [{ value: '', label: t('text.all') }, ...options]

	const filterFields = useMemo(
		() => [
			{
				key: 'status',
				title: t('job_posting.field.status'),
				type: 'select',
				options: withAllOption(_enum.jobPostingManagementStatusOptions),
				required: false,
			},
			{
				key: 'employmentType',
				title: t('job_posting.field.employment_type'),
				type: 'select',
				options: withAllOption(_enum.employmentTypeOptions),
				required: false,
			},
			{
				key: 'role',
				title: t('job_posting.field.role'),
				type: 'select',
				options: withAllOption(_enum.roleOptions),
				required: false,
			},
		],
		[t, _enum]
	)

	const applyFilters = () => setFilters(values)

	const resetFilters = () => {
		const empty = { status: '', employmentType: '', role: '' }
		setFilters(empty)
	}

	return (
		<Paper
			elevation={0}
			sx={{
				borderRadius: 2,
				bgcolor: 'transparent',
			}}
		>
			<Stack spacing={2}>
				<Grid container spacing={2} columns={12} alignItems='center'>
					{filterFields.map((f) => (
						<Grid key={f.key} size={4}>
							{renderField(f)}
						</Grid>
					))}
				</Grid>

				<Grid container spacing={2} columns={12} alignItems='center' justifyContent='flex-end'>
					<Grid size={2}>
						<FilterButton fullWidth loading={loading} onFilterClick={applyFilters} />
					</Grid>
					<Grid size={2}>
						<ResetFilterButton fullWidth loading={loading} onResetFilterClick={resetFilters} />
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default JobPostingFilterBarSection

import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper } from '@mui/material'
import { useCallback, useEffect, useMemo } from 'react'

const JobApplicationFilterSection = ({ filters = {}, setFilters = () => {}, loading = false }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const initialValues = useMemo(
		() => ({
			email: filters.email || '',
			status: filters.applicationStatus || '',
		}),
		[filters.email, filters.applicationStatus]
	)

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
	}, [filters, initialValues, reset])

	const withAllOption = useCallback(
		(options = []) => [{ value: '', label: t('text.all') }, ...options],
		[t]
	)

	const filterFields = useMemo(
		() => [
			{
				key: 'email',
				title: t('job_application.field.email'),
				type: 'text',
				required: false,
			},
			{
				key: 'status',
				title: t('job_application.field.status'),
				type: 'select',
				options: withAllOption(_enum.jobApplicationStatusOptions),
				required: false,
			},
		],
		[t, _enum, withAllOption]
	)

	const applyFilters = () =>
		setFilters({
			email: values.email || '',
			applicationStatus: values.status ? String(values.status) : '',
		})

	const resetFilters = () => {
		const empty = { email: '', applicationStatus: '' }
		reset(empty)
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
			<Grid container spacing={2} columns={12} alignItems='center'>
				{filterFields.map((f) => (
					<Grid key={f.key} size={3}>
						{renderField(f)}
					</Grid>
				))}
				<Grid size={2}>
					<FilterButton fullWidth loading={loading} onFilterClick={applyFilters} />
				</Grid>
				<Grid size={2}>
					<ResetFilterButton fullWidth loading={loading} onResetFilterClick={resetFilters} />
				</Grid>
			</Grid>
		</Paper>
	)
}

export default JobApplicationFilterSection

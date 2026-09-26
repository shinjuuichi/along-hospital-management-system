import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'

const initialFilterValues = {
	status: '',
}

const SalaryAdvanceFilterSection = ({
	filters = initialFilterValues,
	setFilters,
	loading = false,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
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

	const filterFields = [
		{
			key: 'status',
			title: t('salary_advance.field.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.salaryAdvanceStatusOptions],
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
				<Grid container spacing={2}>
					{filterFields.map((field) => (
						<Grid size={{ xs: 12, md: 8 }} key={field.key}>
							{renderField(field)}
						</Grid>
					))}
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
								reset(initialFilterValues)
								setFilters(initialFilterValues)
							}}
							fullWidth
							loading={loading}
							sx={{ flexGrow: 1 }}
						/>
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default SalaryAdvanceFilterSection

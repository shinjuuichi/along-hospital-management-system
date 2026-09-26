import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'

const defaultFilters = {
	month: '',
	year: '',
	status: '',
	name: '',
}

const PayrollManagementFilterSection = ({
	filters = defaultFilters,
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

	const fields = [
		{
			key: 'name',
			title: t('payroll.placeholder.search_payroll'),
			type: 'search',
			required: false,
		},
		{
			key: 'month',
			title: t('payroll.field.month'),
			type: 'number',
			required: false,
			props: {
				inputProps: {
					min: 1,
					max: 12,
				},
			},
		},
		{
			key: 'year',
			title: t('payroll.field.year'),
			type: 'number',
			required: false,
			props: {
				inputProps: {
					min: 2000,
				},
			},
		},
		{
			key: 'status',
			title: t('payroll.field.status'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ..._enum.payrollStatusOptions],
		},
	]

	return (
		<Paper
			sx={{
				bgcolor: 'background.default',
				p: 2,
			}}
		>
			<Grid container spacing={2} columns={12} alignItems='center'>
				{fields.map((field) => (
					<Grid key={field.key} size={{ xs: 12, md: 6, lg: 3 }}>
						{renderField(field)}
					</Grid>
				))}
				<Grid size={{ xs: 12, lg: 3 }}>
					<Stack direction='row' spacing={1}>
						<FilterButton loading={loading} onFilterClick={() => setFilters(values)} />
						<ResetFilterButton
							loading={loading}
							onResetFilterClick={() => {
								reset(defaultFilters)
								setFilters(defaultFilters)
							}}
						/>
					</Stack>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default PayrollManagementFilterSection

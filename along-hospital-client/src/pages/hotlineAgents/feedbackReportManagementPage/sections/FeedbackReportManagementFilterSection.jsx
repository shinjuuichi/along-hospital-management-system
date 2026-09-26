import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack } from '@mui/material'

const FeedbackReportManagementFilterSection = ({ filters, setFilters, loading }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const { values, setField, handleChange, registerRef, reset } = useForm({ status: filters.status })
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
			key: 'status',
			title: t('feedback_report.field.status'),
			type: 'select',
			required: false,
			options: _enum.feedbackReportStatusEnum,
			props: { fullWidth: true },
		},
	]

	return (
		<Stack spacing={2}>
			<Grid container spacing={2}>
				<Grid size={12} md={4}>
					{renderField(fields[0])}
				</Grid>
			</Grid>
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

export default FeedbackReportManagementFilterSection

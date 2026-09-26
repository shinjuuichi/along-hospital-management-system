import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'

const QualificationMangementFilterSection = ({ filters = {}, setFilters, loading = false }) => {
	const { t } = useTranslation()

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
			key: 'name',
			title: t('qualification.field.name'),
			type: 'text',
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
						<Grid size={{ xs: 12, md: filterFields.length === 1 ? 8 : 4 }} key={field.key}>
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
		</Paper>
	)
}

export default QualificationMangementFilterSection

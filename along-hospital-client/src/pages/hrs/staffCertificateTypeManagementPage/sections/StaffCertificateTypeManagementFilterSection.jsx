import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper } from '@mui/material'

const StaffCertificateTypeManagementFilterSection = ({
	filters = {},
	setFilters,
	loading = false,
}) => {
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
			title: t('staff_certificate_type.table.name'),
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
			<Grid container spacing={2} columns={12} alignItems='center'>
				{filterFields.map((field) => (
					<Grid key={field.key} size={8}>
						{renderField(field)}
					</Grid>
				))}

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
		</Paper>
	)
}

export default StaffCertificateTypeManagementFilterSection

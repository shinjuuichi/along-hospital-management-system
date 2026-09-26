import FilterButton from '@/components/buttons/FilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'

const RoomCategoryFilterSection = ({ filters, setFilters, loading }) => {
	const { t } = useTranslation()

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

	const fields = [
		{
			key: 'name',
			title: t('room_category.placeholder.search_name'),
			type: 'search',
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
			<Typography variant='caption'>{t('room_category.title.filters')}</Typography>

			<Grid container spacing={2}>
				{fields.map((field) => (
					<Grid size={{ xs: 12, md: field.key === 'name' ? 5 : 3 }} key={field.key}>
						{renderField(field)}
					</Grid>
				))}
				<Grid size={{ xs: 12, md: 2 }}>
					<FilterButton
						onFilterClick={() => setFilters({ ...values })}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default RoomCategoryFilterSection

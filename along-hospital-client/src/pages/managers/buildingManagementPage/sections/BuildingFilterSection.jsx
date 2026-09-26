import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'

const BuildingFilterSection = ({ filters, setFilters, loading }) => {
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

	const fields = [
		{
			key: 'name',
			title: t('building.field.name'),
			type: 'search',
			required: false,
		},
		{
			key: 'location',
			title: t('building.field.location'),
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
			<Typography variant='caption'>{t('text.filters')}</Typography>
			<Grid container spacing={2}>
				{fields.map((field) => (
					<Grid size={{ xs: 12, md: 4 }} key={field.key}>
						{renderField(field)}
					</Grid>
				))}
				<Grid size={{ xs: 12, md: 4 }}>
					<Stack direction='row' spacing={1} sx={{ height: '100%' }}>
						<FilterButton
							onFilterClick={() => setFilters({ ...values })}
							fullWidth
							loading={loading}
							sx={{ flexGrow: 1, whiteSpace: 'nowrap', minWidth: 110 }}
						/>
						<ResetFilterButton
							loading={loading}
							onResetFilterClick={() => {
								const cleared = Object.fromEntries(Object.keys(values).map((k) => [k, '']))
								reset(cleared)
								setFilters(cleared)
							}}
							sx={{ whiteSpace: 'nowrap', minWidth: 140 }}
						/>
					</Stack>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default BuildingFilterSection

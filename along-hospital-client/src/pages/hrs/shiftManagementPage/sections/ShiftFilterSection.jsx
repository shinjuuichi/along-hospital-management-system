import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack, Typography } from '@mui/material'

const ShiftFilterSection = ({ filters, setFilters, loading }) => {
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
			title: t('shift.placeholder.search_name'),
			type: 'search',
			required: false,
		},
		{
			key: 'startTime',
			title: t('shift.field.start_time'),
			type: 'time',
			required: false,
		},
		{
			key: 'endTime',
			title: t('shift.field.end_time'),
			type: 'time',
			required: false,
		},
	]

	const searchField = fields.find((field) => field.key === 'name')
	const timeFields = fields.filter((field) => field.key !== 'name')

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
			<Typography variant='caption'>{t('shift.title.filters')}</Typography>

			<Stack direction={{ xs: 'column', md: 'row' }} spacing={2} alignItems={{ md: 'flex-start' }}>
				<Box sx={{ flex: 1 }}>{searchField ? renderField(searchField) : null}</Box>
				<Stack direction='row' spacing={1} sx={{ width: { xs: '100%', md: 'auto' } }}>
					<FilterButton
						onFilterClick={() => setFilters({ ...values })}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						fullWidth
						loading={loading}
					/>
				</Stack>
			</Stack>

			<Stack direction={{ xs: 'column', md: 'row' }} spacing={2}>
				{timeFields.map((field) => (
					<Box key={field.key} sx={{ flex: 1 }}>
						{renderField(field)}
					</Box>
				))}
			</Stack>
		</Stack>
	)
}

export default ShiftFilterSection

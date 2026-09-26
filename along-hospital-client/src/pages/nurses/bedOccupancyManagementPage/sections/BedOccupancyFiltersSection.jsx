import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const BedOccupancyFiltersSection = ({
	filters = {},
	setFilters = () => {},
	defaultFilters = {},
	buildingOptions,
	floorOptions,
	specialtyOptions,
}) => {
	const { t } = useTranslation()
	const { bedStatusOptions } = useEnum()
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

	useEffect(() => {
		reset(filters)
	}, [filters, reset])

	const handleApplyFilter = () => {
		setFilters({ ...defaultFilters, ...values })
	}

	const handleResetFilter = () => {
		reset(defaultFilters)
		setFilters(defaultFilters)
	}

	const searchField = {
		key: 'search',
		title: t('bed_occupancy.filter.search'),
		type: 'text',
		required: false,
		props: {
			placeholder: t('bed_occupancy.placeholder.search_board'),
		},
	}

	const fields = [
		{
			key: 'building',
			title: t('bed_occupancy.filter.building'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...buildingOptions.map((building) => ({
				value: building,
				label: building,
			}))],
			required: false,
		},
		{
			key: 'floor',
			title: t('bed_occupancy.filter.floor'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...floorOptions.map((floor) => ({
				value: String(floor),
				label: floor,
			}))],
			required: false,
		},
		{
			key: 'specialtyId',
			title: t('bed_occupancy.filter.specialty'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...specialtyOptions],
			required: false,
		},
		{
			key: 'bedStatus',
			title: t('bed_occupancy.filter.bed_state'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...bedStatusOptions],
			required: false,
		},
	]

	return (
		<Paper sx={{ p: 2.5, borderRadius: 3 }}>
			<Stack spacing={2}>
				<Typography variant='subtitle1' fontWeight={600}>
					{t('bed_occupancy.title.filters')}
				</Typography>

				<Grid container spacing={2} alignItems='center'>
					<Grid size={{ xs: 12, md: 6, lg: 8 }}>
						{renderField(searchField)}
					</Grid>
					<Grid size={{ xs: 6, md: 3, lg: 2 }}>
						<FilterButton fullWidth onFilterClick={handleApplyFilter} />
					</Grid>
					<Grid size={{ xs: 6, md: 3, lg: 2 }}>
						<ResetFilterButton fullWidth onResetFilterClick={handleResetFilter} />
					</Grid>
				</Grid>

				<Grid container spacing={2}>
					{fields.map((field) => (
						<Grid key={field.key} size={{ xs: 12, sm: 6, lg: 3 }}>
							{renderField(field)}
						</Grid>
					))}
				</Grid>
			</Stack>
		</Paper>
	)
}

export default BedOccupancyFiltersSection

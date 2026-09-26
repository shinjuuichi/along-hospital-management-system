import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack } from '@mui/material'
import { useEffect } from 'react'

const ShopFiltersSection = ({
	filters,
	categories = [],
	units = [],
	loading = false,
	onFilterClick,
	onResetFilterClick,
}) => {
	const { t } = useTranslation()
	const { reset, values, handleChange, setField, registerRef } = useForm(filters)
	const { renderField } = useFieldRenderer(values, setField, handleChange, registerRef)

	const fields = [
		{ key: 'name', title: t('text.search'), type: 'search' },
		{
			key: 'medicineCategoryId',
			title: t('medicine.filter.category'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				...categories.map((c) => ({ value: c.id, label: c.name })),
			],
			required: false,
		},
		{
			key: 'medicineUnitId',
			title: t('medicine.filter.unit'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				...units.map((u) => ({ value: u.id, label: u.name })),
			],
			required: false,
		},
	]

	useEffect(() => {
		reset(filters)
	}, [filters, reset])

	return (
		<Stack spacing={2} alignItems='left' flexWrap='wrap'>
			{fields.map((field, index) => (
				<Box key={index} sx={{ flex: 1, minWidth: 150 }}>
					{renderField(field)}
				</Box>
			))}

			<FilterButton
				onFilterClick={() => onFilterClick(values)}
				loading={loading}
				sx={{ minWidth: 100 }}
			>
				{t('button.filter')}
			</FilterButton>
			<ResetFilterButton
				onResetFilterClick={onResetFilterClick}
				loading={loading}
				sx={{ minWidth: 100 }}
			>
				{t('button.reset')}
			</ResetFilterButton>
		</Stack>
	)
}

export default ShopFiltersSection

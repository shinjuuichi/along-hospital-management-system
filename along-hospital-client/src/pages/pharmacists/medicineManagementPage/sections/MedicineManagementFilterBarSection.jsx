import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const MedicineManagementFilterBarSection = ({
	filters,
	categories = [],
	units = [],
	loading = false,
	onFilterClick = () => {},
	onResetFilterClick = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const { reset, values, handleChange, setField, registerRef } = useForm(filters)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const fields1st = [
		{
			key: 'medicineCategoryId',
			title: t('medicine.filter.category'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(categories || [])],
			required: false,
		},
		{
			key: 'medicineUnitId',
			title: t('medicine.filter.unit'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(units || [])],
			required: false,
		},
		{
			key: 'status',
			title: t('medicine.field.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(_enum.medicineStatusOptions || [])],
			required: false,
		},
		{
			key: 'isPublic',
			title: t('medicine.filter.is_public'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('medicine.text.true') },
				{ value: 'false', label: t('medicine.text.false') },
			],
			required: false,
		},
	]

	const fields2nd = [
		{
			key: 'name',
			title: t('medicine.filter.search_place_holder'),
			type: 'search',
			required: false,
		},
	]

	useEffect(() => {
		reset(filters)
	}, [filters, reset])

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
			<Typography variant='caption'>{t('medicine.filter.title')}</Typography>

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields1st.map(renderField)}
			</Stack>

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields2nd.map(renderField)}
				<FilterButton
					onFilterClick={() => onFilterClick(values)}
					loading={loading}
					sx={{ minWidth: 120 }}
				/>
				<ResetFilterButton
					loading={loading}
					onResetFilterClick={() => onResetFilterClick(reset)}
					sx={{ minWidth: 160 }}
				/>
			</Stack>
		</Stack>
	)
}

export default MedicineManagementFilterBarSection

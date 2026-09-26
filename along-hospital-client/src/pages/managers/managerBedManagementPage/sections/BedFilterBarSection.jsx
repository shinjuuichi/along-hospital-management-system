import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const BedFilterBarSection = ({
	filters,
	bedCategories = [],
	roomOptions = [],
	loading = false,
	onFilterClick = () => {},
	onResetFilterClick = () => {},
}) => {
	const { t } = useTranslation()
	const { bedStatusOptions } = useEnum()

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

	const fields = [
		{
			key: 'status',
			title: t('bed.filter.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(bedStatusOptions || [])],
			required: false,
		},
		{
			key: 'bedCategoryId',
			title: t('bed.filter.bed_category'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(bedCategories || [])],
			required: false,
		},
		{
			key: 'roomId',
			title: t('bed.filter.room_id'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(roomOptions || [])],
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
			<Typography variant='caption'>{t('bed.filter.title')}</Typography>

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields.map(renderField)}
				<FilterButton
					onFilterClick={() => onFilterClick(values)}
					loading={loading}
					sx={{ minWidth: 150 }}
				/>
				<ResetFilterButton
					loading={loading}
					onResetFilterClick={onResetFilterClick}
					sx={{ minWidth: 150, whiteSpace: 'nowrap' }}
				/>
			</Stack>
		</Stack>
	)
}

export default BedFilterBarSection

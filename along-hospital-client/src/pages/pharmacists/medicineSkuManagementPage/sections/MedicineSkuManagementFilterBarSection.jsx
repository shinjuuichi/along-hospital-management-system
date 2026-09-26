import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const MedicineSkuManagementFilterBarSection = ({
	filters,
	medicineOptions = [],
	statusOptions = [],
	loading = false,
	onFilterClick = () => {},
	onResetFilterClick = () => {},
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

	const row1Fields = [
		{
			key: 'medicineId',
			title: t('medicine_sku.field.medicine'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(medicineOptions || [])],
			required: false,
		},
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			type: 'select',
			options: statusOptions,
			required: false,
		},
	]

	const row2Fields = [
		{
			key: 'skuCode',
			title: t('medicine_sku.field.sku_code'),
			type: 'text',
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
			<Typography variant='caption'>{t('medicine_sku.filter.title')}</Typography>
			<Stack direction='row' spacing={2} alignItems='center' flexWrap='wrap' useFlexGap>
				{row1Fields.map((field) => (
					<Box key={field.key} sx={{ flex: 1, minWidth: 120 }}>
						{renderField(field)}
					</Box>
				))}
			</Stack>
			<Stack direction='row' spacing={2} alignItems='center' flexWrap='wrap' useFlexGap>
				{row2Fields.map((field) => (
					<Box key={field.key} sx={{ flex: 1, minWidth: 120 }}>
						{renderField(field)}
					</Box>
				))}
				<Box sx={{ display: 'flex', gap: 2, flexShrink: 0 }}>
					<FilterButton onFilterClick={() => onFilterClick(values)} loading={loading} sx={{ minWidth: 120 }} />
					<ResetFilterButton loading={loading} onResetFilterClick={() => onResetFilterClick(reset)} sx={{ minWidth: 120 }} />
				</Box>
			</Stack>
		</Stack>
	)
}

export default MedicineSkuManagementFilterBarSection

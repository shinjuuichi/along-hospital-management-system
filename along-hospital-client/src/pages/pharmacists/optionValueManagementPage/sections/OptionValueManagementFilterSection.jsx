import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const OptionValueManagementFilterSection = ({
	filters,
	optionOptions = [],
	loading = false,
	onFilterClick = () => {},
	onResetFilterClick = () => {},
}) => {
	const { t } = useTranslation()
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
			key: 'name',
			title: t('option_value.filter.searchPlaceholder'),
			required: false,
			props: { sx: { minWidth: 260 } },
		},
		{
			key: 'isActive',
			title: t('option_value.field.active'),
			type: 'select',
			required: false,
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('option_value.text.active') },
				{ value: 'false', label: t('option_value.text.inactive') },
			],
			props: { sx: { minWidth: 160 } },
		},
		{
			key: 'optionId',
			title: t('option_value.filter.option'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ...optionOptions],
			props: { sx: { minWidth: 220 } },
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
			<Typography variant='caption'>{t('option_value.filter.title')}</Typography>

			<Stack direction='row' spacing={2} alignItems='center' flexWrap='wrap'>
				<Box sx={{ flex: 1, minWidth: 200 }}>{renderField(fields[0])}</Box>
				<Stack key={fields[1].key} direction='row' spacing={1} alignItems='center'>
					{renderField(fields[1])}
				</Stack>
				<Stack key={fields[2].key} direction='row' spacing={1} alignItems='center'>
					{renderField(fields[2])}
				</Stack>

				<FilterButton
					onFilterClick={() => onFilterClick(values)}
					loading={loading}
					sx={{ minWidth: 120 }}
				/>
				<ResetFilterButton
					loading={loading}
					onResetFilterClick={() => onResetFilterClick(reset)}
					sx={{ minWidth: 120 }}
				/>
			</Stack>
		</Stack>
	)
}

export default OptionValueManagementFilterSection

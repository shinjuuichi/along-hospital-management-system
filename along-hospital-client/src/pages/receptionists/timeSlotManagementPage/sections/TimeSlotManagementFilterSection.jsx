import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'

const TimeSlotManagementFilterSection = ({ filters, setFilters, loading }) => {
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
			key: 'startTime',
			title: t('text.start_time'),
			type: 'time',
			required: false,
		},
		{
			key: 'endTime',
			title: t('text.end_time'),
			type: 'time',
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

			<Stack gap={1}>
				<Stack direction='row' gap={2} alignItems='center'>
					{fields.map(renderField)}
				</Stack>
				<Stack direction={'row'} gap={1} ml={'auto'}>
					<FilterButton onFilterClick={() => setFilters(values)} loading={loading} />
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						loading={loading}
					/>
				</Stack>
			</Stack>
		</Stack>
	)
}

export default TimeSlotManagementFilterSection

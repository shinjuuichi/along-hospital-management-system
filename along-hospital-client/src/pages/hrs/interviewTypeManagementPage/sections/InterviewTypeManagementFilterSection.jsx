import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack, Typography } from '@mui/material'

const InterviewTypeManagementFilterSection = ({ filters, setFilters, loading }) => {
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
			title: t('interview_type.field.name'),
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

			<Stack direction='row' gap={1} alignItems='center' flexWrap='wrap' sx={{ flexGrow: 1 }}>
				<Box sx={{ flexGrow: 1 }}>{fields.map(renderField)}</Box>
				<Box sx={{ display: 'flex', gap: 1, ml: 'auto' }}>
					<FilterButton onFilterClick={() => setFilters(values)} loading={loading} />
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						loading={loading}
					/>
				</Box>
			</Stack>
		</Stack>
	)
}

export default InterviewTypeManagementFilterSection

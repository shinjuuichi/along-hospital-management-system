import FilterButton from '@/components/buttons/FilterButton'
import GenericTabs from '@/components/generals/GenericTabs'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Grid, Stack, Typography } from '@mui/material'

const ManageAttendanceFilterSection = ({ filters, setFilters, loading }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const { values, handleChange, setField, registerRef } = useForm(filters)
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
		{ key: 'startDate', title: t('text.start_date'), type: 'date', required: false },
		{ key: 'endDate', title: t('text.end_date'), type: 'date', required: false },
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

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields.map(renderField)}
			</Stack>

			<Grid container spacing={2}>
				<Grid size={{ xs: 12, md: 10 }}>
					<Stack direction='row' alignItems='center' spacing={2} width='100%'>
						<Typography variant='body2' sx={{ width: 'max-content' }}>
							{t('attendance.field.log_type')}:{' '}
						</Typography>
						<Box flexGrow={1}>
							<GenericTabs
								currentTab={values?.logType}
								setCurrentTab={(tab) => setField('logType', tab.key)}
								loading={loading}
								tabs={[
									{
										key: '',
										title: t('text.all'),
									},
									..._enum.attendanceLogTypeOptions.map((type) => ({
										key: type.value,
										title: type.label,
									})),
								]}
							/>
						</Box>
					</Stack>
				</Grid>
				<Grid size={{ xs: 6, md: 2 }}>
					<FilterButton
						onFilterClick={() => setFilters(values)}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default ManageAttendanceFilterSection

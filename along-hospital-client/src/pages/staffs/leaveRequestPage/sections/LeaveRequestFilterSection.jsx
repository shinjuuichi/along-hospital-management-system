import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'

const withAllOption = (options = [], t) => [{ value: '', label: t('text.all') }, ...options]

const LeaveRequestFilterSection = ({
	filters = {},
	setFilters = (nextFilters) => nextFilters,
	loading = false,
	isManagement = false,
}) => {
	const _enum = useEnum()
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
			key: 'leaveType',
			title: t('leave_request.field.leave_type'),
			type: 'select',
			options: withAllOption(_enum.leaveTypeOptions, t),
			required: false,
		},
		{
			key: 'status',
			title: t('leave_request.field.status'),
			type: 'select',
			options: withAllOption(_enum.leaveRequestStatusOptions, t),
			required: false,
		},
		{ key: 'fromDate', title: t('leave_request.field.from_date'), type: 'date', required: false },
		{ key: 'toDate', title: t('leave_request.field.to_date'), type: 'date', required: false },
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
			<Typography variant='caption'>{t('leave_request.text.filters')}</Typography>

			<Grid container spacing={2}>
				{fields.map((field) => (
					<Grid size={{ xs: 12, md: isManagement ? 2 : 2.5 }} key={field.key}>
						{renderField(field)}
					</Grid>
				))}
			</Grid>

			<Stack direction='row' spacing={1} justifyContent='flex-end'>
				<FilterButton onFilterClick={() => setFilters({ ...values })} loading={loading} />
				<ResetFilterButton
					loading={loading}
					onResetFilterClick={() => {
						reset({})
						setFilters({})
					}}
				/>
			</Stack>
		</Stack>
	)
}

export default LeaveRequestFilterSection

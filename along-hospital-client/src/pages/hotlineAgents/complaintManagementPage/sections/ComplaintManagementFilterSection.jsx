import FilterButton from '@/components/buttons/FilterButton'
import SearchBar from '@/components/generals/SearchBar'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'

const ComplaintManagementFilterSection = ({ filters, setFilters, loading = false }) => {
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
		{
			key: 'complaintTopic',
			title: t('complaint.field.topic'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ..._enum.complaintTopicOptions],
		},
		{
			key: 'complaintType',
			title: t('complaint.field.type'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ..._enum.complaintTypeOptions],
		},
		{
			key: 'complaintResolveStatus',
			title: t('complaint.field.resolve_status'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ..._enum.complaintResolveStatusOptions],
		},
	]

	return (
		<Paper
			sx={{
				bgcolor: 'background.default',
				p: 2,
			}}
		>
			<Stack spacing={2}>
				<Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
					{fields.map((field) => renderField(field))}
				</Stack>
				<Grid container spacing={2}>
					<Grid size={10}>
						<SearchBar
							value={values.content}
							setValue={(value) => setField('content', value)}
							placeholder={t('complaint.placeholder.search_content')}
							onEnterDown={() => setFilters(values)}
						/>
					</Grid>
					<Grid size={2}>
						<FilterButton fullWidth loading={loading} onFilterClick={() => setFilters(values)} />
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default ComplaintManagementFilterSection

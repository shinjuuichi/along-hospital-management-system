import FilterButton from '@/components/buttons/FilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Box, Dialog, DialogContent, DialogTitle, Grid, Typography } from '@mui/material'

const ComplaintManagementSummaryDialog = ({
	open,
	onClose,
	summary,
	onFilterWeek = (yearWeek) => Promise.resolve(yearWeek),
}) => {
	const { t } = useTranslation()

	const { values, registerRef, handleChange, setField } = useForm()
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const filterFields = [
		{
			key: 'yearWeek',
			title: 'Week',
			type: 'week',
			required: false,
		},
	]

	return (
		<Dialog open={open} onClose={onClose} maxWidth='sm' fullWidth>
			<DialogTitle>{t('complaint.title.complaint_summary')}</DialogTitle>
			<DialogContent>
				<Grid container spacing={2} mb={2}>
					<Grid size={8}>{filterFields.map((field) => renderField(field))}</Grid>
					<Grid size={4} alignItems={'stretch'}>
						<FilterButton fullWidth onFilterClick={() => onFilterWeek(values.yearWeek || '')} />
					</Grid>
				</Grid>
				<Box minHeight={300}>
					<Typography sx={{ whiteSpace: 'pre-line' }}>
						{summary || t('text.placeholder.no_data')}
					</Typography>
				</Box>
			</DialogContent>
		</Dialog>
	)
}

export default ComplaintManagementSummaryDialog

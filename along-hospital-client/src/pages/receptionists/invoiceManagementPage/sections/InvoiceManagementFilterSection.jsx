import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'

const InvoiceManagementFilterSection = ({ filters, setFilters, loading = false }) => {
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
			key: 'invoiceNumber',
			title: t('invoice.field.invoice_number'),
			type: 'search',
			required: false,
		},
		{
			key: 'invoiceStatus',
			title: t('invoice.field.invoice_status'),
			type: 'select',
			required: false,
			options: [{ value: '', label: t('text.all') }, ..._enum.invoiceStatusOptions],
		},
		{
			key: 'paymentDateFrom',
			title: t('invoice.field.payment_date_from'),
			type: 'date',
			required: false,
		},
		{
			key: 'paymentDateTo',
			title: t('invoice.field.payment_date_to'),
			type: 'date',
			required: false,
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
				<Grid container spacing={2} justifyContent={'end'}>
					<Grid size={2}>
						<FilterButton fullWidth loading={loading} onFilterClick={() => setFilters(values)} />
					</Grid>
					<Grid size={2}>
						<ResetFilterButton loading={loading} fullWidth onResetFilterClick={() => setFilters({})} />
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default InvoiceManagementFilterSection

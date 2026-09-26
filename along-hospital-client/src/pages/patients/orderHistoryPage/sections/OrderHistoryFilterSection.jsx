import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack } from '@mui/system'

const OrderHistoryFilterSection = ({ filters, setFilters, loading = false }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

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

	const filterFields = [
		{
			key: 'orderStatus',
			title: t('order.field.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.orderStatusOptions],
			required: false,
		},
		{
			key: 'orderDate',
			title: t('order.field.order_date'),
			type: 'date',
			required: false,
		},
		{
			key: 'deliveryDate',
			title: t('order.field.delivery_date'),
			type: 'date',
			required: false,
		},
		{
			key: 'isPickupAtStore',
			title: t('order_management.field.is_pickup_at_store'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('order.history.pickup_at_store') },
				{ value: 'false', label: t('order.history.delivery') },
			],
			required: false,
		},
	]

	return (
		<Grid container spacing={2} alignItems='center'>
			{filterFields.map((field) => (
				<Grid size={{ xs: 12, md: 6 }} key={field.key}>
					{renderField(field)}
				</Grid>
			))}
			<Grid size={{ xs: 12 }}>
				<Stack direction='row' spacing={2} justifyContent='flex-end'>
					<FilterButton onFilterClick={() => setFilters(values)} loading={loading} />
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						loading={loading}
					/>
				</Stack>
			</Grid>
		</Grid>
	)
}

export default OrderHistoryFilterSection

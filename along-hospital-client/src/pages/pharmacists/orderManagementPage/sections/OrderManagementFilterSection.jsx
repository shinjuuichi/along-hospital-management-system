import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const OrderManagementFilterSection = ({ filters, setFilters, loading = false }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const initialValues = {
		orderStatus: filters.orderStatus || '',
		orderDate: filters.orderDate || '',
		deliveryDate: filters.deliveryDate || '',
		isPickupAtStore: filters.isPickupAtStore || '',
	}

	const { values, handleChange, setField, registerRef, reset } = useForm(initialValues)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	useEffect(() => {
		reset(initialValues)
	}, [filters])

	const statusOptions = [{ value: '', label: t('text.all') }, ..._enum.orderStatusOptions]

	const fields1st = [
		{
			key: 'orderStatus',
			title: t('order_management.field.status'),
			type: 'select',
			options: statusOptions,
			required: false,
		},
		{
			key: 'isPickupAtStore',
			title: t('order_management.field.is_pickup_at_store'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('order_management.text.pickup_at_store') },
				{ value: 'false', label: t('order_management.text.delivery') },
			],
			required: false,
		},
	]

	const fields2nd = [
		{
			key: 'orderDate',
			title: t('order_management.field.order_date'),
			required: false,
			type: 'date',
		},
		{
			key: 'deliveryDate',
			title: t('order_management.field.delivery_date'),
			required: false,
			type: 'date',
		},
	]

	const applyFilters = () => setFilters(values)
	const resetFilters = () => {
		const empty = {
			orderStatus: '',
			orderDate: '',
			deliveryDate: '',
			isPickupAtStore: '',
		}
		reset(empty)
		setFilters(empty)
	}

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
			<Typography variant='caption'>{t('order_management.title.filters')}</Typography>

			<Stack direction='row' spacing={2} alignItems='center'>
				{fields1st.map(renderField)}
			</Stack>
			<Stack direction='row' spacing={2} alignItems='center'>
				{fields2nd.map(renderField)}
			</Stack>

			<Grid container spacing={2} justifyContent='flex-end'>
				<Grid size={{ xs: 6, md: 2 }}>
					<FilterButton onFilterClick={applyFilters} fullWidth loading={loading} sx={{ flexGrow: 1 }} />
				</Grid>
				<Grid size={{ xs: 6, md: 2 }}>
					<ResetFilterButton
						onResetFilterClick={resetFilters}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default OrderManagementFilterSection

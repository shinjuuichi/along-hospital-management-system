import FilterButton from '@/components/buttons/FilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Stack, Typography } from '@mui/material'

const VoucherManagementFilterSection = ({ filters, setFilters, loading = false }) => {
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
			key: 'name',
			title: t('voucher.placeholder.search_voucher'),
			type: 'search',
			required: false,
		},
		{
			key: 'voucherStatus',
			title: t('voucher.field.voucher_status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.voucherStatusOptions],
			required: false,
		},
		{
			key: 'voucherType',
			title: t('voucher.field.voucher_type'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.voucherTypeOptions],
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
			<Typography variant='caption'>{t('voucher.title.filters')}</Typography>

			<Grid container spacing={2}>
				{fields.map((field) => (
					<Grid size={field.key === 'name' ? 5 : field.key === 'voucherStatus' ? 3 : 2} key={field.key}>
						{renderField(field)}
					</Grid>
				))}
				<Grid size={2}>
					<FilterButton
						onFilterClick={() => setFilters({ ...values })}
						fullWidth
						loading={loading}
						sx={{ flexGrow: 1 }}
					/>
				</Grid>
			</Grid>
		</Stack>
	)
}

export default VoucherManagementFilterSection

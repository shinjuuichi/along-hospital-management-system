import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper } from '@mui/material'

const StaffCertificateManagementFilterSection = ({
	filters = {},
	setFilters,
	loading = false,
	staffCertificateTypeOptions = [],
	staffOptions = [],
}) => {
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
			key: 'certificateNo',
			title: t('staff_certificate.table.certificate_no'),
			type: 'text',
			required: false,
		},
		{
			key: 'issuedBy',
			title: t('staff_certificate.table.issued_by'),
			type: 'text',
			required: false,
		},
		{
			key: 'staffCertificateTypeId',
			title: t('staff_certificate.table.certificate_type'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...staffCertificateTypeOptions],
			required: false,
		},
		{
			key: 'staffId',
			title: t('staff_certificate.table.staff'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...staffOptions],
			required: false,
		},
		{
			key: 'status',
			title: t('staff_certificate.table.status'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ..._enum.staffCertificateStatusOptions],
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
			<Grid container spacing={2} columns={12} alignItems='center'>
				{filterFields.map((field) => (
					<Grid key={field.key} size={4}>
						{renderField(field)}
					</Grid>
				))}

				<Grid size={2}>
					<FilterButton fullWidth loading={loading} onFilterClick={() => setFilters(values)} />
				</Grid>
				<Grid size={2}>
					<ResetFilterButton
						fullWidth
						loading={loading}
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
					/>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default StaffCertificateManagementFilterSection

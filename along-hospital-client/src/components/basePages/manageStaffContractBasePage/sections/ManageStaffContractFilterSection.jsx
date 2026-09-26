import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'

const ManageStaffContractFilterSection = ({
	filters,
	setFilters,
	loading,
	staffs,
	regionalWages,
	role,
}) => {
	const isHR = role === EnumConfig.Role.HR
	const { t } = useTranslation()
	const { contractTypeOptions, staffContractStatusOptions } = useEnum()

	const staffOptions = (staffs || []).map((staff) => ({
		value: staff.id,
		label: staff.name,
	}))

	const regionalWageOptions = (regionalWages || []).map((rw) => ({
		value: rw.id,
		label: rw.code,
	}))

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

	const filterFieldsRow1 = [
		{
			key: 'contractCode',
			title: t('staff_contract.field.contract_code'),
			type: 'search',
			required: false,
		},
		...(isHR
			? [
					{
						key: 'staffId',
						title: t('staff_contract.field.staff'),
						type: 'select',
						required: false,
						options: staffOptions,
					},
				]
			: []),
		{
			key: 'contractType',
			title: t('staff_contract.field.contract_type'),
			type: 'select',
			required: false,
			options: contractTypeOptions,
		},
	]

	const filterFieldsRow2 = [
		...(isHR
			? [
					{
						key: 'regionalWageId',
						title: t('staff_contract.field.regional_wage'),
						type: 'select',
						required: false,
						options: regionalWageOptions,
					},
				]
			: []),
		{
			key: 'status',
			title: t('staff_contract.field.status'),
			type: 'select',
			required: false,
			options: staffContractStatusOptions,
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

			<Stack gap={1}>
				<Stack direction='row' gap={2} alignItems='center'>
					{filterFieldsRow1.map((field) => renderField(field))}
				</Stack>

				<Stack direction='row' gap={2} alignItems='center'>
					{filterFieldsRow2.map((field) => renderField(field))}
				</Stack>
				<Stack direction={'row'} gap={1} ml={'auto'}>
					<FilterButton
						onFilterClick={() => {
							const processedFilters = Object.fromEntries(
								Object.entries(values).filter(([, v]) => v != null && v !== '')
							)
							setFilters(processedFilters)
						}}
						loading={loading}
					/>
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						loading={loading}
					/>
				</Stack>
			</Stack>
		</Stack>
	)
}

export default ManageStaffContractFilterSection

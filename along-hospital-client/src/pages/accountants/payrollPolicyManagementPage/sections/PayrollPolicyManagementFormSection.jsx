import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { maxLen } from '@/utils/validateUtil'
import { Avatar, Chip, Stack, Typography } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const POLICY_TYPES = {
	ALLOWANCE: 'ALLOWANCE',
	DEDUCTION: 'DEDUCTION',
}

const resolvePolicyType = (row) => {
	if (row?.allowanceTypeId != null) return POLICY_TYPES.ALLOWANCE
	if (row?.deductionTypeId != null) return POLICY_TYPES.DEDUCTION
}

const PayrollPolicyManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreate,
	onUpdate,
	allowanceTypes,
	deductionTypes,
	staffs,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const today = new Date().toISOString().split('T')[0]

	const policyTypeOptions = useMemo(
		() => [
			{ label: t('payroll_policy.field.allowance_type_id'), value: POLICY_TYPES.ALLOWANCE },
			{ label: t('payroll_policy.field.deduction_type_id'), value: POLICY_TYPES.DEDUCTION },
		],
		[t]
	)

	const createInitialValues = useMemo(
		() => ({
			name: '',
			startDate: today,
			endDate: today,
			staffIds: [],
			policyType: POLICY_TYPES.ALLOWANCE,
			allowanceTypeId: null,
			deductionTypeId: null,
		}),
		[today]
	)

	const allowanceTypeOptions = useMemo(
		() => (allowanceTypes || []).map((item) => ({ value: item.id, label: item.name })),
		[allowanceTypes]
	)

	const deductionTypeOptions = useMemo(
		() => (deductionTypes || []).map((item) => ({ value: item.id, label: item.name })),
		[deductionTypes]
	)

	const staffOptions = useMemo(
		() =>
			(staffs || []).map((staff) => ({
				value: staff.id,
				label: staff,
				searchKey: staff.name,
			})),
		[staffs]
	)

	const renderStaffOption = useCallback(
		(_, label) => (
			<Stack direction={'row'} alignItems={'center'} gap={2}>
				<Avatar src={getImageFromCloud(label?.image)} alt={label?.name} />
				<Stack gap={0.2}>
					<Stack direction={'row'} gap={1} alignItems={'center'}>
						<Typography>{label?.name}</Typography>
						<Typography lineHeight='normal' variant='caption' color='text.secondary'>
							{label?.phone}
						</Typography>
						<Typography lineHeight='normal' variant='caption' color='text.secondary'>
							{label?.email}
						</Typography>
					</Stack>
					<Typography variant='caption' color='text.secondary'>
						{label?.role}
					</Typography>
				</Stack>
			</Stack>
		),
		[]
	)

	const renderStaffOptionValue = useCallback((_, label) => {
		return <Chip label={label?.name} avatar={<Avatar src={getImageFromCloud(label?.image)} />} />
	}, [])

	const initialUpdateValues = useMemo(() => {
		if (!selectedRow) return createInitialValues
		const resolvedPolicyType = resolvePolicyType(selectedRow)
		const staffIds = Array.isArray(selectedRow?.staffIds)
			? selectedRow.staffIds
			: selectedRow?.staffId != null
				? [selectedRow.staffId]
				: []
		return {
			...createInitialValues,
			...selectedRow,
			staffIds,
			policyType: resolvedPolicyType ?? createInitialValues.policyType,
		}
	}, [createInitialValues, selectedRow])

	const [formValues, setFormValues] = useState(createInitialValues)

	useEffect(() => {
		if (openCreate) {
			setFormValues(createInitialValues)
		}
	}, [openCreate, createInitialValues])

	useEffect(() => {
		if (openUpdate) {
			setFormValues(initialUpdateValues)
		}
	}, [openUpdate, initialUpdateValues])

	const buildFields = useCallback(
		(mode) => [
			{
				key: 'name',
				title: t('payroll_policy.field.name'),
				validate: [maxLen(100)],
			},
			{
				key: 'startDate',
				title: t('payroll_policy.field.start_date'),
				type: 'date',
			},
			{
				key: 'endDate',
				title: t('payroll_policy.field.end_date'),
				type: 'date',
				required: false,
			},
			...(mode === 'update'
				? [
						{
							key: 'payrollPolicyStatus',
							title: t('payroll_policy.field.payroll_policy_status'),
							type: 'select',
							options: _enum.payrollPolicyStatusOptions,
						},
					]
				: []),
			{
				key: 'staffIds',
				title: t('payroll_policy.field.staff_id'),
				type: 'select',
				options: staffOptions,
				multiple: true,
				renderOption: renderStaffOption,
				renderOptionValue: renderStaffOptionValue,
			},
			{
				key: 'policyType',
				title: `${t('payroll_policy.field.allowance_type_id')} / ${t(
					'payroll_policy.field.deduction_type_id'
				)}`,
				type: 'radio',
				options: policyTypeOptions,
			},
			...(formValues?.policyType === POLICY_TYPES.ALLOWANCE
				? [
						{
							key: 'allowanceTypeId',
							title: t('payroll_policy.field.allowance_type_id'),
							type: 'select',
							options: allowanceTypeOptions,
							required: false,
						},
					]
				: []),
			...(formValues?.policyType === POLICY_TYPES.DEDUCTION
				? [
						{
							key: 'deductionTypeId',
							title: t('payroll_policy.field.deduction_type_id'),
							type: 'select',
							options: deductionTypeOptions,
							required: false,
						},
					]
				: []),
		],
		[
			t,
			_enum.payrollPolicyStatusOptions,
			staffOptions,
			renderStaffOption,
			renderStaffOptionValue,
			policyTypeOptions,
			formValues?.policyType,
			allowanceTypeOptions,
			deductionTypeOptions,
		]
	)

	const createFields = useMemo(() => buildFields('create'), [buildFields])
	const updateFields = useMemo(() => buildFields('update'), [buildFields])

	const buildBasePayload = (values) => {
		const payload = {
			name: values.name,
			startDate: values.startDate,
			endDate: values.endDate,
			staffIds: Array.isArray(values.staffIds) ? values.staffIds : [],
			allowanceTypeId: values.allowanceTypeId,
			deductionTypeId: values.deductionTypeId,
		}

		if (values.policyType === POLICY_TYPES.ALLOWANCE) {
			payload.deductionTypeId = null
		} else {
			payload.allowanceTypeId = null
		}

		return payload
	}

	const buildCreatePayload = (values) => buildBasePayload(values)

	const buildUpdatePayload = (values) => ({
		...buildBasePayload(values),
		payrollPolicyStatus: values.payrollPolicyStatus,
	})

	const handleCreate = ({ values, closeDialog }) => {
		onCreate({ values: buildCreatePayload(values), closeDialog })
	}

	const handleUpdate = ({ values, closeDialog }) => {
		onUpdate({ values: buildUpdatePayload(values), closeDialog })
	}

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={createInitialValues}
				fields={createFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('payroll_policy.title.create')}
				onValuesChange={setFormValues}
				onSubmit={handleCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={updateFields}
				initialValues={initialUpdateValues}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('payroll_policy.title.update')}
				onValuesChange={setFormValues}
				onSubmit={handleUpdate}
			/>
		</>
	)
}

export default PayrollPolicyManagementFormSection

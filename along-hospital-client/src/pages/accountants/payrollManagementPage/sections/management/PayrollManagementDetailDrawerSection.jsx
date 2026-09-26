import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import GenericDrawer from '@/components/generals/GenericDrawer'
import { defaultPayrollStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
} from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Button,
	Chip,
	Grid,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Typography,
} from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const PayrollManagementDetailDrawerSection = ({
	open,
	onClose,
	payroll,
	loading,
	canPrint = true,
	onPrint,
	allowanceTypes = [],
	deductionTypes = [],
	onCreateAllowance,
	onUpdateAllowance,
	onDeleteAllowance,
	onCreateDeduction,
	onUpdateDeduction,
	onDeleteDeduction,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const [allowances, setAllowances] = useState([])
	const [deductions, setDeductions] = useState([])
	const [openAllowanceDialog, setOpenAllowanceDialog] = useState(false)
	const [openDeductionDialog, setOpenDeductionDialog] = useState(false)
	const [editingAllowance, setEditingAllowance] = useState(null)
	const [editingDeduction, setEditingDeduction] = useState(null)

	useEffect(() => {
		setAllowances(Array.isArray(payroll?.allowances) ? payroll.allowances : [])
		setDeductions(Array.isArray(payroll?.deductions) ? payroll.deductions : [])
	}, [payroll])

	const handlePrint = () => {
		if (!payroll) return
		const currentPayroll = {
			...payroll,
			allowances,
			deductions,
			totalAllowance,
			totalDeduction,
		}
		onPrint?.(currentPayroll)
	}

	const allowanceTypeOptions = useMemo(
		() =>
			Array.isArray(allowanceTypes)
				? allowanceTypes.map((item) => ({ label: item?.name ?? item?.label, value: item?.id }))
				: [],
		[allowanceTypes]
	)

	const deductionTypeOptions = useMemo(
		() =>
			Array.isArray(deductionTypes)
				? deductionTypes.map((item) => ({ label: item?.name ?? item?.label, value: item?.id }))
				: [],
		[deductionTypes]
	)

	const allowanceCreateFields = useMemo(
		() => [
			{
				key: 'allowanceTypeId',
				title: t('payroll.field.allowance_type_name'),
				type: 'select',
				options: allowanceTypeOptions,
			},
			{ key: 'unitPrice', title: t('payroll.field.unit_price'), type: 'number' },
			{ key: 'quantity', title: t('payroll.field.quantity'), type: 'number' },
			{ key: 'note', title: t('payroll.field.note'), multiple: 3, required: false },
			{ key: 'isTaxable', title: t('payroll.field.is_taxable'), type: 'checkbox', required: false },
		],
		[t, allowanceTypeOptions]
	)

	const allowanceUpdateFields = useMemo(
		() => [
			{ key: 'unitPrice', title: t('payroll.field.unit_price'), type: 'number' },
			{ key: 'quantity', title: t('payroll.field.quantity'), type: 'number' },
			{ key: 'note', title: t('payroll.field.note'), multiple: 3, required: false },
		],
		[t]
	)

	const deductionCreateFields = useMemo(
		() => [
			{
				key: 'deductionTypeId',
				title: t('payroll.field.deduction_type_name'),
				type: 'select',
				options: deductionTypeOptions,
			},
			{ key: 'unitPrice', title: t('payroll.field.unit_price'), type: 'number' },
			{ key: 'quantity', title: t('payroll.field.quantity'), type: 'number' },
			{ key: 'note', title: t('payroll.field.note'), multiple: 3, required: false },
			{ key: 'isTaxable', title: t('payroll.field.is_taxable'), type: 'checkbox', required: false },
		],
		[t, deductionTypeOptions]
	)

	const deductionUpdateFields = useMemo(
		() => [
			{ key: 'unitPrice', title: t('payroll.field.unit_price'), type: 'number' },
			{ key: 'quantity', title: t('payroll.field.quantity'), type: 'number' },
			{ key: 'note', title: t('payroll.field.note'), multiple: 3, required: false },
		],
		[t]
	)

	const handleUpsertAllowance = async ({ values, closeDialog }) => {
		if (editingAllowance?.id) {
			const ok = await onUpdateAllowance?.(editingAllowance.id, values)
			if (ok) {
				setEditingAllowance(null)
				setOpenAllowanceDialog(false)
				closeDialog()
			}
			return
		}
		const ok = await onCreateAllowance?.(values)
		if (ok) {
			setEditingAllowance(null)
			setOpenAllowanceDialog(false)
			closeDialog()
		}
	}

	const handleUpsertDeduction = async ({ values, closeDialog }) => {
		if (editingDeduction?.id) {
			const ok = await onUpdateDeduction?.(editingDeduction.id, values)
			if (ok) {
				setEditingDeduction(null)
				setOpenDeductionDialog(false)
				closeDialog()
			}
			return
		}
		const ok = await onCreateDeduction?.(values)
		if (ok) {
			setEditingDeduction(null)
			setOpenDeductionDialog(false)
			closeDialog()
		}
	}

	const handleDeleteAllowance = useCallback(
		async (row) => {
			await onDeleteAllowance?.(row.id)
		},
		[onDeleteAllowance]
	)

	const handleDeleteDeduction = useCallback(
		async (row) => {
			await onDeleteDeduction?.(row.id)
		},
		[onDeleteDeduction]
	)

	const totalAllowance = allowances.reduce(
		(sum, item) => sum + (Number(item?.unitPrice) || 0) * (Number(item?.quantity) || 0),
		0
	)
	const totalDeduction = deductions.reduce(
		(sum, item) => sum + (Number(item?.unitPrice) || 0) * (Number(item?.quantity) || 0),
		0
	)

	const renderMoney = (value) => formatCurrencyBasedOnCurrentLanguage(Number(value) || 0)
	const renderPercent = (value) => formatNumberToPercent(Number(value) || 0)

	const renderTaxableChip = (value) => (
		<Chip
			label={value ? t('text.yes') : t('text.no')}
			size='small'
			color={value ? 'success' : 'default'}
			variant='outlined'
		/>
	)

	const renderSnapshotGroup = (title, items = []) => (
		<Stack spacing={1.25}>
			<Typography variant='subtitle1' sx={{ fontWeight: 600 }}>
				{title}
			</Typography>
			<Grid container spacing={1.5}>
				{items.map((item, idx) => (
					<Grid key={idx} size={{ xs: 12, sm: 6 }}>
						<Stack spacing={0.5}>
							<Typography variant='body2' color='text.secondary'>
								{item.label}
							</Typography>
							<Typography variant='body2' sx={{ fontWeight: 600 }}>
								{item.value}
							</Typography>
						</Stack>
					</Grid>
				))}
			</Grid>
		</Stack>
	)

	const fields = payroll
		? [
				{
					title: t('payroll.section.basic_info'),
					of: [
						{ label: t('payroll.field.id'), value: payroll.id },
						{ label: t('payroll.field.staff_name'), value: payroll.staffName },
						{ label: t('payroll.field.month'), value: payroll.month },
						{ label: t('payroll.field.year'), value: payroll.year },
						{ label: t('payroll.field.total_worked_minutes'), value: payroll.totalWorkedMinutes },
						{ label: t('payroll.field.overtime_minutes'), value: payroll.overtimeMinutes },
						{ label: t('payroll.field.late_minutes'), value: payroll.lateMinutes },
						{ label: t('payroll.field.early_leave_minutes'), value: payroll.earlyLeaveMinutes },
						{
							label: t('payroll.field.status'),
							value: (
								<Chip
									label={getEnumLabelByValue(_enum.payrollStatusOptions, payroll.status) || payroll.status}
									size='small'
									color={defaultPayrollStatusStyle(payroll.status)}
								/>
							),
						},
					],
				},
				{
					title: t('payroll.section.summary'),
					of: [
						{ label: t('payroll.field.base_salary'), value: renderMoney(payroll.baseSalary) },
						{ label: t('payroll.field.gross_salary'), value: renderMoney(payroll.grossSalary) },
						{ label: t('payroll.field.net_salary'), value: renderMoney(payroll.netSalary) },
						{ label: t('payroll.field.total_allowance'), value: renderMoney(totalAllowance) },
						{ label: t('payroll.field.total_deduction'), value: renderMoney(totalDeduction) },
						{
							label: t('payroll.field.salary_advance_amount'),
							value: renderMoney(payroll.salaryAdvanceAmount),
						},
						{ label: t('payroll.field.total_insurance'), value: renderMoney(payroll.totalInsurance) },
						{
							label: t('payroll.field.personal_income_tax'),
							value: renderMoney(payroll.personalIncomeTax),
						},
					],
				},
				{
					title: t('payroll.section.tax_insurance'),
					of: [
						{ label: t('payroll.field.social_insurance'), value: renderMoney(payroll.socialInsurance) },
						{ label: t('payroll.field.health_insurance'), value: renderMoney(payroll.healthInsurance) },
						{
							label: t('payroll.field.unemployment_insurance'),
							value: renderMoney(payroll.unemploymentInsurance),
						},
						{
							label: t('payroll.field.total_allowance_not_taxable'),
							value: renderMoney(payroll.totalAllowanceNotTaxable),
						},
						{ label: t('payroll.field.taxable_income'), value: renderMoney(payroll.taxableIncome) },
						{ label: t('payroll.field.assessable_income'), value: renderMoney(payroll.assessableIncome) },
					],
				},
				{
					title: t('payroll.section.allowances'),
					of: [
						{
							label: t('payroll.field.allowances'),
							fullWidth: true,
							value: (
								<Stack spacing={1.5}>
									<Stack direction='row' alignItems='center' justifyContent='space-between'>
										<Typography variant='body2'>
											{t('payroll.text.items_count', { count: allowances.length })}
										</Typography>
										<Button
											variant='outlined'
											size='small'
											onClick={() => {
												setEditingAllowance(null)
												setOpenAllowanceDialog(true)
											}}
										>
											{t('button.add')}
										</Button>
									</Stack>
									<TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
										<Table size='small' sx={{ minWidth: 760 }}>
											<TableHead>
												<TableRow>
													<TableCell sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.allowance_type_name')}
													</TableCell>
													<TableCell align='right' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.unit_price')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.quantity')}
													</TableCell>
													<TableCell sx={{ whiteSpace: 'nowrap' }}>{t('payroll.field.note')}</TableCell>
													<TableCell align='right' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.total_amount')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.is_taxable')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.actions')}
													</TableCell>
												</TableRow>
											</TableHead>
											<TableBody>
												{allowances.length === 0 ? (
													<TableRow>
														<TableCell colSpan={7} align='center'>
															{t('text.placeholder.no_data')}
														</TableCell>
													</TableRow>
												) : (
													allowances.map((row) => (
														<TableRow key={row.id}>
															<TableCell>{renderEmptyFallback(row.allowanceTypeName)}</TableCell>
															<TableCell align='right'>{renderMoney(row.unitPrice)}</TableCell>
															<TableCell align='center'>{row.quantity ?? 0}</TableCell>
															<TableCell>{renderEmptyFallback(row.note)}</TableCell>
															<TableCell align='right'>
																{renderMoney((row.unitPrice || 0) * (row.quantity || 0))}
															</TableCell>
															<TableCell align='center'>{renderTaxableChip(row.isTaxable)}</TableCell>
															<TableCell align='center'>
																<ActionMenu
																	actions={[
																		{
																			title: t('button.edit'),
																			onClick: () => {
																				setEditingAllowance(row)
																				setOpenAllowanceDialog(true)
																			},
																		},
																		{
																			title: t('button.delete'),
																			onClick: () => handleDeleteAllowance(row),
																		},
																	]}
																/>
															</TableCell>
														</TableRow>
													))
												)}
												<TableRow>
													<TableCell colSpan={4} align='right'>
														<Typography variant='subtitle2'>{t('text.total')}</Typography>
													</TableCell>
													<TableCell align='right'>
														<Typography variant='subtitle2'>{renderMoney(totalAllowance)}</Typography>
													</TableCell>
													<TableCell />
													<TableCell />
												</TableRow>
											</TableBody>
										</Table>
									</TableContainer>
								</Stack>
							),
						},
					],
				},
				{
					title: t('payroll.section.deductions'),
					of: [
						{
							label: t('payroll.field.deductions'),
							fullWidth: true,
							value: (
								<Stack spacing={1.5}>
									<Stack direction='row' alignItems='center' justifyContent='space-between'>
										<Typography variant='body2'>
											{t('payroll.text.items_count', { count: deductions.length })}
										</Typography>
										<Button
											variant='outlined'
											size='small'
											onClick={() => {
												setEditingDeduction(null)
												setOpenDeductionDialog(true)
											}}
										>
											{t('button.add')}
										</Button>
									</Stack>
									<TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
										<Table size='small' sx={{ minWidth: 760 }}>
											<TableHead>
												<TableRow>
													<TableCell sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.deduction_type_name')}
													</TableCell>
													<TableCell align='right' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.unit_price')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.quantity')}
													</TableCell>
													<TableCell sx={{ whiteSpace: 'nowrap' }}>{t('payroll.field.note')}</TableCell>
													<TableCell align='right' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.total_amount')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.is_taxable')}
													</TableCell>
													<TableCell align='center' sx={{ whiteSpace: 'nowrap' }}>
														{t('payroll.field.actions')}
													</TableCell>
												</TableRow>
											</TableHead>
											<TableBody>
												{deductions.length === 0 ? (
													<TableRow>
														<TableCell colSpan={7} align='center'>
															{t('text.placeholder.no_data')}
														</TableCell>
													</TableRow>
												) : (
													deductions.map((row) => (
														<TableRow key={row.id}>
															<TableCell>{renderEmptyFallback(row.deductionTypeName)}</TableCell>
															<TableCell align='right'>{renderMoney(row.unitPrice)}</TableCell>
															<TableCell align='center'>{row.quantity ?? 0}</TableCell>
															<TableCell>{renderEmptyFallback(row.note)}</TableCell>
															<TableCell align='right'>
																{renderMoney((row.unitPrice || 0) * (row.quantity || 0))}
															</TableCell>
															<TableCell align='center'>{renderTaxableChip(row.isTaxable)}</TableCell>
															<TableCell align='center'>
																<ActionMenu
																	actions={[
																		{
																			title: t('button.edit'),
																			onClick: () => {
																				setEditingDeduction(row)
																				setOpenDeductionDialog(true)
																			},
																		},
																		{
																			title: t('button.delete'),
																			onClick: () => handleDeleteDeduction(row),
																		},
																	]}
																/>
															</TableCell>
														</TableRow>
													))
												)}
												<TableRow>
													<TableCell colSpan={4} align='right'>
														<Typography variant='subtitle2'>{t('text.total')}</Typography>
													</TableCell>
													<TableCell align='right'>
														<Typography variant='subtitle2'>{renderMoney(totalDeduction)}</Typography>
													</TableCell>
													<TableCell />
													<TableCell />
												</TableRow>
											</TableBody>
										</Table>
									</TableContainer>
								</Stack>
							),
						},
					],
				},
				{
					title: t('payroll.section.snapshots'),
					of: [
						{
							label: t('payroll.section.global_tax_config'),
							fullWidth: true,
							value: renderSnapshotGroup(t('payroll.section.global_tax_config'), [
								{
									label: t('payroll.field.reference_base_salary'),
									value: renderMoney(payroll.globalTaxConfigSnapshot?.referenceBaseSalary),
								},
								{
									label: t('payroll.field.max_social_health_insurance_salary'),
									value: renderMoney(payroll.globalTaxConfigSnapshot?.maxSocialHealthInsuranceSalary),
								},
								{
									label: t('payroll.field.personal_deduction_amount'),
									value: renderMoney(payroll.globalTaxConfigSnapshot?.personalDeductionAmount),
								},
								{
									label: t('payroll.field.dependent_deduction_amount'),
									value: renderMoney(payroll.globalTaxConfigSnapshot?.dependentDeductionAmount),
								},
								{
									label: t('payroll.field.social_insurance_rate'),
									value: renderPercent(payroll.globalTaxConfigSnapshot?.socialInsuranceRate),
								},
								{
									label: t('payroll.field.health_insurance_rate'),
									value: renderPercent(payroll.globalTaxConfigSnapshot?.healthInsuranceRate),
								},
								{
									label: t('payroll.field.unemployment_insurance_rate'),
									value: renderPercent(payroll.globalTaxConfigSnapshot?.unemploymentInsuranceRate),
								},
							]),
						},
						{
							label: t('payroll.section.regional_wage'),
							fullWidth: true,
							value: renderSnapshotGroup(t('payroll.section.regional_wage'), [
								{
									label: t('payroll.field.monthly_wage'),
									value: renderMoney(payroll.regionalWageSnapshot?.monthlyWage),
								},
								{
									label: t('payroll.field.region'),
									value: renderEmptyFallback(payroll.regionalWageSnapshot?.region),
								},
							]),
						},
						{
							label: t('payroll.section.staff_contract'),
							fullWidth: true,
							value: renderSnapshotGroup(t('payroll.section.staff_contract'), [
								{
									label: t('payroll.field.hourly_rate'),
									value: renderMoney(payroll.staffContractSnapshot?.hourlyRate),
								},
								{
									label: t('payroll.field.insurance_salary_rate'),
									value: renderPercent(payroll.staffContractSnapshot?.insuranceSalaryRate),
								},
							]),
						},
						{
							label: t('payroll.section.staff_info'),
							fullWidth: true,
							value: renderSnapshotGroup(t('payroll.section.staff_info'), [
								{
									label: t('payroll.field.dependent_quantity'),
									value: renderEmptyFallback(payroll.staffSnapshot?.dependentQuantity),
								},
								{
									label: t('payroll.field.hourly_rate'),
									value: renderMoney(payroll.staffSnapshot?.hourlyRate),
								},
							]),
						},
						{
							label: t('payroll.section.salary_advance_info'),
							fullWidth: true,
							value: payroll.salaryAdvanceSnapshot
								? renderSnapshotGroup(t('payroll.section.salary_advance_info'), [
										{
											label: t('payroll.field.salary_advance_amount'),
											value: renderMoney(payroll.salaryAdvanceAmount),
										},
										{
											label: t('payroll.field.salary_advance_reason'),
											value: renderEmptyFallback(payroll.salaryAdvanceSnapshot.reason),
										},
									])
								: renderEmptyFallback(null),
						},
					],
				},
			]
		: []

	const buttons = canPrint ? (
		<Stack direction='row' spacing={2}>
			<Button variant='outlined' onClick={handlePrint} disabled={!payroll || loading} fullWidth>
				{t('payroll.button.print')}
			</Button>
		</Stack>
	) : null

	return (
		<>
			<GenericDrawer
				open={open}
				onClose={onClose}
				title={t('payroll.title.detail')}
				fields={fields}
				buttons={buttons}
			/>

			<GenericFormDialog
				open={openAllowanceDialog}
				onClose={() => setOpenAllowanceDialog(false)}
				title={editingAllowance ? t('payroll.title.edit_allowance') : t('payroll.title.add_allowance')}
				fields={editingAllowance ? allowanceUpdateFields : allowanceCreateFields}
				initialValues={
					editingAllowance || {
						isTaxable: false,
						quantity: 1,
						unitPrice: 0,
						allowanceTypeId: allowanceTypeOptions?.[0]?.value,
					}
				}
				onSubmit={handleUpsertAllowance}
			/>

			<GenericFormDialog
				open={openDeductionDialog}
				onClose={() => setOpenDeductionDialog(false)}
				title={editingDeduction ? t('payroll.title.edit_deduction') : t('payroll.title.add_deduction')}
				fields={editingDeduction ? deductionUpdateFields : deductionCreateFields}
				initialValues={
					editingDeduction || {
						isTaxable: false,
						quantity: 1,
						unitPrice: 0,
						deductionTypeId: deductionTypeOptions?.[0]?.value,
					}
				}
				onSubmit={handleUpsertDeduction}
			/>
		</>
	)
}

export default PayrollManagementDetailDrawerSection

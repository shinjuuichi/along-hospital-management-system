import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
} from '@/utils/formatNumberUtil'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
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

const PayrollManagementPrintPreviewSection = ({ open, onClose, payroll, loading, onPrint }) => {
	const { t } = useTranslation()

	if (!payroll) {
		return null
	}

	const allowances = Array.isArray(payroll.allowances) ? payroll.allowances : []
	const deductions = Array.isArray(payroll.deductions) ? payroll.deductions : []

	const sumAmount = (list) =>
		list.reduce(
			(sum, item) => sum + (Number(item?.unitPrice) || 0) * (Number(item?.quantity) || 0),
			0
		)

	const totalAllowance = payroll.totalAllowance ?? sumAmount(allowances)
	const totalDeduction = payroll.totalDeduction ?? sumAmount(deductions)

	const renderMoney = (value) => formatCurrencyBasedOnCurrentLanguage(Number(value) || 0)
	const renderPercent = (value) => formatNumberToPercent(Number(value) || 0)
	const salaryAdvanceSnapshot = payroll.salaryAdvanceSnapshot

	return (
		<Dialog open={open} onClose={onClose} fullWidth maxWidth='md'>
			<DialogTitle>{t('payroll.title.payslip')}</DialogTitle>
			<DialogContent dividers>
				<Stack spacing={2}>
					<Stack spacing={1}>
						<Stack direction='row' justifyContent='space-between'>
							<Typography variant='body2' color='text.secondary'>
								{t('payroll.field.id')}
							</Typography>
							<Typography variant='body2'>{payroll.id}</Typography>
						</Stack>
						<Stack direction='row' justifyContent='space-between'>
							<Typography variant='body2' color='text.secondary'>
								{t('payroll.field.staff_name')}
							</Typography>
							<Typography variant='body2'>{renderEmptyFallback(payroll.staffName)}</Typography>
						</Stack>
						<Stack direction='row' justifyContent='space-between'>
							<Typography variant='body2' color='text.secondary'>
								{t('payroll.field.month')}/{t('payroll.field.year')}
							</Typography>
							<Typography variant='body2'>
								{payroll.month}/{payroll.year}
							</Typography>
						</Stack>
					</Stack>

					<Divider />

					<Grid container spacing={2}>
						<Grid size={{ xs: 12, md: 6 }}>
							<Typography variant='subtitle2' gutterBottom>
								{t('payroll.section.basic_info')}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.total_worked_minutes')}: {payroll.totalWorkedMinutes}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.overtime_minutes')}: {payroll.overtimeMinutes}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.late_minutes')}: {payroll.lateMinutes}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.early_leave_minutes')}: {payroll.earlyLeaveMinutes}
							</Typography>
						</Grid>
						<Grid size={{ xs: 12, md: 6 }}>
							<Typography variant='subtitle2' gutterBottom>
								{t('payroll.section.summary')}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.base_salary')}: {renderMoney(payroll.baseSalary)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.gross_salary')}: {renderMoney(payroll.grossSalary)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.net_salary')}: {renderMoney(payroll.netSalary)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.personal_income_tax')}: {renderMoney(payroll.personalIncomeTax)}
							</Typography>
						</Grid>
					</Grid>

					<Grid container spacing={2}>
						<Grid size={{ xs: 12, md: 6 }}>
							<Typography variant='subtitle2' gutterBottom>
								{t('payroll.section.tax_insurance')}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.total_allowance')}: {renderMoney(totalAllowance)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.total_deduction')}: {renderMoney(totalDeduction)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.salary_advance_amount')}: {renderMoney(payroll.salaryAdvanceAmount)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.total_insurance')}: {renderMoney(payroll.totalInsurance)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.social_insurance')}: {renderMoney(payroll.socialInsurance)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.health_insurance')}: {renderMoney(payroll.healthInsurance)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.unemployment_insurance')}: {renderMoney(payroll.unemploymentInsurance)}
							</Typography>
						</Grid>
						<Grid size={{ xs: 12, md: 6 }}>
							<Typography variant='subtitle2' gutterBottom>
								{t('payroll.section.tax_insurance')}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.total_allowance_not_taxable')}:{' '}
								{renderMoney(payroll.totalAllowanceNotTaxable)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.taxable_income')}: {renderMoney(payroll.taxableIncome)}
							</Typography>
							<Typography variant='body2'>
								{t('payroll.field.assessable_income')}: {renderMoney(payroll.assessableIncome)}
							</Typography>
						</Grid>
					</Grid>

					<Divider />

					<Stack spacing={1}>
						<Typography variant='subtitle2'>{t('payroll.section.allowances')}</Typography>
						<TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
							<Table size='small' sx={{ minWidth: 640 }}>
								<TableHead>
									<TableRow>
										<TableCell>{t('payroll.field.allowance_type_name')}</TableCell>
										<TableCell align='right'>{t('payroll.field.unit_price')}</TableCell>
										<TableCell align='center'>{t('payroll.field.quantity')}</TableCell>
										<TableCell align='right'>{t('payroll.field.total_amount')}</TableCell>
									</TableRow>
								</TableHead>
								<TableBody>
									{allowances.length === 0 ? (
										<TableRow>
											<TableCell colSpan={4} align='center'>
												{t('text.placeholder.no_data')}
											</TableCell>
										</TableRow>
									) : (
										allowances.map((row) => (
											<TableRow key={row.id}>
												<TableCell>{renderEmptyFallback(row.allowanceTypeName)}</TableCell>
												<TableCell align='right'>{renderMoney(row.unitPrice)}</TableCell>
												<TableCell align='center'>x{row.quantity ?? 0}</TableCell>
												<TableCell align='right'>
													{renderMoney((row.unitPrice || 0) * (row.quantity || 0))}
												</TableCell>
											</TableRow>
										))
									)}
									<TableRow>
										<TableCell colSpan={3} align='right'>
											<Typography variant='subtitle2'>{t('text.total')}</Typography>
										</TableCell>
										<TableCell align='right'>
											<Typography variant='subtitle2'>{renderMoney(totalAllowance)}</Typography>
										</TableCell>
									</TableRow>
								</TableBody>
							</Table>
						</TableContainer>
					</Stack>

					<Stack spacing={1}>
						<Typography variant='subtitle2'>{t('payroll.section.deductions')}</Typography>
						<TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
							<Table size='small' sx={{ minWidth: 640 }}>
								<TableHead>
									<TableRow>
										<TableCell>{t('payroll.field.deduction_type_name')}</TableCell>
										<TableCell align='right'>{t('payroll.field.unit_price')}</TableCell>
										<TableCell align='center'>{t('payroll.field.quantity')}</TableCell>
										<TableCell align='right'>{t('payroll.field.total_amount')}</TableCell>
									</TableRow>
								</TableHead>
								<TableBody>
									{deductions.length === 0 ? (
										<TableRow>
											<TableCell colSpan={4} align='center'>
												{t('text.placeholder.no_data')}
											</TableCell>
										</TableRow>
									) : (
										deductions.map((row) => (
											<TableRow key={row.id}>
												<TableCell>{renderEmptyFallback(row.deductionTypeName)}</TableCell>
												<TableCell align='right'>{renderMoney(row.unitPrice)}</TableCell>
												<TableCell align='center'>x{row.quantity ?? 0}</TableCell>
												<TableCell align='right'>
													{renderMoney((row.unitPrice || 0) * (row.quantity || 0))}
												</TableCell>
											</TableRow>
										))
									)}
									<TableRow>
										<TableCell colSpan={3} align='right'>
											<Typography variant='subtitle2'>{t('text.total')}</Typography>
										</TableCell>
										<TableCell align='right'>
											<Typography variant='subtitle2'>{renderMoney(totalDeduction)}</Typography>
										</TableCell>
									</TableRow>
								</TableBody>
							</Table>
						</TableContainer>
					</Stack>

					<Divider />

					<Stack spacing={1}>
						<Typography variant='subtitle2'>{t('payroll.section.snapshots')}</Typography>
						<Grid container spacing={2}>
							<Grid size={{ xs: 12, md: 6 }}>
								<Typography variant='body2'>
									{t('payroll.field.reference_base_salary')}:{' '}
									{renderMoney(payroll.globalTaxConfigSnapshot?.referenceBaseSalary)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.max_social_health_insurance_salary')}:{' '}
									{renderMoney(payroll.globalTaxConfigSnapshot?.maxSocialHealthInsuranceSalary)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.personal_deduction_amount')}:{' '}
									{renderMoney(payroll.globalTaxConfigSnapshot?.personalDeductionAmount)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.dependent_deduction_amount')}:{' '}
									{renderMoney(payroll.globalTaxConfigSnapshot?.dependentDeductionAmount)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Typography variant='body2'>
									{t('payroll.field.social_insurance_rate')}:{' '}
									{renderPercent(payroll.globalTaxConfigSnapshot?.socialInsuranceRate)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.health_insurance_rate')}:{' '}
									{renderPercent(payroll.globalTaxConfigSnapshot?.healthInsuranceRate)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.unemployment_insurance_rate')}:{' '}
									{renderPercent(payroll.globalTaxConfigSnapshot?.unemploymentInsuranceRate)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Typography variant='body2'>
									{t('payroll.field.monthly_wage')}: {renderMoney(payroll.regionalWageSnapshot?.monthlyWage)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.region')}: {renderEmptyFallback(payroll.regionalWageSnapshot?.region)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Typography variant='body2'>
									{t('payroll.field.hourly_rate')}: {renderMoney(payroll.staffContractSnapshot?.hourlyRate)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.insurance_salary_rate')}:{' '}
									{renderPercent(payroll.staffContractSnapshot?.insuranceSalaryRate)}
								</Typography>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Typography variant='body2'>
									{t('payroll.field.dependent_quantity')}: {renderEmptyFallback(payroll.staffSnapshot?.dependentQuantity)}
								</Typography>
								<Typography variant='body2'>
									{t('payroll.field.hourly_rate')}: {renderMoney(payroll.staffSnapshot?.hourlyRate)}
								</Typography>
							</Grid>
							{salaryAdvanceSnapshot ? (
								<Grid size={{ xs: 12, md: 6 }}>
									<Typography variant='body2'>
										{t('payroll.field.salary_advance_amount')}: {renderMoney(payroll.salaryAdvanceAmount)}
									</Typography>
									<Typography variant='body2'>
										{t('payroll.field.salary_advance_reason')}: {renderEmptyFallback(salaryAdvanceSnapshot.reason)}
									</Typography>
								</Grid>
							) : null}
						</Grid>
					</Stack>
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button variant='contained' onClick={onPrint} disabled={loading}>
					{t('payroll.button.print')}
				</Button>
				<Button onClick={onClose}>{t('button.close')}</Button>
			</DialogActions>
		</Dialog>
	)
}

export default PayrollManagementPrintPreviewSection

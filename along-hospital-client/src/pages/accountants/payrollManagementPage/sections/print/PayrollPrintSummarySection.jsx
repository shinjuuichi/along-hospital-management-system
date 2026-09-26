import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Grid, Stack, Typography } from '@mui/material'

const PayrollPrintSummarySection = ({ payroll, totalAllowance, totalDeduction }) => {
	const { t } = useTranslation()

	if (!payroll) return null

	const renderMoney = (value) => formatCurrencyBasedOnCurrentLanguage(Number(value) || 0)

	return (
		<Grid container spacing={1.5}>
			<Grid size={{ xs: 12, md: 4 }}>
				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
					{t('payroll.section.basic_info')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.total_worked_minutes')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>{payroll.totalWorkedMinutes}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.overtime_minutes')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{payroll.overtimeMinutes}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.late_minutes')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{payroll.lateMinutes}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.early_leave_minutes')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>{payroll.earlyLeaveMinutes}</Typography>
					</Stack>
				</Stack>
			</Grid>

			<Grid size={{ xs: 12, md: 4 }}>
				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
					{t('payroll.section.summary')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.base_salary')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.baseSalary)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.gross_salary')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.grossSalary)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.net_salary')}</Typography>
						<Typography sx={{ fontSize: 12, fontWeight: 700 }}>{renderMoney(payroll.netSalary)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.personal_income_tax')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.personalIncomeTax)}</Typography>
					</Stack>
				</Stack>
			</Grid>

			<Grid size={{ xs: 12, md: 4 }}>
				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
					{t('payroll.section.tax_insurance')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.total_allowance')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(totalAllowance)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.total_deduction')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(totalDeduction)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.salary_advance_amount')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.salaryAdvanceAmount)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.total_insurance')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.totalInsurance)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.social_insurance')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.socialInsurance)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.health_insurance')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.healthInsurance)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.unemployment_insurance')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.unemploymentInsurance)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.total_allowance_not_taxable')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.totalAllowanceNotTaxable)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.taxable_income')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.taxableIncome)}</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.assessable_income')}</Typography>
						<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.assessableIncome)}</Typography>
					</Stack>
				</Stack>
			</Grid>
		</Grid>
	)
}

export default PayrollPrintSummarySection

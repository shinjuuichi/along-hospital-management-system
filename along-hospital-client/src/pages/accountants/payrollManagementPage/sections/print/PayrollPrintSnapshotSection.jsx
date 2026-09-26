import useTranslation from '@/hooks/useTranslation'
import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
} from '@/utils/formatNumberUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Grid, Stack, Typography } from '@mui/material'

const PayrollPrintSnapshotSection = ({ payroll }) => {
	const { t } = useTranslation()

	if (!payroll) return null

	const renderMoney = (value) => formatCurrencyBasedOnCurrentLanguage(Number(value) || 0)
	const renderPercent = (value) => formatNumberToPercent(Number(value) || 0)
	const salaryAdvanceSnapshot = payroll.salaryAdvanceSnapshot

	return (
		<Grid container spacing={1.5} sx={{ mt: 1.5 }}>
			<Grid size={{ xs: 12, md: 6 }}>
				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
					{t('payroll.section.global_tax_config')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.reference_base_salary')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.globalTaxConfigSnapshot?.referenceBaseSalary)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.max_social_health_insurance_salary')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.globalTaxConfigSnapshot?.maxSocialHealthInsuranceSalary)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.personal_deduction_amount')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.globalTaxConfigSnapshot?.personalDeductionAmount)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.dependent_deduction_amount')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.globalTaxConfigSnapshot?.dependentDeductionAmount)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.social_insurance_rate')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderPercent(payroll.globalTaxConfigSnapshot?.socialInsuranceRate)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.health_insurance_rate')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderPercent(payroll.globalTaxConfigSnapshot?.healthInsuranceRate)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.unemployment_insurance_rate')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderPercent(payroll.globalTaxConfigSnapshot?.unemploymentInsuranceRate)}
						</Typography>
					</Stack>
				</Stack>
			</Grid>

			<Grid size={{ xs: 12, md: 6 }}>
				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
					{t('payroll.section.regional_wage')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.monthly_wage')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.regionalWageSnapshot?.monthlyWage)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.region')}</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderEmptyFallback(payroll.regionalWageSnapshot?.region)}
						</Typography>
					</Stack>
				</Stack>

				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase', mt: 1 }}>
					{t('payroll.section.staff_contract')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.hourly_rate')}</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.staffContractSnapshot?.hourlyRate)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.insurance_salary_rate')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderPercent(payroll.staffContractSnapshot?.insuranceSalaryRate)}
						</Typography>
					</Stack>
				</Stack>

				<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase', mt: 1 }}>
					{t('payroll.section.staff_info')}
				</Typography>
				<Stack spacing={0.25}>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>
							{t('payroll.field.dependent_quantity')}
						</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderEmptyFallback(payroll.staffSnapshot?.dependentQuantity)}
						</Typography>
					</Stack>
					<Stack direction='row' justifyContent='space-between'>
						<Typography sx={{ fontSize: 11, color: '#555' }}>{t('payroll.field.hourly_rate')}</Typography>
						<Typography sx={{ fontSize: 11 }}>
							{renderMoney(payroll.staffSnapshot?.hourlyRate)}
						</Typography>
					</Stack>
				</Stack>

				{salaryAdvanceSnapshot ? (
					<>
						<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase', mt: 1 }}>
							{t('payroll.section.salary_advance_info')}
						</Typography>
						<Stack spacing={0.25}>
							<Stack direction='row' justifyContent='space-between'>
								<Typography sx={{ fontSize: 11, color: '#555' }}>
									{t('payroll.field.salary_advance_amount')}
								</Typography>
								<Typography sx={{ fontSize: 11 }}>{renderMoney(payroll.salaryAdvanceAmount)}</Typography>
							</Stack>
							<Stack direction='row' justifyContent='space-between'>
								<Typography sx={{ fontSize: 11, color: '#555' }}>
									{t('payroll.field.salary_advance_reason')}
								</Typography>
								<Typography sx={{ fontSize: 11 }}>
									{renderEmptyFallback(salaryAdvanceSnapshot.reason)}
								</Typography>
							</Stack>
						</Stack>
					</>
				) : null}
			</Grid>
		</Grid>
	)
}

export default PayrollPrintSnapshotSection

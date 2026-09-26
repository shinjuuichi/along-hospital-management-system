import { defaultPayrollStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Chip, Grid, Typography } from '@mui/material'

const PayrollPrintHeaderSection = ({ payroll }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	if (!payroll) return null

	return (
		<Grid container spacing={1.5} alignItems='center'>
			<Grid size={7}>
				<Typography sx={{ fontSize: 18, fontWeight: 700, textTransform: 'uppercase' }}>
					{t('payroll.title.payslip')}
				</Typography>
				<Typography sx={{ fontSize: 16, fontWeight: 700 }}>{renderEmptyFallback(payroll.staffName)}</Typography>
				<Typography sx={{ fontSize: 11, color: '#555' }}>
					{t('payroll.field.month')} {payroll.month}/{payroll.year}
				</Typography>
			</Grid>
			<Grid size={5} sx={{ textAlign: 'right' }}>
				<Typography sx={{ fontSize: 12, fontWeight: 700 }}>#{payroll.id}</Typography>
				<Chip
					size='small'
					label={getEnumLabelByValue(_enum.payrollStatusOptions, payroll.status) || payroll.status}
					color={defaultPayrollStatusStyle(payroll.status)}
					sx={{ mt: 0.5 }}
				/>
			</Grid>
		</Grid>
	)
}

export default PayrollPrintHeaderSection

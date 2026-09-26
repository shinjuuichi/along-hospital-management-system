import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Grid, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@mui/material'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const PayrollPrintAllowanceSection = ({ allowances, totalAllowance }) => {
	const { t } = useTranslation()
	const renderMoney = (value) => formatCurrencyBasedOnCurrentLanguage(Number(value) || 0)

	return (
		<Grid size={{ xs: 12, md: 6 }}>
			<Typography sx={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase' }} gutterBottom>
				{t('payroll.section.allowances')}
			</Typography>
			<Table
				size='small'
				sx={{
					mb: 0.5,
					'& th': { fontSize: 11, fontWeight: 700, color: '#333' },
					'& td': { fontSize: 11, color: '#333' },
					'& th, & td': { py: 0.4 },
					'& thead th': { borderBottomColor: '#B0B7C3' },
					'& tbody td': { borderBottomColor: '#D1D7E0' },
				}}
			>
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
							<Typography sx={{ fontSize: 11, fontWeight: 700 }}>{t('text.total')}</Typography>
						</TableCell>
						<TableCell align='right'>
							<Typography sx={{ fontSize: 11, fontWeight: 700 }}>
								{renderMoney(totalAllowance)}
							</Typography>
						</TableCell>
					</TableRow>
				</TableBody>
			</Table>
		</Grid>
	)
}

export default PayrollPrintAllowanceSection

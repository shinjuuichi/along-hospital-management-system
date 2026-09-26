import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button, Divider, Grid, Paper } from '@mui/material'
import { useMemo } from 'react'
import { useParams } from 'react-router-dom'
import PayrollPrintAllowanceSection from './sections/print/PayrollPrintAllowanceSection'
import PayrollPrintDeductionSection from './sections/print/PayrollPrintDeductionSection'
import PayrollPrintHeaderSection from './sections/print/PayrollPrintHeaderSection'
import PayrollPrintQrSection from './sections/print/PayrollPrintQrSection'
import PayrollPrintSnapshotSection from './sections/print/PayrollPrintSnapshotSection'
import PayrollPrintSummarySection from './sections/print/PayrollPrintSummarySection'

const PayrollPrintPage = () => {
	const { id } = useParams()
	const { t } = useTranslation()

	const { data, loading, error } = useFetch(ApiUrls.PAYROLL.MANAGEMENT.DETAIL(id), {}, [id])
	const payroll = data?.data || data

	const allowances = useMemo(
		() => (Array.isArray(payroll?.allowances) ? payroll.allowances : []),
		[payroll]
	)
	const deductions = useMemo(
		() => (Array.isArray(payroll?.deductions) ? payroll.deductions : []),
		[payroll]
	)

	const sumAmount = (list) =>
		list.reduce(
			(sum, item) => sum + (Number(item?.unitPrice) || 0) * (Number(item?.quantity) || 0),
			0
		)

	const totalAllowance = payroll?.totalAllowance ?? sumAmount(allowances)
	const totalDeduction = payroll?.totalDeduction ?? sumAmount(deductions)

	if (loading) {
		return <Box sx={{ p: 4, bgcolor: 'background.default' }} />
	}

	if (!payroll || error) {
		return <Box sx={{ p: 4, bgcolor: 'background.default' }} />
	}

	return (
		<Box sx={{ p: 3, bgcolor: 'background.default' }}>
			<style>
				{`
				@media print {
					@page {
						margin: 0;
					}
					body * {
						visibility: hidden;
					}
					#payroll-print-paper, #payroll-print-paper * {
						visibility: visible;
					}
					#payroll-print-paper {
						position: absolute;
						top: 0;
						left: 50%;
						transform: translateX(-50%);
						width: 100%;
					}
					body {
						margin: 0;
					}
				}
			`}
			</style>

			<Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
				<Button variant='outlined' onClick={() => window.print()}>
					{t('payroll.button.print')}
				</Button>
			</Box>

			<Paper
				id='payroll-print-paper'
				elevation={3}
				sx={{
					maxWidth: 1000,
					mx: 'auto',
					p: 3,
					bgcolor: 'background.paper',
					fontFamily: 'Inter, Roboto, Lato, sans-serif',
					color: '#333333',
					lineHeight: 1.5,
					'@media print': {
						boxShadow: 'none',
						borderRadius: 0,
					},
				}}
			>
				<PayrollPrintHeaderSection payroll={payroll} />

				<Divider sx={{ my: 1.5, borderColor: '#B0B7C3' }} />

				<PayrollPrintSummarySection
					payroll={payroll}
					totalAllowance={totalAllowance}
					totalDeduction={totalDeduction}
				/>

				<Grid container spacing={1.5} sx={{ mt: 1.5 }}>
					<PayrollPrintAllowanceSection
						allowances={allowances}
						totalAllowance={totalAllowance}
					/>
					<PayrollPrintDeductionSection
						deductions={deductions}
						totalDeduction={totalDeduction}
					/>
				</Grid>

				<PayrollPrintSnapshotSection payroll={payroll} />
				<PayrollPrintQrSection qrCode={payroll?.qrCode} />
			</Paper>
		</Box>
	)
}

export default PayrollPrintPage

import SkeletonInvoice from '@/components/skeletons/SkeletonInvoice'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultInvoiceStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Box,
	Button,
	Chip,
	Divider,
	Grid,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow,
	Typography,
} from '@mui/material'
import QRCode from 'react-qr-code'
import { useParams } from 'react-router-dom'

const InvoicePrintPage = () => {
	const { id } = useParams()
	const { t } = useTranslation()
	const _enum = useEnum()

	const { data: invoice, loading, error } = useFetch(ApiUrls.INVOICE.DETAIL(id), {}, [id])

	if (loading) {
		return <SkeletonInvoice />
	}

	if (!invoice || error) {
		return <Box sx={{ p: 4, bgcolor: 'background.default' }} />
	}

	const charges = Array.isArray(invoice.charges) ? invoice.charges : []
	const invoiceCharges = charges.filter(
		(charge) => charge?.chargeType === EnumConfig.ChargeType.Invoice
	)
	const totalService = invoiceCharges.reduce(
		(sum, charge) => sum + (Number(charge?.quantity) || 0),
		0
	)
	const totalInvoiceAmount = invoice.totalInvoiceAmount
	const taxRate = 0
	const taxAmount = totalInvoiceAmount * taxRate
	const totalAmount = totalInvoiceAmount + taxAmount
	const kioskQueueQrValue = JSON.stringify({
		medicalHistory: invoice.medicalHistoryId ?? null,
	})

	return (
		<Box sx={{ p: 4, bgcolor: 'background.default' }}>
			<style>
				{`
				@media print {
					body * {
						visibility: hidden;
					}
					#invoice-print-paper, #invoice-print-paper * {
						visibility: visible;
					}
					#invoice-print-paper {
						position: absolute;
						top: 0;
						left: 50%;
						transform: translateX(-50%);
						width: 100%;
					}
				}
			`}
			</style>

			<Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
				<Button variant='outlined' onClick={() => window.print()}>
					{t('invoice.button.print_invoice')}
				</Button>
			</Box>

			<Paper
				id='invoice-print-paper'
				elevation={3}
				sx={{
					maxWidth: 'lg',
					mx: 'auto',
					p: 4,
					bgcolor: 'background.paper',
					'@media print': {
						boxShadow: 'none',
						borderRadius: 0,
					},
				}}
			>
				<Grid container spacing={2} alignItems='center'>
					<Grid size={8}>
						<Typography variant='h5' fontWeight={700} textTransform='uppercase'>
							{t('about_us.information.name')}
						</Typography>
						<Typography variant='body2'>
							{t('about_us.field.address')}: {t('about_us.information.address')}
						</Typography>
						<Typography variant='body2'>
							{t('about_us.field.phone')}: {t('about_us.information.phone')}
						</Typography>
					</Grid>

					<Grid size={4} sx={{ textAlign: 'right' }}>
						<Typography variant='h6' fontWeight={700} textTransform='uppercase'>
							{t('invoice.title.invoice_print')}
						</Typography>
						<Typography variant='body2'>
							{t('invoice.field.invoice_number')}: {invoice.invoiceNumber || `#${invoice.id}`}
						</Typography>
						<Typography variant='body2'>
							{t('invoice.field.creation_date')}:{' '}
							{formatDatetimeStringBasedOnCurrentLanguage(invoice.creationDate)}
						</Typography>
					</Grid>
				</Grid>

				<Divider sx={{ my: 3 }} />

				<Grid container spacing={2}>
					<Grid size={{ xs: 12, sm: 6 }}>
						<Typography variant='body2' color='text.secondary'>
							{t('invoice.field.invoice_status')}
						</Typography>
						<Chip
							size='small'
							label={renderEmptyFallback(getEnumLabelByValue(_enum.invoiceStatusOptions, invoice.invoiceStatus))}
							color={defaultInvoiceStatusStyle(invoice.invoiceStatus)}
							sx={{ mt: 0.5 }}
						/>
					</Grid>

					<Grid size={{ xs: 12, sm: 6 }} sx={{ textAlign: { xs: 'left', sm: 'right' } }}>
						<Typography variant='body2' color='text.secondary'>
							{t('invoice.field.payment_date')}
						</Typography>
						<Typography variant='body2' fontWeight={600}>
							{renderEmptyFallback(
								invoice.paymentDate
									? formatDatetimeStringBasedOnCurrentLanguage(invoice.paymentDate)
									: null
							)}
						</Typography>
					</Grid>
				</Grid>

				<Box sx={{ mt: 4 }}>
					<Typography variant='subtitle1' fontWeight={600} gutterBottom>
						{t('invoice.field.charge.medical_service')}
					</Typography>

					{invoiceCharges.length === 0 ? (
						<Typography variant='body2' color='text.secondary'>
							{t('text.placeholder.no_data')}
						</Typography>
					) : (
						<Table size='small'>
							<TableHead>
								<TableRow>
									<TableCell>{t('text.no_')}</TableCell>
									<TableCell>{t('invoice.field.charge.medical_service')}</TableCell>
									<TableCell align='center'>{t('invoice.field.charge.quantity')}</TableCell>
									<TableCell align='right'>{t('invoice.field.charge.unit_price')}</TableCell>
									<TableCell align='right'>{t('invoice.field.charge.total_amount')}</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{invoiceCharges.map((charge, index) => {
									return (
										<TableRow key={charge.id || `${charge.medicalServiceId}-${index}`}>
											<TableCell>{index + 1}</TableCell>
											<TableCell>
												<Typography variant='body2' fontWeight={500}>
													{renderEmptyFallback(charge?.chargeSnapshot?.medicalServiceName)}
												</Typography>
												<Typography variant='caption' color='text.secondary'>
													{charge?.chargeSnapshot?.medicalServiceDescription || t('text.none')}
												</Typography>
											</TableCell>
											<TableCell align='center'>x{charge?.quantity || 0}</TableCell>
											<TableCell align='right'>
												{formatCurrencyBasedOnCurrentLanguage(charge?.unitPrice ?? 0)}
											</TableCell>
											<TableCell align='right'>
												{formatCurrencyBasedOnCurrentLanguage(charge?.totalAmount ?? 0)}
											</TableCell>
										</TableRow>
									)
								})}
							</TableBody>
						</Table>
					)}
				</Box>

				<Grid container spacing={2} sx={{ mt: 4 }} justifyContent='space-between'>
					{invoice.invoiceStatus === EnumConfig.InvoiceStatus.Completed && (
						<Grid size={1}>
							<Box
								sx={{
									display: 'inline-flex',
									mt: 1.5,
									p: 0.75,
									bgcolor: 'common.white',
									border: '1px solid',
									borderColor: 'divider',
									borderRadius: 1,
								}}
							>
								<QRCode value={kioskQueueQrValue} size={150} level='M' />
							</Box>
						</Grid>
					)}
					<Grid size={{ sm: 8, md: 5 }} ml={'auto'}>
						<Box sx={{ border: '1px solid', borderColor: 'divider', p: 2, borderRadius: 1 }}>
							<Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
								<Typography variant='body2'>{t('invoice.field.total_service')}</Typography>
								<Typography variant='body2'>x{totalService}</Typography>
							</Box>
							<Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
								<Typography variant='body2'>{t('invoice.field.total_invoice_amount')}</Typography>
								<Typography variant='body2'>
									{formatCurrencyBasedOnCurrentLanguage(totalInvoiceAmount)}
								</Typography>
							</Box>
							<Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
								<Typography variant='body2'>
									{t('invoice.field.tax_percent', { percent: taxRate })}
								</Typography>
								<Typography variant='body2'>{formatCurrencyBasedOnCurrentLanguage(taxAmount)}</Typography>
							</Box>
							<Divider sx={{ my: 1 }} />
							<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
								<Typography variant='subtitle1' fontWeight={600}>
									{t('invoice.field.total_amount')}
								</Typography>
								<Typography variant='subtitle1' fontWeight={700}>
									{formatCurrencyBasedOnCurrentLanguage(totalAmount)}
								</Typography>
							</Box>
						</Box>
					</Grid>
				</Grid>
			</Paper>
		</Box>
	)
}

export default InvoicePrintPage

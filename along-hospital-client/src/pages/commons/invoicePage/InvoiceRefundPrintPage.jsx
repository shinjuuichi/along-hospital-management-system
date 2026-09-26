import SkeletonInvoice from '@/components/skeletons/SkeletonInvoice'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultRefundStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Divider, Grid, Paper, Stack, Typography } from '@mui/material'
import { useParams } from 'react-router-dom'

const InvoiceRefundPrintPage = () => {
	const { invoiceId, chargeId } = useParams()
	const { t } = useTranslation()
	const _enum = useEnum()

	const { data: invoice, loading } = useFetch(ApiUrls.INVOICE.DETAIL(invoiceId), {}, [invoiceId])

	if (loading) {
		return <SkeletonInvoice />
	}

	const charges = Array.isArray(invoice?.charges) ? invoice.charges : []
	const charge = charges.find((item) => String(item?.id) === String(chargeId))
	const refund = charge?.refund

	if (!invoice || !charge || !refund) {
		return <Box sx={{ p: 4, bgcolor: 'background.default' }} />
	}

	const staff = refund?.staff ?? null
	const chargeSnapshot = charge?.chargeSnapshot ?? null

	return (
		<Box sx={{ p: 4, bgcolor: 'background.default' }}>
			<style>
				{`
				@media print {
					body * {
						visibility: hidden;
					}
					#refund-print-paper, #refund-print-paper * {
						visibility: visible;
					}
					#refund-print-paper {
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
					{t('invoice.button.print_refund')}
				</Button>
			</Box>

			<Paper
				id='refund-print-paper'
				elevation={3}
				sx={{
					maxWidth: 900,
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
							{t('invoice.title.refund_print')}
						</Typography>
						<Typography variant='body2'>
							{t('invoice.field.refund_number')}: {`#${refund.id}`}
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

				<Grid container spacing={3}>
					<Grid size={{ xs: 12, md: 7 }}>
						<Stack spacing={1}>
							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('invoice.field.charge_id')}
								</Typography>
								<Typography variant='body2' fontWeight={600}>
									#{chargeId}
								</Typography>
							</Stack>

							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('invoice.field.charge.refund.status')}
								</Typography>
								<Chip
									size='small'
									label={renderEmptyFallback(getEnumLabelByValue(_enum.refundStatusOptions, refund.refundStatus))}
									color={defaultRefundStatusStyle(refund.refundStatus)}
								/>
							</Stack>

							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('invoice.field.charge.refund.approval_date')}
								</Typography>
								<Typography variant='body2' fontWeight={600} textAlign='right'>
									{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(refund.approvalDate))}
								</Typography>
							</Stack>

							<Stack>
								<Typography variant='body2' color='text.secondary'>
									{t('invoice.field.charge.refund.reason')}
								</Typography>
								<Typography variant='body2' fontWeight={500}>
									{renderEmptyFallback(refund.reason)}
								</Typography>
							</Stack>
						</Stack>
					</Grid>

					<Grid size={{ xs: 12, md: 5 }}>
						<Stack spacing={1}>
							<Typography variant='subtitle2'>{t('invoice.field.charge.refund.approved_by')}</Typography>
							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('profile.field.name')}
								</Typography>
								<Typography variant='body2' fontWeight={600}>
									{renderEmptyFallback(staff?.name)}
								</Typography>
							</Stack>
							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('profile.field.phone')}
								</Typography>
								<Typography variant='body2' fontWeight={600}>
									{renderEmptyFallback(staff?.phone)}
								</Typography>
							</Stack>
							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{t('profile.field.email')}
								</Typography>
								<Typography variant='body2' fontWeight={600}>
									{renderEmptyFallback(staff?.email)}
								</Typography>
							</Stack>
						</Stack>
					</Grid>
				</Grid>

				<Divider sx={{ my: 3 }} />

				<Stack spacing={1}>
					<Typography variant='subtitle1' fontWeight={600}>
						{t('invoice.field.charge.medical_service')}
					</Typography>
					<Typography variant='body2' fontWeight={600}>
						{renderEmptyFallback(chargeSnapshot?.medicalServiceName)}
					</Typography>
					<Typography variant='body2' color='text.secondary'>
						{chargeSnapshot?.medicalServiceDescription || t('text.none')}
					</Typography>
					<Grid container spacing={2} sx={{ mt: 0.5 }}>
						<Grid size={{ xs: 12, sm: 4 }}>
							<Typography variant='body2' color='text.secondary'>
								{t('invoice.field.charge.quantity')}
							</Typography>
							<Typography variant='body2' fontWeight={600}>
								x{charge?.quantity ?? 0}
							</Typography>
						</Grid>
						<Grid size={{ xs: 12, sm: 4 }}>
							<Typography variant='body2' color='text.secondary'>
								{t('invoice.field.charge.unit_price')}
							</Typography>
							<Typography variant='body2' fontWeight={600}>
								{formatCurrencyBasedOnCurrentLanguage(charge?.unitPrice)}
							</Typography>
						</Grid>
						<Grid size={{ xs: 12, sm: 4 }}>
							<Typography variant='body2' color='text.secondary'>
								{t('invoice.field.charge.total_amount')}
							</Typography>
							<Typography variant='body2' fontWeight={600}>
								{formatCurrencyBasedOnCurrentLanguage(charge?.totalAmount)}
							</Typography>
						</Grid>
					</Grid>
				</Stack>

				<Grid container spacing={2} sx={{ mt: 4 }} justifyContent='flex-end'>
					<Grid size={{ xs: 12, sm: 8, md: 5 }}>
						<Box sx={{ border: '1px solid', borderColor: 'divider', p: 2, borderRadius: 1 }}>
							<Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
								<Typography variant='subtitle1' fontWeight={600}>
									{t('invoice.field.total_refund_amount')}
								</Typography>
								<Typography variant='subtitle1' fontWeight={700}>
									{formatCurrencyBasedOnCurrentLanguage(charge.totalAmount)}
								</Typography>
							</Box>
						</Box>
					</Grid>
				</Grid>
			</Paper>
		</Box>
	)
}

export default InvoiceRefundPrintPage

import {
	defaultChargeTypeStyle,
	defaultInvoiceStatusStyle,
	defaultRefundStatusStyle,
} from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Box,
	Button,
	Chip,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
	Paper,
	Stack,
	Typography,
} from '@mui/material'

const InvoiceManagementDetailSection = ({
	open,
	onClose,
	invoice,
	role,
	loadingAction = false,
	onApproveRefundClick = (chargeId) => Promise.resolve(chargeId),
	onPrintInvoiceClick = (invoiceId) => Promise.resolve(invoiceId),
	onPrintRefundClick = (invoiceId, chargeId) => Promise.resolve({ invoiceId, chargeId }),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	if (!invoice) {
		return null
	}

	const isAccountant = role === EnumConfig.Role.Accountant
	const charges = Array.isArray(invoice.charges) ? invoice.charges : []
	const totalService = charges
		.filter((charge) => charge?.chargeType === EnumConfig.ChargeType.Invoice)
		.reduce((acc, charge) => acc + (Number(charge?.quantity) || 0), 0)

	const invoiceInfoFields = [
		{ label: t('invoice.field.invoice_number'), value: renderEmptyFallback(invoice.invoiceNumber) },
		{
			label: t('invoice.field.invoice_status'),
			value: () => (
				<Chip
					size='small'
					label={renderEmptyFallback(getEnumLabelByValue(_enum.invoiceStatusOptions, invoice.invoiceStatus))}
					color={defaultInvoiceStatusStyle(invoice.invoiceStatus)}
				/>
			),
		},
		{
			label: t('invoice.field.creation_date'),
			value: renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(invoice.creationDate)),
		},
		{
			label: t('invoice.field.payment_date'),
			value: renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(invoice.paymentDate)),
		},
		{ label: t('invoice.field.total_service'), value: `x${totalService}` },
		{
			label: t('invoice.field.total_invoice_amount'),
			value: formatCurrencyBasedOnCurrentLanguage(invoice.totalInvoiceAmount),
		},
		{
			label: t('invoice.field.total_refund_amount'),
			value: formatCurrencyBasedOnCurrentLanguage(invoice.totalRefundAmount),
		},
		{
			label: t('invoice.field.total_amount'),
			value: formatCurrencyBasedOnCurrentLanguage(invoice.totalAmount),
		},
	]

	return (
		<Dialog open={open} onClose={onClose} fullWidth maxWidth='md'>
			<DialogTitle>{t('button.detail')}</DialogTitle>
			<DialogContent dividers>
				<Stack spacing={2}>
					<Stack spacing={1}>
						{invoiceInfoFields.map((field, idx) => (
							<Stack key={idx} direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='body2' color='text.secondary'>
									{field.label}:
								</Typography>
								{typeof field.value === 'function' ? (
									field.value()
								) : (
									<Typography variant='body2' textAlign='right'>
										{renderEmptyFallback(field.value)}
									</Typography>
								)}
							</Stack>
						))}
					</Stack>

					<Divider />

					<Stack spacing={1}>
						<Typography variant='subtitle2'>{t('invoice.field.charge.medical_service')}</Typography>
						{charges.length === 0 ? (
							<Typography variant='body2' color='text.secondary'>
								{t('text.placeholder.no_data')}
							</Typography>
						) : (
							charges.map((charge) => {
								const refundStatus = charge?.refund?.refundStatus
								const isInvoiceCharge = charge?.chargeType === EnumConfig.ChargeType.Invoice
								const isPendingRefund = refundStatus === EnumConfig.RefundStatus.Pending

								const chargeFields = [
									{
										label: t('invoice.field.charge.charge_type'),
										value: () => (
											<Chip
												size='small'
												label={renderEmptyFallback(getEnumLabelByValue(_enum.chargeTypeOptions, charge?.chargeType))}
												color={defaultChargeTypeStyle(charge?.chargeType)}
											/>
										),
									},
									{ label: t('invoice.field.charge.quantity'), value: `x${charge?.quantity || 0}` },
									{
										label: t('invoice.field.charge.unit_price'),
										value: formatCurrencyBasedOnCurrentLanguage(charge?.unitPrice ?? 0),
									},
									{
										label: t('invoice.field.charge.total_amount'),
										value: formatCurrencyBasedOnCurrentLanguage(charge?.totalAmount ?? 0),
									},
								]

								const refundFields = charge?.refund
									? [
											{
												label: t('invoice.field.charge.refund.status'),
												value: () => (
													<Chip
														size='small'
														label={renderEmptyFallback(getEnumLabelByValue(_enum.refundStatusOptions, refundStatus))}
														color={defaultRefundStatusStyle(refundStatus)}
													/>
												),
											},
											{
												label: t('invoice.field.charge.refund.reason'),
												value: renderEmptyFallback(charge?.refund?.reason),
											},
											{
												label: t('invoice.field.charge.refund.approved_by'),
												value: renderEmptyFallback(charge?.refund?.staff?.name),
											},
											{
												label: t('invoice.field.charge.refund.approval_date'),
												value: formatDatetimeStringBasedOnCurrentLanguage(charge?.refund?.approvalDate),
											},
										]
									: []

								return (
									<Paper
										key={charge.id}
										variant='outlined'
										sx={{ display: 'flex', flexDirection: 'column', gap: 1, p: 1.5 }}
									>
										<Box>
											<Typography fontWeight={600}>
												{renderEmptyFallback(charge?.chargeSnapshot?.medicalServiceName)}
											</Typography>
											<Typography variant='body2' color='text.secondary'>
												{renderEmptyFallback(charge?.chargeSnapshot?.medicalServiceDescription)}
											</Typography>
										</Box>

										<Stack spacing={0.75}>
											{[...chargeFields, ...refundFields].map((field, idx) => (
												<Stack key={idx} direction='row' justifyContent='space-between' alignItems='center'>
													<Typography variant='body2' color='text.secondary'>
														{field.label}:
													</Typography>
													{typeof field.value === 'function' ? (
														field.value()
													) : (
														<Typography variant='body2' textAlign='right'>
															{renderEmptyFallback(field.value)}
														</Typography>
													)}
												</Stack>
											))}
										</Stack>

										{charge?.refund && (
											<Button
												fullWidth
												variant='contained'
												size='small'
												onClick={() => onPrintRefundClick(invoice?.id, charge.id)}
											>
												{t('invoice.button.print_refund')}
											</Button>
										)}

										{isAccountant && !isInvoiceCharge && isPendingRefund && (
											<Button
												fullWidth
												variant='outlined'
												color='secondary'
												size='small'
												loading={loadingAction}
												onClick={() => onApproveRefundClick(charge.id)}
											>
												{t('invoice.button.approve_refund')}
											</Button>
										)}
									</Paper>
								)
							})
						)}
					</Stack>
				</Stack>
			</DialogContent>

			<DialogActions>
				<Button variant='contained' onClick={() => onPrintInvoiceClick(invoice?.id)}>
					{t('invoice.button.print_invoice')}
				</Button>
				<Button onClick={onClose}>{t('button.close')}</Button>
			</DialogActions>
		</Dialog>
	)
}

export default InvoiceManagementDetailSection

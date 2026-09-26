import SkeletonBox from '@/components/skeletons/SkeletonBox'
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
import ChevronRightIcon from '@mui/icons-material/ChevronRight'
import ExpandMoreIcon from '@mui/icons-material/ExpandMore'
import {
	Box,
	Button,
	Card,
	CardContent,
	Chip,
	Collapse,
	Divider,
	IconButton,
	Paper,
	Stack,
	Typography,
} from '@mui/material'
import { useState } from 'react'

const MedicalHistoryDetailInvoiceSection = ({
	invoices,
	loading = false,
	onPrintInvoiceClick = (invoiceId) => Promise.resolve(invoiceId),
	onPrintRefundClick = (invoiceId, chargeId) => Promise.resolve({ invoiceId, chargeId }),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [openMap, setOpenMap] = useState({})

	const toggleOpen = (id) => {
		setOpenMap((prev) => ({
			...prev,
			[id]: !prev[id],
		}))
	}

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2 }}>
				<Typography variant='h6' mb={2}>
					{t('medical_history.title.invoices')}
				</Typography>
				<SkeletonBox numberOfBoxes={2} heights={[100]} rounded />
			</Paper>
		)
	}

	if (!invoices || invoices.length === 0) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2 }}>
				<Stack direction='row' justifyContent='space-between' alignItems='center' mb={2}>
					<Typography variant='h6'>{t('medical_history.title.invoices')}</Typography>
				</Stack>
				<Stack alignItems='center' justifyContent='center' spacing={2} sx={{ py: 4 }}>
					<Typography color='text.secondary'>{t('medical_history.placeholder.no_invoices')}</Typography>
				</Stack>
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 3, borderRadius: 2 }}>
			<Stack direction='row' justifyContent='space-between' alignItems='center' mb={2}>
				<Typography variant='h6'>{t('medical_history.title.invoices')}</Typography>
			</Stack>

			<Stack spacing={2}>
				{invoices?.map((invoice) => {
					const open = openMap[invoice.id]
					const charges = Array.isArray(invoice.charges) ? invoice.charges : []
					const displayTotalAmount = invoice.totalAmount
					const totalService =
						charges
							.filter((charge) => charge?.chargeType === EnumConfig.ChargeType.Invoice)
							.reduce((acc, charge) => acc + (Number(charge?.quantity) || 0), 0) ?? 0
					const hasInvoiceDetails = charges.length > 0

					return (
						<Card key={invoice.id} variant='outlined'>
							<CardContent sx={{ pb: '16px !important' }}>
								<Stack direction='row' alignItems='center' justifyContent='space-between' spacing={2}>
									<Stack direction='row' alignItems='center' spacing={1}>
										<IconButton size='small' onClick={() => toggleOpen(invoice.id)}>
											{open ? <ExpandMoreIcon /> : <ChevronRightIcon />}
										</IconButton>

										<Box>
											<Stack direction='row' spacing={1} alignItems='center'>
												<Typography fontWeight={600}>{invoice.invoiceNumber}</Typography>
												<Chip
													size='small'
													label={getEnumLabelByValue(_enum.invoiceStatusOptions, invoice.invoiceStatus)}
													color={defaultInvoiceStatusStyle(invoice.invoiceStatus)}
												/>
											</Stack>
											<Typography variant='body2' color='text.secondary'>
												{formatDatetimeStringBasedOnCurrentLanguage(invoice.creationDate)}
											</Typography>
										</Box>
									</Stack>

									<Stack direction='row' spacing={4} alignItems='center'>
										<Box textAlign='right'>
											<Typography variant='body2' color='text.secondary'>
												{t('invoice.field.total_service')}
											</Typography>
											<Typography fontWeight={600}>{totalService}</Typography>
										</Box>

										<Box textAlign='right'>
											<Typography variant='body2' color='text.secondary'>
												{t('invoice.field.total_amount')}
											</Typography>
											<Typography fontWeight={700}>
												{formatCurrencyBasedOnCurrentLanguage(displayTotalAmount)}
											</Typography>
										</Box>

										<Button
											variant='contained'
											size='small'
											onClick={() => onPrintInvoiceClick?.(invoice.id)}
										>
											{t('invoice.button.print_invoice')}
										</Button>
									</Stack>
								</Stack>
							</CardContent>

							<Collapse in={open} timeout='auto' unmountOnExit>
								<Divider />
								<CardContent>
									<Stack direction='row' spacing={3} mb={2} flexWrap='wrap'>
										<Box>
											<Typography variant='body2' color='text.secondary'>
												{t('invoice.field.total_invoice_amount')}
											</Typography>
											<Typography fontWeight={600}>
												{formatCurrencyBasedOnCurrentLanguage(invoice.totalInvoiceAmount)}
											</Typography>
										</Box>
										<Box>
											<Typography variant='body2' color='text.secondary'>
												{t('invoice.field.total_refund_amount')}
											</Typography>
											<Typography fontWeight={600}>
												{formatCurrencyBasedOnCurrentLanguage(invoice.totalRefundAmount)}
											</Typography>
										</Box>
									</Stack>
									<Divider orientation='horizontal' />
									<Stack gap={2}>
										{!hasInvoiceDetails ? (
											<Typography variant='body2' color='text.secondary'>
												{t('text.placeholder.no_data')}
											</Typography>
										) : (
											<>
												{charges.length > 0 && (
													<Stack gap={1} divider={<Divider orientation='horizontal' />}>
														{charges.map((charge) => {
															const isRefundCharge = charge?.chargeType === EnumConfig.ChargeType.Refund
															const hasRefund = !!charge?.refund
															const refundStatus = charge?.refund?.refundStatus

															return (
																<Stack
																	key={charge.id}
																	direction='row'
																	justifyContent='space-between'
																	alignItems='flex-start'
																>
																	<Box>
																		<Stack direction='row' gap={1} sx={{ mt: 0.75, flexWrap: 'wrap' }}>
																			<Chip
																				size='small'
																				label={renderEmptyFallback(
																					getEnumLabelByValue(_enum.chargeTypeOptions, charge?.chargeType)
																				)}
																				color={defaultChargeTypeStyle(charge?.chargeType)}
																			/>
																			<Typography fontWeight={500}>
																				{renderEmptyFallback(charge?.chargeSnapshot?.medicalServiceName)}
																			</Typography>
																			{isRefundCharge && (
																				<Chip
																					size='small'
																					label={renderEmptyFallback(
																						getEnumLabelByValue(_enum.refundStatusOptions, refundStatus)
																					)}
																					color={defaultRefundStatusStyle(refundStatus)}
																				/>
																			)}
																		</Stack>
																		<Typography variant='body2' color='text.secondary'>
																			{charge?.chargeSnapshot?.medicalServiceDescription || t('text.none')}
																		</Typography>
																		{isRefundCharge && charge?.refund?.reason ? (
																			<Typography variant='caption' color='text.secondary'>
																				{t('invoice.field.charge.refund.reason')}: {charge.refund.reason}
																			</Typography>
																		) : null}
																	</Box>

																	<Stack alignItems='flex-end' spacing={0.75}>
																		<Stack direction='row' spacing={3}>
																			<Typography>x{charge?.quantity || 0}</Typography>
																			<Typography>
																				{formatCurrencyBasedOnCurrentLanguage(charge?.unitPrice ?? 0)}
																			</Typography>
																			<Typography fontWeight={600}>
																				{formatCurrencyBasedOnCurrentLanguage(charge?.totalAmount ?? 0)}
																			</Typography>
																		</Stack>
																		{hasRefund && (
																			<Button
																				size='small'
																				variant='outlined'
																				color='info'
																				onClick={() => onPrintRefundClick?.(invoice.id, charge.id)}
																			>
																				{t('invoice.button.print_refund')}
																			</Button>
																		)}
																	</Stack>
																</Stack>
															)
														})}
													</Stack>
												)}
											</>
										)}
									</Stack>
								</CardContent>
							</Collapse>
						</Card>
					)
				})}
			</Stack>
		</Paper>
	)
}

export default MedicalHistoryDetailInvoiceSection

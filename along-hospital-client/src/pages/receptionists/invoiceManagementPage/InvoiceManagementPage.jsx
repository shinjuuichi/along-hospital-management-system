import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import InvoiceManagementDetailSection from './sections/InvoiceManagementDetailSection'
import InvoiceManagementFilterSection from './sections/InvoiceManagementFilterSection'
import InvoiceManagementTableSection from './sections/InvoiceManagementTableSection'

const InvoiceManagementPage = () => {
	const navigate = useNavigate()
	const { auth } = useAuth()
	const role = auth?.role

	const [filters, setFilters] = useState({
		invoiceNumber: '',
		invoiceStatus: '',
		paymentDateFrom: '',
		paymentDateTo: '',
	})
	const [selectedInvoice, setSelectedInvoice] = useState(null)

	const [sort, setSort] = useState({ key: 'creationDate', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getInvoices = useFetch(
		ApiUrls.INVOICE.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	useEffect(() => {
		if (selectedInvoice) {
			const refreshed =
				getInvoices.data?.collection?.find((item) => item.id === selectedInvoice.id) || null
			setSelectedInvoice(refreshed)
		}
	}, [getInvoices.data])

	const getInvoicePaymentUrl = useAxiosSubmit({
		method: 'GET',
		onSuccess: (response) => {
			navigate(response.data)
		},
	})

	const cancelInvoice = useAxiosSubmit({ method: 'PUT', onSuccess: () => getInvoices.fetch() })
	const completeInvoice = useAxiosSubmit({ method: 'PUT', onSuccess: () => getInvoices.fetch() })
	const approveRefund = useAxiosSubmit({ method: 'PUT', onSuccess: () => getInvoices.fetch() })

	const refreshInvoicesAndSelected = async (invoiceId) => {
		const response = await getInvoices.fetch()
		const refreshed = response?.data?.collection?.find((item) => item.id === invoiceId) || null
		setSelectedInvoice(refreshed)
	}

	const handleCancelInvoice = async (invoiceId) => {
		if (!invoiceId) {
			return
		}

		const isConfirmed = await confirm({
			title: t('invoice.dialog.cancel_invoice_title'),
			description: t('invoice.dialog.cancel_invoice_description'),
			confirmColor: 'error',
			confirmText: t('button.confirm'),
		})

		if (!isConfirmed) {
			return
		}

		const response = await cancelInvoice.submit({ overrideUrl: ApiUrls.INVOICE.CANCEL(invoiceId) })
		if (response) {
			await refreshInvoicesAndSelected(invoiceId)
		}
	}

	const handleCompleteInvoice = async (invoiceId) => {
		if (!invoiceId) {
			return
		}

		const isConfirmed = await confirm({
			title: t('invoice.dialog.complete_invoice_title'),
			description: t('invoice.dialog.complete_invoice_description'),
			confirmColor: 'success',
			confirmText: t('invoice.button.complete'),
		})

		if (!isConfirmed) {
			return
		}

		const response = await completeInvoice.submit({
			overrideUrl: ApiUrls.INVOICE.COMPLETE(invoiceId),
		})
		if (response) {
			await refreshInvoicesAndSelected(selectedInvoice.id)
		}
	}

	const handleApproveRefund = async (chargeId) => {
		if (!chargeId) {
			return
		}

		const isConfirmed = await confirm({
			title: t('invoice.dialog.approve_refund_title'),
			description: t('invoice.dialog.approve_refund_description'),
			confirmColor: 'success',
			confirmText: t('invoice.button.approve_refund'),
		})

		if (!isConfirmed) {
			return
		}

		const response = await approveRefund.submit({
			overrideUrl: ApiUrls.REFUND.APPROVE(chargeId),
		})
		if (response) {
			await refreshInvoicesAndSelected(selectedInvoice.id)
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('invoice.title.invoice_management')}</Typography>
				<InvoiceManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getInvoices.loading}
				/>
				<InvoiceManagementTableSection
					invoices={getInvoices.data?.collection}
					loading={getInvoices.loading}
					sort={sort}
					setSort={setSort}
					onOpenDetail={(invoice) => setSelectedInvoice(invoice)}
					onPaymentInvoiceClick={async (invoiceId) =>
						await getInvoicePaymentUrl.submit({ overrideUrl: ApiUrls.INVOICE.PAYMENT_URL(invoiceId) })
					}
					onCancelInvoiceClick={handleCancelInvoice}
					onCompleteInvoiceClick={handleCompleteInvoice}
				/>
				<GenericTablePagination
					totalPage={getInvoices.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getInvoices.loading}
				/>

				<InvoiceManagementDetailSection
					open={!!selectedInvoice}
					invoice={selectedInvoice}
					role={role}
					loadingAction={getInvoicePaymentUrl.loading || cancelInvoice.loading || approveRefund.loading}
					onClose={() => setSelectedInvoice(null)}
					onApproveRefundClick={handleApproveRefund}
					onPrintInvoiceClick={(invoiceId) => navigate(routeUrls.HOME.INVOICE_PRINT(invoiceId))}
					onPrintRefundClick={(invoiceId, chargeId) =>
						navigate(routeUrls.HOME.INVOICE_REFUND_PRINT(invoiceId, chargeId))
					}
				/>
			</Stack>
		</Paper>
	)
}

export default InvoiceManagementPage

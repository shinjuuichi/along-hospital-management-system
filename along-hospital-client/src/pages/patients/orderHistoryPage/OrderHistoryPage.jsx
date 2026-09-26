import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import OrderHistoryFilterSection from './sections/OrderHistoryFilterSection'
import OrderHistoryListSection from './sections/OrderHistoryListSection'

export default function OrderHistoryPage() {
	const { t } = useTranslation()
	const _enum = useEnum()
	const navigate = useNavigate()
	const confirm = useConfirm()

	const [filters, setFilters] = useState({})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(5)
	const [repayDialogOpen, setRepayDialogOpen] = useState(false)
	const [repayOrderId, setRepayOrderId] = useState(null)

	const {
		data: orderData,
		loading,
		fetch,
	} = useFetch(
		ApiUrls.ORDER_HISTORY.INDEX,
		{
			...filters,
			page,
			pageSize,
		},
		[filters, page, pageSize]
	)

	const orders = orderData?.collection || []
	const totalPage = orderData?.totalPage || 1

	const handleDetailClick = (id) => {
		navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.ORDER_HISTORY.DETAIL(id)))
	}

	const cancelOrderSubmit = useAxiosSubmit({
		method: 'PUT',
	})

	const repayOrderSubmit = useAxiosSubmit({
		method: 'PUT',
	})

	const handleCancelClick = async (orderId) => {
		const ok = await confirm({
			title: t('order.cancel_dialog.title'),
			description: t('order.cancel_dialog.description'),
		})
		if (!ok) return
		var response = await cancelOrderSubmit.submit({
			overrideUrl: ApiUrls.ORDER_HISTORY.CANCEL(orderId),
		})
		if (response) fetch()
	}

	const handleRepayClick = (orderId) => {
		setRepayOrderId(orderId)
		setRepayDialogOpen(true)
	}

	const handleRepaySubmit = async ({ values, closeDialog }) => {
		const { paymentType } = values
		const response = await repayOrderSubmit.submit({
			overrideUrl: ApiUrls.ORDER_HISTORY.REPAY(repayOrderId),
			overrideData: { paymentType },
		})
		const paymentUrl = response?.data?.paymentUrl || response?.data
		if (paymentUrl) {
			if (paymentType === EnumConfig.PaymentType.SePay) {
				const order = orders.find((o) => o.id === repayOrderId)
				const items =
					order?.orderDetails?.map((detail) => ({
						skuCode: detail.skuCode,
						quantity: detail.quantity,
						originPrice: detail.unitPrice,
						discountPrice: Math.max(0, detail.unitPrice - (detail.discountAmount || 0)),
						medicineSKU: {
							skuCode: detail.skuCode,
							medicineName: detail.medicineSnapshot?.medicineName,
							medicineImages: detail.medicineSnapshot?.medicineImages,
							price: detail.unitPrice,
						},
					})) || []
				setRepayOrderId(null)
				closeDialog()
				navigate(routeUrls.HOME.PAYMENT.SEPAY, {
					state: {
						paymentUrl,
						total: order?.finalPrice || 0,
						items,
					},
				})
			} else {
				closeDialog()
				navigate(paymentUrl)
			}
			return
		}
		fetch()
	}

	const repayDialogFields = [
		{
			key: 'paymentType',
			title: t('order.repay_dialog.payment_type'),
			type: 'radio',
			required: true,
			options: _enum.paymentTypeOptions.filter((pt) => pt.value !== EnumConfig.PaymentType.Cash),
			defaultValue: EnumConfig.PaymentType.PayOS,
		},
	]

	return (
		<Box sx={{ p: 3 }}>
			<Paper sx={{ p: 3 }} elevation={3}>
				<Stack spacing={2}>
					<Typography variant='h5' sx={{ fontWeight: 700 }}>
						{t('order.title.page')}
					</Typography>

					<Paper sx={{ p: 3 }} elevation={3}>
						<OrderHistoryFilterSection filters={filters} setFilters={setFilters} loading={loading} />
					</Paper>

					<OrderHistoryListSection
						orders={orders}
						loading={loading}
						onDetailClick={handleDetailClick}
						onCancelClick={handleCancelClick}
						onRepayClick={handleRepayClick}
					/>

					<GenericTablePagination
						totalPage={totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 15]}
						loading={loading}
					/>

					<GenericFormDialog
						open={repayDialogOpen}
						onClose={() => setRepayDialogOpen(false)}
						title={t('order.repay_dialog.title')}
						fields={repayDialogFields}
						initialValues={{ paymentType: EnumConfig.PaymentType.PayOS }}
						submitLabel={t('order.button.repay')}
						submitButtonColor='primary'
						maxWidth='xs'
						onSubmit={handleRepaySubmit}
					/>
				</Stack>
			</Paper>
		</Box>
	)
}

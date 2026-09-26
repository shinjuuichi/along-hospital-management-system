import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setMedicinesStore } from '@/redux/reducers/managementReducer'
import { Add } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import OrderManagementDetailDrawerSection from './sections/OrderManagementDetailDrawerSection'
import OrderManagementFilterSection from './sections/OrderManagementFilterSection'
import OrderManagementFormSection from './sections/OrderManagementFormSection'
import OrderManagementTableSection from './sections/OrderManagementTableSection'

const OrderManagementPage = () => {
	const { t } = useTranslation()

	const [sort, setSort] = useState({ key: 'orderDate', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({ orderStatus: '', orderDate: '', deliveryDate: '', isPickupAtStore: '' })
	const [selectedOrder, setSelectedOrder] = useState(null)
	const [openDetail, setOpenDetail] = useState(false)
	const [openCreate, setOpenCreate] = useState(false)

	const handleCloseDetail = () => {
		setOpenDetail(false)
		setSelectedOrder(null)
	}

	const handleViewDetail = useCallback((order) => {
		setSelectedOrder(order)
		setOpenDetail(true)
	}, [])

	const fetchParams = useMemo(
		() => ({
			page,
			pageSize,
			sort: `${sort.key} ${sort.direction}`,
			...filters,
		}),
		[page, pageSize, sort, filters]
	)

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(ApiUrls.ORDER.MANAGEMENT.INDEX, fetchParams, [fetchParams])
	const orders = data?.collection || []
	const totalPage = data?.totalPage ?? 1

	const shippingSubmit = useAxiosSubmit({ method: 'PUT' })
	const paidSubmit = useAxiosSubmit({ method: 'PUT' })
	const completeSubmit = useAxiosSubmit({ method: 'PUT' })
	const createOrderSubmit = useAxiosSubmit({
		url: ApiUrls.ORDER.MANAGEMENT.CREATE,
		method: 'POST',
	})

	const { data: medicinesRaw = [] } = useReduxStore({
		selector: (state) => state.management.medicines,
		setStore: setMedicinesStore,
	})

	const medicines = useMemo(() => {
		const meds = medicinesRaw || []
		return meds.map((m) => ({
			...m,
			label: m.name,
		}))
	}, [medicinesRaw])

	const handleCreateOrder = useCallback(
		async (payload) => {
			const ok = await createOrderSubmit.submit({ overrideData: payload })
			if (ok) {
				await refetch()
			}
			return ok
		},
		[createOrderSubmit, refetch]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5'>{t('order_management.title.order_management')}</Typography>
					<Button
						variant='contained'
						color='primary'
						startIcon={<Add />}
						onClick={() => setOpenCreate(true)}
					>
						{t('order_management.button.create_order')}
					</Button>
				</Stack>
				<OrderManagementFilterSection
					filters={filters}
					setFilters={(values) => {
						setFilters(values)
						setPage(1)
					}}
					loading={loading}
				/>
				<OrderManagementTableSection
					data={orders}
					sort={sort}
					setSort={setSort}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					totalPage={totalPage}
					loading={loading}
					onViewDetail={handleViewDetail}
				/>
			</Stack>

			<OrderManagementDetailDrawerSection
				open={openDetail}
				onClose={handleCloseDetail}
				order={selectedOrder}
				shippingSubmit={shippingSubmit}
				paidSubmit={paidSubmit}
				completeSubmit={completeSubmit}
				onSuccess={refetch}
			/>

			<OrderManagementFormSection
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				medicines={medicines}
				onSubmit={handleCreateOrder}
			/>
		</Paper>
	)
}

export default OrderManagementPage

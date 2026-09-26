import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setAllowanceTypesStore, setDeductionTypesStore } from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import PayrollManagementDetailDrawerSection from './sections/management/PayrollManagementDetailDrawerSection'
import PayrollManagementFilterSection from './sections/management/PayrollManagementFilterSection'
import PayrollManagementTableSection from './sections/management/PayrollManagementTableSection'

const PayrollManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({
		month: '',
		year: '',
		status: '',
		name: '',
	})
	const [drawerOpen, setDrawerOpen] = useState(false)
	const [selectedPayroll, setSelectedPayroll] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])

	const navigate = useNavigate()
	const { auth } = useAuth()
	const role = auth?.role
	const isAccountant = role === EnumConfig.Role.Accountant
	const isManager = role === EnumConfig.Role.Manager

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getPayrolls = useFetch(
		ApiUrls.PAYROLL.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const payrollDetail = useFetch(
		selectedPayroll?.id ? ApiUrls.PAYROLL.MANAGEMENT.DETAIL(selectedPayroll.id) : '',
		{},
		selectedPayroll?.id ? [selectedPayroll.id] : [],
		!!selectedPayroll?.id
	)

	const allowancePost = useAxiosSubmit({ url: ApiUrls.ALLOWANCE.MANAGEMENT.INDEX, method: 'POST' })
	const allowancePut = useAxiosSubmit({
		url: ApiUrls.ALLOWANCE.MANAGEMENT.DETAIL(selectedPayroll?.id),
		method: 'PUT',
	})
	const allowanceDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const deductionPost = useAxiosSubmit({ url: ApiUrls.DEDUCTION.MANAGEMENT.INDEX, method: 'POST' })
	const deductionPut = useAxiosSubmit({
		url: ApiUrls.DEDUCTION.MANAGEMENT.DETAIL(selectedPayroll?.id),
		method: 'PUT',
	})
	const deductionDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const pendingPayrolls = useAxiosSubmit({
		url: ApiUrls.PAYROLL.MANAGEMENT.PENDING,
		method: 'PUT',
	})
	const approvePayrolls = useAxiosSubmit({
		url: ApiUrls.PAYROLL.MANAGEMENT.APPROVE,
		method: 'PUT',
	})

	const allowanceTypes = useReduxStore({
		selector: (state) => state.management.allowanceTypes,
		setStore: setAllowanceTypesStore,
	})

	const deductionTypes = useReduxStore({
		selector: (state) => state.management.deductionTypes,
		setStore: setDeductionTypesStore,
	})

	const payrolls = getPayrolls.data?.collection
	const loading = getPayrolls.loading
	const totalPage = getPayrolls.data?.totalPage
	const totalElement = getPayrolls.data?.totalCount

	const handleOpenDetail = (row) => {
		setSelectedPayroll(row)
		setDrawerOpen(true)
	}

	const handleCloseDetail = () => {
		setDrawerOpen(false)
		setSelectedPayroll(null)
	}

	const handlePrint = (row) => {
		if (!row) return
		if (!isAccountant) return
		navigate(routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.PAYROLL.PRINT(row.id)), {
			state: { payroll: row },
		})
	}

	const refreshPayroll = () => {
		payrollDetail.fetch()
		getPayrolls.fetch()
	}

	const handleCreateAllowance = async (values) => {
		if (!selectedPayroll?.id) return undefined
		const payload = { ...values, payrollId: selectedPayroll.id }
		const res = await allowancePost.submit({ overrideData: payload })
		if (res) refreshPayroll()
		return res
	}

	const handleUpdateAllowance = async (id, values) => {
		if (!id) return undefined
		const res = await allowancePut.submit({
			overrideUrl: ApiUrls.ALLOWANCE.MANAGEMENT.DETAIL(id),
			overrideData: values,
		})
		if (res) refreshPayroll()
		return res
	}

	const handleDeleteAllowance = async (id) => {
		const ok = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('payroll.dialog.delete_allowance_title'),
			description: t('payroll.dialog.delete_allowance_confirm'),
		})
		if (!ok) return undefined
		const res = await allowanceDelete.submit({
			overrideUrl: ApiUrls.ALLOWANCE.MANAGEMENT.DETAIL(id),
		})
		if (res) refreshPayroll()
		return res
	}

	const handleCreateDeduction = async (values) => {
		if (!selectedPayroll?.id) return undefined
		const payload = { ...values, payrollId: selectedPayroll.id }
		const res = await deductionPost.submit({ overrideData: payload })
		if (res) refreshPayroll()
		return res
	}

	const handleUpdateDeduction = async (id, values) => {
		if (!id) return undefined
		const res = await deductionPut.submit({
			overrideUrl: ApiUrls.DEDUCTION.MANAGEMENT.DETAIL(id),
			overrideData: values,
		})
		if (res) refreshPayroll()
		return res
	}

	const handleDeleteDeduction = async (id) => {
		const ok = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('payroll.dialog.delete_deduction_title'),
			description: t('payroll.dialog.delete_deduction_confirm'),
		})
		if (!ok) return undefined
		const res = await deductionDelete.submit({
			overrideUrl: ApiUrls.DEDUCTION.MANAGEMENT.DETAIL(id),
		})
		if (res) refreshPayroll()
		return res
	}

	const handlePending = async () => {
		if (!selectedIds.length) return

		const isConfirmed = await confirm({
			confirmText: t('payroll.button.pending'),
			confirmColor: 'warning',
			title: t('payroll.dialog.pending_title'),
			description: t('payroll.dialog.pending_description'),
		})
		if (!isConfirmed) return

		const res = await pendingPayrolls.submit({
			overrideParam: { ids: selectedIds },
		})
		if (res) {
			setSelectedIds([])
			getPayrolls.fetch()
		}
	}

	const handleApprove = async () => {
		if (!selectedIds.length) return

		const isConfirmed = await confirm({
			confirmText: t('payroll.button.approve'),
			confirmColor: 'success',
			title: t('payroll.dialog.approve_title'),
			description: t('payroll.dialog.approve_description'),
		})
		if (!isConfirmed) return

		const res = await approvePayrolls.submit({
			overrideParam: { ids: selectedIds },
		})
		if (res) {
			setSelectedIds([])
			getPayrolls.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('payroll.title.payroll_management')}</Typography>
				<PayrollManagementFilterSection
					filters={filters}
					setFilters={(nextFilters) => {
						setPage(1)
						setFilters(nextFilters)
					}}
					loading={getPayrolls.loading}
				/>
				<PayrollManagementTableSection
					payrolls={payrolls}
					loading={loading}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					sort={sort}
					setSort={setSort}
					onOpenDetail={handleOpenDetail}
					onPending={handlePending}
					onApprove={handleApprove}
					pendingLoading={pendingPayrolls.loading}
					approveLoading={approvePayrolls.loading}
					isAccountant={isAccountant}
					isManager={isManager}
				/>
				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					totalElement={totalElement}
				/>
			</Stack>
			<PayrollManagementDetailDrawerSection
				open={drawerOpen}
				onClose={handleCloseDetail}
				payroll={payrollDetail.data?.data || payrollDetail.data || selectedPayroll}
				loading={payrollDetail.loading}
				canPrint={isAccountant}
				onPrint={handlePrint}
				allowanceTypes={allowanceTypes.data}
				deductionTypes={deductionTypes.data}
				onCreateAllowance={handleCreateAllowance}
				onUpdateAllowance={handleUpdateAllowance}
				onDeleteAllowance={handleDeleteAllowance}
				onCreateDeduction={handleCreateDeduction}
				onUpdateDeduction={handleUpdateDeduction}
				onDeleteDeduction={handleDeleteDeduction}
			/>
		</Paper>
	)
}

export default PayrollManagementPage

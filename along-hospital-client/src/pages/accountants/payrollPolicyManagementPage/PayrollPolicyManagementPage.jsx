import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setAllowanceTypesStore,
	setDeductionTypesStore,
	setStaffsStore,
} from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import PayrollPolicyManagementFormSection from './sections/PayrollPolicyManagementFormSection'
import PayrollPolicyManagementTableSection from './sections/PayrollPolicyManagementTableSection'

const PayrollPolicyManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()

	const getPayrollPolicies = useFetch(
		ApiUrls.PAYROLL_POLICY.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const allowanceTypes = useReduxStore({
		selector: (state) => state.management.allowanceTypes,
		setStore: setAllowanceTypesStore,
	})

	const deductionTypes = useReduxStore({
		selector: (state) => state.management.deductionTypes,
		setStore: setDeductionTypesStore,
	})

	const staffs = useReduxStore({
		selector: (state) => state.management.staffs,
		setStore: setStaffsStore,
	})

	const payrollPolicyPost = useAxiosSubmit({
		url: ApiUrls.PAYROLL_POLICY.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const payrollPolicyPut = useAxiosSubmit({
		url: ApiUrls.PAYROLL_POLICY.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const respond = await payrollPolicyPost.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getPayrollPolicies.fetch()
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await payrollPolicyPut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getPayrollPolicies.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('payroll_policy.title.payroll_policy_management')}</Typography>
				<PayrollPolicyManagementTableSection
					payrollPolicies={getPayrollPolicies.data?.collection}
					loading={getPayrollPolicies.loading}
					sort={sort}
					setSort={setSort}
					setOpenCreate={setOpenCreate}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
					staffs={staffs.data}
				/>
				<GenericTablePagination
					totalPage={getPayrollPolicies.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getPayrollPolicies.loading}
				/>
			</Stack>
			<PayrollPolicyManagementFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onCreate={handleCreate}
				onUpdate={handleUpdate}
				allowanceTypes={allowanceTypes.data}
				deductionTypes={deductionTypes.data}
				staffs={staffs.data}
			/>
		</Paper>
	)
}

export default PayrollPolicyManagementPage

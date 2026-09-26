import ManageStaffContractBasePage from '@/components/basePages/manageStaffContractBasePage/ManageStaffContractBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { useEffect, useMemo, useState } from 'react'

const ManagerStaffContractManagementPage = () => {
	const [filters, setFilters] = useState({})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'desc'}`, [sort])
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(5)

	const { t } = useTranslation()
	const getStaffContracts = useFetch(
		ApiUrls.STAFF_CONTRACT.MANAGEMENT.INDEX,
		{
			sort: sortParam,
			...filters,
			page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)

	const refreshContracts = () => getStaffContracts.fetch()

	const signStaffContract = useAxiosSubmit({
		method: 'PUT',
		onSuccess: refreshContracts,
	})

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	return (
		<ManageStaffContractBasePage
			headerTitle={t('staff_contract.title.staff_contract_management')}
			staffContracts={getStaffContracts.data?.collection}
			totalPage={getStaffContracts.data?.totalPage}
			loading={getStaffContracts.loading}
			filters={filters}
			setFilters={setFilters}
			page={page}
			setPage={setPage}
			pageSize={pageSize}
			setPageSize={setPageSize}
			sort={sort}
			setSort={setSort}
			onSignSubmit={async ({ values, closeDialog, contractId }) => {
				await signStaffContract.submit({
					overrideUrl: ApiUrls.STAFF_CONTRACT.MANAGEMENT.SIGN(contractId),
					overrideData: values,
				})
				closeDialog()
			}}
			role={EnumConfig.Role.Manager}
		/>
	)
}

export default ManagerStaffContractManagementPage

import ManageStaffContractBasePage from '@/components/basePages/manageStaffContractBasePage/ManageStaffContractBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setRegionalWagesStore, setStaffsStore } from '@/redux/reducers/managementReducer'
import { useEffect, useMemo, useState } from 'react'

const HRStaffContractManagementPage = () => {
	const [filters, setFilters] = useState({})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'desc'}`, [sort])
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(5)

	const { t } = useTranslation()
	const confirm = useConfirm()

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

	const getStaffsStore = useReduxStore({
		selector: (state) => state.management.staffs,
		setStore: setStaffsStore,
	})
	const getRegionalWagesStore = useReduxStore({
		selector: (state) => state.management.regionalWages,
		setStore: setRegionalWagesStore,
	})

	const refreshContracts = () => getStaffContracts.fetch()

	const createStaffContract = useAxiosSubmit({
		url: ApiUrls.STAFF_CONTRACT.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: refreshContracts,
	})

	const updateStaffContract = useAxiosSubmit({
		method: 'PUT',
		onSuccess: refreshContracts,
	})

	const deleteStaffContract = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: refreshContracts,
	})

	const terminateStaffContract = useAxiosSubmit({
		method: 'PUT',
		onSuccess: refreshContracts,
	})

	const renewStaffContract = useAxiosSubmit({
		method: 'PUT',
		onSuccess: refreshContracts,
	})

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('staff_contract.title.delete_title'),
			description: t('staff_contract.title.delete_description', { code: row.contractCode }),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			deleteStaffContract.submit({
				overrideUrl: ApiUrls.STAFF_CONTRACT.MANAGEMENT.DETAIL(row.id),
			})
		}
	}

	const handleTerminate = async (row) => {
		const isConfirm = await confirm({
			title: t('staff_contract.title.terminate_title'),
			description: t('staff_contract.title.terminate_description', { code: row.contractCode }),
			confirmText: t('staff_contract.button.terminate'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			terminateStaffContract.submit({
				overrideUrl: ApiUrls.STAFF_CONTRACT.MANAGEMENT.TERMINATE(row.id),
			})
		}
	}

	return (
		<ManageStaffContractBasePage
			headerTitle={t('staff_contract.title.hr_contract_management')}
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
			onCreateSubmit={async ({ values, closeDialog }) => {
				const response = await createStaffContract.submit({ overrideData: values })
				if (response) closeDialog()
			}}
			onUpdateSubmit={async ({ values, closeDialog }) => {
				const id = values?.id
				const response = await updateStaffContract.submit({
					overrideUrl: ApiUrls.STAFF_CONTRACT.MANAGEMENT.DETAIL(id),
					overrideData: values,
				})
				if (response) closeDialog()
			}}
			onRenewSubmit={async ({ values, closeDialog }) => {
				const id = values?.id
				const response = await renewStaffContract.submit({
					overrideUrl: ApiUrls.STAFF_CONTRACT.MANAGEMENT.RENEW(id),
					overrideData: values,
				})
				if (response) closeDialog()
			}}
			staffs={getStaffsStore.data}
			regionalWages={getRegionalWagesStore.data}
			role={EnumConfig.Role.HR}
			onDeleteClick={handleDelete}
			onTerminateClick={handleTerminate}
		/>
	)
}

export default HRStaffContractManagementPage

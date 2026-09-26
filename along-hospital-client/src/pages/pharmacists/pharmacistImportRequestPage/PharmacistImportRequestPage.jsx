import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { setMedicineSkusStore } from '@/redux/reducers/managementReducer'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { useCallback, useMemo, useState } from 'react'
import ImportRequestManagementBasePage from '@/components/basePages/manageImportRequestBasePage/ImportRequestManagementBasePage'

const PharmacistImportRequestPage = () => {
	const { t } = useTranslation()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [filters, setFilters] = useState({})

	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'desc'}`, [sort])

	const fetchParams = useMemo(
		() => ({
			page,
			pageSize,
			sort: sortParam,
			...(filters.status && { status: filters.status }),
		}),
		[page, pageSize, sortParam, filters]
	)

	const {
		loading: importRequestLoading,
		data,
		fetch: refetch,
	} = useFetch(ApiUrls.IMPORT_REQUEST.MANAGEMENT.INDEX, fetchParams, [
		page,
		pageSize,
		sortParam,
		filters,
	])

	const { data: medicineSkus = [] } = useReduxStore({
		selector: (state) => state.management.medicineSkus,
		setStore: setMedicineSkusStore,
	})

	const medicineSkuOptions = useMemo(() => {
		const skus = medicineSkus?.collection || medicineSkus?.items || medicineSkus || []
		return skus.map((sku) => ({
			value: sku.skuCode,
			searchKey: `${sku.skuCode ?? ''} ${sku.medicineName ?? sku.name ?? sku.medicine?.name ?? ''}`.trim(),
			label: {
				skuCode: sku.skuCode ?? '',
				medicineName: sku.medicineName ?? sku.name ?? sku.medicine?.name ?? null,
			},
		}))
	}, [medicineSkus])

	const totalPage = data?.totalPage ?? 1

	const approveRequest = useAxiosSubmit({
		url: ApiUrls.IMPORT_REQUEST.MANAGEMENT.APPROVE(''),
		method: 'PUT',
	})
	const rejectRequest = useAxiosSubmit({
		url: ApiUrls.IMPORT_REQUEST.MANAGEMENT.REJECT(''),
		method: 'PUT',
	})
	const createRequest = useAxiosSubmit({
		url: ApiUrls.IMPORT_REQUEST.MANAGEMENT.INDEX,
		method: 'POST',
	})
	const updateRequest = useAxiosSubmit({
		url: ApiUrls.IMPORT_REQUEST.MANAGEMENT.DETAIL(''),
		method: 'PUT',
	})

	const handleApprove = useCallback(
		async (id) => {
			const ok = await approveRequest.submit({
				overrideUrl: ApiUrls.IMPORT_REQUEST.MANAGEMENT.APPROVE(id),
			})
			if (ok) await refetch()
		},
		[approveRequest, refetch]
	)

	const handleReject = useCallback(
		async (id) => {
			const ok = await rejectRequest.submit({
				overrideUrl: ApiUrls.IMPORT_REQUEST.MANAGEMENT.REJECT(id),
			})
			if (ok) await refetch()
		},
		[rejectRequest, refetch]
	)

	const handleCreateRequest = useCallback(
		async (data) => {
			const ok = await createRequest.submit({ overrideData: data })
			if (ok) await refetch()
			return ok
		},
		[createRequest, refetch]
	)

	const handleUpdateRequest = useCallback(
		async (id, data) => {
			const ok = await updateRequest.submit({
				overrideUrl: ApiUrls.IMPORT_REQUEST.MANAGEMENT.DETAIL(id),
				overrideData: data,
			})
			if (ok) await refetch()
			return ok
		},
		[updateRequest, refetch]
	)

	return (
		<ImportRequestManagementBasePage
			headerTitle={t('import_request.title.main')}
			importRequests={data?.collection || data?.items || []}
			totalPage={totalPage}
			filters={filters}
			setFilters={setFilters}
			page={page}
			setPage={setPage}
			pageSize={pageSize}
			setPageSize={setPageSize}
			sort={sort}
			setSort={setSort}
			medicineSkuOptions={medicineSkuOptions}
			loading={importRequestLoading}
			onSuccess={refetch}
			onCreateRequest={handleCreateRequest}
			onUpdateRequest={handleUpdateRequest}
			onApprove={handleApprove}
			onReject={handleReject}
			role={EnumConfig.Role.Pharmacist}
		/>
	)
}

export default PharmacistImportRequestPage

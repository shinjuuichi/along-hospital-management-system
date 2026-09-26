import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import MedicineInfoSection from '@/pages/pharmacists/medicineDetailPage/sections/MedicineInfoSection'
import MedicineSkuSection from '@/pages/pharmacists/medicineDetailPage/sections/MedicineSkuSection'
import {
	setMedicineCategoriesStore,
	setMedicineUnitsStore,
} from '@/redux/reducers/managementReducer'
import { Paper } from '@mui/material'
import { useMemo, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

const MedicineDetailPage = () => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const navigate = useNavigate()
	const { id } = useParams()
	const medicineId = id ? Number(id) : null
	const isValidMedicineId = Number.isFinite(medicineId) && medicineId > 0

	const [selectedSku, setSelectedSku] = useState(null)
	const [openEditSku, setOpenEditSku] = useState(false)
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [skuFilters, setSkuFilters] = useState({ skuCode: '', isActive: '', optionValueId: '' })
	const [skuSort, setSkuSort] = useState({ key: 'id', direction: 'desc' })

	const fetchMedicine = useFetch(
		isValidMedicineId ? ApiUrls.MEDICINE.MANAGEMENT.GET_BY_ID(medicineId) : null,
		{},
		[medicineId]
	)
	const skuFetchParams = useMemo(() => {
		return {
			medicineId,
			page,
			pageSize,
			skuCode: skuFilters.skuCode,
			isActive: skuFilters.isActive,
			optionValueId: skuFilters.optionValueId,
			sort: `${skuSort.key} ${skuSort.direction}`,
		}
	}, [medicineId, page, pageSize, skuFilters, skuSort])
	const fetchSkus = useFetch(ApiUrls.MEDICINE_SKU.MANAGEMENT.INDEX, skuFetchParams, [skuFetchParams])

	const { data: categories = [] } = useReduxStore({
		selector: (state) => state.management.medicineCategories,
		setStore: setMedicineCategoriesStore,
	})

	const { data: unitsRaw = [] } = useReduxStore({
		selector: (state) => state.management.medicineUnits,
		setStore: setMedicineUnitsStore,
	})

	const skus = fetchSkus.data?.collection || []
	const totalPage = fetchSkus.data?.totalPage || 1

	const unitOptions = useMemo(() => {
		return (unitsRaw || []).map((u) => ({
			label: u.name,
			value: u.id,
		}))
	}, [unitsRaw])

	const categoryOptions = useMemo(() => {
		return (categories || []).map((c) => ({
			label: c.name,
			value: c.id,
		}))
	}, [categories])

	const optionOptions = useMemo(() => {
		return (
			fetchMedicine.data?.medicineUnit?.options
				?.filter((o) => o.isActive)
				?.map((o) => o.option) || []
		)
	}, [fetchMedicine.data])



	const createSku = useAxiosSubmit({
		url: ApiUrls.MEDICINE_SKU.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenEditSku(false)
			setSelectedSku(null)
			await fetchSkus.fetch()
		},
	})

	const updateSku = useAxiosSubmit({
		url: selectedSku ? ApiUrls.MEDICINE_SKU.MANAGEMENT.DETAIL(selectedSku.id) : '',
		method: 'PUT',
		onSuccess: async () => {
			setOpenEditSku(false)
			setSelectedSku(null)
			await fetchSkus.fetch()
		},
	})

	const stats = useMemo(() => {
		const total = skus.length
		const active = skus.filter((s) => s.isActive !== false).length
		return { total, active }
	}, [skus])

	return (
		<Paper sx={{ p: { xs: 2, md: 3 }, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<MedicineInfoSection
				t={t}
				selectedMedicine={fetchMedicine.data}
				stats={stats}
				medicineStatusOptions={_enum.medicineStatusOptions}
				onBack={() =>
					navigate(routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_MANAGEMENT.INDEX))
				}
			/>

			<MedicineSkuSection
				t={t}
				skus={skus}
				loading={fetchSkus.loading}
				sort={skuSort}
				setSort={(next) => {
					setSkuSort(next)
					setPage(1)
				}}
				filters={skuFilters}
				setFilters={setSkuFilters}
				onResetFilters={() => setSkuFilters({ skuCode: '', isActive: '', optionValueId: '' })}
				optionOptions={optionOptions}
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				openEditSku={openEditSku}
				selectedSku={selectedSku}
				createSku={createSku}
				updateSku={updateSku}
				medicineId={medicineId}
				setOpenEditSku={setOpenEditSku}
				setSelectedSku={setSelectedSku}
			/>

		</Paper>
	)
}

export default MedicineDetailPage

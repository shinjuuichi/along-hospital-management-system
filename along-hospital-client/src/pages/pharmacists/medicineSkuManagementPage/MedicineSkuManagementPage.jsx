import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'

import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'

import MedicineSkuDialogsSection from '@/pages/pharmacists/medicineSkuManagementPage/sections/MedicineSkuDialogsSection'
import MedicineSkuManagementFilterBarSection from '@/pages/pharmacists/medicineSkuManagementPage/sections/MedicineSkuManagementFilterBarSection'
import MedicineSkuTableSection from '@/pages/pharmacists/medicineSkuManagementPage/sections/MedicineSkuTableSection'

import { setMedicinesStore, setOptionsStore } from '@/redux/reducers/managementReducer'

const MedicineSkuManagementPage = () => {
	const { t } = useTranslation()
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({
		medicineId: '',
		skuCode: '',
		isActive: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })

	const [skus, setSkus] = useState([])
	const [totalPage, setTotalPage] = useState(1)

	const [selected, setSelected] = useState(null)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const fetchParams = useMemo(
		() => ({ page, pageSize, sort: `${sort.key} ${sort.direction}`, ...filters }),
		[page, pageSize, sort, filters],
	)

	const getSkus = useFetch(ApiUrls.MEDICINE_SKU.MANAGEMENT.INDEX, fetchParams, [fetchParams])

	const { data: medicines = [] } = useReduxStore({
		selector: (state) => state.management.medicines,
		setStore: setMedicinesStore,
	})

	const medicineOptions = useMemo(
		() =>
			(medicines || []).map((m) => ({
				label: `${m.name} - ${m.brand}`,
				value: m.id,
				medicineUnit: m.medicineUnit,
			})),
		[medicines]
	)

	const { data: options = [] } = useReduxStore({
		selector: (state) => state.management.options,
		setStore: setOptionsStore,
	})

	useEffect(() => {
		if (!getSkus.data) return
		setSkus(getSkus.data.collection || [])
		setTotalPage(getSkus.data.totalPage || 1)
	}, [getSkus.data])

	const createSku = useAxiosSubmit({
		url: ApiUrls.MEDICINE_SKU.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			await getSkus.fetch()
		},
	})

	const updateSku = useAxiosSubmit({
		url: ApiUrls.MEDICINE_SKU.MANAGEMENT.DETAIL(selected?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdate(false)
			setSelected(null)
			await getSkus.fetch()
		},
	})

	const statusOptions = [
		{ value: '', label: t('text.all') },
		{ value: 'true', label: t('medicine_sku.text.active') },
		{ value: 'false', label: t('medicine_sku.text.inactive') },
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('medicine_sku.title.management')}</Typography>

				<MedicineSkuManagementFilterBarSection
					filters={filters}
					medicineOptions={medicineOptions}
					statusOptions={statusOptions}
					loading={getSkus.loading}
					onFilterClick={(newFilters) => {
						setFilters(newFilters)
						setPage(1)
					}}
					onResetFilterClick={(resetFn) => {
						const resetFilters = {
							medicineId: '',
							skuCode: '',
							isActive: '',
						}
						setFilters(resetFilters)
						resetFn(resetFilters)
						setPage(1)
					}}
				/>

				<Stack direction='row' justifyContent='flex-end'>
					<Button
						variant='contained'
						color='primary'
						onClick={() => setOpenCreate(true)}
						sx={{ minWidth: 140 }}
					>
						{t('button.create')}
					</Button>
				</Stack>

				<Stack spacing={2}>
					<MedicineSkuTableSection
						skus={skus}
						loading={getSkus.loading}
						t={t}
						sort={sort}
						setSort={setSort}
						onOpenUpdate={(row) => {
							setSelected(row)
							setOpenUpdate(true)
						}}
					/>

					<Stack px={2} justifyContent='center'>
						<GenericTablePagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							pageSize={pageSize}
							setPageSize={setPageSize}
							pageSizeOptions={[5, 10, 20]}
							loading={getSkus.loading}
						/>
					</Stack>
				</Stack>
			</Stack>

			<MedicineSkuDialogsSection
				t={t}
				openCreate={openCreate}
				onCloseCreate={() => setOpenCreate(false)}
				openUpdate={openUpdate}
				onCloseUpdate={() => {
					setOpenUpdate(false)
					setSelected(null)
				}}
				medicineOptions={medicineOptions}
				options={options}
				selected={selected}
				createSku={createSku}
				updateSku={updateSku}
			/>
		</Paper>
	)
}

export default MedicineSkuManagementPage

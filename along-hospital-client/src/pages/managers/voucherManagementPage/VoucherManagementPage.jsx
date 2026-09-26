import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setMedicinesStore } from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import VoucherManagementFilterSection from './sections/VoucherManagementFilterSection'
import VoucherManagementTableSection from './sections/VoucherManagementTableSection'

const normalizeMedicineIds = (medicineIds) => {
	if (!Array.isArray(medicineIds)) {
		return []
	}

	return medicineIds
		.map((item) => item?.medicineId ?? item?.id ?? item)
		.filter((id) => id !== null && id !== undefined && id !== '')
}

const buildVoucherFormData = (rawValues) => {
	const formData = new FormData()
	if (!rawValues) return formData

	const processedValues = { ...rawValues }
	const isMedicineVoucher = processedValues.voucherType === EnumConfig.VoucherType.Medicine
	const isUnsupportedMedicineFixedAmount =
		isMedicineVoucher && processedValues.discountType === EnumConfig.VoucherDiscountType.FixedAmount

	if (isUnsupportedMedicineFixedAmount) {
		return null
	}

	if (Array.isArray(processedValues.medicineIds)) {
		processedValues.medicineIds = normalizeMedicineIds(processedValues.medicineIds)
	}

	if (isMedicineVoucher) {
		delete processedValues.quantity
		delete processedValues.image
		delete processedValues.maxDiscount
		delete processedValues.minPurchaseAmount
	} else {
		delete processedValues.medicineIds
	}

	if (processedValues.discountType === EnumConfig.VoucherDiscountType.FixedAmount) {
		delete processedValues.maxDiscount
	}

	Object.entries(processedValues).forEach(([key, value]) => {
		if (value === null || value === undefined) return
		if (key === 'medicineIds' && Array.isArray(value)) {
			value.forEach((id) => formData.append('medicineIds', id))
			return
		}
		formData.append(key, value)
	})

	return formData
}

const VoucherManagementPage = () => {
	const [filters, setFilters] = useState('')
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const { t } = useTranslation()

	const voucherParams = { ...filters, page, pageSize }
	const voucherDeps = [filters, page, pageSize]
	const getVouchers = useFetch(ApiUrls.VOUCHER.MANAGEMENT.INDEX, voucherParams, voucherDeps)
	const medicineStore = useReduxStore({
		selector: (state) => state.management.medicines,
		setStore: setMedicinesStore,
	})

	const voucherPost = useAxiosSubmit({
		url: ApiUrls.VOUCHER.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const voucherPut = useAxiosSubmit({
		method: 'PUT',
	})

	const voucherDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const medicineOptions = useMemo(
		() =>
			medicineStore.data
				?.filter((medicine) => medicine.isPublic)
				.map((medicine) => ({
					searchKey: medicine.name,
					value: medicine.id,
					label: `${medicine.name}${medicine.brand ? ` - ${medicine.brand}` : ''}`,
				})) ?? [],
		[medicineStore.data]
	)

	const handleCreateVoucher = async ({ values, closeDialog }) => {
		const formData = buildVoucherFormData(values)
		if (!formData) return

		const response = await voucherPost.submit({
			overrideData: formData,
		})

		if (response) {
			closeDialog()
			getVouchers.fetch()
		}
	}

	const handleUpdateVoucher = async ({ rowId, values, closeDialog }) => {
		const formData = buildVoucherFormData(values)
		if (!formData) return

		const response = await voucherPut.submit({
			overrideUrl: ApiUrls.VOUCHER.MANAGEMENT.DETAIL(rowId),
			overrideData: formData,
		})

		if (response) {
			closeDialog()
			getVouchers.fetch()
		}
	}

	const handleDeleteVoucher = async (rowId) => {
		const response = await voucherDelete.submit({
			overrideUrl: ApiUrls.VOUCHER.MANAGEMENT.DETAIL(rowId),
		})

		if (response) {
			getVouchers.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('voucher.title.voucher_management')}</Typography>
				<VoucherManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getVouchers.loading}
				/>
				<VoucherManagementTableSection
					vouchers={getVouchers.data?.collection}
					loading={getVouchers.loading}
					medicineOptions={medicineOptions}
					onCreateSubmit={handleCreateVoucher}
					onUpdateSubmit={handleUpdateVoucher}
					onDelete={handleDeleteVoucher}
				/>
				<GenericTablePagination
					totalPage={getVouchers.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getVouchers.loading}
				/>
			</Stack>
		</Paper>
	)
}

export default VoucherManagementPage

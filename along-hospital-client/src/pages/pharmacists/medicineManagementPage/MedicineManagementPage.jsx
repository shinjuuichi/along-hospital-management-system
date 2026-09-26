import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultMedicineStatusStyle } from '@/configs/defaultStylesConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import MedicineFilterBar from '@/pages/pharmacists/medicineManagementPage/sections/MedicineManagementFilterBarSection'
import MedicineImagesPreview from '@/pages/pharmacists/medicineManagementPage/sections/MedicineManagementImagesPreviewSection'
import {
	setMedicineCategoriesStore,
	setMedicineUnitsStore,
} from '@/redux/reducers/managementReducer'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const MedicineManagementPage = () => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const navigate = useNavigate()

	const [filters, setFilters] = useState({
		name: '',
		medicineCategoryId: '',
		medicineUnitId: '',
		status: '',
		isPublic: '',
	})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [medicines, setMedicines] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openEditDialog, setOpenEditDialog] = useState(false)
	const [selectedMedicine, setSelectedMedicine] = useState(null)

	const fetchParams = useMemo(
		() => ({
			page,
			pageSize,
			sort: `${sort.key} ${sort.direction}`,
			...filters,
		}),
		[page, pageSize, sort, filters]
	)

	const getAllMedicines = useFetch(ApiUrls.MEDICINE.MANAGEMENT.INDEX, fetchParams, [fetchParams])

	const { data: categoryOptions = [] } = useReduxStore({
		selector: (state) => state.management.medicineCategories,
		setStore: setMedicineCategoriesStore,
		dataToGet: (storeData) => (storeData || []).map((c) => ({ label: c.name, value: c.id })),
	})

	const { data: units = [] } = useReduxStore({
		selector: (state) => state.management.medicineUnits,
		setStore: setMedicineUnitsStore,
		dataToGet: (storeData) => (storeData || []).map((u) => ({ label: u.name, value: u.id })),
	})

	useEffect(() => {
		if (getAllMedicines.data) {
			const data = getAllMedicines.data
			setMedicines(data.collection)
			setTotalPage(data.totalPage)
		}
	}, [getAllMedicines.data])

	const createMedicine = useAxiosSubmit({
		url: ApiUrls.MEDICINE.MANAGEMENT.CREATE,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await getAllMedicines.fetch()
		},
	})

	const updateMedicine = useAxiosSubmit({
		url: ApiUrls.MEDICINE.MANAGEMENT.UPDATE(selectedMedicine?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenEditDialog(false)
			setSelectedMedicine(null)
			await getAllMedicines.fetch()
		},
	})

	const createFormFields = [
		{ key: 'name', title: t('medicine.field.name') },
		{ key: 'brand', title: t('medicine.field.brand') },
		{
			key: 'medicineUnitId',
			title: t('medicine.field.unit'),
			type: 'select-dialog',
			options: units,
		},
		{
			key: 'medicineCategoryId',
			title: t('medicine.field.medicine_category.name'),
			type: 'select-dialog',
			options: categoryOptions,
		},
		{ key: 'images', title: t('medicine.field.images'), type: 'image', multiple: 5 },
		{
			key: 'isPublic',
			title: t('medicine.filter.is_public'),
			type: 'checkbox',
		},
	]

	const editFormFields = [
		...createFormFields,
		{
			key: 'status',
			title: t('medicine.field.status'),
			type: 'select',
			options: _enum.medicineStatusOptions,
		},
	]

	const getMedicineInitialValues = (medicine) =>
		medicine
			? {
					name: medicine.name || '',
					brand: medicine.brand || '',
					images: medicine.images || [],
					medicineUnitId: medicine.medicineUnit?.id,
					medicineCategoryId: medicine.medicineCategory?.id,
					status: medicine.status || '',
					isPublic: medicine.isPublic ?? false,
				}
			: {}

	const renderStatus = (status) => {
		const color = defaultMedicineStatusStyle(status)
		const label = renderEmptyFallback(getEnumLabelByValue(_enum.medicineStatusOptions, status) || status)
		return <Chip size='small' color={color === 'default' ? undefined : color} label={label} />
	}

	const tableFields = [
		{ key: 'name', title: t('medicine.field.name'), width: 15, sortable: true },
		{ key: 'brand', title: t('medicine.field.brand'), width: 12, sortable: true },
		{
			key: 'status',
			title: t('medicine.field.status'),
			width: 10,
			sortable: true,
			render: (_, row) => renderStatus(row.status),
		},
		{
			key: 'isPublic',
			title: t('medicine.filter.is_public'),
			width: 10,
			sortable: true,
			render: (_, row) => (row.isPublic ? t('medicine.text.true') : t('medicine.text.false')),
		},
		{
			key: 'medicineUnit.name',
			title: t('medicine.field.unit'),
			width: 10,
			sortable: true,
		},
		{
			key: 'medicineCategory.name',
			title: t('medicine.field.medicine_category.name'),
			width: 10,
			sortable: true,
		},
		{
			key: 'images',
			title: t('medicine.field.images'),
			width: 10,
			render: (_, row) => <MedicineImagesPreview images={row.images || []} rowId={row.id} />,
		},
		{
			key: 'actions',
			title: t('medicine.field.action'),
			width: 10,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedMedicine(row)
								setOpenEditDialog(true)
							},
						},
						{
							title: t('button.detail'),
							onClick: () =>
								navigate(
									routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_MANAGEMENT.DETAIL(row.id))
								),
						},
					]}
				/>
			),
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('medicine.title.medicine_management')}</Typography>

				<MedicineFilterBar
					filters={filters}
					categories={categoryOptions}
					units={units}
					loading={getAllMedicines.loading}
					onFilterClick={(newValues) => {
						setFilters(newValues)
						setPage(1)
					}}
					onResetFilterClick={(resetFn) => {
						const resetFilters = {
							name: '',
							medicineCategoryId: '',
							medicineUnitId: '',
							status: '',
							isPublic: '',
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
						onClick={() => setOpenCreateDialog(true)}
						sx={{ minWidth: 120 }}
					>
						{t('button.create')}
					</Button>
				</Stack>

				<Stack spacing={2}>
					<GenericTable
						data={medicines}
						fields={tableFields}
						rowKey='id'
						sort={sort}
						setSort={setSort}
						loading={getAllMedicines.loading}
					/>
					<Stack justifyContent='center' px={2}>
						<GenericTablePagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							pageSize={pageSize}
							setPageSize={setPageSize}
							pageSizeOptions={[5, 10, 20]}
							loading={getAllMedicines.loading}
						/>
					</Stack>
				</Stack>
			</Stack>

			<GenericFormDialog
				title={t('medicine.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				fields={createFormFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const payload = {
						...values,
						isPublic: values.isPublic ?? false,
					}
					const response = await createMedicine.submit({ overrideData: payload })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('medicine.dialog.update_title')}
				open={openEditDialog}
				onClose={() => {
					setOpenEditDialog(false)
					setSelectedMedicine(null)
				}}
				fields={editFormFields}
				initialValues={getMedicineInitialValues(selectedMedicine)}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const payload = {
						...values,
						isPublic: values.isPublic ?? false,
					}
					const response = await updateMedicine.submit({ overrideData: payload })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}
export default MedicineManagementPage

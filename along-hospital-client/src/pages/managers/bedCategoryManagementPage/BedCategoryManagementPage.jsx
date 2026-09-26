import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBedCategoriesStore, setMedicalServicesStore } from '@/redux/reducers/managementReducer'
import { Delete, Edit } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const BedCategoryManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [categories, setCategories] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [selectedRows, setSelectedRows] = useState([])
	const [selectedCategoryId, setSelectedCategoryId] = useState(null)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })

	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const { fetch: refreshReduxCategories } = useReduxStore({
		selector: (state) => state.management.bedCategories,
		setStore: setBedCategoriesStore,
	})

	const { data: medicalServices = [] } = useReduxStore({
		selector: (state) => state.management.medicalServices,
		setStore: setMedicalServicesStore,
		dataToGet: (storeData) =>
			(storeData || []).map((s) => ({
				label: `${s.name} (${s.code})`,
				value: s.code,
			})),
	})

	const getAllCategories = useFetch(
		ApiUrls.BED_CATEGORY.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	useEffect(() => {
		if (getAllCategories.data) {
			const data = getAllCategories.data
			setCategories(data.collection || [])
			setTotalPage(data.totalPage || 1)
		}
	}, [getAllCategories.data])

	const createCategory = useAxiosSubmit({
		url: ApiUrls.BED_CATEGORY.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await getAllCategories.fetch()
			await refreshReduxCategories()
		},
	})

	const deleteCategory = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			await getAllCategories.fetch()
			await refreshReduxCategories()
		},
	})

	const updateCategory = useAxiosSubmit({
		url: ApiUrls.BED_CATEGORY.MANAGEMENT.DETAIL(selectedCategoryId),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedCategoryId(null)
			await getAllCategories.fetch()
			await refreshReduxCategories()
		},
	})

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			title: t('bed_category.dialog.delete_title'),
			description: t('bed_category.dialog.delete_description', { name: row.name }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteCategory.submit({
				overrideUrl: ApiUrls.BED_CATEGORY.MANAGEMENT.DETAIL(row.id),
			})
		}
	}

	const handleDeleteMany = async () => {
		if (!selectedRows.length) return
		const isConfirmed = await confirm({
			title: t('bed_category.dialog.delete_many_title'),
			description: t('bed_category.dialog.delete_many_description', { number: selectedRows.length }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteCategory.submit({
				overrideUrl: ApiUrls.BED_CATEGORY.MANAGEMENT.DELETE_SELECTED,
				overrideParam: { ids: selectedRows },
			})
			setSelectedRows([])
			await getAllCategories.fetch()
			await refreshReduxCategories()
		}
	}

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('bed_category.field.id'), width: 10, sortable: true },
			{ key: 'code', title: t('bed_category.field.code'), width: 20, sortable: true },
			{ key: 'name', title: t('bed_category.field.name'), width: 25, sortable: true },
			{
				key: 'description',
				title: t('bed_category.field.description'),
				width: 40,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.update'),
								icon: <Edit fontSize='small' />,
								onClick: () => {
									setSelectedCategoryId(row.id)
									setOpenUpdateDialog(true)
								},
							},
							{
								title: t('button.delete'),
								icon: <Delete color='error' fontSize='small' />,
								onClick: () => handleDelete(row),
							},
						]}
					/>
				),
			},
		],
		[t, handleDelete]
	)

	const createFormFields = useMemo(
		() => [
			{
				key: 'code',
				title: t('bed_category.field.code'),
				type: 'select',
				options: medicalServices,
			},
			{ key: 'name', title: t('bed_category.field.name') },
			{ key: 'description', title: t('bed_category.field.description'), required: false },
		],
		[t, medicalServices]
	)

	const updateFormFields = useMemo(
		() => [
			{
				key: 'code',
				title: t('bed_category.field.code'),
				required: false,
				props: {
					readOnly: true,
				},
			},
			{ key: 'name', title: t('bed_category.field.name') },
			{ key: 'description', title: t('bed_category.field.description'), required: false },
		],
		[t]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5'>{t('bed_category.title.management')}</Typography>
					<Stack spacing={2} direction='row' alignItems='center'>
						<Button variant='contained' color='primary' onClick={() => setOpenCreateDialog(true)}>
							{t('button.create')}
						</Button>
						<Button
							variant='outlined'
							color='error'
							disabled={!selectedRows.length}
							onClick={handleDeleteMany}
						>
							{t('button.delete_selected')}
						</Button>
					</Stack>
				</Stack>

				<GenericTable
					data={categories}
					fields={tableFields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					canSelectRows={true}
					selectedRows={selectedRows}
					setSelectedRows={setSelectedRows}
					loading={getAllCategories.loading}
				/>

				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					pageSizeOptions={[5, 10, 20]}
					loading={getAllCategories.loading}
				/>
			</Stack>

			<GenericFormDialog
				title={t('bed_category.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				fields={createFormFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createCategory.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('bed_category.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => {
					setOpenUpdateDialog(false)
					setSelectedCategoryId(null)
				}}
				fields={updateFormFields}
				initialValues={categories.find((c) => c.id === selectedCategoryId) || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateCategory.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default BedCategoryManagementPage

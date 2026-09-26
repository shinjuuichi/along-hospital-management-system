import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import MedicineCategoryFilterBar from '@/pages/pharmacists/medicineCategoryManagementPage/section/MedicineCategoryManagementFilterSection'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const MedicineCategoryManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [filters, setFilters] = useState({ name: '' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [categories, setCategories] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [selectedCategory, setSelectedCategory] = useState(null)
	const [selectedRows, setSelectedRows] = useState([])
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })

	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const fetchParams = useMemo(
		() => ({
			page,
			pageSize,
			sort: sort ? `${sort.key} ${sort.direction}` : undefined,
			...filters,
		}),
		[page, pageSize, sort, filters]
	)

	const getAllCategories = useFetch(ApiUrls.MEDICINE_CATEGORY.INDEX, fetchParams, [fetchParams])

	useEffect(() => {
		if (getAllCategories.data) {
			const data = getAllCategories.data
			setCategories(data.collection || [])
			setTotalPage(data.totalPage || 1)
		}
	}, [getAllCategories.data])

	const createCategory = useAxiosSubmit({
		url: ApiUrls.MEDICINE_CATEGORY.CREATE,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await getAllCategories.fetch()
		},
	})

	const updateCategory = useAxiosSubmit({
		url: ApiUrls.MEDICINE_CATEGORY.UPDATE(selectedCategory?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedCategory(null)
			await getAllCategories.fetch()
		},
	})

	const deleteCategory = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedCategory(null)
			setSelectedRows([])
			await getAllCategories.fetch()
		},
	})

	const tableFields = [
		{
			key: 'id',
			title: t('medicine_category.field.id'),
			width: 15,
			sortable: true,
			fixedColumn: true,
		},
		{ key: 'name', title: t('medicine_category.field.name'), width: 35, sortable: true },
		{
			key: 'description',
			title: t('medicine_category.field.description'),
			width: 40,
			sortable: true,
			render: (value) => renderEmptyFallback(value),
		},
		{
			key: 'actions',
			title: t('medicine_category.field.actions'),
			width: 20,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedCategory(row)
								setOpenUpdateDialog(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									confirmText: t('button.delete'),
									confirmColor: 'error',
									title: t('medicine_category.dialog.confirm_delete_title'),
									description: t('medicine_category.dialog.confirm_delete_description', { name: row.name }),
								})
								if (isConfirmed) {
									await deleteCategory.submit({
										overrideUrl: ApiUrls.MEDICINE_CATEGORY.DELETE(row.id),
									})
								}
							},
						},
					]}
				/>
			),
		},
	]

	const formFields = [
		{ key: 'name', title: t('medicine_category.field.name') },
		{
			key: 'description',
			title: t('medicine_category.field.description'),
			multiline: true,
			rows: 3,
			required: false,
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>
					{t('medicine_category.title.medicine_category_management')}
				</Typography>

				<MedicineCategoryFilterBar
					filters={filters}
					loading={getAllCategories.loading}
					onFilterClick={(newValues) => {
						setFilters({ ...newValues })
						setPage(1)
					}}
					onResetFilterClick={(resetFn) => {
						const resetFilters = { name: '', page: 1 }
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

				<Stack spacing={2} sx={{ width: '100%' }}>
					<GenericTable
						data={categories}
						fields={tableFields}
						rowKey='id'
						sort={sort}
						setSort={setSort}
						selectedRows={selectedRows}
						setSelectedRows={setSelectedRows}
						stickyHeader
						onRowClick={setSelectedCategory}
					/>

					<Stack justifyContent='center' px={2}>
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
				</Stack>
			</Stack>

			<GenericFormDialog
				title={t('medicine_category.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				fields={formFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createCategory.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('medicine_category.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				fields={formFields}
				initialValues={selectedCategory ? { ...selectedCategory } : {}}
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

export default MedicineCategoryManagementPage

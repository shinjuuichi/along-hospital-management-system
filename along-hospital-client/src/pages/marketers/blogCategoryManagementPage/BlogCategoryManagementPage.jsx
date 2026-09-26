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
import { setBlogCategoriesStore } from '@/redux/reducers/managementReducer'
import { Delete, Edit } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const BlogCategoryManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [categories, setCategories] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [selectedRows, setSelectedRows] = useState([])
	const [selectedCategoryId, setSelectedCategoryId] = useState(null)
	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })

	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const { fetch: refreshReduxCategories } = useReduxStore({
		selector: (state) => state.management.blogCategories,
		setStore: setBlogCategoriesStore,
	})

	const fetchParams = useMemo(() => {
		const params = { Page: page, PageSize: pageSize }
		if (sort) {
			params.Sort = `${sort.key} ${sort.direction}`
		}
		return params
	}, [page, pageSize, sort])

	const getAllCategories = useFetch(
		ApiUrls.BLOG_CATEGORY.INDEX,
		fetchParams,
		[fetchParams]
	)

	useEffect(() => {
		if (getAllCategories.data) {
			const data = getAllCategories.data
			setCategories(data.collection || [])
			setTotalPage(data.totalPage || 1)
		}
	}, [getAllCategories.data])

	const createCategory = useAxiosSubmit({
		url: ApiUrls.BLOG_CATEGORY.CREATE,
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
		url: ApiUrls.BLOG_CATEGORY.UPDATE(selectedCategoryId),
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
			title: t('blog_category.dialog.delete_title'),
			description: t('blog_category.dialog.delete_description', { name: row.name }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteCategory.submit({
				overrideUrl: ApiUrls.BLOG_CATEGORY.DELETE(row.id),
			})
		}
	}

	const handleDeleteMany = async () => {
		if (!selectedRows.length) return
		const isConfirmed = await confirm({
			title: t('blog_category.dialog.delete_many_title'),
			description: t('blog_category.dialog.delete_many_description', { number: selectedRows.length }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteCategory.submit({
				overrideUrl: ApiUrls.BLOG_CATEGORY.DELETE_SELECTED,
				overrideParam: { ids: selectedRows },
			})
			setSelectedRows([])
			await getAllCategories.fetch()
			await refreshReduxCategories()
		}
	}

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('blog_category.field.id'), width: 10, sortable: true },
			{ key: 'name', title: t('blog_category.field.name'), width: 35, sortable: true },
			{
				key: 'description',
				title: t('blog_category.field.description'),
				width: 50,
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

	const formFields = [
		{ key: 'name', title: t('blog_category.field.name'), required: true },
		{
			key: 'description',
			title: t('blog_category.field.description'),
			multiline: true,
			rows: 3,
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5'>{t('blog_category.title.management')}</Typography>
					<Stack spacing={2} direction='row' alignItems='center'>
						<Button
							variant='contained'
							color='primary'
							onClick={() => setOpenCreateDialog(true)}
						>
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
			title={t('blog_category.dialog.create_title')}
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
			title={t('blog_category.dialog.update_title')}
			open={openUpdateDialog}
			onClose={() => {
				setOpenUpdateDialog(false)
				setSelectedCategoryId(null)
			}}
			fields={formFields}
			initialValues={categories.find(c => c.id === selectedCategoryId) || {}}
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

export default BlogCategoryManagementPage

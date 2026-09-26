import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import SearchBar from '@/components/generals/SearchBar'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { stripHtml, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Delete, Edit } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const getPlainText = (content) => {
	return (
		stripHtml(String(content ?? ''))
			.replace(/\s+/g, ' ')
			.trim()
			.substring(0, 120) + '...'
	)
}

const BlogManagementPage = () => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedIds, setSelectedIds] = useState([])
	const [search, setSearch] = useState('')

	const fetchParams = useMemo(() => {
		const params = {
			Page: page,
			PageSize: pageSize,
			Sort: `${sort.key} ${sort.direction}`,
		}
		if (search && search.trim()) {
			params.Title = search.trim()
		}
		return params
	}, [page, pageSize, sort, search])

	const { loading, data, fetch } = useFetch(ApiUrls.BLOG.MANAGEMENT.INDEX, fetchParams, [
		fetchParams,
	])

	const blogs = useMemo(() => (Array.isArray(data?.collection) ? data.collection : []), [data])
	const totalItems = data?.totalCount ?? 0

	const deleteBlog = useAxiosSubmit({ method: 'DELETE' })

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			title: t('blog.dialog.delete_title'),
			description: t('blog.dialog.delete_description', { title: row.title }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteBlog.submit({
				overrideUrl: ApiUrls.BLOG.MANAGEMENT.DETAIL(row.id),
			})
			await fetch()
		}
	}

	const handleDeleteMany = async () => {
		if (!selectedIds.length) return
		const isConfirmed = await confirm({
			title: t('blog.dialog.delete_many_title'),
			description: t('blog.dialog.delete_many_description', { number: selectedIds.length }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteBlog.submit({
				overrideUrl: ApiUrls.BLOG.MANAGEMENT.DELETE_SELECTED,
				overrideParam: { ids: selectedIds },
			})
			setSelectedIds([])
			await fetch()
		}
	}

	const fields = useMemo(
		() => [
			{ key: 'id', title: 'ID', width: 10, sortable: true, fixedColumn: true },
			{ key: 'title', title: t('blog.field.title'), width: 25, sortable: true },
			{
				key: 'blogCategory',
				title: t('blog.field.category'),
				width: 15,
				sortable: false,
				render: (value) => renderEmptyFallback(value?.name),
			},
			{
				key: 'content',
				title: t('blog.field.preview'),
				width: 30,
				render: (value) => renderEmptyFallback(getPlainText(value)),
			},
		{
			key: 'creationDate',
			title: t('blog.field.creation_date'),
			width: 15,
			sortable: true,
			render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
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
								onClick: () =>
									navigate(routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.UPDATE(row.id))),
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
		[navigate, t, handleDelete]
	)

	return (
		<Paper sx={{ py: 1, px: 2, mt: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('blog.title.management')}</Typography>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<SearchBar widthPercent={30} value={search} setValue={setSearch} />
					<Stack spacing={2} direction='row' alignItems='center'>
						<Button
							variant='contained'
							color='primary'
							onClick={() =>
								navigate(routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.CREATE))
							}
						>
							{t('button.create')}
						</Button>
						<Button
							variant='outlined'
							color='error'
							disabled={!selectedIds.length}
							onClick={handleDeleteMany}
						>
							{t('button.delete_selected')}
						</Button>
					</Stack>
				</Stack>
				<GenericTable
					data={blogs}
					fields={fields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					canSelectRows={true}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					loading={loading}
				/>
				<GenericTablePagination
					totalPage={Math.ceil(totalItems / pageSize)}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={loading}
				/>
			</Stack>
		</Paper>
	)
}

export default BlogManagementPage

import { GenericPagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import BlogFilterBarSection from './sections/BlogFilterBarSection'
import BlogListSection from './sections/BlogListSection'
import BlogTabsSection from './sections/BlogTabsSection'

export default function BlogPage() {
	const { t } = useTranslation()
	const [filters, setFilters] = useState({
		title: '',
		creationDate: '',
		blogCategoryId: '',
	})
	const [page, setPage] = useState(1)
	const [pageSize] = useState(6)

	const getBlogs = useFetch(ApiUrls.BLOG.INDEX, { ...filters, page, pageSize }, [
		filters,
		page,
		pageSize,
	])

	useEffect(() => {
		setPage(1)
	}, [filters])

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('blog.title.list')}</Typography>
				<BlogFilterBarSection
					filters={filters}
					setFilters={setFilters}
					loading={getBlogs.loading}
				/>
				<Stack spacing={3}>
					<BlogTabsSection
						filters={filters}
						setFilters={setFilters}
						loading={getBlogs.loading}
						setPage={setPage}
					/>
					<BlogListSection blogs={getBlogs.data?.collection} loading={getBlogs.loading} />
				</Stack>
				<Stack alignItems='center' sx={{ mt: 3 }}>
					<GenericPagination
						totalPage={getBlogs.data?.totalPage || 1}
						page={page}
						setPage={setPage}
						loading={getBlogs.loading}
					/>
				</Stack>
			</Stack>
		</Paper>
	)
}

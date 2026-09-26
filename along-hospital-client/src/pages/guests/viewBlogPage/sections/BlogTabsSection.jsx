import GenericTabs from '@/components/generals/GenericTabs'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBlogCategoriesStore } from '@/redux/reducers/managementReducer'
import { Stack, Typography } from '@mui/material'
import { useMemo } from 'react'

const BlogTabsSection = ({
	filters = {},
	setFilters = () => {},
	loading = false,
	setPage = () => {},
}) => {
	const { t } = useTranslation()

	const { data: blogCategories, loading: categoriesLoading } = useReduxStore({
		selector: (state) => state.management.blogCategories,
		setStore: setBlogCategoriesStore,
	})

	const categories = useMemo(() => {
		return Array.isArray(blogCategories) ? blogCategories : []
	}, [blogCategories])

	const tabs = useMemo(() => {
		return categories.map((category) => ({
			key: category.id,
			title: category.name,
		}))
	}, [categories])

	const currentTab = useMemo(() => {
		return tabs.find((tab) => String(tab.key) === String(filters.blogCategoryId))
	}, [tabs, filters.blogCategoryId])

	const setCurrentTab = (tab) => {
		const nextCategoryId = filters.blogCategoryId === tab?.key ? '' : tab?.key || ''
		setFilters({ ...filters, blogCategoryId: nextCategoryId })
		setPage(1)
	}

	if (categoriesLoading || categories.length === 0) {
		return null
	}

	return (
		<Stack spacing={2}>
			<Typography variant='subtitle1' sx={{ fontWeight: 600 }}>
				{t('blog.field.category')}
			</Typography>
			<GenericTabs
				tabs={tabs}
				currentTab={currentTab}
				setCurrentTab={setCurrentTab}
				loading={loading}
			/>
		</Stack>
	)
}

export default BlogTabsSection

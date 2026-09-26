import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import { ArticleRounded, CategoryRounded } from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutMarketer = () => {
	const [mobileOpen, setMobileOpen] = useState(false)
	const { logout } = useAuth()
	const { t } = useTranslation()

	const profileStore = useReduxStore({
		selector: (s) => s.patient.profile,
		setStore: setProfileStore,
	})

	const sections = [
		{
			title: t('sidebar.shared.customer_support'),
			items: [
				{
					key: 'blog',
					label: t('sidebar.items.blog'),
					icon: <ArticleRounded />,
					of: [
						{
							key: 'blog-list',
							label: t('sidebar.items.blog_management'),
							icon: <ArticleRounded />,
							url: routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX),
						},
						{
							key: 'blog-category',
							label: t('sidebar.items.blog_category'),
							icon: <CategoryRounded />,
							url: routeUrls.BASE_ROUTE.MARKETER(
								routeUrls.MARKETER.BLOG_CATEGORY_MANAGEMENT.INDEX
							),
						},
					],
				},
			],
		},
		...defaultStaffMenuSections(t),
	]

	return (
		<Stack direction={'row'}>
			<DashboardDrawer
				sections={sections}
				mobileOpen={mobileOpen}
				onMobileClose={() => setMobileOpen(false)}
			/>
			<Stack flexGrow={1}>
				<DashboardHeader
					profile={profileStore.data}
					onLogout={logout}
					onOpenDrawer={() => setMobileOpen(true)}
					userMenuItems={defaultStaffMenuItems(t)}
				/>
				<Container sx={{ flexGrow: 1, py: 3 }} maxWidth='xl'>
					<Outlet />
				</Container>
			</Stack>
		</Stack>
	)
}

export default LayoutMarketer

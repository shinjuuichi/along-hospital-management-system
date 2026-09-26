import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import { FlagRounded, RateReviewRounded } from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutHotlineAgent = () => {
	const [mobileOpen, setMobileOpen] = useState(false)
	const { logout } = useAuth()
	const { t } = useTranslation()

	const profileStore = useReduxStore({
		selector: (s) => s.patient.profile,
		setStore: setProfileStore,
	})

	const sections = [
		{
			items: [
				{
					key: 'feedback',
					label: t('feedback.title.feedback_management'),
					icon: <RateReviewRounded />,
					url: routeUrls.BASE_ROUTE.HOTLINE_AGENT(routeUrls.HOTLINE_AGENT.FEEDBACK_MANAGEMENT),
				},
			],
		},
		{
			title: t('sidebar.shared.customer_support'),
			items: [
				{
					key: 'feedback-report',
					label: t('sidebar.items.feedback_report'),
					icon: <RateReviewRounded />,
					url: routeUrls.BASE_ROUTE.HOTLINE_AGENT(routeUrls.HOTLINE_AGENT.FEEDBACK_REPORT_MANAGEMENT),
				},
				{
					key: 'complaint',
					label: t('sidebar.items.complaint'),
					icon: <FlagRounded />,
					url: routeUrls.BASE_ROUTE.HOTLINE_AGENT(routeUrls.HOTLINE_AGENT.COMPLAINT_MANAGEMENT),
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

export default LayoutHotlineAgent

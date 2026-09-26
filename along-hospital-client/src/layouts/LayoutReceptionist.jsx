import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	DescriptionRounded,
	FormatListNumberedRounded,
	TimerRounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutReceptionist = () => {
	const [mobileOpen, setMobileOpen] = useState(false)
	const { logout } = useAuth()
	const { t } = useTranslation()

	const profileStore = useReduxStore({
		selector: (s) => s.patient.profile,
		setStore: setProfileStore,
	})

	const sections = [
		{
			title: t('sidebar.shared.work'),
			items: [
				{
					key: 'medical-history',
					label: t('sidebar.items.medical_history'),
					icon: <DescriptionRounded />,
					url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.INDEX),
				},
				{
					key: 'queue',
					label: t('sidebar.items.queue'),
					icon: <FormatListNumberedRounded />,
					url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.QUEUE_MANAGEMENT.INDEX),
				},
				{
					key: 'time-slot-management',
					label: t('sidebar.items.time_slot_management'),
					icon: <TimerRounded />,
					url: routeUrls.BASE_ROUTE.RECEPTIONIST(routeUrls.RECEPTIONIST.TIME_SLOT_MANAGEMENT),
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

export default LayoutReceptionist

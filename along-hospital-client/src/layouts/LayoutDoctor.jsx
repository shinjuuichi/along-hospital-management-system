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
	EventAvailableRounded,
	FormatListNumberedRounded,
	VideocamOffRounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutDoctor = () => {
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
					key: 'appointment',
					label: t('sidebar.items.appointments'),
					icon: <EventAvailableRounded />,
					url: routeUrls.BASE_ROUTE.DOCTOR(routeUrls.DOCTOR.APPOINTMENT_MANAGEMENT),
				},
				{
					key: 'medical-history',
					label: t('sidebar.items.medical_history'),
					icon: <DescriptionRounded />,
					url: routeUrls.BASE_ROUTE.DOCTOR(routeUrls.DOCTOR.MEDICAL_HISTORY),
				},
				{
					key: 'queue',
					label: t('sidebar.items.queue'),
					icon: <FormatListNumberedRounded />,
					url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.QUEUE_MANAGEMENT.INDEX),
				},
			],
		},
		...defaultStaffMenuSections(t),
		{
			title: t('sidebar.shared.system'),
			items: [
				{
					key: 'join-tele-room',
					label: t('sidebar.items.join_meeting_room'),
					icon: <VideocamOffRounded />,
					url: routeUrls.BASE_ROUTE.DOCTOR(routeUrls.DOCTOR.APPOINTMENT.JOIN_MEETING_ROOM),
				},
			],
		},
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

export default LayoutDoctor

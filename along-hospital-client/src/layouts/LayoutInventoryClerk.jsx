import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	AssignmentRounded,
	BadgeRounded,
	DashboardRounded,
	Inventory2Rounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutInventoryClerk = () => {
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
					key: 'dashboard',
					label: t('sidebar.items.dashboard'),
					icon: <DashboardRounded />,
					url: routeUrls.BASE_ROUTE.INVENTORY_CLERK(routeUrls.INVENTORY_CLERK.DASHBOARD),
				},
			],
		},
		{
			title: t('sidebar.shared.commerce'),
			items: [
				{
					key: 'supplier',
					label: t('sidebar.items.supplier_management'),
					icon: <BadgeRounded />,
					url: routeUrls.BASE_ROUTE.INVENTORY_CLERK(routeUrls.INVENTORY_CLERK.SUPPLIER_MANAGEMENT),
				},
				{
					key: 'import-request',
					label: t('sidebar.items.import_request'),
					icon: <AssignmentRounded />,
					url: routeUrls.BASE_ROUTE.INVENTORY_CLERK(routeUrls.INVENTORY_CLERK.IMPORT_REQUEST.INDEX),
				},
				{
					key: 'import-management',
					label: t('sidebar.items.import_management'),
					icon: <Inventory2Rounded />,
					url: routeUrls.BASE_ROUTE.INVENTORY_CLERK(routeUrls.INVENTORY_CLERK.IMPORT_MANAGEMENT.INDEX),
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

export default LayoutInventoryClerk

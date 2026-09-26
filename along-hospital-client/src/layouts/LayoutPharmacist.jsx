import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	CategoryRounded,
	DashboardRounded,
	FlagRounded,
	Inventory2Rounded,
	LocalPharmacyRounded,
	ReceiptLongRounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutPharmacist = () => {
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
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.DASHBOARD),
				},
			],
		},
		{
			title: t('sidebar.shared.medicine'),
			items: [
				{
					key: 'medicine',
					label: t('sidebar.items.medicine'),
					icon: <LocalPharmacyRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_MANAGEMENT.INDEX),
				},
				{
					key: 'medicine-sku',
					label: t('sidebar.items.medicine_sku'),
					icon: <LocalPharmacyRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_SKU_MANAGEMENT.INDEX),
				},
				{
					key: 'medicine-unit',
					label: t('sidebar.items.medicine_unit'),
					icon: <CategoryRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_UNIT_MANAGEMENT.INDEX),
				},
				{
					key: 'medicine-category',
					label: t('sidebar.items.medicine_category'),
					icon: <CategoryRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.MEDICINE_CATEGORY_MANAGEMENT.INDEX),
				},
				{
					key: 'option',
					label: t('sidebar.items.option'),
					icon: <FlagRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.OPTION_MANAGEMENT.INDEX),
				},
				{
					key: 'option-value',
					label: t('sidebar.items.option_value'),
					icon: <FlagRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.OPTION_VALUE_MANAGEMENT.INDEX),
				},
			],
		},
		{
			title: t('sidebar.shared.import'),
			items: [
				{
					key: 'import-request',
					label: t('sidebar.items.import_request'),
					icon: <Inventory2Rounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.IMPORT_REQUEST.INDEX),
				},
			],
		},
		{
			title: t('sidebar.shared.commerce'),
			items: [
				{
					key: 'order',
					label: t('sidebar.items.order'),
					icon: <ReceiptLongRounded />,
					url: routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.ORDER_MANAGEMENT),
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

export default LayoutPharmacist

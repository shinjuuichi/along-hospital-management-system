import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	ApartmentRounded,
	AccountBalanceWalletRounded,
	AssignmentRounded,
	BusinessRounded,
	CategoryRounded,
	CheckBoxRounded,
	DashboardRounded,
	DescriptionRounded,
	DiscountRounded,
	DomainAddRounded,
	FactCheckRounded,
	HomeWorkOutlined,
	KingBedRounded,
	LocationCityRounded,
	PersonRounded,
	ReceiptLongRounded,
	VideoCallRounded,
	WorkHistoryRounded,
	WorkOutlineRounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutManager = () => {
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
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.DASHBOARD),
				},
			],
		},

		{
			title: t('sidebar.shared.clinical_management'),
			items: [
				{
					key: 'medical-service',
					label: t('sidebar.items.medical_service'),
					icon: <AssignmentRounded />,
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.MEDICAL_SERVICE_MANAGEMENT),
				},
			],
		},
		{
			title: t('sidebar.shared.facility_management'),
			items: [
				{
					key: 'bed',
					label: t('sidebar.items.bed'),
					icon: <KingBedRounded />,
					of: [
						{
							key: 'bed-management',
							label: t('sidebar.items.bed_management'),
							icon: <KingBedRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.BED_MANAGEMENT.INDEX),
						},
						{
							key: 'bed-category',
							label: t('sidebar.items.bed_category'),
							icon: <CategoryRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.BED_CATEGORY_MANAGEMENT.INDEX),
						},
					],
				},
				{
					key: 'room',
					label: t('sidebar.items.room'),
					icon: <BusinessRounded />,
					of: [
						{
							key: 'room-management',
							label: t('sidebar.items.room_management'),
							icon: <DomainAddRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.ROOM_MANAGEMENT.INDEX),
						},
						{
							key: 'room-category',
							label: t('sidebar.items.room_category'),
							icon: <LocationCityRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.ROOM_CATEGORY_MANAGEMENT.INDEX),
						},
						{
							key: 'tele-room-management',
							label: t('sidebar.items.tele_room_management'),
							icon: <VideoCallRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.TELE_ROOM_MANAGEMENT),
						},
					],
				},
				{
					key: 'building-management',
					label: t('sidebar.items.building'),
					icon: <HomeWorkOutlined />,
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.BUILDING_MANAGEMENT.INDEX),
				},
			],
		},
		{
			title: t('sidebar.shared.commerce'),
			items: [
				{
					key: 'voucher',
					label: t('sidebar.items.voucher'),
					icon: <DiscountRounded />,
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.VOUCHER_MANAGEMENT),
				},
			],
		},
		{
			title: t('sidebar.shared.human_resource'),
			items: [
				{
					key: 'leave-request-management',
					label: t('sidebar.items.leave_request_management'),
					icon: <FactCheckRounded />,
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.LEAVE_REQUEST_MANAGEMENT),
				},
				{
					key: 'salary-advance-management',
					label: t('sidebar.items.salary_advance_management'),
					icon: <AccountBalanceWalletRounded />,
					url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.SALARY_ADVANCE_MANAGEMENT),
				},
				{
					key: 'organization',
					label: t('sidebar.items.organization'),
					icon: <ApartmentRounded />,
					of: [
						{
							key: 'specialty',
							label: t('sidebar.items.specialty'),
							icon: <WorkOutlineRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.SPECIALTY_MANAGEMENT),
						},
						{
							key: 'qualification',
							label: t('sidebar.items.qualification'),
							icon: <DescriptionRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.QUALIFICATION_MANAGEMENT),
						},
					],
				},
				{
					key: 'staff',
					label: t('sidebar.items.staff'),
					icon: <PersonRounded />,
					of: [
						{
							key: 'staff-contract-management',
							label: t('sidebar.items.staff_contract_management'),
							icon: <DescriptionRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.STAFF_CONTRACT_MANAGEMENT),
						},
					],
				},
				{
					key: 'workforce',
					label: t('sidebar.items.workforce'),
					icon: <WorkHistoryRounded />,
					of: [
						{
							key: 'payroll-management',
							label: t('sidebar.shared.payroll_management'),
							icon: <ReceiptLongRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.PAYROLL.INDEX),
						},
						{
							key: 'attendance',
							label: t('sidebar.items.attendance'),
							icon: <CheckBoxRounded />,
							url: routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.ATTENDANCE_MANAGEMENT),
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

export default LayoutManager

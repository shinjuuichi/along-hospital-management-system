import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	DashboardRounded,
	DescriptionRounded,
	EventBusyRounded,
	FactCheckRounded,
	GroupAddRounded,
	PersonAddAlt1Rounded,
	PersonRounded,
	ScheduleRounded,
	TableViewRounded,
	VerifiedUserRounded,
	WorkHistoryRounded,
	WorkRounded,
	WorkspacePremiumRounded,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutHR = () => {
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
					url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.DASHBOARD),
				},
			],
		},
		{
			title: t('sidebar.shared.human_resource'),
			items: [
				{
					key: 'staff',
					label: t('sidebar.items.staff'),
					icon: <PersonRounded />,
					of: [
						{
							key: 'staff-management',
							label: t('sidebar.items.staff_management'),
							icon: <PersonAddAlt1Rounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.STAFF_MANAGEMENT),
						},
						{
							key: 'staff-group-management',
							label: t('sidebar.items.staff_group_management'),
							icon: <GroupAddRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.STAFF_GROUP_MANAGEMENT),
						},
						{
							key: 'staff-contract-management',
							label: t('sidebar.items.staff_contract_management'),
							icon: <DescriptionRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.STAFF_CONTRACT_MANAGEMENT),
						},
						{
							key: 'staff-certificate-type-management',
							label: t('sidebar.items.certificate_type_management'),
							icon: <WorkspacePremiumRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.STAFF_CERTIFICATE_TYPE_MANAGEMENT),
						},
						{
							key: 'staff-certificate-management',
							label: t('sidebar.items.certificate_management'),
							icon: <VerifiedUserRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.STAFF_CERTIFICATE_MANAGEMENT),
						},
					],
				},
				{
					key: 'job-management',
					label: t('sidebar.items.interview'),
					icon: <WorkRounded />,
					of: [
						{
							key: 'interview-type-management',
							label: t('sidebar.items.interview_type_management'),
							icon: <WorkRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.INTERVIEW_TYPE_MANAGEMENT),
						},
						{
							key: 'job-posting-management',
							label: t('job_posting.title.menu'),
							icon: <WorkRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.INDEX),
						},
					],
				},
			],
		},
		{
			title: t('sidebar.shared.work_schedule_management'),
			items: [
				{
					key: 'work-schedule',
					label: t('sidebar.items.work_schedule'),
					icon: <WorkHistoryRounded />,
					of: [
						{
							key: 'work-schedule-management',
							label: t('sidebar.items.work_schedule_management'),
							icon: <FactCheckRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_MANAGEMENT.INDEX),
						},
						{
							key: 'work-schedule-template-management',
							label: t('sidebar.items.work_schedule_template_management'),
							icon: <TableViewRounded />,
							url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_TEMPLATE_MANAGEMENT.INDEX),
						},
					],
				},
				{
					key: 'shift-management',
					label: t('sidebar.items.shift_management'),
					icon: <ScheduleRounded />,
					url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.SHIFT_MANAGEMENT),
				},
				{
					key: 'holiday-management',
					label: t('sidebar.items.holiday_management'),
					icon: <EventBusyRounded />,
					url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.HOLIDAY_MANAGEMENT),
				},
				{
					key: 'leave-request-management',
					label: t('sidebar.items.leave_request_management'),
					icon: <FactCheckRounded />,
					url: routeUrls.BASE_ROUTE.HR(routeUrls.HR.LEAVE_REQUEST_MANAGEMENT),
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

export default LayoutHR

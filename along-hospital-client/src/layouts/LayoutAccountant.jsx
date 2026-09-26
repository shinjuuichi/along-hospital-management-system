import DashboardDrawer from '@/components/layouts/DashboardDrawer'
import DashboardHeader from '@/components/layouts/DashboardHeader'
import { defaultStaffMenuItems, defaultStaffMenuSections } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import {
	AccountBalanceRounded,
	AccountBalanceWalletRounded,
	AttachMoneyRounded,
	CurrencyExchangeRounded,
	DashboardRounded,
	DescriptionRounded,
	GavelRounded,
	MoneyOffRounded,
	PaymentsRounded,
	Receipt,
} from '@mui/icons-material'
import { Container, Stack } from '@mui/material'
import { useState } from 'react'
import { Outlet } from 'react-router-dom'

const LayoutAccountant = () => {
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
					url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.DASHBOARD),
				},
			],
		},
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
					key: 'invoice-management',
					label: t('sidebar.items.invoice_management'),
					icon: <Receipt />,
					url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.INVOICE.INDEX),
				},
				{
					key: 'payroll',
					label: t('sidebar.items.payroll'),
					icon: <PaymentsRounded />,
					of: [
						{
							key: 'payroll-management',
							label: t('sidebar.items.payroll'),
							icon: <PaymentsRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.PAYROLL.INDEX),
						},
						{
							key: 'salary-advance-management',
							label: t('sidebar.items.salary_advance_management'),
							icon: <AccountBalanceWalletRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.SALARY_ADVANCE_MANAGEMENT),
						},
						{
							key: 'allowance-type',
							label: t('sidebar.items.allowance_type'),
							icon: <AttachMoneyRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.ALLOWANCE_TYPE),
						},
						{
							key: 'deduction-type',
							label: t('sidebar.items.deduction_type'),
							icon: <MoneyOffRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.DEDUCTION_TYPE),
						},
						{
							key: 'regional-wage',
							label: t('regional_wage.title.menu'),
							icon: <CurrencyExchangeRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.REGIONAL_WAGE_MANAGEMENT),
						},
						{
							key: 'tax-bracket',
							label: t('sidebar.items.tax_bracket'),
							icon: <AccountBalanceRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.TAX_BRACKET),
						},
						{
							key: 'global-tax-config',
							label: t('sidebar.items.global_tax_config'),
							icon: <AccountBalanceRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.GLOBAL_TAX_CONFIG),
						},
						{
							key: 'payroll-policy',
							label: t('sidebar.items.payroll_policy'),
							icon: <GavelRounded />,
							url: routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.PAYROLL_POLICY),
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

export default LayoutAccountant

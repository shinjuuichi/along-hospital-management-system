import ChatBotFloatingButton from '@/components/layouts/chatbot/ChatBotFloatingButton'
import Footer from '@/components/layouts/Footer'
import Header from '@/components/layouts/Header'
import { EnumConfig } from '@/configs/enumConfig'
import { getReturnUrlByRole } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setCartStore, setProfileStore } from '@/redux/reducers/patientReducer'
import {
	AssignmentOutlined,
	Dashboard,
	EventAvailable,
	HistoryOutlined,
	LocalOffer,
	Person,
} from '@mui/icons-material'
import { Box, Container, Stack } from '@mui/material'
import { Outlet, useLocation } from 'react-router-dom'

const LayoutPatient = () => {
	const { auth } = useAuth()
	const isPatient = auth?.role === EnumConfig.Role.Patient
	const location = useLocation()
	const isHomePage = location.pathname === routeUrls.HOME.INDEX

	const profileStore = useReduxStore({
		selector: (s) => s.patient.profile,
		setStore: setProfileStore,
	})

	const cartCountStore = useReduxStore({
		selector: (s) => s.patient.cart,
		setStore: isPatient ? setCartStore : null,
		dataToGet: (cart) =>
			cart?.cartDetails
				?.filter((item) => item.medicineSKU !== null)
				?.reduce((total, item) => total + (item.quantity || 0), 0) || 0,
	})

	const { t } = useTranslation()

	const items = [
		{ label: t('header.home'), url: '/' },
		{
			label: t('header.service'),
			of: [
				{ label: t('header.medical_service'), url: routeUrls.HOME.MEDICAL_SERVICE },
				{ label: t('header.medicine'), url: routeUrls.HOME.MEDICINE },
				{ label: t('header.doctor'), url: routeUrls.HOME.DOCTOR },
				{ label: t('header.specialty'), url: routeUrls.HOME.SPECIALTY },
			],
		},
		{ label: t('header.vouchers'), url: routeUrls.HOME.VOUCHERS },
		{
			label: t('header.booking'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.CREATE),
		},
		{
			label: t('header.meeting_room'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.JOIN_MEETING_ROOM),
		},
		{
			label: t('header.more'),
			of: [
				{ label: t('header.blog'), url: routeUrls.HOME.BLOG },
				{ label: t('header.career'), url: routeUrls.HOME.JOB_POSTING.INDEX },
				{ label: t('header.about_us'), url: routeUrls.HOME.ABOUT_US },
			],
		},
	]

	const notUserMenuItems = [
		{
			label: t('header.user_menu.dashboard'),
			url: getReturnUrlByRole(auth?.role),
			icon: <Dashboard />,
		},
	]

	const userMenuItems = [
		{
			label: t('header.user_menu.profile'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.PROFILE),
			icon: <Person />,
		},
		{
			label: t('header.user_menu.appointments'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.INDEX),
			icon: <EventAvailable />,
		},
		{
			label: t('header.user_menu.medical_history'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.MEDICAL_HISTORY.INDEX),
			icon: <AssignmentOutlined />,
		},
		{
			label: t('header.user_menu.order_history'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.ORDER_HISTORY.INDEX),
			icon: <HistoryOutlined />,
		},
		{
			label: t('header.user_menu.my_vouchers'),
			url: routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.VOUCHER.MY_VOUCHERS),
			icon: <LocalOffer />,
		},
	]

	const footerSections = [
		{
			title: t('footer.medical'),
			links: [
				{ label: t('footer.service'), url: routeUrls.HOME.MEDICAL_SERVICE },
				{ label: t('footer.doctor'), url: routeUrls.HOME.DOCTOR },
				{ label: t('footer.specialty'), url: routeUrls.HOME.SPECIALTY },
			],
		},
		{
			title: t('footer.resources'),
			links: [
				{ label: t('footer.blog'), url: routeUrls.HOME.BLOG },
				{ label: t('footer.medicine'), url: routeUrls.HOME.MEDICINE },
				{ label: t('footer.career'), url: routeUrls.HOME.JOB_POSTING.INDEX },
			],
		},
		// {
		// 	title: t('footer.support'),
		// 	links: [
		// 		{ label: t('footer.contact_us'), url: routeUrls.HOME.CONTACT },
		// 		{ label: t('footer.faq'), url: routeUrls.HOME.FAQ },
		// 	],
		// },
		{
			title: t('footer.about'),
			links: [
				{ label: t('footer.our_hospital'), url: routeUrls.HOME.ABOUT_US },
				{ label: t('footer.career'), url: routeUrls.HOME.JOB_POSTING.INDEX },
				{ label: t('footer.privacy_policy'), url: routeUrls.HOME.PRIVACY_POLICY },
				{ label: t('footer.terms_of_service'), url: routeUrls.HOME.TERMS_OF_SERVICE },
			],
		},
	]

	return (
		<Stack minHeight='100vh'>
			<Header
				items={items}
				isAuthenticated={true}
				userMenuItems={isPatient ? userMenuItems : notUserMenuItems}
				profile={profileStore.data}
				cartCount={cartCountStore?.data || 0}
			/>
			{isHomePage ? (
				<Box sx={{ flexGrow: 1, width: '100%' }}>
					<Outlet />
				</Box>
			) : (
				<Container sx={{ flexGrow: 1 }} maxWidth='xl'>
					<Outlet />
				</Container>
			)}
			<ChatBotFloatingButton />
			<Footer sections={footerSections} />
		</Stack>
	)
}

export default LayoutPatient

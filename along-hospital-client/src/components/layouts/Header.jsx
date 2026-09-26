import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import { alpha, AppBar, Stack, Toolbar, useMediaQuery, useTheme } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CartButton from '../buttons/CartButton'
import LoginButton from '../buttons/LoginButton'
import MobileMenuButton from '../buttons/MobileMenuButton'
import SwitchLanguageButton from '../buttons/SwitchLanguageButton'
import SwitchThemeButton from '../buttons/SwitchThemeButton'
import UserAvatarMenu from '../menus/UserAvatarMenu'
import SystemLogoAndName from './commons/SystemLogoAndName'
import MobileDrawer from './MobileDrawer'
import NavItem from './navItems/NavItem'

const Header = ({
	items = [],
	isAuthenticated = false,
	userMenuItems = [],
	profile = {},
	cartCount = 0,
}) => {
	const { auth } = useAuth()
	const isPatient = auth?.role === EnumConfig.Role.Patient

	const navigate = useNavigate()
	const [openDrawer, setOpenDrawer] = useState(false)
	const { logout } = useAuth()
	const theme = useTheme()

	const isDownSm = useMediaQuery(theme.breakpoints.down('sm'))
	const isDownMd = useMediaQuery(theme.breakpoints.down('md'))

	return (
		<>
			<AppBar
				position='sticky'
				color='transparent'
				elevation={0}
				sx={{
					backdropFilter: 'saturate(180%) blur(8px)',
					bgcolor: (theme) => alpha(theme.palette.background.paper, 0.6),
					borderBottom: '1px solid',
					borderColor: 'divider',
				}}
			>
				<Toolbar sx={{ minHeight: 72 }}>
					<Stack direction='row' alignItems='center' spacing={2} sx={{ flex: 1 }}>
						<SystemLogoAndName onClick={() => navigate(routeUrls.HOME.INDEX)} onlyShowIcon={isDownSm} />

						{!isDownMd && (
							<Stack direction='row' spacing={0.5} sx={{ display: 'flex', ml: 2 }} role='menubar'>
								{items.map((item) => (
									<NavItem key={item.label} label={item.label} url={item.url} of={item.of} />
								))}
							</Stack>
						)}
					</Stack>

					<Stack direction='row' alignItems='center' spacing={1.5}>
						<SwitchThemeButton />
						<SwitchLanguageButton />
						{!isAuthenticated ? (
							<LoginButton onClick={() => navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN))} />
						) : (
							<>
								{isPatient && (
									<CartButton
										count={cartCount}
										onClick={() => navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.CART))}
									/>
								)}
								<UserAvatarMenu items={userMenuItems} profile={profile} onLogout={logout} />
							</>
						)}
						{isDownMd && <MobileMenuButton onOpen={() => setOpenDrawer(true)} />}
					</Stack>
				</Toolbar>
			</AppBar>
			<MobileDrawer items={items} openDrawer={openDrawer} setOpenDrawer={setOpenDrawer} />
		</>
	)
}

export default Header

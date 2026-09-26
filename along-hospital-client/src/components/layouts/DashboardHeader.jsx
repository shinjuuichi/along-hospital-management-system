import { NavigateNextRounded } from '@mui/icons-material'
import {
	AppBar,
	Box,
	Breadcrumbs,
	ButtonBase,
	Stack,
	Toolbar,
	Typography,
	useMediaQuery,
	useTheme,
} from '@mui/material'
import { useMemo } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import MobileMenuButton from '../buttons/MobileMenuButton'
import SwitchLanguageButton from '../buttons/SwitchLanguageButton'
import SwitchThemeButton from '../buttons/SwitchThemeButton'
import UserAvatarMenu from '../menus/UserAvatarMenu'

const DashboardHeader = ({ onOpenDrawer, profile, userMenuItems = [], onLogout = () => {} }) => {
	const theme = useTheme()
	const isDownMd = useMediaQuery(theme.breakpoints.down('md'))
	const navigate = useNavigate()
	const location = useLocation()

	const { crumbs, currentLabel } = useMemo(() => {
		const rawPath = location?.pathname || '/'
		const pathWithoutQuery = rawPath.split('?')[0].split('#')[0]
		const segments = pathWithoutQuery.split('/').filter(Boolean)

		const items = segments.map((seg, idx) => {
			const cumulative = '/' + segments.slice(0, idx + 1).join('/')
			const labelMaker = (seg) => seg.replace(/-/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
			return {
				label: labelMaker(decodeURIComponent(seg), cumulative),
				path: cumulative,
				isLast: idx === segments.length - 1,
			}
		})

		if (items.length === 0) {
			return { crumbs: [], currentLabel: 'Dashboard' }
		}

		return {
			crumbs: items.slice(0, -1),
			currentLabel: items[items.length - 1].label,
		}
	}, [location])

	return (
		<AppBar
			position='sticky'
			elevation={0}
			color='default'
			sx={{
				bgcolor: 'background.paper',
				color: 'text.primary',
				borderBottom: `1px solid ${theme.palette.divider}`,
			}}
		>
			<Toolbar
				variant='regular'
				sx={{
					minHeight: { xs: 56, sm: 64 },
					px: { xs: 1, sm: 2 },
					gap: 1.5,
				}}
			>
				<Stack direction='row' alignItems='center' spacing={1.5} sx={{ flex: 1, minWidth: 0 }}>
					{isDownMd && <MobileMenuButton onOpen={onOpenDrawer} />}

					<Box role='navigation' sx={{ overflow: 'hidden', minWidth: 0 }}>
						<Breadcrumbs
							separator={
								<NavigateNextRounded fontSize='small' aria-hidden='true' sx={{ color: 'text.disabled' }} />
							}
							aria-label='breadcrumb'
							sx={{
								'& .MuiBreadcrumbs-ol': { alignItems: 'center' },
							}}
						>
							<ButtonBase
								onClick={() => navigate('/')}
								sx={{
									borderRadius: 1,
									px: 0.5,
									py: 0.25,
									display: 'inline-flex',
									alignItems: 'center',
									'&:focus-visible': {
										outline: `2px solid ${theme.palette.primary.main}`,
										outlineOffset: 2,
									},
								}}
							>
								<Typography
									variant='body2'
									sx={{
										color: location.pathname !== '/' ? 'text.blue.dark' : 'text.secondary',
										whiteSpace: 'nowrap',
									}}
								>
									Home
								</Typography>
							</ButtonBase>
							{crumbs.map((c) => (
								<ButtonBase
									key={c.path}
									onClick={() => navigate(c.path)}
									sx={{
										borderRadius: 1,
										px: 0.5,
										py: 0.25,
										display: 'inline-flex',
										alignItems: 'center',
										'&:focus-visible': {
											outline: `2px solid ${theme.palette.primary.main}`,
											outlineOffset: 2,
										},
									}}
								>
									<Typography
										variant='body2'
										noWrap
										title={c.label}
										sx={{ color: 'text.blue.dark', maxWidth: 'inherit' }}
									>
										{c.label}
									</Typography>
								</ButtonBase>
							))}
							<Typography
								variant='body2'
								noWrap
								title={currentLabel}
								sx={{
									px: 0.5,
									py: 0.25,
									lineHeight: '1.5rem',
								}}
							>
								{currentLabel}
							</Typography>
						</Breadcrumbs>
					</Box>
				</Stack>
				<Stack direction='row' alignItems='center' spacing={0.5}>
					<SwitchThemeButton />
					<SwitchLanguageButton />
					<UserAvatarMenu profile={profile} items={userMenuItems} onLogout={onLogout} />
				</Stack>
			</Toolbar>
		</AppBar>
	)
}

export default DashboardHeader

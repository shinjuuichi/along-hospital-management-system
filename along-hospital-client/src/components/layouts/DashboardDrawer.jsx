import { Box, Divider, Drawer, List, ListSubheader, Stack, useMediaQuery } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'
import { useCallback, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CollapseToggleButton from '../buttons/CollapseToggleButton'
import SystemLogoAndName from './commons/SystemLogoAndName'
import NavItemDashboard from './navItems/NavItemDashboard'
import SubmenuListDashboardPopover from './navItems/SubmenuListDashboardPopover'

const DashboardDrawer = ({ sections = [], mobileOpen = false, onMobileClose = () => {} }) => {
	const navigate = useNavigate()
	const theme = useTheme()
	const isMdUp = useMediaQuery(theme.breakpoints.up('md'))

	const [expanded, setExpanded] = useState({})
	const [collapsed, setCollapsed] = useState(false)
	const [popoverAnchor, setPopoverAnchor] = useState(null)
	const [popoverItem, setPopoverItem] = useState(null)
	const popoverOpen = Boolean(popoverAnchor) && !!popoverItem

	const drawerWidth = collapsed ? 80 : 300

	const handleToggleExpand = useCallback((key) => {
		setExpanded((prev) => ({ ...prev, [key]: !prev[key] }))
	}, [])

	const handleOpenPopover = useCallback((anchorEl, item) => {
		setPopoverAnchor(anchorEl)
		setPopoverItem(item)
	}, [])

	const handleClosePopover = useCallback(() => {
		setPopoverAnchor(null)
		setPopoverItem(null)
	}, [])

	const handleNavigate = useCallback(
		(url) => {
			navigate(url)
			onMobileClose?.()
		},
		[navigate, onMobileClose]
	)

	const handleToggleCollapse = () => {
		setCollapsed((prev) => !prev)
	}

	const header = (
		<Box
			role='banner'
			sx={{
				color: theme.palette.primary.contrastText,
				borderBottom: `1px solid ${alpha(theme.palette.primary.contrastText, 0.1)}`,
				px: 1.25,
				py: 1,
			}}
		>
			<Stack
				direction='row'
				alignItems='center'
				justifyContent={collapsed ? 'center' : 'flex-start'}
				spacing={1}
			>
				<SystemLogoAndName onClick={() => navigate('/')} onlyShowIcon={collapsed} />
			</Stack>
		</Box>
	)

	const body = (
		<Box role='navigation' sx={{ flex: 1, overflowY: 'auto' }}>
			{sections.map((section, idx) => (
				<Box key={`sec-${idx}`} sx={{ pt: idx === 0 ? 1 : 0, pb: 1 }}>
					<List
						dense
						disablePadding
						subheader={
							collapsed ? (
								<Divider
									aria-hidden
									sx={{
										my: 2,
										mx: 'auto',
										width: '50%',
										borderColor: theme.palette.divider,
									}}
								/>
							) : (
								<ListSubheader sx={{ lineHeight: 0, my: 2, position: 'relative' }}>
									{section.title}
								</ListSubheader>
							)
						}
					>
						{section.items.map((item, index) => (
							<NavItemDashboard
								key={item.key}
								item={item}
								collapsed={collapsed}
								expanded={expanded}
								onToggleExpand={handleToggleExpand}
								onOpenPopover={handleOpenPopover}
								onNavigate={handleNavigate}
								index={index}
							/>
						))}
					</List>
				</Box>
			))}
		</Box>
	)

	const footer = <CollapseToggleButton collapsed={collapsed} onClick={handleToggleCollapse} />

	const drawerContent = (isMobile = false) => (
		<Stack
			sx={{
				height: '100%',
				bgcolor: theme.palette.background.paper,
			}}
		>
			{header}
			<Divider />
			{body}
			{!isMobile && (
				<>
					<Divider />
					{footer}
				</>
			)}
		</Stack>
	)

	const popover = popoverItem && (
		<SubmenuListDashboardPopover
			open={popoverOpen}
			anchorEl={popoverAnchor}
			onClose={handleClosePopover}
			item={popoverItem}
			onNavigate={handleNavigate}
		/>
	)
	return (
		<Box component='nav' sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}>
			{isMdUp ? (
				<Drawer
					variant='permanent'
					open
					slotProps={{
						paper: {
							sx: {
								width: drawerWidth,
								boxSizing: 'border-box',
								borderRight: `1px solid ${theme.palette.divider}`,
								borderRadius: 0,
								transition: (t) =>
									t.transitions.create('width', { duration: t.transitions.duration.shorter }),
							},
						},
					}}
				>
					{drawerContent()}
					{popover}
				</Drawer>
			) : (
				<Drawer
					variant='temporary'
					open={mobileOpen}
					onClose={onMobileClose}
					ModalProps={{ keepMounted: true }}
					slotProps={{
						paper: {
							sx: {
								width: drawerWidth,
								boxSizing: 'border-box',
								borderRight: `1px solid ${theme.palette.divider}`,
							},
						},
					}}
				>
					{drawerContent(true)}
					{popover}
				</Drawer>
			)}
		</Box>
	)
}

export default DashboardDrawer

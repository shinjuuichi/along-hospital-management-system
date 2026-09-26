import { useTheme } from '@emotion/react'
import { ExpandLessRounded, ExpandMoreRounded } from '@mui/icons-material'
import { alpha, ListItemButton, ListItemIcon, ListItemText } from '@mui/material'
import React, { useRef } from 'react'
import { useLocation } from 'react-router-dom'
import SubmenuListDashboard from './SubmenuListDashboard'

const NavItemDashboard = ({
	item,
	collapsed,
	expanded,
	onToggleExpand,
	onOpenPopover,
	onNavigate,
}) => {
	const theme = useTheme()
	const location = useLocation()
	const buttonRef = useRef(null)
	const hasSubmenu = Array.isArray(item.of) && item.of.length > 0
	const selected = !!location.pathname && !hasSubmenu && item.url === location.pathname

	const handleActivate = () => {
		if (hasSubmenu) {
			if (collapsed) {
				onOpenPopover(buttonRef.current, item)
			} else {
				onToggleExpand(item.key)
			}
		} else if (item.url) {
			onNavigate(item.url)
		}
	}

	const onKeyDown = (e) => {
		if (e.key === 'Enter' || e.key === ' ') {
			e.preventDefault()
			handleActivate()
		}
	}

	return (
		<>
			<ListItemButton
				ref={buttonRef}
				onClick={handleActivate}
				onKeyDown={onKeyDown}
				tabIndex={0}
				sx={{
					mx: 1,
					my: 0.25,
					borderRadius: 1.5,
					py: 1.1,
					px: collapsed ? 1.25 : 1.5,
					transition: (t) =>
						t.transitions.create(['background-color', 'padding'], {
							duration: t.transitions.duration.shortest,
						}),
					...(selected && {
						bgcolor: alpha(theme.palette.primary.main, 0.12),
						'&:hover': { bgcolor: alpha(theme.palette.primary.main, 0.18) },
					}),
					'&:focus-visible': {
						outline: `2px solid ${alpha(theme.palette.primary.main, 0.6)}`,
						outlineOffset: 2,
					},
				}}
			>
				<ListItemIcon
					sx={{
						minWidth: collapsed ? 'auto' : 40,
						mx: collapsed ? 'auto' : 0,
						color: selected ? theme.palette.primary.main : theme.palette.grey[600],
					}}
				>
					{React.isValidElement(item.icon) ? item.icon : React.createElement(item.icon)}
				</ListItemIcon>

				{!collapsed && (
					<ListItemText
						primary={item.label}
						slotProps={{
							primary: {
								noWrap: true,
								sx: {
									transition: (t) =>
										t.transitions.create('opacity', { duration: t.transitions.duration.shortest }),
									color: selected ? theme.palette.text.primary : theme.palette.text.secondary,
									fontWeight: selected ? 600 : 500,
									fontSize: 14,
								},
							},
						}}
						sx={{ m: 0 }}
					/>
				)}

				{hasSubmenu &&
					!collapsed &&
					(expanded[item.key] ? (
						<ExpandLessRounded sx={{ color: theme.palette.grey[500] }} />
					) : (
						<ExpandMoreRounded sx={{ color: theme.palette.grey[500] }} />
					))}
			</ListItemButton>

			{hasSubmenu && !collapsed && expanded[item.key] && (
				<SubmenuListDashboard parentKey={item.key} items={item.of} onNavigate={onNavigate} />
			)}
		</>
	)
}

export default NavItemDashboard

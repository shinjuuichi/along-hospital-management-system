import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { ArrowDropDown, LogoutOutlined } from '@mui/icons-material'
import {
	Avatar,
	Box,
	ButtonBase,
	Divider,
	List,
	ListItemButton,
	ListItemIcon,
	ListItemText,
	Popover,
	Typography,
} from '@mui/material'
import React, { useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const UserAvatarMenu = ({ profile, items = [], onLogout }) => {
	const [open, setOpen] = useState(false)
	const anchorRef = useRef(null)
	const listRefs = useRef([])
	const navigate = useNavigate()
	const { t } = useTranslation()
	const name = profile?.name || 'Patient Name'
	const email = profile?.email || 'patient@example.com'
	const avatarUrl = profile?.image ? getImageFromCloud(profile.image) : null
	const initials = name
		.split(' ')
		.map((p) => p[0])
		.join('')
		.slice(0, 2)
		.toUpperCase()

	useEffect(() => {
		if (!open) listRefs.current = []
	}, [open])

	return (
		<>
			<ButtonBase
				ref={anchorRef}
				onClick={() => setOpen((o) => !o)}
				sx={{
					borderRadius: 999,
					border: '2px solid',
					borderColor: 'divider',
					pl: 0.5,
					pr: 1,
					py: 0.25,
					gap: 1,
					bgcolor: 'background.default',
				}}
			>
				<Avatar
					src={avatarUrl}
					alt={name}
					sx={{ width: 32, height: 32, bgcolor: 'primary.main', color: 'primary.contrastText' }}
				>
					{!avatarUrl && initials}
				</Avatar>
				<ArrowDropDown fontSize='small' />
			</ButtonBase>

			<Popover
				open={open}
				onClose={() => setOpen(false)}
				anchorEl={anchorRef.current}
				anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
				transformOrigin={{ vertical: 'top', horizontal: 'right' }}
				slotProps={{ paper: { sx: { mt: 1, borderRadius: 2, width: 300, maxWidth: '90vw' } } }}
			>
				<Box sx={{ p: 2, display: 'flex', gap: 1.5, alignItems: 'center' }}>
					<Avatar
						src={avatarUrl}
						alt={name}
						sx={{ width: 40, height: 40, bgcolor: 'primary.main', color: 'primary.contrastText' }}
					>
						{!avatarUrl && initials}
					</Avatar>
					<Box sx={{ minWidth: 0 }}>
						<Typography variant='subtitle2' sx={{ fontWeight: 700, lineHeight: 1.2 }} noWrap>
							{name}
						</Typography>
						<Typography variant='body2' color='text.secondary' noWrap>
							{email}
						</Typography>
					</Box>
				</Box>
				<Divider />
				<List role='menu' dense sx={{ py: 0.5 }}>
					{items.map((item, i) => (
						<ListItemButton
							key={item.label}
							role='menuitem'
							ref={(el) => (listRefs.current[i] = el)}
							onClick={() => {
								setOpen(false)
								navigate(item.url)
							}}
						>
							{item.icon ? (
								<ListItemIcon sx={{ minWidth: 36 }}>
									{React.isValidElement(item.icon) ? item.icon : React.createElement(item.icon)}
								</ListItemIcon>
							) : null}
							<ListItemText primary={item.label} />
						</ListItemButton>
					))}
				</List>
				<Divider />
				<List dense sx={{ py: 0.5 }}>
					<ListItemButton
						onClick={() => {
							setOpen(false)
							if (onLogout) onLogout()
							else navigate('/logout')
						}}
					>
						<ListItemIcon sx={{ minWidth: 36 }}>
							<LogoutOutlined />
						</ListItemIcon>
						<ListItemText primary={t('header.user_menu.logout')} />
					</ListItemButton>
				</List>
			</Popover>
		</>
	)
}

export default UserAvatarMenu

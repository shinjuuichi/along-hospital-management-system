import SearchBar from '@/components/generals/SearchBar'
import { Close } from '@mui/icons-material'
import {
	Box,
	Divider,
	Drawer,
	IconButton,
	List,
	ListItemButton,
	ListItemText,
	Stack,
} from '@mui/material'
import { useCallback } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import SystemLogoAndName from './commons/SystemLogoAndName'

const MobileDrawer = ({ items, openDrawer, setOpenDrawer }) => {
	const location = useLocation()
	const navigate = useNavigate()

	const go = useCallback(
		(url) => {
			navigate(url)
			setOpenDrawer(false)
		},
		[navigate]
	)

	return (
		<Drawer
			anchor='left'
			open={openDrawer}
			onClose={() => setOpenDrawer(false)}
			slotProps={{
				paper: { sx: { width: 320, borderTopRightRadius: 12, borderBottomRightRadius: 12 } },
			}}
		>
			<Box sx={{ p: 2 }}>
				<Stack direction='row' alignItems='center' justifyContent='space-between'>
					<Stack direction='row' alignItems='center' spacing={1.5}>
						<SystemLogoAndName onClick={() => go('/')} />
					</Stack>
					<IconButton onClick={() => setOpenDrawer(false)}>
						<Close />
					</IconButton>
				</Stack>
			</Box>
			<Divider />
			<Box sx={{ p: 2 }}>
				<Box sx={{ mb: 2 }}>
					<SearchBar />
				</Box>

				<List dense>
					{items.map((it, idx) => {
						const active = it.url === location.pathname
						return (
							<Box key={it.label + idx} sx={{ mb: 0.5 }}>
								<ListItemButton
									onClick={() => go(it.url)}
									sx={{
										borderRadius: 1.5,
										...(active && {
											bgcolor: 'primary.softBg',
											color: 'primary.main',
											'& .MuiTypography-root': { fontWeight: 700 },
										}),
									}}
								>
									<ListItemText primary={it.label} />
								</ListItemButton>

								{Array.isArray(it.of) && it.of.length > 0 && (
									<List sx={{ pl: 2, pt: 0.5 }} dense>
										{it.of.map((sub, i) => {
											const subActive = sub.url === location.pathname
											return (
												<ListItemButton
													key={sub.label + i}
													onClick={() => go(sub.url)}
													sx={{
														borderRadius: 1.25,
														...(subActive && {
															bgcolor: 'primary.softBg',
															color: 'primary.main',
															'& .MuiTypography-root': { fontWeight: 700 },
														}),
													}}
												>
													<ListItemText primary={sub.label} />
												</ListItemButton>
											)
										})}
									</List>
								)}
							</Box>
						)
					})}
				</List>

				<Divider sx={{ my: 1.5 }} />
			</Box>
		</Drawer>
	)
}

export default MobileDrawer

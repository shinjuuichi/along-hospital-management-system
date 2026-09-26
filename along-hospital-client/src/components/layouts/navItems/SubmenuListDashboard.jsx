import {
	alpha,
	List,
	ListItemButton,
	ListItemIcon,
	ListItemText,
	ListSubheader,
	useTheme,
} from '@mui/material'
import { useLocation } from 'react-router-dom'

const SubmenuListDashboard = ({ parentKey, title, items = [], onNavigate, onClose }) => {
	const theme = useTheme()
	const location = useLocation()

	return (
		<List
			role='group'
			id={`submenu-${parentKey}`}
			dense
			subheader={title && <ListSubheader sx={{ my: 2, lineHeight: 0 }}>{title}</ListSubheader>}
			sx={{ py: 0, my: 0 }}
		>
			{items.map((sub) => {
				const currentPath = location.pathname
				const selected = !!currentPath && sub.url === currentPath
				return (
					<ListItemButton
						key={sub.key}
						onClick={() => {
							if (sub.url) onNavigate(sub.url)
							onClose?.()
						}}
						sx={{
							pl: 5.5,
							py: 1,
							borderRadius: 1.5,
							mx: 1,
							my: 0.25,
							...(selected && {
								bgcolor: alpha(theme.palette.primary.main, 0.08),
								'&:hover': { bgcolor: alpha(theme.palette.primary.main, 0.12) },
							}),
						}}
					>
						{sub.icon ? (
							<ListItemIcon
								sx={{
									minWidth: 32,
									color: selected ? theme.palette.primary.main : theme.palette.grey[600],
								}}
							>
								{sub.icon}
							</ListItemIcon>
						) : null}
						<ListItemText
							primary={sub.label}
							slotProps={{
								primary: {
									fontSize: 13.5,
									color: 'text.secondary',
								},
							}}
						/>
					</ListItemButton>
				)
			})}
		</List>
	)
}

export default SubmenuListDashboard

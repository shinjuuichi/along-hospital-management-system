import { Box, Popover, useTheme } from '@mui/material'
import SubmenuListDashboard from './SubmenuListDashboard'

const SubmenuListDashboardPopover = ({ open, anchorEl, onClose, item, onNavigate }) => {
	const theme = useTheme()

	return (
		<Popover
			open={open}
			anchorEl={anchorEl}
			onClose={onClose}
			anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
			transformOrigin={{ vertical: 'top', horizontal: 'left' }}
			slotProps={{
				paper: {
					elevation: 8,
					sx: {
						mt: -0.5,
						ml: 1,
						borderRadius: 1,
						minWidth: 220,
						border: `1px solid ${theme.palette.divider}`,
					},
				},
			}}
			disableAutoFocus={false}
			disableRestoreFocus={false}
		>
			<Box role='dialog' sx={{ py: 0.75 }}>
				<SubmenuListDashboard
					parentKey={item?.key}
					title={item?.label}
					items={item?.of}
					onNavigate={onNavigate}
					onClose={onClose}
				/>
			</Box>
		</Popover>
	)
}

export default SubmenuListDashboardPopover

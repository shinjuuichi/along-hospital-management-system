import useTranslation from '@/hooks/useTranslation'
import { MoreHoriz } from '@mui/icons-material'
import { IconButton, ListItemIcon, ListItemText, Menu, MenuItem, Tooltip } from '@mui/material'
import { useState } from 'react'

/**
 * @param {Object} props
 * @param {Array<{title?: string, icon?: ReactNode, disabled?: boolean, onClick?: () => void | Promise<void>}>} props.actions
 * @param {import('@mui/material').SxProps} props.itemSx
 * @param {string} props.menuTooltip
 */
const ActionMenu = ({ actions = [], itemSx = {}, menuTooltip }) => {
	const [anchorEl, setAnchorEl] = useState(null)
	const { t } = useTranslation()

	const open = Boolean(anchorEl)
	const close = () => setAnchorEl(null)

	const makeRun = (a) => async () => {
		close()
		await a.onClick?.()
	}

	if (actions.length === 0) return null

	return (
		<>
			<Tooltip title={menuTooltip || t('tooltip.menu')}>
				<IconButton
					size='small'
					onClick={(e) => setAnchorEl(e.currentTarget)}
					aria-controls={open ? 'row-action-menu' : undefined}
					aria-haspopup='menu'
					aria-expanded={open ? 'true' : undefined}
				>
					<MoreHoriz />
				</IconButton>
			</Tooltip>

			<Menu
				id='row-action-menu'
				anchorEl={anchorEl}
				open={open}
				onClose={close}
				anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
				transformOrigin={{ vertical: 'top', horizontal: 'right' }}
			>
				{actions.map((a, idx) => {
					if (!a) return null
					const run = makeRun(a)
					return (
						<MenuItem key={idx} onClick={run} disabled={a.disabled} sx={itemSx}>
							{a.icon && <ListItemIcon>{a.icon}</ListItemIcon>}
							<ListItemText>{a.title}</ListItemText>
						</MenuItem>
					)
				})}
			</Menu>
		</>
	)
}

export default ActionMenu

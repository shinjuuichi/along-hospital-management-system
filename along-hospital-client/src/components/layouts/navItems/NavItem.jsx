import { ArrowDropDown } from '@mui/icons-material'
import { ButtonBase, List, ListItemButton, ListItemText, Popover, Typography } from '@mui/material'
import { useCallback, useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'

export default function NavItem({ label, url, of = [] }) {
	const hasSub = Array.isArray(of) && of.length > 0
	const [open, setOpen] = useState(false)
	const anchorRef = useRef(null)
	const listRefs = useRef([])

	const navigate = useNavigate()

	const close = useCallback(() => setOpen(false), [])
	const toggle = useCallback(() => setOpen((o) => !o), [])

	const defaultTextColor = 'text.primary'

	useEffect(() => {
		if (!open) listRefs.current = []
	}, [open])

	const onKeyDown = (e) => {
		if (!hasSub) return
		if (e.key === 'ArrowDown') {
			e.preventDefault()
			setOpen(true)
			setTimeout(() => listRefs.current[0]?.focus(), 0)
		} else if (e.key === 'Enter' || e.key === ' ') {
			e.preventDefault()
			toggle()
		}
	}

	return (
		<>
			<ButtonBase
				ref={anchorRef}
				onClick={() => (hasSub ? toggle() : navigate(url))}
				onKeyDown={onKeyDown}
				aria-haspopup={hasSub ? 'menu' : undefined}
				aria-expanded={hasSub ? (open ? 'true' : 'false') : undefined}
				aria-controls={hasSub && open ? `submenu-${label}` : undefined}
				sx={{
					px: 1.5,
					py: 1,
					borderRadius: 1.5,
					':hover': { bgcolor: 'action.hover' },
					':focus-visible': { outline: '3px solid', outlineColor: defaultTextColor },
				}}
			>
				<Typography variant='body2' sx={{ fontWeight: 500, color: defaultTextColor }}>
					{label}
				</Typography>
				{hasSub && <ArrowDropDown fontSize='small' sx={{ ml: 0.25, color: defaultTextColor }} />}
			</ButtonBase>

			{hasSub && (
				<Popover
					open={open}
					anchorEl={anchorRef.current}
					onClose={close}
					anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
					transformOrigin={{ vertical: 'top', horizontal: 'left' }}
					slotProps={{ paper: { sx: { mt: 1, borderRadius: 1, width: 220 } } }}
				>
					<List
						id={`submenu-${label}`}
						role='menu'
						dense
						onKeyDown={(e) => {
							const idx = listRefs.current.findIndex((el) => el === document.activeElement)
							if (e.key === 'Escape') {
								e.preventDefault()
								close()
								anchorRef.current?.focus()
							} else if (e.key === 'ArrowDown') {
								e.preventDefault()
								const next = (idx + 1) % listRefs.current.length
								listRefs.current[next]?.focus()
							} else if (e.key === 'ArrowUp') {
								e.preventDefault()
								const prev = (idx - 1 + listRefs.current.length) % listRefs.current.length
								listRefs.current[prev]?.focus()
							}
						}}
					>
						{of.map((sub, i) => (
							<ListItemButton
								key={sub.label + i}
								role='menuitem'
								ref={(el) => (listRefs.current[i] = el)}
								onClick={() => {
									close()
									navigate(sub.url)
								}}
							>
								<ListItemText primary={sub.label} sx={{ color: defaultTextColor }} />
							</ListItemButton>
						))}
					</List>
				</Popover>
			)}
		</>
	)
}

import useTranslation from '@/hooks/useTranslation'
import { ArrowDropDown, Language } from '@mui/icons-material'
import { ButtonBase, List, ListItemButton, ListItemText, Popover, Typography } from '@mui/material'
import { useRef, useState } from 'react'

const SwitchLanguageButton = () => {
	const [open, setOpen] = useState(false)
	const anchorRef = useRef(null)
	const { language, setLanguage } = useTranslation()

	const handleToggle = () => setOpen((o) => !o)
	const handleClose = () => setOpen(false)

	return (
		<>
			<ButtonBase
				ref={anchorRef}
				aria-haspopup='listbox'
				aria-expanded={open ? 'true' : undefined}
				onClick={handleToggle}
				sx={{
					px: 1.25,
					py: 0.75,
					borderRadius: 999,
					border: '2px solid',
					borderColor: 'divider',
					bgcolor: 'background.default',
					gap: 1,
				}}
			>
				<Language />
				<Typography variant='body2' sx={{ fontWeight: 600, textTransform: 'uppercase' }}>
					{language}
				</Typography>
				<ArrowDropDown fontSize='small' />
			</ButtonBase>

			<Popover
				open={open}
				onClose={handleClose}
				anchorEl={anchorRef.current}
				anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
				transformOrigin={{ vertical: 'top', horizontal: 'right' }}
				slotProps={{ paper: { sx: { mt: 1, borderRadius: 2 } } }}
			>
				<List role='listbox' sx={{ minWidth: 220, py: 0.5 }} dense>
					<ListItemButton role='option' selected={language === 'en'} onClick={() => setLanguage('en')}>
						<ListItemText primary='English (EN)' />
					</ListItemButton>
					<ListItemButton role='option' selected={language === 'vi'} onClick={() => setLanguage('vi')}>
						<ListItemText primary='Tiếng Việt (VI)' />
					</ListItemButton>
				</List>
			</Popover>
		</>
	)
}

export default SwitchLanguageButton

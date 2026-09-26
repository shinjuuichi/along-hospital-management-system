import SearchBar from '@/components/generals/SearchBar'
import useTranslation from '@/hooks/useTranslation'
import {
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	List,
	ListItem,
	ListItemButton,
	ListItemIcon,
	ListItemText,
	Paper,
	Radio,
} from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const SelectDialog = ({
	open = false,
	onClose,
	options = [],
	value = '',
	onChange,
	renderOption,
	title = '',
}) => {
	const { t } = useTranslation()
	const [searchTerm, setSearchTerm] = useState('')
	const [selectedValue, setSelectedValue] = useState(value)

	useEffect(() => {
		setSelectedValue(value)
	}, [value])

	const filteredOptions = useMemo(() => {
		if (!searchTerm.trim()) return options || []

		const lowerSearch = searchTerm.toLowerCase()
		return (options || []).filter((opt) => {
			const label = String(opt.searchKey || opt.label || '').toLowerCase()
			return label.includes(lowerSearch)
		})
	}, [options, searchTerm])

	const handleSelect = (optionValue) => {
		setSelectedValue(optionValue)
	}

	const handleConfirm = () => {
		onChange?.(selectedValue)
		onClose?.()
	}

	const handleCancel = () => {
		setSelectedValue(value)
		onClose?.()
	}

	return (
		<Dialog open={open} onClose={handleCancel} maxWidth='sm' fullWidth>
			{title && <DialogTitle>{title}</DialogTitle>}
			<DialogContent dividers sx={{ display: 'flex', flexGrow: 1, gap: 2, p: 2 }}>
				<Box sx={{ flex: 1, display: 'flex', flexDirection: 'column', minWidth: 0, gap: 1 }}>
					<SearchBar value={searchTerm} setValue={setSearchTerm} />
					<Box
						variant='outlined'
						component={Paper}
						sx={() => ({
							flex: 1,
							overflow: 'auto',
							minHeight: 200,
						})}
					>
						<List sx={{ p: 0 }}>
							{filteredOptions.map((opt) => {
								const checked = String(selectedValue) === String(opt.value)
								return (
									<ListItem key={String(opt.value)} disablePadding divider disabled={opt.disabled}>
										<ListItemButton onClick={() => handleSelect(opt.value)} disabled={opt.disabled} dense>
											<ListItemIcon sx={{ minWidth: 40 }}>
												<Radio
													edge='start'
													checked={checked}
													tabIndex={-1}
													disableRipple
													disabled={opt.disabled}
												/>
											</ListItemIcon>
											<ListItemText primary={renderOption ? renderOption(opt.value, opt.label) : opt.label} />
										</ListItemButton>
									</ListItem>
								)
							})}
						</List>
					</Box>
				</Box>
			</DialogContent>
			<DialogActions>
				<Button onClick={handleCancel} variant='outlined'>
					{t('button.cancel')}
				</Button>
				<Button onClick={handleConfirm} variant='contained'>
					{t('button.confirm')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default SelectDialog

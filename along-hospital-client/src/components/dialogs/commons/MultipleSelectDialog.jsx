import SearchBar from '@/components/generals/SearchBar'
import useTranslation from '@/hooks/useTranslation'
import {
	Box,
	Button,
	Checkbox,
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
} from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const MultipleSelectDialog = ({
	open = false,
	onClose,
	options = [],
	value = [],
	onChange,
	renderOption,
	title = '',
}) => {
	const { t } = useTranslation()
	const [searchTerm, setSearchTerm] = useState('')
	const [selectedValues, setSelectedValues] = useState([])

	useEffect(() => {
		setSelectedValues(Array.isArray(value) ? value : [])
	}, [value])

	const selectedSet = useMemo(() => new Set(selectedValues.map((v) => String(v))), [selectedValues])

	const unselectedOptions = useMemo(() => {
		return (options || []).filter((opt) => !selectedSet.has(String(opt.value)))
	}, [options, selectedSet])

	const selectedOptions = useMemo(() => {
		return (options || []).filter((opt) => selectedSet.has(String(opt.value)))
	}, [options, selectedSet])

	const filteredUnselectedOptions = useMemo(() => {
		if (!searchTerm.trim()) return unselectedOptions

		const lowerSearch = searchTerm.toLowerCase()
		return unselectedOptions.filter((opt) => {
			const label = String(opt.searchKey || opt.label || '').toLowerCase()
			return label.includes(lowerSearch)
		})
	}, [unselectedOptions, searchTerm])

	const disabledValueSet = useMemo(
		() =>
			new Set(
				(options || []).filter((opt) => opt.disabled).map((opt) => String(opt.value))
			),
		[options]
	)

	const selectableUnselectedOptions = useMemo(
		() => filteredUnselectedOptions.filter((opt) => !opt.disabled),
		[filteredUnselectedOptions]
	)

	const handleToggle = (optionValue) => {
		const strValue = String(optionValue)
		if (disabledValueSet.has(strValue)) return

		if (selectedSet.has(strValue)) {
			setSelectedValues((prev) => prev.filter((v) => String(v) !== strValue))
		} else {
			setSelectedValues((prev) => [...prev, optionValue])
		}
	}

	const handleSelectAll = () => {
		setSelectedValues((prev) => [...prev, ...selectableUnselectedOptions.map((opt) => opt.value)])
	}

	const handleDeselectAll = () => {
		setSelectedValues([])
	}

	const handleConfirm = () => {
		onChange?.(selectedValues)
		onClose?.()
	}

	const handleCancel = () => {
		setSelectedValues(Array.isArray(value) ? value : [])
		onClose?.()
	}

	return (
		<Dialog open={open} onClose={handleCancel} maxWidth={'lg'} fullWidth>
			{title && <DialogTitle>{title}</DialogTitle>}
			<DialogContent dividers sx={{ display: 'flex', flexGrow: 1, gap: 2, p: 2 }}>
				<Box sx={{ flex: 1, display: 'flex', flexDirection: 'column', minWidth: 0, gap: 1 }}>
					<SearchBar size='small' fullWidth value={searchTerm} setValue={setSearchTerm} sx={{ mb: 1 }} />
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
							{filteredUnselectedOptions.map((opt) => (
								<ListItem key={String(opt.value)} disablePadding divider disabled={opt.disabled}>
									<ListItemButton onClick={() => handleToggle(opt.value)} disabled={opt.disabled} dense>
										<ListItemIcon sx={{ minWidth: 40 }}>
											<Checkbox
												edge='start'
												checked={false}
												tabIndex={-1}
												disableRipple
												disabled={opt.disabled}
											/>
										</ListItemIcon>
										<ListItemText primary={renderOption ? renderOption(opt.value, opt.label) : opt.label} />
									</ListItemButton>
								</ListItem>
							))}
						</List>
					</Box>
					<Box sx={{ display: 'flex', gap: 1 }}>
						<Button onClick={handleSelectAll} variant='contained' color='primary' size='small'>
							{t('button.select_all')}
						</Button>
						<Button onClick={handleDeselectAll} variant='outlined' color='secondary' size='small'>
							{t('button.deselect_all')}
						</Button>
					</Box>
				</Box>

				<Box sx={{ flex: 1, display: 'flex', flexDirection: 'column', minWidth: 0 }}>
					<Box
						component={Paper}
						variant='outlined'
						sx={{
							flex: 1,
							overflow: 'auto',
							minHeight: 200,
						}}
					>
						<List sx={{ p: 0 }}>
							{selectedOptions.map((opt) => (
								<ListItem key={String(opt.value)} disablePadding divider>
									<ListItemButton onClick={() => handleToggle(opt.value)} dense>
										<ListItemIcon sx={{ minWidth: 40 }}>
											<Checkbox edge='start' checked={true} tabIndex={-1} disableRipple />
										</ListItemIcon>
										<ListItemText primary={renderOption ? renderOption(opt.value, opt.label) : opt.label} />
									</ListItemButton>
								</ListItem>
							))}
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

export default MultipleSelectDialog

import useTranslation from '@/hooks/useTranslation'
import { Close, Search } from '@mui/icons-material'
import { Autocomplete, IconButton, InputAdornment, TextField } from '@mui/material'

const SearchBar = ({
	widthPercent = 100,
	value,
	setValue,
	placeholder,
	options = [], // if provided, enables autocomplete
	getOptionLabel = (opt) => opt?.label || opt,
	onEnterDown = () => {},
}) => {
	const { t } = useTranslation()
	const isAutocomplete = Array.isArray(options) && options.length > 0

	const commonProps = {
		sx: widthPercent !== 0 ? { width: widthPercent + '%' } : {},
		size: 'small',
		placeholder: placeholder || t('text.search'),
	}

	return isAutocomplete ? (
		<Autocomplete
			freeSolo
			fullWidth
			options={options}
			value={value}
			onChange={(_, newValue) => setValue(newValue)}
			getOptionLabel={getOptionLabel}
			onKeyDown={(e) => {
				if (e.key === 'Enter') {
					e.preventDefault()
					onEnterDown?.()
				}
			}}
			renderInput={(params) => (
				<TextField
					{...params}
					{...commonProps}
					sx={{ width: '100%' }}
					InputProps={{
						...params.InputProps,
						startAdornment: (
							<InputAdornment position='start' sx={{ ml: 1 }}>
								<Search />
							</InputAdornment>
						),
						disableUnderline: true,
					}}
				/>
			)}
		/>
	) : (
		<TextField
			fullWidth
			{...commonProps}
			value={value}
			onChange={(e) => setValue(e.target.value)}
			onKeyDown={(e) => {
				if (e.key === 'Enter') {
					e.preventDefault()
					onEnterDown?.()
				}
			}}
			slotProps={{
				input: {
					startAdornment: (
						<InputAdornment position='start'>
							<Search />
						</InputAdornment>
					),
					endAdornment: value && value !== '' && (
						<InputAdornment position='end'>
							<IconButton size='small' disableTouchRipple onClick={() => setValue('')} sx={{ mr: -1 }}>
								<Close sx={{ fontSize: '1.25rem' }} />
							</IconButton>
						</InputAdornment>
					),
				},
			}}
		/>
	)
}

export default SearchBar

// Usage example:

/*
<SearchBar
	widthPercent={50}
	value={searchTerm}
	setValue={setSearchTerm}
	placeholder="Search items..."
/>
*/

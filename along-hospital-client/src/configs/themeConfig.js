import { alpha, createTheme } from '@mui/material'
import { darkPalette, lightPalette } from './themePaletteConfig'

const customTheme = (palette, otherComponents) => ({
	palette: palette,
	typography: {
		fontFamily: 'lexend, sans-serif',
	},
	shape: { borderRadius: 12 },
	components: {
		MuiCssBaseline: {
			styleOverrides: (theme) => ({
				'::-webkit-scrollbar': {
					width: 8,
				},
				'::-webkit-scrollbar-thumb': {
					backgroundColor: alpha(theme.palette.text.primary, 0.1),
					borderRadius: 4,
				},
				'::-webkit-scrollbar-thumb:hover': {
					backgroundColor: alpha(theme.palette.text.primary, 0.3),
				},
				'::-webkit-scrollbar-corner': {
					background: 'transparent',
				},
				body: {
					wordBreak: 'break-word',
					overflowWrap: 'anywhere',
					hyphens: 'auto',
				},
			}),
		},
		MuiPaper: { styleOverrides: { root: { borderRadius: 12 } } },
		MuiButtonBase: { defaultProps: { disableRipple: true } },
		MuiDivider: { styleOverrides: { root: { borderColor: palette.divider } } },
		MuiIconButton: { styleOverrides: { root: { color: palette.text.primary } } },
		MuiFormLabel: {
			styleOverrides: {
				root: {
					color: palette.text.secondary,
					opacity: 0.9,
					'&.Mui-focused': { color: palette.primary.main, opacity: 1 },
					'&.Mui-error': { color: palette.error.main, opacity: 1 },
					'&.Mui-disabled': {
						color: palette.text.secondary,
						opacity: 0.7,
					},
				},
			},
		},
		MuiFormControl: {
			styleOverrides: {
				root: {
					'&:has(.MuiOutlinedInput-root input[readonly]) .MuiInputLabel-root': {
						color: palette.info.main,
						opacity: 1,
					},
				},
			},
		},
		MuiInputBase: {
			styleOverrides: {
				root: {
					'&.Mui-disabled': {
						opacity: 1,
						backgroundColor:
							palette.action?.disabledBackground ||
							alpha(palette.mode === 'dark' ? '#0F172A' : '#F3F4F6', 0.6),
					},
					'&.Mui-disabled .MuiInputBase-input': {
						color: palette.text.secondary,
						WebkitTextFillColor: palette.text.secondary,
					},
				},
				input: {
					'&.Mui-disabled': {
						color: palette.text.secondary,
						WebkitTextFillColor: palette.text.secondary,
					},
					'&[readonly]': {
						backgroundColor: alpha(palette.primary.softBg, 0.5),
						borderRadius: 'inherit',
						cursor: 'default',
						caretColor: 'transparent',
					},
				},
			},
		},

		MuiOutlinedInput: {
			styleOverrides: {
				root: {
					'&.Mui-disabled .MuiOutlinedInput-notchedOutline': {
						borderColor: palette.divider,
					},
					'&:has(input[readonly])': {
						backgroundColor: palette.mode === 'dark' ? 'rgba(255,255,255,0.03)' : '#F8FAFF',
					},
					'&:has(input[readonly]) .MuiOutlinedInput-notchedOutline': {
						borderColor: palette.primary.main,
					},
				},
				input: {
					'&[readonly]': {
						color: palette.text.primary,
					},
				},
			},
		},
		...otherComponents,
	},
})

export const hospitalLightTheme = createTheme({
	...customTheme(lightPalette, {
		MuiAppBar: {
			styleOverrides: {
				root: {
					backgroundColor: 'rgba(255,255,255,0.6)',
					backdropFilter: 'saturate(180%) blur(8px)',
					borderBottom: '1px solid',
					borderColor: lightPalette.divider,
					borderRadius: 0,
				},
			},
		},
	}),
})

export const hospitalDarkTheme = createTheme({
	...customTheme(darkPalette, {
		MuiAppBar: {
			styleOverrides: {
				root: {
					backgroundColor: 'rgba(15,23,42,0.6)',
					backdropFilter: 'saturate(180%) blur(8px)',
					borderBottom: '1px solid',
					borderColor: darkPalette.divider,
					borderRadius: 0,
				},
			},
		},
		MuiTextField: {
			variants: [
				{
					props: { type: 'date' },
					style: {
						'& input::-webkit-calendar-picker-indicator': { filter: 'invert(1)' },
					},
				},
				{
					props: { type: 'time' },
					style: {
						'& input::-webkit-calendar-picker-indicator': {
							filter: 'invert(1)',
							opacity: 0.9,
						},
						'& input::-webkit-clear-button': { display: 'none' },
						'& input::-webkit-datetime-edit': { color: 'inherit' },
					},
				},
			],
		},
	}),
})

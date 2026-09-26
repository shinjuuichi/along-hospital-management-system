import { useLocalStorage } from '@/hooks/useStorage'
import useTranslation from '@/hooks/useTranslation'
import { DarkMode, LightMode } from '@mui/icons-material'
import { IconButton, Tooltip } from '@mui/material'

const SwitchThemeButton = () => {
	const [theme, setTheme] = useLocalStorage('theme', 'light')
	const { t } = useTranslation()

	return (
		<Tooltip title={t(theme === 'light' ? 'tooltip.switch_dark' : 'tooltip.switch_light')}>
			<IconButton
				onClick={() => setTheme(theme === 'light' ? 'dark' : 'light')}
				sx={{ borderRadius: 2 }}
			>
				{theme === 'light' ? <LightMode /> : <DarkMode />}
			</IconButton>
		</Tooltip>
	)
}

export default SwitchThemeButton

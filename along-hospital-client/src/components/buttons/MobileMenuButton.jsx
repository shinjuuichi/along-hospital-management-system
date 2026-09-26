import useTranslation from '@/hooks/useTranslation'
import { Menu } from '@mui/icons-material'
import { IconButton, Tooltip } from '@mui/material'

export default function MobileMenuButton({ onOpen }) {
	const { t } = useTranslation()
	return (
		<Tooltip title={t('tooltip.menu')}>
			<IconButton
				onClick={onOpen}
				sx={{
					display: 'inline-flex',
					border: (theme) => `1px solid ${theme.palette.divider}`,
				}}
			>
				<Menu />
			</IconButton>
		</Tooltip>
	)
}

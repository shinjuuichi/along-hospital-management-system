import useTranslation from '@/hooks/useTranslation'
import { useTheme } from '@emotion/react'
import { ChevronLeftRounded, ChevronRightRounded } from '@mui/icons-material'
import { alpha, Box, IconButton, Tooltip } from '@mui/material'

const CollapseToggleButton = ({ collapsed, onClick }) => {
	const theme = useTheme()
	const { t } = useTranslation()

	return (
		<Box
			sx={{
				position: 'relative',
				px: collapsed ? 0 : 1,
				py: 1,
				display: 'flex',
				alignItems: 'center',
				justifyContent: collapsed ? 'center' : 'flex-end',
				bgcolor: theme.palette.background.paper,
			}}
		>
			<Tooltip title={collapsed ? t('tooltip.expand') : t('tooltip.collapse')} placement='top'>
				<IconButton
					onClick={onClick}
					size='small'
					sx={{
						border: `1px solid ${theme.palette.divider}`,
						bgcolor: theme.palette.background.paper,
						'&:hover': {
							bgcolor: alpha(theme.palette.primary.main, 0.06),
							borderColor: alpha(theme.palette.primary.main, 0.4),
						},
					}}
				>
					{collapsed ? <ChevronRightRounded /> : <ChevronLeftRounded />}
				</IconButton>
			</Tooltip>
		</Box>
	)
}
export default CollapseToggleButton

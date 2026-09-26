import { Box, List, ListItemButton, Typography } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'

const FooterSection = ({ title, links = [], onNavigate }) => {
	const theme = useTheme()

	return (
		<Box>
			<Typography variant='subtitle1' sx={{ fontWeight: 700, mb: 1, color: 'text.primary' }}>
				{title}
			</Typography>

			<List
				sx={{
					py: 0,
					width: 'max-content',
					'& .MuiListItemButton-root': {
						px: 0,
						py: 0.5,
						minHeight: 36,
						justifyContent: 'flex-start',
						textAlign: 'left',
						color: 'text.primary',
						opacity: 0.9,
						borderRadius: 1,
						transition: 'all .15s ease',
						'&:hover': {
							opacity: 1,
							textDecoration: 'underline',
							textUnderlineOffset: '3px',
							bgcolor: 'transparent',
						},
						'&:focus-visible': {
							outline: `2px solid ${alpha(theme.palette.text.primary, 0.8)}`,
							outlineOffset: '2px',
						},
					},
				}}
			>
				{links.map((item) => (
					<ListItemButton
						disableGutters
						disableRipple
						key={item.label + item.url}
						onClick={() => onNavigate(item.url)}
					>
						<Typography variant='body2' sx={{ color: 'inherit' }}>
							{item.label}
						</Typography>
					</ListItemButton>
				))}
			</List>
		</Box>
	)
}

export default FooterSection

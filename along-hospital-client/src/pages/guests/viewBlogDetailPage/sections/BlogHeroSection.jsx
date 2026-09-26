import { Box, Container, Stack, Typography } from '@mui/material'

const BlogHeroSection = ({ headerTitle, title, subtitle = '' }) => (
	<Box
		sx={{
			position: 'relative',
			background: (theme) =>
				`linear-gradient(135deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
			color: 'common.white',
			py: { xs: 3, md: 4 },
			mb: { xs: 3, md: 5 },
		}}
	>
		<Container maxWidth='lg'>
			<Stack spacing={1.25}>
				<Typography variant='overline' sx={{ letterSpacing: 2 }}>
					{headerTitle}
				</Typography>
				<Typography variant='h5' sx={{ fontWeight: 600, lineHeight: 1.5 }}>
					{title}
				</Typography>
				{subtitle && (
					<Typography variant='body2' sx={{ maxWidth: 720, opacity: 0.85 }}>
						{subtitle}
					</Typography>
				)}
			</Stack>
		</Container>
	</Box>
)

export default BlogHeroSection

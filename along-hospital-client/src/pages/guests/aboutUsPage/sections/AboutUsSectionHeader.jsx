import { Chip, Stack, Typography } from '@mui/material'

const AboutUsSectionHeader = ({ eyebrow, title, subtitle, align = 'left' }) => {
	const isCentered = align === 'center'

	return (
		<Stack spacing={1.25} alignItems={isCentered ? 'center' : 'flex-start'} textAlign={align}>
			{eyebrow ? (
				<Chip
					label={eyebrow}
					color='secondary'
					variant='outlined'
					sx={{ borderRadius: 999, fontWeight: 700 }}
				/>
			) : null}
			<Typography
				variant='h3'
				sx={{
					fontWeight: 800,
					fontSize: { xs: '1.85rem', md: '2.45rem' },
					lineHeight: 1.08,
					maxWidth: 760,
				}}
			>
				{title}
			</Typography>
			{subtitle ? (
				<Typography variant='body1' color='text.secondary' sx={{ maxWidth: 720 }}>
					{subtitle}
				</Typography>
			) : null}
		</Stack>
	)
}

export default AboutUsSectionHeader

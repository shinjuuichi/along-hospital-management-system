import { ArrowForward } from '@mui/icons-material'
import { Button, Chip, Stack, Typography } from '@mui/material'

const HomeSectionHeader = ({
	eyebrow,
	title,
	subtitle,
	actionLabel,
	onAction,
	align = 'left',
	actionVariant = 'outlined',
}) => {
	const isCentered = align === 'center'

	return (
		<Stack
			direction={{ xs: 'column', md: 'row' }}
			spacing={2}
			alignItems={isCentered ? 'center' : { md: 'flex-end' }}
			justifyContent='space-between'
			textAlign={align}
		>
			<Stack spacing={1.25} alignItems={isCentered ? 'center' : 'flex-start'}>
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
						maxWidth: 720,
					}}
				>
					{title}
				</Typography>
				{subtitle ? (
					<Typography variant='body1' color='text.secondary' sx={{ maxWidth: 700 }}>
						{subtitle}
					</Typography>
				) : null}
			</Stack>

			{actionLabel && onAction ? (
				<Button
					variant={actionVariant}
					color='primary'
					endIcon={<ArrowForward />}
					onClick={onAction}
					sx={{
						flexShrink: 0,
						borderRadius: 999,
						px: 2.5,
						py: 1.15,
						fontWeight: 700,
						textTransform: 'none',
						alignSelf: { xs: isCentered ? 'center' : 'flex-start', md: 'center' },
					}}
				>
					{actionLabel}
				</Button>
			) : null}
		</Stack>
	)
}

export default HomeSectionHeader

import useTranslation from '@/hooks/useTranslation'
import { alpha, Box, Button, Chip, Container, Stack, Typography, useTheme } from '@mui/material'
import { motion } from 'framer-motion'

const AboutUsHeroSection = ({
	badges = [],
	onBookAppointment,
	onOrderMedicine,
	shouldReduceMotion,
}) => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Box
			component='section'
			sx={{
				position: 'relative',
				minHeight: { xs: 560, md: 640 },
				marginLeft: 'calc(50% - 50vw)',
				marginRight: 'calc(50% - 50vw)',
				overflow: 'hidden',
			}}
		>
			<Box
				component={motion.div}
				initial={shouldReduceMotion ? false : { scale: 1 }}
				animate={shouldReduceMotion ? { scale: 1 } : { scale: 1.08 }}
				transition={
					shouldReduceMotion
						? undefined
						: {
								duration: 16,
								ease: 'easeInOut',
								repeat: Infinity,
								repeatType: 'mirror',
							}
				}
				sx={{
					position: 'absolute',
					inset: 0,
					backgroundImage: 'url(/about/hero.png)',
					backgroundPosition: 'center',
					backgroundRepeat: 'no-repeat',
					backgroundSize: 'cover',
				}}
			/>

			<Box
				sx={{
					position: 'absolute',
					inset: 0,
					background:
						theme.palette.mode === 'dark'
							? 'linear-gradient(90deg, rgba(11,18,32,0.88) 0%, rgba(11,18,32,0.72) 45%, rgba(11,18,32,0.42) 100%)'
							: 'linear-gradient(90deg, rgba(15,23,42,0.74) 0%, rgba(15,23,42,0.5) 42%, rgba(15,23,42,0.2) 100%)',
				}}
			/>

			<Box
				sx={{
					position: 'absolute',
					inset: 0,
					background:
						theme.palette.mode === 'dark'
							? 'radial-gradient(circle at 80% 20%, rgba(79,209,197,0.14) 0%, transparent 28%)'
							: 'radial-gradient(circle at 82% 18%, rgba(0,191,166,0.22) 0%, transparent 28%)',
				}}
			/>

			<Container
				maxWidth='xl'
				sx={{
					position: 'relative',
					zIndex: 1,
					height: '100%',
					px: { xs: 2, sm: 3, md: 4, lg: 6 },
					py: { xs: 8, md: 10 },
					display: 'flex',
					alignItems: 'center',
				}}
			>
				<Box
					component={motion.div}
					initial={shouldReduceMotion ? false : { opacity: 0, y: 30 }}
					animate={{ opacity: 1, y: 0 }}
					transition={{
						duration: shouldReduceMotion ? 0 : 0.7,
						ease: [0.22, 1, 0.36, 1],
						delay: shouldReduceMotion ? 0 : 0.08,
					}}
					sx={{ maxWidth: 700 }}
				>
					<Stack spacing={{ xs: 2.5, md: 3 }}>
						<Chip
							label={t('about_us.hero.eyebrow')}
							sx={{
								width: 'fit-content',
								borderRadius: 999,
								bgcolor: alpha(theme.palette.common.white, theme.palette.mode === 'dark' ? 0.1 : 0.16),
								color: theme.palette.common.white,
								fontWeight: 700,
								border: `1px solid ${alpha(theme.palette.common.white, 0.18)}`,
							}}
						/>

						<Typography
							variant='h1'
							sx={{
								fontWeight: 800,
								color: theme.palette.common.white,
								fontSize: { xs: '2.35rem', sm: '3rem', md: '4.25rem' },
								lineHeight: 1.06,
								letterSpacing: '-0.03em',
								maxWidth: 820,
							}}
						>
							{t('about_us.hero.title')}
						</Typography>

						<Typography
							variant='h6'
							sx={{
								color: alpha(theme.palette.common.white, 0.84),
								fontWeight: 400,
								lineHeight: 1.7,
								maxWidth: 620,
							}}
						>
							{t('about_us.hero.subtitle')}
						</Typography>

						<Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5} sx={{ pt: 1 }}>
							<Button
								variant='contained'
								size='large'
								onClick={onBookAppointment}
								sx={{
									alignSelf: { xs: 'stretch', sm: 'flex-start' },
									borderRadius: 999,
									px: 3.5,
									py: 1.45,
									fontWeight: 700,
									textTransform: 'none',
									bgcolor: theme.palette.common.white,
									color: theme.palette.primary.dark,
									boxShadow: `0 18px 40px ${alpha(theme.palette.common.black, 0.18)}`,
									'&:hover': {
										bgcolor: alpha(theme.palette.common.white, 0.94),
									},
								}}
							>
								{t('about_us.hero.primary_action')}
							</Button>
							<Button
								variant='outlined'
								size='large'
								onClick={onOrderMedicine}
								sx={{
									alignSelf: { xs: 'stretch', sm: 'flex-start' },
									borderRadius: 999,
									px: 3.5,
									py: 1.45,
									fontWeight: 700,
									textTransform: 'none',
									borderColor: alpha(theme.palette.common.white, 0.3),
									color: theme.palette.common.white,
									bgcolor: alpha(theme.palette.common.white, 0.04),
									'&:hover': {
										borderColor: alpha(theme.palette.common.white, 0.46),
										bgcolor: alpha(theme.palette.common.white, 0.1),
									},
								}}
							>
								{t('about_us.hero.secondary_action')}
							</Button>
						</Stack>

						<Stack direction='row' flexWrap='wrap' gap={1.25} sx={{ pt: 1 }}>
							{badges.map((badge) => {
								const Icon = badge.icon

								return (
									<Chip
										key={badge.key}
										icon={<Icon sx={{ color: 'inherit !important', fontSize: 18 }} />}
										label={badge.label}
										sx={{
											borderRadius: 999,
											height: 40,
											bgcolor: alpha(theme.palette.common.white, theme.palette.mode === 'dark' ? 0.08 : 0.12),
											color: alpha(theme.palette.common.white, 0.92),
											border: `1px solid ${alpha(theme.palette.common.white, 0.14)}`,
											'& .MuiChip-label': { fontWeight: 600 },
										}}
									/>
								)
							})}
						</Stack>
					</Stack>
				</Box>
			</Container>
		</Box>
	)
}

export default AboutUsHeroSection

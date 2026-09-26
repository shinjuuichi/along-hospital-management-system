import useTranslation from '@/hooks/useTranslation'
import { alpha, Box, Button, Container, Stack, Typography, useTheme } from '@mui/material'
import { motion } from 'framer-motion'

const AboutUsCTASection = ({ onBookAppointment, onOrderMedicine, shouldReduceMotion }) => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Box
			component={motion.section}
			{...(shouldReduceMotion
				? {}
				: {
						initial: { opacity: 0, y: 24 },
						whileInView: { opacity: 1, y: 0 },
						viewport: { once: true, amount: 0.25 },
						transition: { duration: 0.55, ease: [0.22, 1, 0.36, 1] },
				  })}
			sx={{
				position: 'relative',
				marginLeft: 'calc(50% - 50vw)',
				marginRight: 'calc(50% - 50vw)',
				overflow: 'hidden',
				background: theme.palette.gradients.brand_reverse_135deg,
			}}
		>
			<Box
				sx={{
					position: 'absolute',
					inset: 0,
					opacity: 0.18,
					background:
						'radial-gradient(circle at 15% 20%, rgba(255,255,255,0.26) 0%, transparent 22%), radial-gradient(circle at 85% 80%, rgba(255,255,255,0.18) 0%, transparent 26%)',
				}}
			/>

			<Container
				maxWidth='xl'
				sx={{ position: 'relative', zIndex: 1, px: { xs: 2, sm: 3, md: 4, lg: 6 }, py: { xs: 6, md: 8 } }}
			>
				<Stack spacing={3} alignItems='center' textAlign='center'>
					<Typography
						variant='h3'
						sx={{
							fontWeight: 800,
							color: theme.palette.common.white,
							fontSize: { xs: '2rem', md: '2.9rem' },
							maxWidth: 760,
						}}
					>
						{t('about_us.cta.title')}
					</Typography>

					<Typography
						variant='body1'
						sx={{
							maxWidth: 720,
							color: alpha(theme.palette.common.white, 0.88),
							lineHeight: 1.8,
						}}
					>
						{t('about_us.cta.subtitle')}
					</Typography>

					<Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5}>
						<Button
							variant='contained'
							size='large'
							onClick={onBookAppointment}
							sx={{
								borderRadius: 999,
								px: 3.5,
								py: 1.45,
								fontWeight: 700,
								textTransform: 'none',
								bgcolor: theme.palette.common.white,
								color: theme.palette.primary.dark,
								boxShadow: `0 18px 32px ${alpha(theme.palette.common.black, 0.16)}`,
								animation: shouldReduceMotion ? 'none' : 'aboutUsPulse 3s ease-in-out infinite',
								'@keyframes aboutUsPulse': {
									'0%, 100%': {
										boxShadow: `0 18px 32px ${alpha(theme.palette.common.black, 0.16)}`,
									},
									'50%': {
										boxShadow: `0 22px 40px ${alpha(theme.palette.common.black, 0.22)}`,
									},
								},
								'&:hover': {
									bgcolor: alpha(theme.palette.common.white, 0.94),
								},
							}}
						>
							{t('about_us.cta.primary_action')}
						</Button>

						<Button
							variant='outlined'
							size='large'
							onClick={onOrderMedicine}
							sx={{
								borderRadius: 999,
								px: 3.5,
								py: 1.45,
								fontWeight: 700,
								textTransform: 'none',
								color: theme.palette.common.white,
								borderColor: alpha(theme.palette.common.white, 0.34),
								bgcolor: alpha(theme.palette.common.white, 0.05),
								'&:hover': {
									borderColor: alpha(theme.palette.common.white, 0.54),
									bgcolor: alpha(theme.palette.common.white, 0.12),
								},
							}}
						>
							{t('about_us.cta.secondary_action')}
						</Button>
					</Stack>
				</Stack>
			</Container>
		</Box>
	)
}

export default AboutUsCTASection

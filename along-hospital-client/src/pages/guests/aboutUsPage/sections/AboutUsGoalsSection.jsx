import useTranslation from '@/hooks/useTranslation'
import {
	aboutItemReveal,
	aboutItemStagger,
	aboutSectionReveal,
	aboutViewport,
} from '@/pages/guests/aboutUsPage/helpers/aboutUsMotion'
import AboutUsSectionHeader from '@/pages/guests/aboutUsPage/sections/AboutUsSectionHeader'
import { alpha, Box, Grid, Paper, Stack, Typography, useTheme } from '@mui/material'
import { motion } from 'framer-motion'

const AboutUsGoalsSection = ({ items = [], shouldReduceMotion }) => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Box
			component={motion.section}
			{...(shouldReduceMotion
				? {}
				: {
						initial: 'hidden',
						whileInView: 'show',
						viewport: aboutViewport,
						variants: aboutSectionReveal,
					})}
		>
			<Stack spacing={4}>
				<AboutUsSectionHeader
					eyebrow={t('about_us.goals.eyebrow')}
					title={t('about_us.goals.title')}
					subtitle={t('about_us.goals.subtitle')}
					align='center'
				/>

				<Box component={motion.div} {...(shouldReduceMotion ? {} : { variants: aboutItemStagger })}>
					<Grid container spacing={3}>
						{items.map((item) => {
							const Icon = item.icon

							return (
								<Grid key={item.key} size={{ xs: 12, md: 4 }}>
									<Box component={motion.div} {...(shouldReduceMotion ? {} : { variants: aboutItemReveal })}>
										<Paper
											variant='outlined'
											sx={{
												p: 3,
												height: '100%',
												borderRadius: 4,
												position: 'relative',
												overflow: 'hidden',
												borderColor: alpha(theme.palette[item.tone].main, 0.16),
												background:
													theme.palette.mode === 'dark'
														? `linear-gradient(180deg, ${alpha(
																theme.palette.background.paper,
																0.96
															)} 0%, ${alpha(theme.palette[item.tone].softBg, 0.86)} 100%)`
														: `linear-gradient(180deg, ${alpha(
																theme.palette.background.paper,
																0.98
															)} 0%, ${alpha(theme.palette[item.tone].softBg, 0.74)} 100%)`,
												transition: 'transform 0.24s ease, box-shadow 0.24s ease',
												'&:hover': {
													transform: 'translateY(-6px)',
													boxShadow: theme.shadows[10],
												},
											}}
										>
											<Box
												sx={{
													position: 'absolute',
													top: -46,
													right: -24,
													width: 138,
													height: 138,
													borderRadius: '50%',
													bgcolor: alpha(theme.palette[item.tone].main, 0.08),
												}}
											/>

											<Stack spacing={2.25} sx={{ position: 'relative', zIndex: 1 }}>
												<Box
													sx={{
														width: 58,
														height: 58,
														borderRadius: 3,
														display: 'flex',
														alignItems: 'center',
														justifyContent: 'center',
														bgcolor: alpha(theme.palette[item.tone].main, 0.12),
														color: theme.palette[item.tone].main,
													}}
												>
													<Icon sx={{ fontSize: 30 }} />
												</Box>
												<Typography variant='h5' sx={{ fontWeight: 800 }}>
													{item.title}
												</Typography>
												<Typography variant='body1' color='text.secondary' sx={{ lineHeight: 1.8 }}>
													{item.description}
												</Typography>
											</Stack>
										</Paper>
									</Box>
								</Grid>
							)
						})}
					</Grid>
				</Box>
			</Stack>
		</Box>
	)
}

export default AboutUsGoalsSection

import useTranslation from '@/hooks/useTranslation'
import { aboutSectionReveal, aboutViewport } from '@/pages/guests/aboutUsPage/helpers/aboutUsMotion'
import AboutUsSectionHeader from '@/pages/guests/aboutUsPage/sections/AboutUsSectionHeader'
import { ChevronLeftRounded, ChevronRightRounded } from '@mui/icons-material'
import {
	alpha,
	Avatar,
	Box,
	Chip,
	Divider,
	Grid,
	IconButton,
	Paper,
	Stack,
	Typography,
	useTheme,
} from '@mui/material'
import { AnimatePresence, motion } from 'framer-motion'
import { useEffect, useState } from 'react'

const AboutUsTeamSection = ({ members = [], shouldReduceMotion }) => {
	const { t } = useTranslation()
	const theme = useTheme()
	const [activeIndex, setActiveIndex] = useState(0)

	if (members.length === 0) {
		return null
	}

	const activeMember = members[activeIndex]
	const slideLabel = `${String(activeIndex + 1).padStart(2, '0')} / ${String(members.length).padStart(2, '0')}`

	useEffect(() => {
		if (shouldReduceMotion || members.length <= 1) {
			return undefined
		}

		const intervalId = window.setInterval(() => {
			setActiveIndex((prev) => (prev + 1) % members.length)
		}, 4500)

		return () => window.clearInterval(intervalId)
	}, [members.length, shouldReduceMotion])

	const handleMove = (direction) => {
		setActiveIndex((prev) => {
			if (direction === 'next') {
				return (prev + 1) % members.length
			}

			return (prev - 1 + members.length) % members.length
		})
	}

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
					eyebrow={t('about_us.team.eyebrow')}
					title={t('about_us.team.title')}
					subtitle={t('about_us.team.subtitle')}
					align='center'
				/>

				<Stack direction={{ xs: 'column', lg: 'row' }} spacing={3} alignItems='stretch'>
					<Box sx={{ flex: 1, minWidth: 0 }}>
						<AnimatePresence mode='wait'>
							<Box
								key={activeMember.key}
								component={motion.div}
								initial={shouldReduceMotion ? false : { opacity: 0, x: 42 }}
								animate={{ opacity: 1, x: 0 }}
								exit={shouldReduceMotion ? undefined : { opacity: 0, x: -42 }}
								transition={shouldReduceMotion ? undefined : { duration: 0.38, ease: [0.22, 1, 0.36, 1] }}
							>
								<Paper
									variant='outlined'
									sx={{
										height: '100%',
										borderRadius: 5,
										overflow: 'hidden',
										borderColor: alpha(theme.palette[activeMember.tone].main, 0.18),
										background:
											theme.palette.mode === 'dark'
												? `linear-gradient(180deg, ${alpha(theme.palette.background.paper, 0.96)} 0%, ${alpha(
														theme.palette[activeMember.tone].softBg,
														0.86
													)} 100%)`
												: `linear-gradient(180deg, ${alpha(theme.palette.background.paper, 0.98)} 0%, ${alpha(
														theme.palette[activeMember.tone].softBg,
														0.74
													)} 100%)`,
										boxShadow: theme.shadows[8],
									}}
								>
									<Grid container sx={{ minHeight: { xs: 'auto', md: 440 } }}>
										<Grid size={{ xs: 12, md: 6.5 }}>
											<Stack spacing={2.5} sx={{ p: { xs: 3, md: 4 }, height: '100%' }}>
												<Stack
													direction={{ xs: 'column', sm: 'row' }}
													spacing={2}
													alignItems={{ xs: 'flex-start', sm: 'center' }}
													justifyContent='space-between'
												>
													<Stack spacing={1}>
														<Chip
															label={activeMember.role}
															size='small'
															sx={{
																width: 'fit-content',
																bgcolor: alpha(theme.palette[activeMember.tone].main, 0.12),
																color: theme.palette[activeMember.tone].main,
																fontWeight: 700,
															}}
														/>
														<Typography
															variant='h3'
															sx={{
																fontWeight: 800,
																fontSize: { xs: '1.9rem', md: '2.4rem' },
																lineHeight: 1.08,
															}}
														>
															{activeMember.name}
														</Typography>
													</Stack>

													<Stack direction='row' spacing={1}>
														<IconButton
															aria-label={t('about_us.team.previous')}
															onClick={() => handleMove('prev')}
															sx={{
																border: `1px solid ${alpha(theme.palette.divider, 0.9)}`,
																bgcolor: alpha(theme.palette.background.paper, 0.8),
															}}
														>
															<ChevronLeftRounded />
														</IconButton>
														<IconButton
															aria-label={t('about_us.team.next')}
															onClick={() => handleMove('next')}
															sx={{
																border: `1px solid ${alpha(theme.palette.divider, 0.9)}`,
																bgcolor: alpha(theme.palette.background.paper, 0.8),
															}}
														>
															<ChevronRightRounded />
														</IconButton>
													</Stack>
												</Stack>

												<Divider />

												<Box
													sx={{
														p: 1.8,
														borderRadius: 3,
														bgcolor: alpha(theme.palette[activeMember.tone].main, 0.08),
														border: `1px solid ${alpha(theme.palette[activeMember.tone].main, 0.14)}`,
														width: 'fit-content',
													}}
												>
													<Typography
														variant='caption'
														sx={{ color: 'text.disabled', display: 'block', mb: 0.5 }}
													>
														{t('about_us.team.experience_label')}
													</Typography>
													<Typography variant='body1' sx={{ fontWeight: 700 }}>
														{activeMember.experience}
													</Typography>
												</Box>

												<Typography
													variant='body1'
													color='text.secondary'
													sx={{ lineHeight: 1.85, maxWidth: 520 }}
												>
													{activeMember.bio}
												</Typography>

												<Box sx={{ mt: 'auto' }}>
													<Typography
														variant='body2'
														color='text.secondary'
														sx={{ fontWeight: 700, letterSpacing: 1.4 }}
													>
														{slideLabel}
													</Typography>
												</Box>
											</Stack>
										</Grid>

										<Grid size={{ xs: 12, md: 5.5 }}>
											<Box
												sx={{
													position: 'relative',
													height: '100%',
													bgcolor: alpha(theme.palette[activeMember.tone].main, 0.08),
													overflow: 'hidden',
													display: 'flex',
													alignItems: 'center',
													justifyContent: 'center',
													p: { xs: 2, md: 2.5 },
												}}
											>
												<Box
													sx={{
														position: 'relative',
														width: '100%',
														aspectRatio: '4 / 3',
														borderRadius: 4,
														overflow: 'hidden',
														boxShadow: theme.shadows[10],
														bgcolor: alpha(theme.palette.background.paper, 0.4),
													}}
												>
													<Box
														component='img'
														src={activeMember.avatar}
														alt={activeMember.name}
														onError={(event) => {
															event.currentTarget.src = '/placeholder-image.png'
														}}
														sx={{
															width: '100%',
															height: '100%',
															objectFit: 'cover',
															display: 'block',
														}}
													/>
													<Box
														sx={{
															position: 'absolute',
															left: 20,
															bottom: 20,
															px: 1.6,
															py: 0.9,
															borderRadius: 999,
															bgcolor: alpha(theme.palette.common.black, 0.56),
															color: theme.palette.common.white,
															backdropFilter: 'blur(10px)',
														}}
													>
														<Typography variant='body2' sx={{ fontWeight: 700 }}>
															{t('about_us.team.member_badge')}
														</Typography>
													</Box>
												</Box>
											</Box>
										</Grid>
									</Grid>
								</Paper>
							</Box>
						</AnimatePresence>
					</Box>

					<Stack
						spacing={1.25}
						sx={{
							width: { xs: '100%', lg: 190 },
							flexShrink: 0,
						}}
					>
						{members.map((member, index) => (
							<Box
								key={member.key}
								component='button'
								type='button'
								onClick={() => setActiveIndex(index)}
								aria-label={`${t('about_us.team.go_to_member')} ${member.name}`}
								sx={{
									width: '100%',
									px: 1.25,
									py: 1.2,
									borderRadius: 4,
									border: `1px solid ${
										index === activeIndex
											? alpha(theme.palette[member.tone].main, 0.28)
											: alpha(theme.palette.divider, 0.9)
									}`,
									bgcolor:
										index === activeIndex
											? alpha(theme.palette[member.tone].main, 0.08)
											: alpha(theme.palette.background.paper, 0.82),
									cursor: 'pointer',
									textAlign: 'left',
									transition: 'transform 0.24s ease, border-color 0.24s ease, background-color 0.24s ease',
									'&:hover': {
										transform: 'translateX(-4px)',
										borderColor: alpha(theme.palette[member.tone].main, 0.24),
									},
								}}
							>
								<Stack direction='row' spacing={1.2} alignItems='center'>
									<Avatar
										src={member.avatar}
										alt={member.name}
										imgProps={{
											onError: (event) => {
												event.currentTarget.src = '/placeholder-image.png'
											},
										}}
										sx={{
											width: 56,
											height: 56,
											border: `3px solid ${alpha(theme.palette[member.tone].main, 0.18)}`,
											boxShadow:
												index === activeIndex
													? `0 12px 22px ${alpha(theme.palette[member.tone].main, 0.16)}`
													: 'none',
										}}
									/>
									<Box sx={{ minWidth: 0 }}>
										<Typography
											variant='body2'
											sx={{
												fontWeight: index === activeIndex ? 800 : 700,
												color:
													index === activeIndex ? theme.palette.text.primary : theme.palette.text.secondary,
											}}
										>
											{member.name}
										</Typography>
										<Typography variant='caption' color='text.disabled'>
											{member.role}
										</Typography>
									</Box>
								</Stack>
							</Box>
						))}
					</Stack>
				</Stack>
			</Stack>
		</Box>
	)
}

export default AboutUsTeamSection

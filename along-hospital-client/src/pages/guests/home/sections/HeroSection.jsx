import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import {
	ArrowForward,
	CalendarMonthOutlined,
	HealthAndSafetyOutlined,
	LocalHospitalOutlined,
	MedicalServicesOutlined,
	MedicationOutlined,
} from '@mui/icons-material'
import { alpha, Box, Button, Chip, Container, Grid, Paper, Stack, Typography, useTheme } from '@mui/material'
import { motion } from 'framer-motion'
import { useNavigate } from 'react-router-dom'

const HeroSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const theme = useTheme()

	const quickLinks = [
		{
			key: 'medical-service',
			label: t('header.medical_service'),
			url: routeUrls.HOME.MEDICAL_SERVICE,
			icon: <MedicalServicesOutlined sx={{ fontSize: 18 }} />,
			position: { top: '16%', right: { md: '7%' } },
		},
		{
			key: 'specialty',
			label: t('header.specialty'),
			url: routeUrls.HOME.SPECIALTY,
			icon: <HealthAndSafetyOutlined sx={{ fontSize: 18 }} />,
			position: { top: '44%', left: { md: '5%' } },
		},
		{
			key: 'doctor',
			label: t('header.doctor'),
			url: routeUrls.HOME.DOCTOR,
			icon: <LocalHospitalOutlined sx={{ fontSize: 18 }} />,
			position: { bottom: '16%', right: { md: '2%' } },
		},
		{
			key: 'medicine',
			label: t('header.medicine'),
			url: routeUrls.HOME.MEDICINE,
			icon: <MedicationOutlined sx={{ fontSize: 18 }} />,
			position: { bottom: '12%', left: { md: '10%' } },
		},
	]

	return (
		<Box
			component='section'
			sx={{
				position: 'relative',
				minHeight: { xs: 'calc(100vh - 72px)', md: 'calc(100vh - 84px)' },
				background:
					theme.palette.mode === 'dark'
						? `linear-gradient(180deg, ${theme.palette.background.default} 0%, ${alpha(
								theme.palette.primary.softBg,
								0.92
						  )} 100%)`
						: `linear-gradient(180deg, ${alpha(theme.palette.primary.softBg, 0.75)} 0%, ${alpha(
								theme.palette.secondary.softBg,
								0.62
						  )} 100%)`,
				overflow: 'hidden',
				display: 'flex',
				alignItems: 'center',
				py: { xs: 6, md: 8 },
			}}
		>
			<Box
				sx={{
					position: 'absolute',
					inset: 0,
					opacity: theme.palette.mode === 'dark' ? 0.2 : 0.36,
					backgroundImage: `linear-gradient(${alpha(theme.palette.primary.main, 0.08)} 1px, transparent 1px), linear-gradient(90deg, ${alpha(theme.palette.primary.main, 0.08)} 1px, transparent 1px)`,
					backgroundSize: '36px 36px',
					maskImage: 'linear-gradient(180deg, rgba(0,0,0,0.72) 0%, rgba(0,0,0,0.1) 100%)',
				}}
			/>

			<Box
				sx={{
					position: 'absolute',
					top: { xs: -120, md: -160 },
					right: { xs: -80, md: -120 },
					width: { xs: 280, md: 420 },
					height: { xs: 280, md: 420 },
					borderRadius: '50%',
					background: `radial-gradient(circle, ${alpha(theme.palette.primary.main, 0.26)} 0%, transparent 68%)`,
					filter: 'blur(18px)',
				}}
			/>

			<Box
				sx={{
					position: 'absolute',
					bottom: { xs: -140, md: -180 },
					left: { xs: -120, md: -80 },
					width: { xs: 340, md: 460 },
					height: { xs: 340, md: 460 },
					borderRadius: '50%',
					background: `radial-gradient(circle, ${alpha(theme.palette.secondary.main, 0.2)} 0%, transparent 72%)`,
					filter: 'blur(20px)',
				}}
			/>

			<Container
				maxWidth='xl'
				sx={{
					position: 'relative',
					zIndex: 1,
					px: { xs: 2, sm: 3, md: 4, lg: 6 },
				}}
			>
				<Grid container spacing={{ xs: 5, md: 7 }} alignItems='center'>
					<Grid size={{ xs: 12, md: 6 }}>
						<Stack
							component={motion.div}
							initial={{ opacity: 0, y: 24 }}
							animate={{ opacity: 1, y: 0 }}
							transition={{ duration: 0.55, ease: [0.22, 1, 0.36, 1] }}
							spacing={{ xs: 2.5, md: 3.5 }}
						>
							<Chip
								label={t('home.hero.badge')}
								color='secondary'
								variant='outlined'
								sx={{ width: 'fit-content', borderRadius: 999, fontWeight: 700 }}
							/>

							<Typography
								variant='h1'
								component='h1'
								sx={{
									fontWeight: 900,
									fontSize: { xs: '2.35rem', sm: '3rem', md: '4.25rem' },
									lineHeight: 1.03,
									color: theme.palette.text.primary,
									letterSpacing: '-0.02em',
									maxWidth: 720,
								}}
							>
								{t('home.hero.title')}
							</Typography>

							<Typography
								variant='body1'
								sx={{
									fontSize: { xs: '1rem', sm: '1.05rem', md: '1.15rem' },
									color: theme.palette.text.secondary,
									lineHeight: 1.75,
									maxWidth: '620px',
								}}
							>
								{t('home.hero.description')}
							</Typography>

							<Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} sx={{ pt: 1 }}>
								<Button
									variant='contained'
									size='large'
									onClick={() =>
										navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.CREATE))
									}
									endIcon={<CalendarMonthOutlined />}
									sx={{
										bgcolor: theme.palette.primary.main,
										color: theme.palette.primary.contrastText,
										fontWeight: 800,
										px: 4,
										py: 1.5,
										borderRadius: 999,
										textTransform: 'none',
										fontSize: '1rem',
										boxShadow: `0 14px 28px ${alpha(theme.palette.primary.main, 0.28)}`,
										'&:hover': {
											bgcolor: theme.palette.primary.dark,
										},
									}}
								>
									{t('button.book_appointment')}
								</Button>

								<Button
									variant='outlined'
									size='large'
									onClick={() => navigate(routeUrls.HOME.MEDICAL_SERVICE)}
									endIcon={<ArrowForward />}
									sx={{
										fontWeight: 700,
										px: 3.25,
										py: 1.5,
										borderRadius: 999,
										textTransform: 'none',
										fontSize: '1rem',
										'&:hover': {
											bgcolor:
												theme.palette.mode === 'dark'
													? 'rgba(255,255,255,0.05)'
													: 'rgba(0,0,0,0.04)',
										},
									}}
								>
									{t('button.view_services')}
								</Button>
							</Stack>
						</Stack>
					</Grid>

					<Grid size={{ xs: 12, md: 6 }}>
						<Box
							component={motion.div}
							initial={{ opacity: 0, y: 28 }}
							animate={{ opacity: 1, y: 0 }}
							transition={{ delay: 0.08, duration: 0.6, ease: [0.22, 1, 0.36, 1] }}
							sx={{
								position: 'relative',
								minHeight: { xs: 420, md: 560 },
							}}
						>
							<Paper
								variant='outlined'
								sx={{
									position: 'relative',
									overflow: 'hidden',
									minHeight: { xs: 420, md: 560 },
									borderRadius: 6,
									borderColor: alpha(theme.palette.primary.main, 0.16),
									background:
										theme.palette.mode === 'dark'
											? `linear-gradient(135deg, ${alpha(theme.palette.background.paper, 0.98)} 0%, ${alpha(
													theme.palette.primary.softBg,
													0.9
											  )} 100%)`
											: `linear-gradient(135deg, ${alpha(theme.palette.background.paper, 0.98)} 0%, ${alpha(
													theme.palette.secondary.softBg,
													0.9
											  )} 100%)`,
									boxShadow: theme.shadows[10],
								}}
							>
								<Box
									sx={{
										position: 'absolute',
										top: 24,
										left: 24,
										width: 180,
										height: 180,
										borderRadius: '50%',
										background: `radial-gradient(circle, ${alpha(theme.palette.primary.main, 0.26)} 0%, transparent 70%)`,
										filter: 'blur(12px)',
									}}
								/>
								<Box
									sx={{
										position: 'absolute',
										right: 12,
										bottom: 16,
										width: 220,
										height: 220,
										borderRadius: '50%',
										background: `radial-gradient(circle, ${alpha(theme.palette.secondary.main, 0.22)} 0%, transparent 70%)`,
										filter: 'blur(20px)',
									}}
								/>

								<Stack sx={{ position: 'relative', zIndex: 1, height: '100%', p: { xs: 3, md: 4 } }}>
									<Typography variant='overline' sx={{ letterSpacing: 1.8, color: 'text.secondary', fontWeight: 700 }}>
										{t('home.hero.quick_access')}
									</Typography>
									<Box
										component='img'
										src='/doctor-hero.png'
										alt={t('header.doctor')}
										onError={(event) => {
											event.currentTarget.style.display = 'none'
										}}
										sx={{
											alignSelf: 'center',
											width: { xs: '88%', md: '86%' },
											maxWidth: 420,
											height: 'auto',
											objectFit: 'contain',
											mt: { xs: 2, md: 1 },
											filter: 'drop-shadow(0 24px 38px rgba(15, 23, 42, 0.18))',
										}}
									/>
									<Stack
										direction='row'
										spacing={1}
										flexWrap='wrap'
										useFlexGap
										sx={{ mt: 'auto', display: { xs: 'flex', md: 'none' } }}
									>
										{quickLinks.map((item) => (
											<Chip
												key={item.key}
												icon={item.icon}
												label={item.label}
												onClick={() => navigate(item.url)}
												sx={{
													borderRadius: 999,
													bgcolor: alpha(theme.palette.background.paper, 0.92),
													border: `1px solid ${alpha(theme.palette.primary.main, 0.12)}`,
													fontWeight: 700,
												}}
											/>
										))}
									</Stack>
								</Stack>
							</Paper>

							<Box
								sx={{
									position: 'absolute',
									inset: 0,
									display: { xs: 'none', md: 'block' },
									zIndex: 2,
								}}
							>
								{quickLinks.map((item, index) => (
									<Box
										key={item.key}
										component={motion.div}
										initial={{ opacity: 0, scale: 0.92 }}
										animate={{ opacity: 1, scale: 1 }}
										transition={{ delay: 0.18 + index * 0.06, duration: 0.35 }}
										sx={{
											position: 'absolute',
											...item.position,
										}}
									>
										<Button
											type='button'
											onClick={() => navigate(item.url)}
											startIcon={
												<Box
													sx={{
														width: 32,
														height: 32,
														borderRadius: '50%',
														display: 'flex',
														alignItems: 'center',
														justifyContent: 'center',
														bgcolor: alpha(theme.palette.primary.main, 0.12),
														color: theme.palette.primary.main,
													}}
												>
													{item.icon}
												</Box>
											}
											sx={{
												pl: 1.15,
												pr: 1.75,
												py: 1,
												borderRadius: 999,
												bgcolor: alpha(theme.palette.background.paper, 0.94),
												border: `1px solid ${alpha(theme.palette.primary.main, 0.12)}`,
												backdropFilter: 'blur(10px)',
												boxShadow: theme.shadows[6],
												color: 'text.primary',
												fontWeight: 700,
												textTransform: 'none',
												whiteSpace: 'nowrap',
												transition: 'transform 0.24s ease, box-shadow 0.24s ease',
												'& .MuiButton-startIcon': {
													mr: 1,
													ml: 0,
												},
												'&:hover': {
													bgcolor: alpha(theme.palette.background.paper, 0.98),
													transform: 'translateY(-4px)',
													boxShadow: theme.shadows[10],
												},
											}}
										>
											{item.label}
										</Button>
									</Box>
								))}
							</Box>
						</Box>
					</Grid>
				</Grid>
			</Container>
		</Box>
	)
}

export default HeroSection

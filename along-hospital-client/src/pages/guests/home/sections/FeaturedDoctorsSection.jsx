import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import {
	homeItemReveal,
	homeSectionReveal,
	homeStagger,
	homeViewport,
} from '@/pages/guests/home/helpers/homeMotion'
import HomeSectionFallback from '@/pages/guests/home/sections/HomeSectionFallback'
import HomeSectionHeader from '@/pages/guests/home/sections/HomeSectionHeader'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import {
	ArrowOutward,
	CalendarMonthOutlined,
	LocalHospitalOutlined,
	SchoolRounded,
	WcRounded,
} from '@mui/icons-material'
import {
	alpha,
	Avatar,
	Box,
	Button,
	Chip,
	Container,
	Grid,
	Paper,
	Skeleton,
	Stack,
	Typography,
} from '@mui/material'
import { motion } from 'framer-motion'
import { useNavigate } from 'react-router-dom'

const FeaturedDoctorsSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const { data, loading, error } = useFetch(
		ApiUrls.STAFF.GET_BY_ROLE('doctor'),
		{ page: 1, pageSize: 4 },
		[]
	)

	const doctors = data?.collection || []

	return (
		<Box
			component={motion.section}
			initial='hidden'
			whileInView='show'
			viewport={homeViewport}
			variants={homeSectionReveal}
			sx={{ py: { xs: 7, md: 10 }, bgcolor: 'background.default' }}
		>
			<Container maxWidth='xl' sx={{ px: { xs: 2, sm: 3, md: 4, lg: 6 } }}>
				<Stack spacing={4.5}>
					<HomeSectionHeader
						eyebrow={t('header.doctor')}
						title={t('home.doctor.title')}
						subtitle={t('home.doctor.subtitle')}
						actionLabel={t('home.doctor.button.view_all')}
						onAction={() => navigate(routeUrls.HOME.DOCTOR)}
					/>

					{loading ? (
						<Grid container spacing={3}>
							{Array.from({ length: 4 }).map((_, index) => (
								<Grid key={index} size={{ xs: 12, sm: 6, xl: 3 }}>
									<Paper variant='outlined' sx={{ p: 3, borderRadius: 4 }}>
										<Stack spacing={2}>
											<Stack direction='row' spacing={2} alignItems='center'>
												<Skeleton variant='circular' width={72} height={72} />
												<Stack sx={{ flex: 1 }}>
													<Skeleton variant='text' height={28} />
													<Skeleton variant='text' width='70%' height={22} />
												</Stack>
											</Stack>
											<Skeleton variant='rounded' height={28} width='45%' />
											<Skeleton variant='text' height={22} />
											<Skeleton variant='text' width='85%' height={22} />
											<Skeleton variant='rounded' height={44} />
										</Stack>
									</Paper>
								</Grid>
							))}
						</Grid>
					) : doctors.length === 0 || error ? (
						<HomeSectionFallback
							title={t('home.doctor.text.no_doctors')}
							subtitle={t('home.doctor.subtitle')}
							icon={<LocalHospitalOutlined sx={{ fontSize: 30 }} />}
						/>
					) : (
						<Box
							component={motion.div}
							initial='hidden'
							whileInView='show'
							viewport={homeViewport}
							variants={homeStagger}
						>
							<Grid container spacing={3}>
								{doctors.map((doctor) => {
									const quickDetails = [
										{
											key: 'specialty',
											icon: <LocalHospitalOutlined sx={{ fontSize: 16 }} />,
											value: doctor?.specialtyName,
										},
										{
											key: 'qualification',
											icon: <SchoolRounded sx={{ fontSize: 16 }} />,
											value: doctor?.qualificationName,
										},
										{
											key: 'gender',
											icon: <WcRounded sx={{ fontSize: 16 }} />,
											value: doctor?.gender,
										},
										{
											key: 'dateOfBirth',
											icon: <CalendarMonthOutlined sx={{ fontSize: 16 }} />,
											value: doctor?.dateOfBirth ? formatDateBasedOnCurrentLanguage(doctor.dateOfBirth) : null,
										},
									].filter((item) => item.value)

									return (
										<Grid key={doctor.id} size={{ xs: 12, sm: 6, xl: 3 }}>
											<Box component={motion.div} variants={homeItemReveal}>
												<Paper
													variant='outlined'
													sx={(theme) => ({
														p: 3,
														height: '100%',
														borderRadius: 4,
														display: 'flex',
														flexDirection: 'column',
														gap: 2,
														background:
															theme.palette.mode === 'dark'
																? `linear-gradient(180deg, ${alpha(
																		theme.palette.background.paper,
																		0.96
																	)} 0%, ${alpha(theme.palette.secondary.softBg, 0.86)} 100%)`
																: `linear-gradient(180deg, ${theme.palette.background.paper} 0%, ${alpha(
																		theme.palette.secondary.softBg,
																		0.52
																	)} 100%)`,
														borderColor: alpha(theme.palette.secondary.main, 0.15),
														transition: 'transform 0.24s ease, box-shadow 0.24s ease, border-color 0.24s ease',
														'&:hover': {
															transform: 'translateY(-6px)',
															boxShadow: theme.shadows[10],
															borderColor: alpha(theme.palette.secondary.main, 0.3),
														},
													})}
												>
													<Stack direction='row' spacing={2} alignItems='center'>
														<Avatar
															src={getImageFromCloud(doctor?.image)}
															alt={doctor?.name || t('doctor.ticket.pending_update')}
															sx={{
																width: 72,
																height: 72,
																border: (theme) => `3px solid ${alpha(theme.palette.primary.main, 0.14)}`,
															}}
														/>
														<Stack spacing={0.75} sx={{ minWidth: 0 }}>
															<Typography variant='h6' sx={{ fontWeight: 800 }}>
																{doctor?.name || t('doctor.ticket.pending_update')}
															</Typography>
															{doctor?.specialtyName ? (
																<Chip
																	label={doctor.specialtyName}
																	size='small'
																	sx={(theme) => ({
																		width: 'fit-content',
																		bgcolor: alpha(theme.palette.primary.main, 0.12),
																		color: theme.palette.primary.main,
																		fontWeight: 700,
																	})}
																/>
															) : null}
														</Stack>
													</Stack>

													<Typography variant='body2' color='text.secondary' sx={{ minHeight: 44 }}>
														{doctor?.qualificationName || t('doctor.ticket.pending_update')}
													</Typography>

													<Stack spacing={1.1} sx={{ flex: 1 }}>
														{quickDetails.length > 0 ? (
															quickDetails.map((detail) => (
																<Stack key={detail.key} direction='row' spacing={1.1} alignItems='center'>
																	<Paper
																		elevation={0}
																		sx={(theme) => ({
																			width: 28,
																			height: 28,
																			borderRadius: 2,
																			display: 'flex',
																			alignItems: 'center',
																			justifyContent: 'center',
																			bgcolor: alpha(theme.palette.secondary.main, 0.12),
																			color: theme.palette.secondary.main,
																		})}
																	>
																		{detail.icon}
																	</Paper>
																	<Typography variant='body2' color='text.secondary'>
																		{detail.value}
																	</Typography>
																</Stack>
															))
														) : (
															<Typography variant='body2' color='text.secondary'>
																{t('doctor.ticket.pending_update')}
															</Typography>
														)}
													</Stack>
												</Paper>
											</Box>
										</Grid>
									)
								})}
							</Grid>
						</Box>
					)}
				</Stack>
			</Container>
		</Box>
	)
}

export default FeaturedDoctorsSection

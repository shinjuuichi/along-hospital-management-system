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
import MedicalServiceDetailDialog from '@/pages/patients/medicalServicePage/sections/MedicalServiceDetailDialog'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { ArrowOutward, HealthAndSafetyOutlined } from '@mui/icons-material'
import {
	alpha,
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
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const truncateWords = (value, maxWords = 20) => {
	const rawValue = value?.trim() || ''
	if (!rawValue) return ''

	const words = rawValue.split(/\s+/)
	if (words.length <= maxWords) return rawValue

	return `${words.slice(0, maxWords).join(' ')}...`
}

const FeaturedServicesSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [selectedService, setSelectedService] = useState(null)

	const { data, loading, error } = useFetch(
		ApiUrls.MEDICAL_SERVICE.INDEX,
		{ page: 1, pageSize: 6, isActive: true },
		[]
	)

	const services = data?.collection || []

	const handleBookAppointment = () => {
		setSelectedService(null)
		navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.CREATE))
	}

	return (
		<Box
			component={motion.section}
			initial='hidden'
			whileInView='show'
			viewport={homeViewport}
			variants={homeSectionReveal}
			sx={(theme) => ({
				py: { xs: 7, md: 10 },
				background:
					theme.palette.mode === 'dark'
						? `linear-gradient(180deg, ${alpha(theme.palette.primary.softBg, 0.18)} 0%, transparent 100%)`
						: `linear-gradient(180deg, ${alpha(theme.palette.primary.softBg, 0.7)} 0%, ${alpha(
								theme.palette.background.paper,
								0.96
							)} 100%)`,
			})}
		>
			<Container maxWidth='xl' sx={{ px: { xs: 2, sm: 3, md: 4, lg: 6 } }}>
				<Stack spacing={4.5}>
					<HomeSectionHeader
						eyebrow={t('header.service')}
						title={t('home.service.title')}
						subtitle={t('home.service.subtitle')}
						actionLabel={t('button.view_all')}
						onAction={() => navigate(routeUrls.HOME.MEDICAL_SERVICE)}
					/>

					{loading ? (
						<Grid container spacing={3}>
							{Array.from({ length: 6 }).map((_, index) => (
								<Grid key={index} size={{ xs: 12, sm: 6, lg: 4 }}>
									<Paper variant='outlined' sx={{ p: 3, borderRadius: 4 }}>
										<Stack spacing={2}>
											<Skeleton variant='circular' width={56} height={56} />
											<Skeleton variant='text' height={34} width='72%' />
											<Skeleton variant='text' height={22} />
											<Skeleton variant='text' height={22} width='90%' />
											<Skeleton variant='rounded' height={30} width={140} />
											<Skeleton variant='rounded' height={40} width={130} />
										</Stack>
									</Paper>
								</Grid>
							))}
						</Grid>
					) : services.length === 0 || error ? (
						<HomeSectionFallback
							title={t('medical_service.placeholder.no_result_title')}
							subtitle={t('home.service.subtitle')}
							icon={<HealthAndSafetyOutlined sx={{ fontSize: 30 }} />}
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
								{services.map((service) => {
									const specialtyName = service?.specialtyName || service?.specialty?.name
									const description =
										truncateWords(service?.description, 20) || t('medical_service.placeholder.no_description')

									return (
										<Grid key={service.id} size={{ xs: 12, sm: 6, lg: 4 }}>
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
														borderColor: alpha(theme.palette.primary.main, 0.12),
														background:
															theme.palette.mode === 'dark'
																? `linear-gradient(180deg, ${alpha(
																		theme.palette.background.paper,
																		0.96
																	)} 0%, ${alpha(theme.palette.primary.softBg, 0.78)} 100%)`
																: `linear-gradient(180deg, ${theme.palette.background.paper} 0%, ${alpha(
																		theme.palette.primary.softBg,
																		0.6
																	)} 100%)`,
														transition: 'transform 0.24s ease, box-shadow 0.24s ease, border-color 0.24s ease',
														'&:hover': {
															transform: 'translateY(-6px)',
															boxShadow: theme.shadows[10],
															borderColor: alpha(theme.palette.primary.main, 0.3),
															'& .service-icon-shell': {
																transform: 'scale(1.03)',
															},
															'& .service-action-icon': {
																transform: 'translateX(4px)',
															},
														},
													})}
												>
													<Paper
														elevation={0}
														className='service-icon-shell'
														sx={(theme) => ({
															width: 56,
															height: 56,
															borderRadius: 3,
															display: 'flex',
															alignItems: 'center',
															justifyContent: 'center',
															bgcolor: alpha(theme.palette.primary.main, 0.14),
															color: 'primary.main',
															transition: 'transform 0.24s ease',
														})}
													>
														<HealthAndSafetyOutlined sx={{ fontSize: 28 }} />
													</Paper>

													<Stack spacing={1}>
														<Stack
															direction='row'
															spacing={1}
															justifyContent='space-between'
															alignItems='flex-start'
															flexWrap='wrap'
															useFlexGap
														>
															<Typography variant='h5' sx={{ fontWeight: 800, flex: 1, minWidth: 220 }}>
																{service?.name}
															</Typography>
															<Chip
																label={formatCurrencyBasedOnCurrentLanguage(service?.price)}
																color='primary'
																variant='outlined'
																sx={{ fontWeight: 700 }}
															/>
														</Stack>
														{specialtyName ? (
															<Chip
																label={specialtyName}
																size='small'
																sx={(theme) => ({
																	width: 'fit-content',
																	bgcolor: alpha(theme.palette.secondary.main, 0.12),
																	color: theme.palette.secondary.main,
																	fontWeight: 700,
																})}
															/>
														) : null}
													</Stack>

													<Typography
														variant='body2'
														color='text.secondary'
														sx={{ minHeight: { xs: 'auto', md: 66 } }}
													>
														{description}
													</Typography>

													<Box sx={{ mt: 'auto', pt: 1 }}>
														<Button
															variant='text'
															size='small'
															onClick={() => setSelectedService(service)}
															endIcon={<ArrowOutward className='service-action-icon' />}
															sx={{
																px: 0,
																fontWeight: 700,
																textTransform: 'none',
																'& .service-action-icon': {
																	transition: 'transform 0.2s ease',
																},
															}}
														>
															{t('medical_service.button.view_detail')}
														</Button>
													</Box>
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

			<MedicalServiceDetailDialog
				open={Boolean(selectedService)}
				service={selectedService}
				onClose={() => setSelectedService(null)}
				onBook={handleBookAppointment}
			/>
		</Box>
	)
}

export default FeaturedServicesSection

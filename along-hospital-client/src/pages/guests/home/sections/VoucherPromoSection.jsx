import { ApiUrls } from '@/configs/apiUrls'
import { defaultVoucherTypeStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
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
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { AccessTime, ArrowForward, LocalOfferOutlined } from '@mui/icons-material'
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
import { useNavigate } from 'react-router-dom'

const getDiscountLabel = (voucher) => {
	if (!voucher) return ''

	return voucher.discountType === EnumConfig.VoucherDiscountType.Percentage
		? `-${voucher.discountValue}%`
		: `-${formatCurrencyBasedOnCurrentLanguage(voucher.discountValue)}`
}

const getMinPurchase = (voucher) => {
	return voucher?.minPurchaseAmount ?? voucher?.minPrice ?? 0
}

const VoucherPromoSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const { data, loading, error } = useFetch(
		ApiUrls.VOUCHER.COLLECTIBLE,
		{ page: 1, pageSize: 4 },
		[]
	)

	const vouchers = data?.collection || []
	const featuredVoucher = vouchers[0]
	const sideVouchers = vouchers.slice(1, 4)

	const navigateToVouchers = () => {
		navigate(routeUrls.HOME.VOUCHERS)
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
						? `linear-gradient(180deg, ${alpha(theme.palette.warning.softBg, 0.3)} 0%, ${theme.palette.background.default} 100%)`
						: `linear-gradient(180deg, ${alpha(theme.palette.warning.softBg, 0.7)} 0%, ${alpha(
								theme.palette.background.default,
								0.96
							)} 100%)`,
			})}
		>
			<Container maxWidth='xl' sx={{ px: { xs: 2, sm: 3, md: 4, lg: 6 } }}>
				<Stack spacing={4.5}>
					<HomeSectionHeader
						eyebrow={t('header.vouchers')}
						title={t('home.voucher.title')}
						subtitle={t('home.voucher.subtitle')}
						actionLabel={t('voucher.button.explore_vouchers')}
						onAction={navigateToVouchers}
						actionVariant='contained'
					/>

					{loading ? (
						<Grid container spacing={3}>
							<Grid size={{ xs: 12, md: 7 }}>
								<Skeleton variant='rounded' height={320} sx={{ borderRadius: 5 }} />
							</Grid>
							<Grid size={{ xs: 12, md: 5 }}>
								<Stack spacing={3}>
									{Array.from({ length: 3 }).map((_, index) => (
										<Skeleton key={index} variant='rounded' height={96} sx={{ borderRadius: 4 }} />
									))}
								</Stack>
							</Grid>
						</Grid>
					) : vouchers.length === 0 || error ? (
						<HomeSectionFallback
							title={t('voucher.description.no_collectible_title')}
							subtitle={t('voucher.description.no_collectible_subtitle')}
							icon={<LocalOfferOutlined sx={{ fontSize: 30 }} />}
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
								{featuredVoucher ? (
									<Grid size={{ xs: 12, md: sideVouchers.length > 0 ? 7 : 12 }}>
										<Box component={motion.div} variants={homeItemReveal}>
											<Paper
												variant='outlined'
												sx={(theme) => ({
													position: 'relative',
													minHeight: 320,
													overflow: 'hidden',
													borderRadius: 5,
													borderColor: alpha(theme.palette.warning.main, 0.22),
													background:
														theme.palette.mode === 'dark'
															? `linear-gradient(135deg, ${alpha(
																	theme.palette.warning.softBg,
																	0.84
																)} 0%, ${alpha(theme.palette.background.paper, 0.96)} 100%)`
															: `linear-gradient(135deg, ${alpha(
																	theme.palette.warning.softBg,
																	0.96
																)} 0%, ${theme.palette.background.paper} 100%)`,
													boxShadow: theme.shadows[8],
												})}
											>
												<Box
													sx={{
														position: 'absolute',
														inset: 0,
														backgroundImage: `url(${getImageFromCloud(featuredVoucher.image)})`,
														backgroundPosition: 'center',
														backgroundSize: 'cover',
														opacity: 0.18,
														transform: 'scale(1.05)',
													}}
												/>
												<Box
													sx={(theme) => ({
														position: 'absolute',
														inset: 0,
														background:
															theme.palette.mode === 'dark'
																? 'linear-gradient(90deg, rgba(15,23,42,0.94) 0%, rgba(15,23,42,0.56) 56%, rgba(15,23,42,0.26) 100%)'
																: 'linear-gradient(90deg, rgba(255,255,255,0.98) 0%, rgba(255,255,255,0.82) 54%, rgba(255,255,255,0.28) 100%)',
													})}
												/>
												<Stack
													sx={{
														position: 'relative',
														zIndex: 1,
														height: '100%',
														p: { xs: 3, md: 4 },
														maxWidth: { xs: '100%', md: '72%' },
													}}
													spacing={2.25}
												>
													<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
														<Chip
															label={t(`voucher.type.${featuredVoucher?.voucherType || 'Patient'}`)}
															color={defaultVoucherTypeStyle(featuredVoucher?.voucherType)}
															variant='outlined'
															sx={{ width: 'fit-content', fontWeight: 700 }}
														/>
														<Chip
															label={t('home.voucher.highlight')}
															color='warning'
															sx={{ width: 'fit-content', fontWeight: 700 }}
														/>
													</Stack>

													<Typography
														variant='h3'
														sx={{
															fontWeight: 900,
															fontSize: { xs: '2rem', md: '2.6rem' },
															lineHeight: 1.05,
														}}
													>
														{featuredVoucher?.name}
													</Typography>

													{featuredVoucher?.description ? (
														<Typography variant='body1' color='text.secondary'>
															{featuredVoucher.description}
														</Typography>
													) : null}

													<Chip
														label={getDiscountLabel(featuredVoucher)}
														color='warning'
														sx={{
															width: 'fit-content',
															fontWeight: 900,
															fontSize: '1rem',
															px: 1.2,
															py: 2.1,
														}}
													/>

													<Stack spacing={1}>
														{getMinPurchase(featuredVoucher) > 0 ? (
															<Typography variant='body2' color='text.secondary'>
																{t('voucher.label.min_purchase', {
																	amount: formatCurrencyBasedOnCurrentLanguage(getMinPurchase(featuredVoucher)),
																})}
															</Typography>
														) : null}
														{featuredVoucher?.expireDate ? (
															<Stack direction='row' spacing={1} alignItems='center'>
																<AccessTime sx={{ fontSize: 18, color: 'text.secondary' }} />
																<Typography variant='body2' color='text.secondary'>
																	{t('voucher.label.expire_date')}:{' '}
																	{formatDateBasedOnCurrentLanguage(featuredVoucher.expireDate)}
																</Typography>
															</Stack>
														) : null}
													</Stack>

													<Button
														variant='contained'
														color='warning'
														endIcon={<ArrowForward />}
														onClick={navigateToVouchers}
														sx={{
															mt: 'auto',
															alignSelf: 'flex-start',
															borderRadius: 999,
															px: 2.75,
															py: 1.2,
															fontWeight: 800,
															textTransform: 'none',
														}}
													>
														{t('voucher.button.explore_vouchers')}
													</Button>
												</Stack>
											</Paper>
										</Box>
									</Grid>
								) : null}

								{sideVouchers.length > 0 ? (
									<Grid size={{ xs: 12, md: 5 }}>
										<Box component={motion.div} variants={homeStagger}>
											<Stack spacing={3}>
												{sideVouchers.map((voucher) => (
													<Box key={voucher.id} component={motion.div} variants={homeItemReveal}>
														<Paper
															variant='outlined'
															onClick={navigateToVouchers}
															sx={(theme) => ({
																p: 2.5,
																borderRadius: 4,
																display: 'grid',
																gridTemplateColumns: '92px 1fr',
																gap: 2,
																cursor: 'pointer',
																overflow: 'hidden',
																borderColor: alpha(theme.palette.warning.main, 0.18),
																background:
																	theme.palette.mode === 'dark'
																		? alpha(theme.palette.background.paper, 0.94)
																		: theme.palette.background.paper,
																transition: 'transform 0.24s ease, box-shadow 0.24s ease, border-color 0.24s ease',
																'&:hover': {
																	transform: 'translateY(-4px)',
																	boxShadow: theme.shadows[8],
																	borderColor: alpha(theme.palette.warning.main, 0.3),
																	'& .voucher-mini-action': {
																		transform: 'translateX(4px)',
																	},
																},
															})}
														>
															<Box
																component='img'
																src={getImageFromCloud(voucher.image)}
																alt={voucher?.name || t('voucher.title.collectible_vouchers')}
																onError={(event) => {
																	event.currentTarget.src = '/placeholder-image.png'
																}}
																sx={{
																	width: '100%',
																	height: 92,
																	objectFit: 'cover',
																	borderRadius: 3,
																}}
															/>
															<Stack spacing={1.1} minWidth={0}>
																<Stack
																	direction='row'
																	spacing={1}
																	justifyContent='space-between'
																	alignItems='flex-start'
																>
																	<Typography
																		variant='subtitle1'
																		sx={{
																			fontWeight: 800,
																			lineHeight: 1.2,
																			display: '-webkit-box',
																			WebkitLineClamp: 2,
																			WebkitBoxOrient: 'vertical',
																			overflow: 'hidden',
																		}}
																	>
																		{voucher?.name}
																	</Typography>
																	<Typography
																		variant='caption'
																		className='voucher-mini-action'
																		sx={{
																			color: 'warning.main',
																			fontWeight: 800,
																			transition: 'transform 0.2s ease',
																		}}
																	>
																		<ArrowForward sx={{ fontSize: 18 }} />
																	</Typography>
																</Stack>
																<Chip
																	label={getDiscountLabel(voucher)}
																	color='warning'
																	size='small'
																	sx={{ width: 'fit-content', fontWeight: 800 }}
																/>
																<Typography variant='body2' color='text.secondary'>
																	{voucher?.expireDate
																		? `${t('voucher.label.expire_date')}: ${formatDateBasedOnCurrentLanguage(
																				voucher.expireDate
																			)}`
																		: t('voucher.description.collectible_subtitle')}
																</Typography>
															</Stack>
														</Paper>
													</Box>
												))}
											</Stack>
										</Box>
									</Grid>
								) : null}
							</Grid>
						</Box>
					)}
				</Stack>
			</Container>
		</Box>
	)
}

export default VoucherPromoSection

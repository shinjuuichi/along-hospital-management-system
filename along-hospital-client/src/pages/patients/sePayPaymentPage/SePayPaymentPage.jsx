import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import { Box, Button, Card, Divider, Grid, Link, Paper, Stack, Typography } from '@mui/material'
import CardMedia from '@mui/material/CardMedia'
import { useEffect, useMemo, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'

const SePayPaymentPage = () => {
	const { t } = useTranslation()
	const location = useLocation()
	const navigate = useNavigate()
	const { paymentUrl, total, items } = location.state || {}

	useEffect(() => {
		if (!location.state) {
			navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.ORDER_HISTORY.INDEX))
		}
	}, [location.state, navigate])

	const [qrError, setQrError] = useState(false)

	const calculatedTotal = useMemo(() => {
		if (total != null) return total
		if (!items) return 0
		return (
			items.reduce((sum, item) => {
				const unitPrice = item.discountPrice ?? item.originPrice ?? item.medicineSKU?.price ?? 0
				return sum + unitPrice * item.quantity
			}, 0) || 0
		)
	}, [items, total])

	return (
		<Box sx={{ minHeight: '100vh', bgcolor: 'background.default', py: 4 }}>
			<Box sx={{ maxWidth: 900, mx: 'auto', px: 2 }}>
				<Stack direction='row' justifyContent='space-between' alignItems='center' mb={4}>
					<Button startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)}>
						{t('sepay.back')}
					</Button>
					<Typography variant='h4' fontWeight={900}>
						{t('sepay.payment_title')}
					</Typography>
					<Box />
				</Stack>

				<Grid container spacing={3} alignItems='stretch'>
					<Grid size={{ xs: 12, md: 5 }}>
						<Paper
							sx={{
								p: 3,
								borderRadius: 3,
								textAlign: 'center',
								border: (theme) => `1px solid ${theme.palette.divider}`,
								display: 'flex',
								flexDirection: 'column',
								alignItems: 'center',
								height: '100%',
							}}
						>
							<Stack direction='row' alignItems='center' justifyContent='center' spacing={1} mb={2}>
								<Typography variant='h6' fontWeight={800}>
									{t('sepay.qr_code')}
								</Typography>
							</Stack>

							<Box
								component='img'
								src={paymentUrl}
								alt={t('sepay.qr_code')}
								onError={() => setQrError(true)}
								sx={{ width: 200, height: 200, objectFit: 'contain', borderRadius: 2 }}
							/>
							{qrError && <Typography color='text.secondary'>{t('sepay.no_payment_url')}</Typography>}

							<Typography variant='body2' color='text.secondary' mt={2}>
								{t('sepay.scan_instruction')}
							</Typography>

							<Typography variant='body2' color='text.secondary' mt={2}>
								{t('sepay.view_order_history')}{' '}
								<Link
									component='button'
									underline='always'
									fontWeight='bold'
									onClick={() =>
										navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.ORDER_HISTORY.INDEX))
									}
								>
									{t('sepay.here')}
								</Link>
							</Typography>
						</Paper>
					</Grid>

					<Grid size={{ xs: 12, md: 7 }}>
						<Paper
							sx={{
								p: 3,
								borderRadius: 3,
								border: (theme) => `1px solid ${theme.palette.divider}`,
								display: 'flex',
								flexDirection: 'column',
								height: '100%',
							}}
						>
							<Typography variant='h6' fontWeight={800} mb={2}>
								{t('sepay.order_summary')}
							</Typography>

							<Divider sx={{ mb: 2 }} />

							<Box sx={{ flex: 1, overflowY: 'auto', maxHeight: 300 }}>
								<Stack spacing={2}>
									{(items || []).map((item) => {
										const skuCode = item.medicineSKU?.skuCode || item.skuCode
										if (!skuCode) return null

										const displayName = item.medicineSKU?.medicineName || ''
										const images = item.medicineSKU?.medicineImages ?? []
										const unitPrice = item.discountPrice ?? item.originPrice ?? item.medicineSKU?.price ?? 0
										const preview = images.length > 0 ? getImageFromCloud(images[0]) : null

										return (
											<Card key={skuCode} sx={{ display: 'flex', overflow: 'visible', alignItems: 'center' }}>
												<Box
													sx={{
														width: 80,
														height: 80,
														bgcolor: 'background.default',
														flexShrink: 0,
														display: 'flex',
														alignItems: 'center',
														justifyContent: 'center',
													}}
												>
													{preview ? (
														<CardMedia
															component='img'
															src={preview}
															alt={displayName}
															sx={{ width: '100%', height: '100%', borderRadius: 1 }}
														/>
													) : (
														<Typography color='text.secondary'>{t('sepay.no_image')}</Typography>
													)}
												</Box>

												<Box
													sx={{
														ml: 2,
														flex: 1,
														display: 'flex',
														flexDirection: 'column',
														alignItems: 'flex-end',
														textAlign: 'right',
													}}
												>
													<Typography variant='subtitle1' fontWeight={700}>
														{displayName || t('sepay.unknown')}
													</Typography>
													<Typography variant='body2' color='text.secondary'>
														x {item.quantity}
													</Typography>
													<Typography variant='body2' fontWeight={600}>
														{formatCurrencyBasedOnCurrentLanguage(unitPrice * item.quantity)}
													</Typography>
												</Box>
											</Card>
										)
									})}
								</Stack>
							</Box>

							<Divider sx={{ my: 2 }} />

							<Stack direction='row' justifyContent='space-between' alignItems='center'>
								<Typography variant='h6' fontWeight={800}>
									{t('sepay.total')}
								</Typography>
								<Typography variant='h5' fontWeight={900} color='primary.main'>
									{formatCurrencyBasedOnCurrentLanguage(calculatedTotal)}
								</Typography>
							</Stack>
						</Paper>
					</Grid>
				</Grid>
			</Box>
		</Box>
	)
}

export default SePayPaymentPage

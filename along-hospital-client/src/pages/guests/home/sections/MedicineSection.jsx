import { ApiUrls } from '@/configs/apiUrls'
import { defaultLineClampStyle } from '@/configs/defaultStylesConfig'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	homeItemReveal,
	homeSectionReveal,
	homeStagger,
	homeViewport,
} from '@/pages/guests/home/helpers/homeMotion'
import HomeSectionFallback from '@/pages/guests/home/sections/HomeSectionFallback'
import HomeSectionHeader from '@/pages/guests/home/sections/HomeSectionHeader'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { LocalMallOutlined, LocalOfferOutlined } from '@mui/icons-material'
import { alpha, Box, Button, Container, Grid, Paper, Skeleton, Stack, Typography } from '@mui/material'
import { motion } from 'framer-motion'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const MedicineCard = ({ medicine }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [selectedOptionValueIds, setSelectedOptionValueIds] = useState({})

	const activeSkus = (medicine?.skus ?? []).filter((sku) => sku?.isActive !== false)

	const selectedSku = (() => {
		if (activeSkus.length === 0) return null

		const selectedIds = Object.values(selectedOptionValueIds).filter((id) => typeof id === 'number')

		if (selectedIds.length === 0) return activeSkus[0]

		const matchedSku = activeSkus.find((sku) => {
			const skuValueIds = (sku?.skuValues ?? []).map((skuValue) => skuValue.optionValueId)
			return selectedIds.every((id) => skuValueIds.includes(id))
		})

		return matchedSku ?? activeSkus[0]
	})()

	const coverImage = getImageFromCloud(medicine?.images?.[0]) || '/placeholder-image.png'
	const currentPrice = selectedSku?.finalPrice ?? selectedSku?.price ?? null
	const originalPrice = selectedSku?.origionPrice ?? selectedSku?.price ?? null
	const discountAmount =
		selectedSku?.discountAmount ??
		(Math.max(Number(originalPrice || 0) - Number(currentPrice || 0), 0) || 0)
	const hasDiscount =
		Number(discountAmount || 0) > 0 && Number(originalPrice || 0) > Number(currentPrice || 0)

	return (
		<Paper
			variant='outlined'
			sx={(theme) => ({
				height: '100%',
				display: 'flex',
				flexDirection: 'column',
				borderRadius: 4,
				overflow: 'hidden',
				borderColor: alpha(theme.palette.primary.main, 0.12),
				background:
					theme.palette.mode === 'dark'
						? `linear-gradient(180deg, ${alpha(theme.palette.background.paper, 0.96)} 0%, ${alpha(
								theme.palette.primary.softBg,
								0.66
							)} 100%)`
						: `linear-gradient(180deg, ${theme.palette.background.paper} 0%, ${alpha(
								theme.palette.background.default,
								0.95
							)} 100%)`,
				'&:hover': {
					transform: 'translateY(-6px)',
					boxShadow: theme.shadows[10],
					borderColor: alpha(theme.palette.primary.main, 0.25),
					'& .medicine-card-image': {
						transform: 'scale(1.03)',
					},
					'& .medicine-card-action': {
						transform: 'translateX(4px)',
					},
				},
			})}
		>
			<Box
				onClick={() => navigate(`${routeUrls.HOME.MEDICINE}/${medicine.id}`)}
				sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', cursor: 'pointer' }}
			>
				<Box sx={{ position: 'relative', overflow: 'hidden', bgcolor: 'background.default' }}>
					<Box
						sx={(theme) => ({
							position: 'absolute',
							top: 14,
							left: 14,
							zIndex: 2,
							display: 'inline-flex',
							alignItems: 'center',
							gap: 0.75,
							px: 1.1,
							py: 0.7,
							borderRadius: 999,
							bgcolor: alpha(theme.palette.background.paper, 0.9),
							backdropFilter: 'blur(10px)',
							border: `1px solid ${alpha(theme.palette.primary.main, 0.12)}`,
						})}
					>
						<LocalMallOutlined sx={{ fontSize: 16, color: 'primary.main' }} />
						<Typography variant='caption' sx={{ fontWeight: 700 }}>
							{medicine?.medicineCategory?.name || medicine?.brand || 'Along'}
						</Typography>
					</Box>

					{hasDiscount ? (
						<Box
							sx={(theme) => ({
								position: 'absolute',
								top: 14,
								right: 14,
								zIndex: 2,
								display: 'inline-flex',
								alignItems: 'center',
								gap: 0.5,
								px: 1,
								py: 0.65,
								borderRadius: 999,
								bgcolor: alpha(theme.palette.warning.main, 0.14),
								color: 'warning.dark',
								fontWeight: 800,
							})}
						>
							<LocalOfferOutlined sx={{ fontSize: 16 }} />
							<Typography variant='caption' fontWeight={800}>
								-{formatCurrencyBasedOnCurrentLanguage(discountAmount)}
							</Typography>
						</Box>
					) : null}

					<Box
						component='img'
						className='medicine-card-image'
						src={coverImage}
						alt={medicine.name}
						onError={(event) => {
							event.currentTarget.src = '/placeholder-image.png'
						}}
						sx={{
							width: '100%',
							height: 240,
							objectFit: 'cover',
							transition: 'transform 0.28s ease',
						}}
					/>
				</Box>

				<Stack sx={{ flex: 1, p: 2.5 }} spacing={1.4}>
					<Typography
						variant='h6'
						component='h3'
						sx={{
							fontWeight: 800,
							fontSize: '1.08rem',
							minHeight: '3.1em',
							...defaultLineClampStyle(2),
						}}
					>
						{medicine.name}
					</Typography>
					<Typography variant='body2' color='text.secondary'>
						{renderEmptyFallback(medicine.brand)}
					</Typography>
					{medicine?.medicineUnit?.name ? (
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.unit')}: {medicine.medicineUnit.name}
						</Typography>
					) : null}
					{activeSkus.length > 0 ? (
						<Box sx={{ mt: 0.25 }}>
							<Stack spacing={1.1}>
								<Box
									sx={{
										display: 'grid',
										gridTemplateColumns: 'repeat(auto-fill, minmax(72px, 1fr))',
										gap: 1,
									}}
								>
									{activeSkus.map((sku) => {
										const skuValues = sku?.skuValues ?? []
										const skuLabel = sku.name
										const isSelected = selectedSku?.id === sku.id

										return (
											<Button
												key={sku.id}
												variant={isSelected ? 'contained' : 'outlined'}
												size='small'
												onClick={(event) => {
													event.stopPropagation()
													const newSelected = {}
													skuValues.forEach((skuValue) => {
														if (skuValue.optionValue?.optionId && skuValue.optionValue?.id) {
															newSelected[skuValue.optionValue.optionId] = skuValue.optionValue.id
														}
													})
													setSelectedOptionValueIds(newSelected)
												}}
												sx={(theme) => ({
													minWidth: 0,
													px: 1,
													py: 0.7,
													fontSize: '0.72rem',
													fontWeight: 700,
													textTransform: 'none',
													borderRadius: 999,
													borderColor: alpha(theme.palette.primary.main, 0.18),
													backgroundColor: isSelected
														? theme.palette.primary.main
														: alpha(theme.palette.background.default, 0.92),
													color: isSelected ? theme.palette.primary.contrastText : theme.palette.text.secondary,
												})}
											>
												{skuLabel}
											</Button>
										)
									})}
								</Box>
							</Stack>
						</Box>
					) : null}
					<Stack direction='row' justifyContent='space-between' alignItems='flex-end' sx={{ mt: 'auto' }}>
						<Stack spacing={0.35}>
							{hasDiscount ? (
								<Typography
									variant='body2'
									sx={{ color: 'text.disabled', textDecoration: 'line-through', fontWeight: 700 }}
								>
									{formatCurrencyBasedOnCurrentLanguage(originalPrice)}
								</Typography>
							) : null}
							<Typography variant='h6' color='primary.main' sx={{ fontWeight: 900 }}>
								{selectedSku
									? formatCurrencyBasedOnCurrentLanguage(currentPrice || 0)
									: t('cart.not_available')}
							</Typography>
						</Stack>
						<Typography
							variant='body2'
							className='medicine-card-action'
							sx={{ fontWeight: 700, color: 'text.secondary', transition: 'transform 0.2s ease' }}
						>
							{t('button.view_detail')}
						</Typography>
					</Stack>
				</Stack>
			</Box>
		</Paper>
	)
}

const MedicineSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const { data, loading, error } = useFetch(
		ApiUrls.MEDICINE.INDEX,
		{ page: 1, pageSize: 8, status: 'Active', isPublic: true },
		[]
	)

	const medicines = data?.collection || []

	return (
		<Box
			component={motion.section}
			initial='hidden'
			whileInView='show'
			viewport={homeViewport}
			variants={homeSectionReveal}
			sx={{ py: { xs: 7, md: 10 }, bgcolor: 'background.paper' }}
		>
			<Container maxWidth='xl' sx={{ px: { xs: 2, sm: 3, md: 4, lg: 6 } }}>
				<Stack spacing={4.5}>
					<HomeSectionHeader
						eyebrow={t('header.medicine')}
						title={t('home.medicine.title')}
						subtitle={t('home.medicine.subtitle')}
						actionLabel={t('button.view_all')}
						onAction={() => navigate(routeUrls.HOME.MEDICINE)}
					/>

					{loading ? (
						<Grid container spacing={{ xs: 2.5, sm: 3, md: 4 }}>
							{Array.from({ length: 8 }).map((_, index) => (
								<Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={index}>
									<Paper variant='outlined' sx={{ height: '100%', borderRadius: 4, overflow: 'hidden' }}>
										<Skeleton variant='rectangular' height={240} />
										<Stack spacing={1.2} sx={{ p: 2.5 }}>
											<Skeleton variant='text' height={30} width='80%' />
											<Skeleton variant='text' height={22} width='50%' />
											<Skeleton variant='text' height={28} width='38%' />
										</Stack>
									</Paper>
								</Grid>
							))}
						</Grid>
					) : medicines.length === 0 || error ? (
						<HomeSectionFallback
							title={t('medicine.placeholder.no_data')}
							subtitle={t('home.medicine.subtitle')}
							icon={<LocalMallOutlined sx={{ fontSize: 30 }} />}
						/>
					) : (
						<Box
							component={motion.div}
							initial='hidden'
							whileInView='show'
							viewport={homeViewport}
							variants={homeStagger}
						>
							<Grid container spacing={{ xs: 2.5, sm: 3, md: 4 }}>
								{medicines.map((medicine) => (
									<Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={medicine.id}>
										<Box component={motion.div} variants={homeItemReveal}>
											<MedicineCard medicine={medicine} />
										</Box>
									</Grid>
								))}
							</Grid>
						</Box>
					)}
				</Stack>
			</Container>
		</Box>
	)
}

export default MedicineSection

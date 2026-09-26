import { GenericPagination } from '@/components/generals/GenericPagination'
import SkeletonVoucher from '@/components/skeletons/SkeletonVoucher'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { LocalOffer, SearchOff } from '@mui/icons-material'
import { Box, Button, Grid, Paper, Stack, Typography, useMediaQuery, useTheme } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import VoucherCardSection from './sections/VoucherCardSection'

const MyVoucherListPage = () => {
	const theme = useTheme()
	const { t } = useTranslation()
	const navigate = useNavigate()
	const isMobile = useMediaQuery(theme.breakpoints.down('md'))
	const { auth } = useAuth()
	const [page, setPage] = useState(1)

	const { data, loading } = useFetch(ApiUrls.VOUCHER.MY_VOUCHERS, { page, pageSize: 10 }, [page])

	const handleCopyCode = (code) => navigator.clipboard.writeText(code)

	const handleUseVoucher = (voucher) => {
		navigate(routeUrls.HOME.MEDICINE, { state: { voucherCode: voucher.code } })
	}

	const vouchers = data?.collection || []
	const totalPages = data?.totalPage || 1

	return (
		<Box sx={{ minHeight: '100vh', bgcolor: 'background.default', pb: isMobile ? 8 : 4 }}>
			<Box
				sx={{
					bgcolor: 'background.paper',
					borderBottom: `1px solid ${theme.palette.divider}`,
					py: 3,
				}}
			>
				<Stack direction='row' alignItems='center' spacing={2}>
					<LocalOffer sx={{ fontSize: '2rem', color: 'primary.main' }} />
					<Box>
						<Typography variant='h4' fontWeight={700}>
							{t('voucher.title.my_vouchers')}
						</Typography>
						<Typography variant='body2' color='text.secondary'>
							{t('voucher.description.my_vouchers_subtitle')}
						</Typography>
					</Box>
				</Stack>
			</Box>

			{!isMobile && auth?.role === EnumConfig.Role.Patient && (
				<Box
					sx={{ bgcolor: 'background.paper', borderBottom: `1px solid ${theme.palette.divider}`, mb: 3 }}
				>
					<Box sx={{ display: 'flex', gap: 3 }}>
						<Box
							sx={{
								py: 2,
								px: 3,
								color: 'text.secondary',
								fontWeight: 500,
								cursor: 'pointer',
								'&:hover': { color: 'primary.main' },
							}}
							onClick={() => navigate(routeUrls.HOME.VOUCHERS)}
						>
							{t('voucher.title.collectible_vouchers')}
						</Box>
						<Box
							sx={{
								py: 2,
								px: 3,
								borderBottom: `3px solid ${theme.palette.primary.main}`,
								color: 'primary.main',
								fontWeight: 600,
								cursor: 'pointer',
							}}
						>
							{t('voucher.title.my_vouchers')}
						</Box>
					</Box>
				</Box>
			)}

			<Box sx={{ flexGrow: 1 }}>
				{loading && (
					<Grid container spacing={2}>
						{[...Array(10)].map((_, index) => (
							<Grid key={index} size={{ xs: 12, sm: 6 }}>
								<SkeletonVoucher />
							</Grid>
						))}
					</Grid>
				)}

				{!loading && vouchers.length === 0 && (
					<Paper
						elevation={0}
						sx={{
							p: 6,
							textAlign: 'center',
							bgcolor: 'background.paper',
							borderRadius: 2,
							border: `1px dashed ${theme.palette.divider}`,
						}}
					>
						<SearchOff sx={{ fontSize: 64, color: 'text.secondary', mb: 2 }} />
						<Typography variant='h6' gutterBottom color='text.primary'>
							{t('voucher.description.no_vouchers_title')}
						</Typography>
						<Typography variant='body2' color='text.secondary' sx={{ mb: 3 }}>
							{t('voucher.description.no_vouchers_subtitle')}
						</Typography>
						<Button variant='contained' onClick={() => navigate(routeUrls.HOME.VOUCHERS)}>
							{t('voucher.button.explore_vouchers')}
						</Button>
					</Paper>
				)}

				{!loading && vouchers.length > 0 && (
					<>
						<Grid container spacing={2}>
							{vouchers.map((voucher) => (
								<Grid key={voucher.id || voucher.code} size={{ xs: 12, sm: 6 }}>
									<VoucherCardSection
										voucher={voucher}
										mode='my'
										onCopy={handleCopyCode}
										onUse={handleUseVoucher}
									/>
								</Grid>
							))}
						</Grid>

						<Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
							<GenericPagination page={page} totalPage={totalPages} setPage={setPage} />
						</Box>
					</>
				)}
			</Box>
		</Box>
	)
}

export default MyVoucherListPage

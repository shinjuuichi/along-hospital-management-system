import { GenericPagination } from '@/components/generals/GenericPagination'
import EmptyPage from '@/components/placeholders/EmptyPage'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Card, CardContent, Grid, Skeleton, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import DoctorCardSection from './sections/DoctorCardSection'

export default function DoctorPage() {
	const { t } = useTranslation()
	const [page, setPage] = useState(1)
	const pageSize = 8

	const { data, loading } = useFetch(
		ApiUrls.STAFF.GET_BY_ROLE('doctor'),
		{ page, pageSize },
		[page, pageSize]
	)

	const doctors = data?.collection || []
	const totalPage = data?.totalPage || 1

	return (
		<Box
			sx={(theme) => ({
				p: { xs: 2, md: 3 },
				background:
					theme.palette.mode === 'dark'
						? 'radial-gradient(circle at top left, rgba(56, 189, 248, 0.12), transparent 32%), linear-gradient(180deg, #07111f 0%, #0b1728 100%)'
						: 'radial-gradient(circle at top left, rgba(14, 165, 233, 0.14), transparent 32%), linear-gradient(180deg, #f8fbff 0%, #edf5f7 100%)',
			})}
		>
			<Card
				elevation={0}
				sx={(theme) => ({
					mb: 4,
					overflow: 'hidden',
					borderRadius: 5,
					border: '1px solid',
					borderColor: 'divider',
					background:
						theme.palette.mode === 'dark'
							? 'linear-gradient(135deg, #10233a 0%, #123a52 56%, #0f5e62 100%)'
							: 'linear-gradient(135deg, #173b62 0%, #1f587f 54%, #257e86 100%)',
					color: 'common.white',
					boxShadow:
						theme.palette.mode === 'dark'
							? '0 18px 40px rgba(0, 0, 0, 0.28)'
							: '0 18px 40px rgba(23, 59, 98, 0.12)',
				})}
			>
				<Stack
					spacing={1.5}
					sx={{
						p: { xs: 3, md: 4 },
						maxWidth: 860,
						mx: 'auto',
						alignItems: 'center',
						textAlign: 'center',
					}}
				>
					<Typography variant='overline' sx={{ letterSpacing: 1.8, opacity: 0.78, fontWeight: 700 }}>
						{t('doctor.badge.queue_style')}
					</Typography>
					<Typography
						variant='h3'
						sx={{ fontWeight: 800, lineHeight: 1.1, fontSize: { xs: '2.1rem', md: '3.2rem' } }}
					>
						{t('doctor.title.team')}
					</Typography>
					<Typography
						sx={{ color: 'rgba(255,255,255,0.82)', maxWidth: 700, fontSize: { xs: 15, md: 18 } }}
					>
						{t('doctor.text.queue_description')}
					</Typography>
				</Stack>
			</Card>
			{loading ? (
				<Grid container spacing={3}>
					{Array.from({ length: 6 }).map((_, index) => (
						<Grid size={{ xs: 12, sm: 6, md: 4 }} key={index}>
							<Card
								sx={{
									display: 'flex',
									flexDirection: 'column',
									height: '100%',
									borderRadius: 5,
									overflow: 'hidden',
								}}
							>
								<Skeleton variant='rectangular' height={220} />
								<CardContent sx={{ flexGrow: 1 }}>
									<Skeleton variant='text' width='60%' height={28} sx={{ mb: 1 }} />
									<Skeleton variant='text' width='40%' height={20} sx={{ mb: 0.5 }} />
									<Skeleton variant='text' width='80%' height={20} />
								</CardContent>
								<Box sx={{ p: 2, pt: 0 }}>
									<Skeleton variant='rectangular' width={100} height={36} />
								</Box>
							</Card>
						</Grid>
					))}
				</Grid>
			) : doctors.length === 0 ? (
				<EmptyPage title={t('doctor.placeholder.no_doctors')} showButton={false} />
			) : (
				<Stack spacing={4}>
					<Grid container spacing={2.5} sx={{ height: '100%' }}>
						{doctors.map((d, idx) => (
							<Grid size={{ xs: 12, sm: 6, lg: 4, xl: 3 }} key={d.id ?? idx}>
								<DoctorCardSection doctor={d} />
							</Grid>
						))}
					</Grid>
					{totalPage > 1 && (
						<Box sx={{ display: 'flex', justifyContent: 'center' }}>
							<GenericPagination page={page} totalPage={totalPage} setPage={setPage} />
						</Box>
					)}
				</Stack>
			)}
		</Box>
	)
}

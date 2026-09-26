import EmptyPage from '@/components/placeholders/EmptyPage'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import { Box, Container, Grid, Paper, Skeleton, Stack } from '@mui/material'
import { useMemo, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import JobPostingContentSection from './sections/JobPostingContentSection'
import JobApplicationFormSection from './sections/JobApplicationFormSection'
import RecentJobPostingsSection from './sections/RecentJobPostingsSection'

const RECENT_PARAMS = { Page: 1, PageSize: 6, Sort: 'id desc' }

const JobPostingDetailPage = () => {
	const { id } = useParams()
	const navigate = useNavigate()
	const [applyDialogOpen, setApplyDialogOpen] = useState(false)

	const { data: job, loading } = useFetch(ApiUrls.JOB_POSTING.DETAIL(id), {}, [id])

	const handleApply = () => {
		if (!loading && id) setApplyDialogOpen(true)
	}
	const { data: recentData, loading: recentLoading } = useFetch(ApiUrls.JOB_POSTING.INDEX, RECENT_PARAMS, [])

	const recentJobs = useMemo(() => {
		return (recentData?.collection || []).filter((item) => item.id !== job?.id).slice(0, 5)
	}, [recentData?.collection, job?.id])

	const handleBack = () => {
		navigate(-1)
	}

	const navigateToJob = (jobId) => jobId && navigate(routeUrls.HOME.JOB_POSTING.DETAIL(jobId))

	if (loading) {
		return (
			<Box sx={{ bgcolor: 'background.default', py: { xs: 3, md: 5 } }}>
				<Container maxWidth='lg'>
					<Grid
						container
						spacing={{ xs: 3, lg: 4 }}
						alignItems='flex-start'
						sx={{ flexWrap: { xs: 'wrap', lg: 'nowrap' } }}
					>
						<Grid size={{ xs: 12, md: 8, lg: 9 }} sx={{ minWidth: 0 }}>
							<Paper sx={{ borderRadius: 3, boxShadow: 6, overflow: 'hidden' }}>
								<Box sx={{ p: { xs: 3, md: 5 } }}>
									<Stack spacing={3}>
										<Stack spacing={2}>
											<Skeleton variant='text' width='90%' height={56} />
											<Stack direction='row' spacing={2}>
												<Skeleton variant='text' width={100} height={24} />
												<Skeleton variant='text' width={120} height={24} />
											</Stack>
										</Stack>
										<Skeleton variant='rectangular' height={{ xs: 150, md: 200 }} sx={{ borderRadius: 2 }} />
									</Stack>
								</Box>
							</Paper>
						</Grid>

						<Grid
							size={{ xs: 12, md: 4, lg: 3 }}
							sx={{
								minWidth: { lg: 280, xs: '100%' },
								maxWidth: { lg: 320, xs: '100%' },
								flexShrink: 0,
							}}
						>
							<Paper sx={{ p: { xs: 2.5, md: 3 }, borderRadius: 3, boxShadow: 4 }}>
								<Stack spacing={2}>
									<Skeleton variant='text' width={120} height={24} />
									<Box sx={{ height: 1, bgcolor: 'divider' }} />
									<Stack spacing={2}>
										{[0, 1, 2].map((key) => (
											<Stack key={key} spacing={1}>
												<Skeleton variant='text' width='100%' height={24} />
												<Skeleton variant='text' width='70%' />
											</Stack>
										))}
									</Stack>
								</Stack>
							</Paper>
						</Grid>
					</Grid>
				</Container>
			</Box>
		)
	}

	if (!job?.id) {
		return <EmptyPage showButton />
	}

	return (
		<Box sx={{ bgcolor: 'background.default', py: { xs: 3, md: 5 }, minHeight: '80vh' }}>
			<Container maxWidth='lg'>
				<Grid
					container
					spacing={{ xs: 3, lg: 4 }}
					alignItems='flex-start'
					sx={{ flexWrap: { xs: 'wrap', lg: 'nowrap' } }}
				>
					<Grid size={{ xs: 12, md: 8, lg: 9 }} sx={{ minWidth: 0 }}>
						<JobPostingContentSection
							job={job}
							onBack={handleBack}
							onApply={handleApply}
						/>
					</Grid>

					<Grid
						size={{ xs: 12, md: 4, lg: 3 }}
						sx={{
							minWidth: { lg: 280, xs: '100%' },
							maxWidth: { lg: 320, xs: '100%' },
							flexShrink: 0,
						}}
					>
						<RecentJobPostingsSection loading={recentLoading} jobs={recentJobs} onNavigate={navigateToJob} />
					</Grid>

					<JobApplicationFormSection
						open={applyDialogOpen}
						onClose={() => setApplyDialogOpen(false)}
						jobPostingId={id}
					/>
				</Grid>
			</Container>
		</Box>
	)
}

export default JobPostingDetailPage

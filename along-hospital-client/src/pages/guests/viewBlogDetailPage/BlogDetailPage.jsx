import EmptyPage from '@/components/placeholders/EmptyPage'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { Box, Container, Grid, Paper, Skeleton, Stack } from '@mui/material'
import { useMemo } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import BlogContentSection from './sections/BlogContentSection'
import RecentBlogsSection from './sections/RecentBlogsSection'

const RECENT_PARAMS = { Page: 1, PageSize: 6 }

const BlogDetailPage = () => {
	const { id } = useParams()
	const navigate = useNavigate()
	const { t } = useTranslation()

	const { data: blogRaw, loading } = useFetch(`${ApiUrls.BLOG.INDEX}/${id}`, {}, [id])
	const { data: recentRaw, loading: recentLoading } = useFetch(ApiUrls.BLOG.INDEX, RECENT_PARAMS, [
		id,
	])

	const blog = blogRaw || {}
	const coverImage = getImageFromCloud(blog.image)
	const formattedDate = formatDateBasedOnCurrentLanguage(blog.creationDate)
	const categoryName = blog.blogCategory?.name || t('blog.text.no_category')

	const recentBlogs = useMemo(() => {
		const collection = recentRaw?.collection || []
		return collection.filter((item) => item.id !== blog.id).slice(0, 2)
	}, [recentRaw?.collection, blog.id])

	const handleBack = () => {
		navigate(-1)
	}

	const navigateToBlog = (blogId) => blogId && navigate(`${routeUrls.HOME.BLOG}/${blogId}`)

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

										<Skeleton variant='rectangular' height={{ xs: 280, sm: 360, md: 480 }} sx={{ borderRadius: 2 }} />

										<Stack spacing={2}>
											<Skeleton variant='text' width='100%' height={24} />
											<Skeleton variant='text' width='95%' height={24} />
											<Skeleton variant='text' width='90%' height={24} />
											<Skeleton variant='text' width='85%' height={24} />
											<Skeleton variant='text' width='100%' height={24} />
											<Skeleton variant='text' width='80%' height={24} />
										</Stack>

										<Skeleton variant='rectangular' width={120} height={40} sx={{ borderRadius: 2 }} />
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
										{[0, 1].map((key) => (
											<Stack key={key} direction='row' spacing={2} alignItems='center'>
												<Skeleton variant='rectangular' width={84} height={60} sx={{ borderRadius: 2 }} />
												<Stack spacing={1} sx={{ flex: 1 }}>
													<Skeleton variant='text' width='100%' />
													<Skeleton variant='text' width='70%' />
												</Stack>
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

	if (!blog?.id) {
		return <EmptyPage showButton />
	}

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
						<BlogContentSection
							title={blog.title}
							categoryName={categoryName}
							formattedDate={formattedDate}
							content={blog.content}
							onBack={handleBack}
							showImage={Boolean(blog.image)}
							coverImage={coverImage}
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
						<RecentBlogsSection loading={recentLoading} blogs={recentBlogs} onNavigate={navigateToBlog} />
					</Grid>
				</Grid>
			</Container>
		</Box>
	)
}

export default BlogDetailPage

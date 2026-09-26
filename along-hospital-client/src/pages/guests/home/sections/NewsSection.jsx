import { ApiUrls } from '@/configs/apiUrls'
import { defaultLineClampStyle } from '@/configs/defaultStylesConfig'
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
import { ArrowForward, ArticleOutlined } from '@mui/icons-material'
import {
	alpha,
	Box,
	Button,
	Container,
	Grid,
	Paper,
	Skeleton,
	Stack,
	Typography,
} from '@mui/material'
import { motion } from 'framer-motion'
import { useNavigate } from 'react-router-dom'

const stripHtml = (value) => {
	if (!value) return ''
	return value
		.replace(/<[^>]+>/g, ' ')
		.replace(/\s+/g, ' ')
		.trim()
}

const buildSnippet = (blog, fallback) => {
	const content = stripHtml(blog?.description || blog?.content || '')
	if (!content) return fallback

	const words = content.split(/\s+/)
	if (words.length <= 24) return content

	return `${words.slice(0, 24).join(' ')}...`
}

const LargeNewsCard = ({ blog }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const formattedDate = blog.creationDate ? new Date(blog.creationDate).toLocaleDateString() : ''
	const categoryName = blog.blogCategory?.name || t('blog.text.no_category')

	return (
		<Paper
			variant='outlined'
			onClick={() => navigate(`${routeUrls.HOME.BLOG}/${blog.id}`)}
			sx={(theme) => ({
				position: 'relative',
				overflow: 'hidden',
				height: '100%',
				minHeight: 420,
				borderRadius: 5,
				cursor: 'pointer',
				borderColor: alpha(theme.palette.primary.main, 0.14),
				transition: 'transform 0.24s ease, box-shadow 0.24s ease',
				'&:hover': {
					transform: 'translateY(-6px)',
					boxShadow: theme.shadows[10],
					'& .news-large-image': {
						transform: 'scale(1.03)',
					},
					'& .news-large-action': {
						transform: 'translateX(4px)',
					},
				},
			})}
		>
			<Box sx={{ position: 'absolute', inset: 0, overflow: 'hidden' }}>
				<Box
					component='img'
					className='news-large-image'
					src={getImageFromCloud(blog.image) || '/placeholder-image.png'}
					alt={blog.title || t('blog.text.image_alt')}
					onError={(event) => {
						event.currentTarget.src = '/placeholder-image.png'
					}}
					sx={{
						width: '100%',
						height: '100%',
						objectFit: 'cover',
						transition: 'transform 0.28s ease',
					}}
				/>
				<Box
					sx={(theme) => ({
						position: 'absolute',
						inset: 0,
						background:
							theme.palette.mode === 'dark'
								? 'linear-gradient(180deg, rgba(15,23,42,0.12) 0%, rgba(15,23,42,0.92) 100%)'
								: 'linear-gradient(180deg, rgba(15,23,42,0.08) 0%, rgba(15,23,42,0.78) 100%)',
					})}
				/>
			</Box>

			<Stack
				justifyContent='flex-end'
				sx={{ position: 'relative', zIndex: 1, height: '100%', p: { xs: 3, md: 4 } }}
				spacing={1.4}
			>
				<Stack direction='row' justifyContent='space-between' alignItems='center' spacing={1}>
					<Typography variant='caption' sx={{ fontWeight: 800, color: 'rgba(255,255,255,0.82)' }}>
						{categoryName}
					</Typography>
					<Typography variant='caption' sx={{ color: 'rgba(255,255,255,0.7)' }}>
						{formattedDate}
					</Typography>
				</Stack>
				<Typography
					variant='h4'
					sx={{
						fontWeight: 900,
						lineHeight: 1.08,
						fontSize: { xs: '1.8rem', md: '2.25rem' },
						color: 'common.white',
						maxWidth: 640,
					}}
				>
					{blog.title || t('blog.title.untitled')}
				</Typography>
				<Typography sx={{ color: 'rgba(255,255,255,0.75)', maxWidth: 640 }}>
					{buildSnippet(blog, t('home.news.subtitle'))}
				</Typography>
				<Button
					variant='text'
					color='inherit'
					endIcon={<ArrowForward className='news-large-action' />}
					sx={{
						px: 0,
						alignSelf: 'flex-start',
						fontWeight: 800,
						textTransform: 'none',
						'& .news-large-action': { transition: 'transform 0.2s ease' },
					}}
				>
					{t('button.view_detail')}
				</Button>
			</Stack>
		</Paper>
	)
}

const SmallNewsCard = ({ blog }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const formattedDate = blog.creationDate ? new Date(blog.creationDate).toLocaleDateString() : ''
	const categoryName = blog.blogCategory?.name || t('blog.text.no_category')

	return (
		<Paper
			variant='outlined'
			onClick={() => navigate(`${routeUrls.HOME.BLOG}/${blog.id}`)}
			sx={(theme) => ({
				display: 'grid',
				gridTemplateColumns: { xs: '1fr', sm: '140px 1fr' },
				gap: 2,
				p: 2,
				borderRadius: 4,
				cursor: 'pointer',
				borderColor: alpha(theme.palette.primary.main, 0.12),
				transition: 'transform 0.24s ease, box-shadow 0.24s ease',
				'&:hover': {
					transform: 'translateY(-4px)',
					boxShadow: theme.shadows[8],
					'& .news-small-image': {
						transform: 'scale(1.03)',
					},
				},
			})}
		>
			<Box sx={{ overflow: 'hidden', borderRadius: 3, minHeight: 120 }}>
				<Box
					component='img'
					className='news-small-image'
					src={getImageFromCloud(blog.image) || '/placeholder-image.png'}
					alt={blog.title || t('blog.text.image_alt')}
					onError={(event) => {
						event.currentTarget.src = '/placeholder-image.png'
					}}
					sx={{
						width: '100%',
						height: '100%',
						minHeight: 120,
						objectFit: 'cover',
						transition: 'transform 0.28s ease',
					}}
				/>
			</Box>
			<Stack spacing={1} justifyContent='center' minWidth={0}>
				<Stack direction='row' justifyContent='space-between' spacing={1}>
					<Typography variant='caption' sx={{ fontWeight: 800, color: 'primary.main' }}>
						{categoryName}
					</Typography>
					<Typography variant='caption' color='text.secondary'>
						{formattedDate}
					</Typography>
				</Stack>
				<Typography variant='h6' sx={{ fontWeight: 800, ...defaultLineClampStyle(2) }}>
					{blog.title || t('blog.title.untitled')}
				</Typography>
				<Typography variant='body2' color='text.secondary' sx={{ ...defaultLineClampStyle(2) }}>
					{buildSnippet(blog, t('home.news.subtitle'))}
				</Typography>
			</Stack>
		</Paper>
	)
}

const NewsSection = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const { data, loading, error } = useFetch(ApiUrls.BLOG.INDEX, { page: 1, pageSize: 4 }, [])

	const blogs = data?.collection || []
	const featuredBlog = blogs[0]
	const sideBlogs = blogs.slice(1)

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
						? `linear-gradient(180deg, ${theme.palette.background.default} 0%, ${alpha(
								theme.palette.primary.softBg,
								0.28
							)} 100%)`
						: `linear-gradient(180deg, ${theme.palette.background.paper} 0%, ${alpha(
								theme.palette.primary.softBg,
								0.42
							)} 100%)`,
			})}
		>
			<Container maxWidth='xl' sx={{ px: { xs: 2, sm: 3, md: 4, lg: 6 } }}>
				<Stack spacing={4.5}>
					<HomeSectionHeader
						eyebrow={t('header.blog')}
						title={t('home.news.title')}
						subtitle={t('home.news.subtitle')}
						actionLabel={t('button.view_all')}
						onAction={() => navigate(routeUrls.HOME.BLOG)}
					/>

					{loading ? (
						<Grid container spacing={3}>
							<Grid size={{ xs: 12, md: 7 }}>
								<Skeleton variant='rounded' height={420} sx={{ borderRadius: 5 }} />
							</Grid>
							<Grid size={{ xs: 12, md: 5 }}>
								<Stack spacing={3}>
									{Array.from({ length: 3 }).map((_, index) => (
										<Skeleton key={index} variant='rounded' height={136} sx={{ borderRadius: 4 }} />
									))}
								</Stack>
							</Grid>
						</Grid>
					) : blogs.length === 0 || error ? (
						<HomeSectionFallback
							title={t('home.news.no_news')}
							subtitle={t('home.news.subtitle')}
							icon={<ArticleOutlined sx={{ fontSize: 30 }} />}
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
								{featuredBlog ? (
									<Grid size={{ xs: 12, md: 7 }}>
										<Box component={motion.div} variants={homeItemReveal}>
											<LargeNewsCard blog={featuredBlog} />
										</Box>
									</Grid>
								) : null}

								{sideBlogs.length > 0 ? (
									<Grid size={{ xs: 12, md: 5 }}>
										<Box component={motion.div} variants={homeStagger}>
											<Stack spacing={3}>
												{sideBlogs.map((blog) => (
													<Box key={blog.id} component={motion.div} variants={homeItemReveal}>
														<SmallNewsCard blog={blog} />
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

export default NewsSection

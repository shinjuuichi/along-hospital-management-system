import EmptyPage from '@/components/placeholders/EmptyPage'
import { defaultLineClampStyle } from '@/configs/defaultStylesConfig'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import {
    Box,
    Button,
    Card,
    CardActionArea,
    CardContent,
    CardMedia,
    Grid,
    Skeleton,
    Stack,
    Typography,
} from '@mui/material'
import { useNavigate } from 'react-router-dom'

const BlogCard = ({ blog = {} }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const formattedDate = formatDateBasedOnCurrentLanguage(blog.creationDate)

	const categoryName = blog.blogCategory?.name || t('blog.text.no_category')

	const handleCardClick = () => {
		navigate(`${routeUrls.HOME.BLOG}/${blog.id}`)
	}

	return (
		<Card
			sx={{
				display: 'flex',
				flexDirection: 'column',
				height: '100%',
				borderRadius: 2,
				overflow: 'hidden',
				transition: 'all 0.3s ease',
				'&:hover': {
					boxShadow: 8,
					transform: 'translateY(-8px)',
				},
			}}
		>
			<CardActionArea onClick={handleCardClick} sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
				<CardMedia
					component='img'
					image={getImageFromCloud(blog.image) || '/placeholder-image.png'}
					alt={blog.title || t('blog.text.image_alt')}
					onError={(event) => {
						event.currentTarget.src = '/placeholder-image.png'
					}}
					sx={{
						width: '100%',
						height: 200,
						objectFit: 'cover',
						bgcolor: 'grey.100',
					}}
				/>
				<CardContent sx={{ flexGrow: 1, width: '100%', p: 2.5 }}>
					<Stack spacing={1.5}>
						<Stack direction='row' justifyContent='space-between' alignItems='center' spacing={1}>
							<Typography
								variant='caption'
								sx={{
									fontWeight: 700,
									color: 'primary.main',
									fontSize: '0.75rem',
									textTransform: 'uppercase',
								}}
							>
								{categoryName}
							</Typography>
							<Typography variant='caption' color='text.secondary' sx={{ fontSize: '0.75rem' }}>
								{formattedDate}
							</Typography>
						</Stack>
						<Typography
							variant='h6'
							component='h3'
							sx={{
								...defaultLineClampStyle(2),
								fontWeight: 600,
								fontSize: '1.125rem',
								lineHeight: 1.4,
								minHeight: '3em',
							}}
						>
							{blog.title || t('blog.title.untitled')}
						</Typography>
					</Stack>
				</CardContent>
			</CardActionArea>
			<Box sx={{ px: 2.5, pb: 2.5 }}>
				<Button
					size='small'
					onClick={handleCardClick}
					sx={{
						textTransform: 'none',
						fontWeight: 500,
					}}
				>
					{t('blog.text.read_more')}
				</Button>
			</Box>
		</Card>
	)
}

const BlogListSection = ({ blogs = [], loading = false }) => {
	const { t } = useTranslation()
	const safeBlogs = Array.isArray(blogs) ? blogs : []

	if (loading) {
		return (
			<Grid container spacing={3}>
				{Array.from({ length: 6 }).map((_, index) => (
					<Grid size={{ xs: 12, sm: 6, md: 4 }} key={index}>
						<Card sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
							<Skeleton variant='rectangular' height={180} />
							<CardContent sx={{ flexGrow: 1 }}>
								<Stack
									direction='row'
									justifyContent='space-between'
									alignItems='center'
									spacing={1}
									sx={{ mb: 1 }}
								>
									<Skeleton variant='text' width={80} height={20} />
									<Skeleton variant='text' width={60} height={20} />
								</Stack>
								<Skeleton variant='text' width='100%' height={32} sx={{ mb: 0.5 }} />
								<Skeleton variant='text' width='90%' height={32} sx={{ mb: 1 }} />
							</CardContent>
							<Box sx={{ p: 2, pt: 0 }}>
								<Skeleton variant='rectangular' width={100} height={36} />
							</Box>
						</Card>
					</Grid>
				))}
			</Grid>
		)
	}

	if (safeBlogs.length === 0) {
		return (
			<EmptyPage
				title={t('blog.text.no_posts')}
				subtitle={t('blog.text.no_posts_subtitle')}
				showButton={false}
			/>
		)
	}

	return (
		<Grid container spacing={3}>
			{safeBlogs.map((b, index) => (
				<Grid size={{ xs: 12, sm: 6, md: 4 }} key={b.id ?? b.blogId ?? index}>
					<BlogCard blog={b} />
				</Grid>
			))}
		</Grid>
	)
}

export default BlogListSection

import useTranslation from '@/hooks/useTranslation'
import { processHtmlImages } from '@/utils/commons'
import { sanitizeHtml } from '@/utils/sanitizeHtml'
import { ArrowBack, CalendarToday, FolderOutlined } from '@mui/icons-material'
import { Box, Button, CardMedia, Divider, Paper, Stack, Typography } from '@mui/material'
import { HTML_CONTENT_GUEST_SX } from '@/configs/htmlContentStylesConfig'

const IMAGE_ERROR_HANDLER = (event) => {
	event.currentTarget.onerror = null
	event.currentTarget.src = '/placeholder-image.png'
}

const IMAGE_STYLES = {
	cover: {
		width: '100%',
		height: { xs: 280, sm: 360, md: 480 },
		objectFit: 'cover',
		borderRadius: 2,
	},
}

const PAPER_STYLES = {
	borderRadius: 3,
	boxShadow: 6,
	overflow: 'hidden',
	width: '100%',
	maxWidth: '100%',
	boxSizing: 'border-box',
	p: { xs: 3, md: 5 },
}

const BlogContentSection = ({
	title = '',
	categoryName = '',
	formattedDate = '',
	content = '',
	onBack,
	showImage,
	coverImage = '',
}) => {
	const { t } = useTranslation()
	const fallbackTitle = t('blog.title.untitled')

	return (
		<Stack component={Paper} spacing={3} sx={PAPER_STYLES}>
			<Typography
				variant='h3'
				component='h1'
				sx={{
					fontSize: { xs: '1.75rem', sm: '2.25rem', md: '2.75rem' },
					fontWeight: 700,
					lineHeight: 1.2,
					color: 'text.primary',
				}}
			>
				{title || fallbackTitle}
			</Typography>

			<Stack direction='row' spacing={2} alignItems='center' flexWrap='wrap' sx={{ mt: -1 }}>
				<Stack direction='row' spacing={0.75} alignItems='center'>
					<FolderOutlined sx={{ fontSize: 18, color: 'primary.main' }} />
					<Typography
						variant='body2'
						sx={{ color: 'primary.main', fontWeight: 600, fontSize: '0.875rem' }}
					>
						{categoryName}
					</Typography>
				</Stack>

				{formattedDate && (
					<>
						<Divider orientation='vertical' flexItem sx={{ height: 20 }} />
						<Stack direction='row' spacing={0.75} alignItems='center'>
							<CalendarToday sx={{ fontSize: 16, color: 'text.secondary' }} />
							<Typography variant='body2' color='text.secondary' sx={{ fontSize: '0.875rem' }}>
								{formattedDate}
							</Typography>
						</Stack>
					</>
				)}
			</Stack>

			{showImage && (
				<CardMedia
					component='img'
					image={coverImage}
					alt={title || fallbackTitle}
					sx={IMAGE_STYLES.cover}
					onError={IMAGE_ERROR_HANDLER}
				/>
			)}

			<Box
				className='blog-content-wrapper'
				sx={{
					...HTML_CONTENT_GUEST_SX,
					fontSize: { xs: '1rem', md: '1.0625rem' },
					color: 'text.primary',
					minHeight: { xs: '200px', md: '300px' },
					py: 2,
				}}
				dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(content)) }}
			/>

			<Button
				variant='outlined'
				startIcon={<ArrowBack />}
				onClick={onBack}
				sx={{
					alignSelf: 'flex-start',
					borderRadius: 2,
					px: 3,
					py: 1,
					textTransform: 'none',
					fontSize: '0.9375rem',
				}}
			>
				{t('button.back')}
			</Button>
		</Stack>
	)
}

export default BlogContentSection

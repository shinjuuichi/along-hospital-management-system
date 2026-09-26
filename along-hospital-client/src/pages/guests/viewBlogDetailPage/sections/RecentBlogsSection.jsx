import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { ArrowForward } from '@mui/icons-material'
import { Box, Button, Divider, Paper, Skeleton, Stack, Typography } from '@mui/material'

const IMAGE_ERROR_HANDLER = (event) => {
	event.currentTarget.onerror = null
	event.currentTarget.src = '/placeholder-image.png'
}

const IMAGE_STYLES = {
	thumbnail: {
		width: 84,
		height: 60,
		borderRadius: 2,
		overflow: 'hidden',
		objectFit: 'cover',
		bgcolor: 'primary.softBg',
	},
}

const PAPER_STYLES = {
	borderRadius: 3,
	boxShadow: 4,
}

const RecentBlogsSection = ({ loading = false, blogs = [], onNavigate = () => { } }) => {
	const { t } = useTranslation()
	const title = t('blog.text.recent_posts')
	const untitledLabel = t('blog.title.untitled')
	const readMoreLabel = t('blog.text.read_more')
	const emptyLabel = t('blog.text.no_recent')
	return (
		<Paper sx={{ p: { xs: 2.5, md: 3 }, width: '100%', ...PAPER_STYLES }}>
			<Stack spacing={2}>
				<Typography variant='subtitle1' sx={{ fontWeight: 700 }}>
					{title}
				</Typography>
				<Divider />
				{loading ? (
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
				) : blogs.length > 0 ? (
					<Stack spacing={2.5}>
						{blogs.map((item) => {
							const recentImage = getImageFromCloud(item.image)
							const recentDate = formatDateBasedOnCurrentLanguage(item.creationDate)

							return (
								<Box
									key={item.id}
									sx={{
										display: 'flex',
										gap: 1.5,
										alignItems: 'flex-start',
										cursor: 'pointer',
										'&:hover .recent-title': {
											color: 'primary.main',
										},
									}}
									onClick={() => onNavigate(item.id)}
								>
									<Box
										component='img'
										src={recentImage || '/placeholder-image.png'}
										alt={item.title || untitledLabel}
										sx={IMAGE_STYLES.thumbnail}
										onError={IMAGE_ERROR_HANDLER}
									/>
									<Stack spacing={0.5} sx={{ flex: 1 }}>
										<Typography variant='caption' color='text.secondary'>
											{recentDate}
										</Typography>
										<Typography
											variant='subtitle2'
											className='recent-title'
											sx={{ fontWeight: 600, transition: 'color 0.2s' }}
										>
											{item.title || untitledLabel}
										</Typography>
										<Button
											variant='text'
											size='small'
											endIcon={<ArrowForward fontSize='small' />}
											sx={{ alignSelf: 'flex-start', mt: 0.5, textTransform: 'none' }}
										>
											{readMoreLabel}
										</Button>
									</Stack>
								</Box>
							)
						})}
					</Stack>
				) : (
					<Typography variant='body2' color='text.secondary'>
						{emptyLabel}
					</Typography>
				)}
			</Stack>
		</Paper>
	)
}

export default RecentBlogsSection

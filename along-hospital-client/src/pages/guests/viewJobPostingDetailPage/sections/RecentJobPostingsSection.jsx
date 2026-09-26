import useTranslation from '@/hooks/useTranslation'
import { Box, CardActionArea, Divider, Paper, Skeleton, Stack, Typography } from '@mui/material'

const RecentJobPostingsSection = ({ loading, jobs = [], onNavigate }) => {
	const { t } = useTranslation()

	if (loading) {
		return (
			<Paper sx={{ p: { xs: 2.5, md: 3 }, borderRadius: 3, boxShadow: 4 }}>
				<Stack spacing={2}>
					<Skeleton variant='text' width={120} height={32} />
					<Skeleton variant='text' width={80} height={8} />
					{[1, 2, 3].map((i) => (
						<Box key={i}>
							<Skeleton variant='text' width='90%' height={24} />
							<Skeleton variant='text' width={100} height={20} />
						</Box>
					))}
				</Stack>
			</Paper>
		)
	}

	if (!jobs.length) {
		return null
	}

	return (
		<Paper sx={{ p: { xs: 2.5, md: 3 }, borderRadius: 3, boxShadow: 4, position: 'sticky', top: 24 }}>
			<Stack spacing={2}>
				<Typography variant='h6' fontWeight={700} sx={{ fontSize: '1.125rem' }}>
					{t('job_posting.guest.title.recent')}
				</Typography>

				<Box sx={{ height: 2, width: 40, bgcolor: 'primary.main', borderRadius: 1 }} />

				<Stack spacing={2} divider={<Divider />}>
					{jobs.map((job) => (
						<CardActionArea
							key={job.id}
							onClick={() => onNavigate(job.id)}
							sx={{ borderRadius: 2, p: 1, m: -1 }}
						>
							<Stack spacing={1}>
								<Typography
									variant='subtitle2'
									fontWeight={600}
									sx={{
										display: '-webkit-box',
										WebkitLineClamp: 2,
										WebkitBoxOrient: 'vertical',
										overflow: 'hidden',
										lineHeight: 1.4,
										'&:hover': { color: 'primary.main' },
									}}
								>
									{job.title}
								</Typography>
								<Typography variant='body2' color='text.secondary'>
									{job.role}
								</Typography>
							</Stack>
						</CardActionArea>
					))}
				</Stack>
			</Stack>
		</Paper>
	)
}

export default RecentJobPostingsSection

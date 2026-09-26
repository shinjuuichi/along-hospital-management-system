import useTranslation from '@/hooks/useTranslation'
import { Box, Button, Grid, LinearProgress, Paper, Rating, Stack, Typography } from '@mui/material'

const FeedbackSummarySection = ({ summary, onOpenReview }) => {
	const { t } = useTranslation()
	const total = summary?.total || 0
	const avg = Number(summary?.average || 0).toFixed(1)
	const dist = summary?.distribution || { 5: 0, 4: 0, 3: 0, 2: 0, 1: 0 }

	const percent = (n) => (total > 0 ? Math.round((n * 100) / total) : 0)

	return (
		<Paper sx={{ p: 2, borderRadius: 2 }}>
			<Grid container spacing={2} alignItems='center'>
				<Grid size={{ xs: 12, md: 4 }}>
					<Stack alignItems='center' spacing={1} py={2}>
						<Typography variant='h3' fontWeight={700}>
							{avg}
						</Typography>
						<Rating name='avg-rating' value={Number(summary?.average || 0)} precision={0.1} readOnly />
						<Typography variant='body2' color='text.secondary'>
							{t('feedback.text.total_reviews', { count: total })}
						</Typography>
						<Button variant='contained' onClick={onOpenReview} sx={{ mt: 1 }}>
							{t('feedback.button.submit_review')}
						</Button>
					</Stack>
				</Grid>
				<Grid size={{ xs: 12, md: 8 }}>
					<Stack spacing={1.2} py={2}>
						{[5, 4, 3, 2, 1].map((star) => (
							<Stack direction='row' alignItems='center' spacing={1} key={star}>
								<Typography width={28}>{star}</Typography>
								<Box width={90} display='flex' alignItems='center'>
									<Rating size='small' value={star} readOnly />
								</Box>
								<Box flex={1}>
									<LinearProgress
										variant='determinate'
										value={percent(dist[star] || 0)}
										sx={{ height: 8, borderRadius: 6 }}
									/>
								</Box>
								<Typography width={39} textAlign='right' variant='body2' color='text.secondary'>
									{percent(dist[star] || 0)}%
								</Typography>
							</Stack>
						))}
					</Stack>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default FeedbackSummarySection

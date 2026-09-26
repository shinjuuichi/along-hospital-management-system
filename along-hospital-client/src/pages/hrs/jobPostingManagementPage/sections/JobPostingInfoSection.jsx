import { ArrowBack } from '@mui/icons-material'
import { Box, Button, Card, Chip, Grid, Stack, Typography } from '@mui/material'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { processHtmlImages } from '@/utils/commons'
import { sanitizeHtml } from '@/utils/sanitizeHtml'
import { HTML_CONTENT_SX } from '@/configs/htmlContentStylesConfig'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const JobPostingInfoSection = ({
	t,
	selectedPosting,
	stats,
	onBack = () => {},
}) => {
	return (
		<>
			<Stack direction='row' alignItems='center' justifyContent='space-between' spacing={2}>
				<Stack direction='row' spacing={1} alignItems='center'>
					<Button startIcon={<ArrowBack />} onClick={onBack}>
						{t('button.back')}
					</Button>
				</Stack>
			</Stack>

			<Stack
				direction={{ xs: 'column', md: 'row' }}
				spacing={2}
				alignItems={{ xs: 'flex-start', md: 'center' }}
			>
				<Typography variant='h6'>{renderEmptyFallback(selectedPosting?.title)}</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('job_posting.field.role')}: {renderEmptyFallback(selectedPosting?.role)}
				</Typography>
			</Stack>

			<Card variant='outlined' sx={{ p: 2 }}>
				<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ mb: 2 }}>
					<Typography variant='subtitle1' sx={{ fontWeight: 700 }}>
						{t('job_posting.title.posting_information')}
					</Typography>
					<Stack direction='row' spacing={1}>
						<Chip
							size='small'
							label={`${t('job_posting.label.total_applications')}: ${stats.total}`}
						/>
					</Stack>
				</Stack>

				<Grid container spacing={2}>
					<Grid size={{ xs: 12, md: 4 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.employment_type')}
						</Typography>
						<Typography variant='body1'>{renderEmptyFallback(selectedPosting?.employmentType)}</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 4 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.salary_min')}
						</Typography>
						<Typography variant='body1'>
							{renderEmptyFallback(
								selectedPosting?.salaryMin
									? formatCurrencyBasedOnCurrentLanguage(selectedPosting.salaryMin)
									: null
							)}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 4 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.close_date')}
						</Typography>
						<Typography variant='body1'>
							{renderEmptyFallback(
								selectedPosting?.closeDate
									? formatDateBasedOnCurrentLanguage(selectedPosting.closeDate)
									: null
							)}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 4 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.status')}
						</Typography>
						<Typography variant='body1'>{renderEmptyFallback(selectedPosting?.status)}</Typography>
					</Grid>
					<Grid size={{ xs: 12 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.description')}
						</Typography>
						{selectedPosting?.description ? (
							<Box
								sx={{ ...HTML_CONTENT_SX, mt: 0.5 }}
								dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(selectedPosting.description)) }}
							/>
						) : (
							<Typography variant='body1'>{renderEmptyFallback(null)}</Typography>
						)}
					</Grid>
					<Grid size={{ xs: 12 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.requirement')}
						</Typography>
						{selectedPosting?.requirement ? (
							<Box
								sx={{ ...HTML_CONTENT_SX, mt: 0.5 }}
								dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(selectedPosting.requirement)) }}
							/>
						) : (
							<Typography variant='body1'>{renderEmptyFallback(null)}</Typography>
						)}
					</Grid>
					<Grid size={{ xs: 12 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('job_posting.field.benefit')}
						</Typography>
						{selectedPosting?.benefit ? (
							<Box
								sx={{ ...HTML_CONTENT_SX, mt: 0.5 }}
								dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(selectedPosting.benefit)) }}
							/>
						) : (
							<Typography variant='body1'>{renderEmptyFallback(null)}</Typography>
						)}
					</Grid>
				</Grid>
			</Card>
		</>
	)
}

export default JobPostingInfoSection

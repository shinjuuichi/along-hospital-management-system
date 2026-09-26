import useTranslation from '@/hooks/useTranslation'
import { processHtmlImages } from '@/utils/commons'
import { sanitizeHtml } from '@/utils/sanitizeHtml'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { ArrowBack, BusinessCenter, CheckCircle, MonetizationOn } from '@mui/icons-material'
import { Box, Button, Chip, Divider, Paper, Stack, Typography } from '@mui/material'
import useEnum from '@/hooks/useEnum'
import { defaultJobPostingStatusStyle } from '@/configs/defaultStylesConfig'
import { HTML_CONTENT_GUEST_SX } from '@/configs/htmlContentStylesConfig'

const PAPER_STYLES = {
	borderRadius: 3,
	boxShadow: 6,
	overflow: 'hidden',
	width: '100%',
	maxWidth: '100%',
	boxSizing: 'border-box',
	p: { xs: 3, md: 5 },
}

const JobPostingContentSection = ({ job, onBack, onApply }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const employmentTypeLabel =
		getEnumLabelByValue(_enum.employmentTypeOptions, job.employmentType) || job.employmentType

	return (
		<Stack component={Paper} spacing={4} sx={PAPER_STYLES}>
			<Stack spacing={2}>
				<Stack direction='row' justifyContent='space-between' alignItems='flex-start' spacing={2}>
					<Typography
						variant='h3'
						component='h1'
						sx={{
							fontSize: { xs: '1.75rem', sm: '2.25rem', md: '2.5rem' },
							fontWeight: 700,
							lineHeight: 1.2,
							color: 'text.primary',
						}}
					>
						{job.title}
					</Typography>
					<Chip
						label={getEnumLabelByValue(_enum.jobPostingManagementStatusOptions, job.status) || job.status}
						color={defaultJobPostingStatusStyle(job.status)}
						size='medium'
					/>
				</Stack>

				<Stack
					direction='row'
					spacing={3}
					alignItems='center'
					flexWrap='wrap'
					sx={{ color: 'text.secondary' }}
				>
					<Stack direction='row' spacing={1} alignItems='center'>
						<BusinessCenter fontSize='small' />
						<Typography variant='body1' fontWeight={500}>
							{job.role}
						</Typography>
					</Stack>
					<Stack direction='row' spacing={1} alignItems='center'>
						<CheckCircle fontSize='small' />
						<Typography variant='body1'>{employmentTypeLabel}</Typography>
					</Stack>
					<Stack direction='row' spacing={1} alignItems='center'>
						<MonetizationOn fontSize='small' color='success' />
						<Typography variant='body1' color='success.main' fontWeight={600}>
							{job.salaryMin
								? formatCurrencyBasedOnCurrentLanguage(job.salaryMin)
								: t('job_posting.guest.text.negotiable')}
						</Typography>
					</Stack>
				</Stack>
			</Stack>

			<Divider />

			{job.description && (
				<Stack spacing={2}>
					<Typography variant='h5' fontWeight={600} color='primary.main'>
						{t('job_posting.guest.text.description')}
					</Typography>
					<Box
						sx={{ ...HTML_CONTENT_GUEST_SX, fontSize: { xs: '1rem', md: '1.0625rem' } }}
						dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(job.description)) }}
					/>
				</Stack>
			)}

			{job.requirement && (
				<Stack spacing={2}>
					<Typography variant='h5' fontWeight={600} color='primary.main'>
						{t('job_posting.guest.text.requirement')}
					</Typography>
					<Box
						sx={{ ...HTML_CONTENT_GUEST_SX, fontSize: { xs: '1rem', md: '1.0625rem' } }}
						dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(job.requirement)) }}
					/>
				</Stack>
			)}

			{job.benefit && (
				<Stack spacing={2}>
					<Typography variant='h5' fontWeight={600} color='primary.main'>
						{t('job_posting.guest.text.benefit')}
					</Typography>
					<Box
						sx={{ ...HTML_CONTENT_GUEST_SX, fontSize: { xs: '1rem', md: '1.0625rem' } }}
						dangerouslySetInnerHTML={{ __html: sanitizeHtml(processHtmlImages(job.benefit)) }}
					/>
				</Stack>
			)}

			<Stack direction='row' spacing={2} sx={{ mt: 4 }}>
				<Button
					variant='outlined'
					startIcon={<ArrowBack />}
					onClick={onBack}
					sx={{ px: 3, py: 1.5, borderRadius: 2 }}
				>
					{t('button.back')}
				</Button>
				<Button
					variant='contained'
					color='primary'
					onClick={onApply}
					sx={{ px: 4, py: 1.5, borderRadius: 2, fontWeight: 'bold' }}
				>
					{t('job_posting.guest.button.apply')}
				</Button>
			</Stack>
		</Stack>
	)
}

export default JobPostingContentSection

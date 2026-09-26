import EmptyPage from '@/components/placeholders/EmptyPage'
import { defaultLineClampStyle } from '@/configs/defaultStylesConfig'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { AccessTime, AttachMoney, BusinessCenter } from '@mui/icons-material'
import { Box, Button, Card, CardActionArea, CardContent, Chip, Grid, Skeleton, Stack, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { defaultJobPostingStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'

const JobPostingCard = ({ job }) => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const _enum = useEnum()

	const employmentTypeLabel =
		getEnumLabelByValue(_enum.employmentTypeOptions, job.employmentType) || job.employmentType

	const handleCardClick = () => {
		navigate(routeUrls.HOME.JOB_POSTING.DETAIL(job.id))
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
				<CardContent sx={{ flexGrow: 1, width: '100%', p: 2.5 }}>
					<Stack spacing={1.5}>
						<Stack direction='row' justifyContent='space-between' alignItems='flex-start' spacing={1}>
							<Typography
								variant='h6'
								component='h3'
								sx={{
									...defaultLineClampStyle(2),
									fontWeight: 600,
									fontSize: '1.125rem',
									lineHeight: 1.4,
								}}
							>
								{job.title}
							</Typography>
							<Chip
								label={getEnumLabelByValue(_enum.jobPostingManagementStatusOptions, job.status) || job.status}
								color={defaultJobPostingStatusStyle(job.status)}
								size='small'
							/>
						</Stack>

						<Stack spacing={1}>
							<Stack direction='row' spacing={1} alignItems='center'>
								<BusinessCenter fontSize='small' color='action' />
								<Typography variant='body2' color='text.secondary'>
									{job.role}
								</Typography>
							</Stack>
							<Stack direction='row' spacing={1} alignItems='center'>
								<AccessTime fontSize='small' color='action' />
								<Typography variant='body2' color='text.secondary'>
									{employmentTypeLabel}
								</Typography>
							</Stack>
							<Stack direction='row' spacing={1} alignItems='center'>
								<AttachMoney fontSize='small' color='success' />
								<Typography variant='body2' color='success.main' fontWeight={600}>
									{job.salaryMin
										? formatCurrencyBasedOnCurrentLanguage(job.salaryMin)
										: t('job_posting.guest.text.negotiable')}
								</Typography>
							</Stack>
						</Stack>
					</Stack>
				</CardContent>
			</CardActionArea>
			<Box sx={{ px: 2.5, pb: 2.5 }}>
				<Button
					variant='outlined'
					fullWidth
					onClick={handleCardClick}
					sx={{
						textTransform: 'none',
						fontWeight: 500,
					}}
				>
					{t('button.view_detail')}
				</Button>
			</Box>
		</Card>
	)
}

const JobPostingListSection = ({ jobPostings, loading }) => {
	const { t } = useTranslation()
	const safeJobPostings = Array.isArray(jobPostings) ? jobPostings : []

	if (loading) {
		return (
			<Grid container spacing={3}>
				{Array.from({ length: 6 }).map((_, index) => (
					<Grid size={{ xs: 12, sm: 6, md: 4 }} key={index}>
						<Card sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
							<CardContent sx={{ flexGrow: 1 }}>
								<Stack
									direction='row'
									justifyContent='space-between'
									alignItems='flex-start'
									spacing={1}
									sx={{ mb: 1.5 }}
								>
									<Skeleton variant='text' width={150} height={40} />
									<Skeleton variant='rounded' width={60} height={24} />
								</Stack>
								<Stack spacing={1}>
									<Skeleton variant='rectangular' height={20} width='80%' />
									<Skeleton variant='rectangular' height={20} width='60%' />
									<Skeleton variant='rectangular' height={20} width='70%' />
								</Stack>
							</CardContent>
							<Box sx={{ p: 2, pt: 0 }}>
								<Skeleton variant='rectangular' width='100%' height={36} />
							</Box>
						</Card>
					</Grid>
				))}
			</Grid>
		)
	}

	if (safeJobPostings.length === 0) {
		return (
			<EmptyPage
				title={t('text.placeholder.no_data')}
				showButton={false}
			/>
		)
	}

	return (
		<Grid container spacing={3}>
			{safeJobPostings.map((job, index) => (
				<Grid size={{ xs: 12, sm: 6, md: 4 }} key={job.id ?? index}>
					<JobPostingCard job={job} />
				</Grid>
			))}
		</Grid>
	)
}

export default JobPostingListSection

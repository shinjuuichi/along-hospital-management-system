import { GenericPagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Container, Stack, Typography, Box } from '@mui/material'
import { useEffect, useState } from 'react'
import JobPostingFilterBarSection from './sections/JobPostingFilterBarSection'
import JobPostingListSection from './sections/JobPostingListSection'

const DEFAULT_PAGE_SIZE = 9

export default function JobPostingPage() {
	const { t } = useTranslation()
	const [filters, setFilters] = useState({
		employmentType: '',
		role: '',
		status: '',
	})
	const [page, setPage] = useState(1)

	const getJobPostings = useFetch(ApiUrls.JOB_POSTING.INDEX, { ...filters, page, pageSize: DEFAULT_PAGE_SIZE }, [
		filters,
		page,
		DEFAULT_PAGE_SIZE,
	])

	useEffect(() => {
		setPage(1)
	}, [filters])

	return (
		<Box sx={{ bgcolor: 'background.default', py: { xs: 4, md: 6 }, minHeight: '80vh' }}>
			<Container maxWidth='lg'>
				<Stack spacing={4}>
					<Stack spacing={1} alignItems='center' textAlign='center'>
						<Typography variant='h3' fontWeight={700} color='primary.main'>
							{t('job_posting.guest.title.list')}
						</Typography>
					</Stack>

					<JobPostingFilterBarSection
						filters={filters}
						setFilters={setFilters}
						loading={getJobPostings.loading}
					/>

					<JobPostingListSection
						jobPostings={getJobPostings.data?.collection || []}
						loading={getJobPostings.loading}
					/>

					<Stack alignItems='center' sx={{ mt: 4 }}>
						<GenericPagination
							totalPage={getJobPostings.data?.totalPage || 1}
							page={page}
							setPage={setPage}
							loading={getJobPostings.loading}
						/>
					</Stack>
				</Stack>
			</Container>
		</Box>
	)
}

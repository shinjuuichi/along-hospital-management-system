import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import QualificationManagementTableSection from './sections/QualificationManagementTableSection'
import QualificationMangementFilterSection from './sections/QualificationMangementFilterSection'

const QualificationManagementPage = () => {
	const [filters, setFilters] = useState({
		name: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const { t } = useTranslation()

	const getQualifications = useFetch(
		ApiUrls.QUALIFICATION.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('qualification.title.qualification_management')}</Typography>
				<QualificationMangementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getQualifications.loading}
				/>
				<QualificationManagementTableSection
					qualifications={getQualifications.data?.collection}
					loading={getQualifications.loading}
					sort={sort}
					setSort={setSort}
					refetch={getQualifications.fetch}
				/>
				<GenericTablePagination
					totalPage={getQualifications.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getQualifications.loading}
				/>
			</Stack>
		</Paper>
	)
}

export default QualificationManagementPage

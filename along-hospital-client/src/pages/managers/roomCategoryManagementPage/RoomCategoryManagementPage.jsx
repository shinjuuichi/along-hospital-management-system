import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import RoomCategoryFilterSection from '@/pages/managers/roomCategoryManagementPage/sections/RoomCategoryFilterSection'
import RoomCategoryTableManagementSection from '@/pages/managers/roomCategoryManagementPage/sections/RoomCategoryTableManagementSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const RoomCategoryManagementPage = () => {
	const { t } = useTranslation()

	const [filters, setFilters] = useState({ name: '' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selectedIds, setSelectedIds] = useState([])

	const getAllCategories = useFetch(
		ApiUrls.ROOM_CATEGORY.MANAGEMENT.INDEX,
		{ ...filters, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[filters, page, pageSize, sort]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('room_category.title.management')}</Typography>

				<RoomCategoryFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getAllCategories.loading}
				/>

				<RoomCategoryTableManagementSection
					roomCategories={getAllCategories.data?.collection || []}
					loading={getAllCategories.loading}
					refetch={getAllCategories.fetch}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					sort={sort}
					setSort={setSort}
				/>

				<Stack justifyContent='center' px={2}>
					<GenericTablePagination
						totalPage={getAllCategories.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getAllCategories.loading}
					/>
				</Stack>
			</Stack>
		</Paper>
	)
}

export default RoomCategoryManagementPage

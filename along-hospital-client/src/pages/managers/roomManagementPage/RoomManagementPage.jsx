import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import RoomFilterSection from '@/pages/managers/roomManagementPage/sections/RoomFilterSection'
import RoomManagementTableSection from '@/pages/managers/roomManagementPage/sections/RoomManagementTableSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const RoomManagementPage = () => {
	const { t } = useTranslation()

	const [filters, setFilters] = useState()
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selectedIds, setSelectedIds] = useState([])

	const getAllRooms = useFetch(
		ApiUrls.ROOM.MANAGEMENT.INDEX,
		{ ...filters, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[filters, page, pageSize, sort]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('room.title.management')}</Typography>

				<RoomFilterSection filters={filters} setFilters={setFilters} loading={getAllRooms.loading} />

				<RoomManagementTableSection
					rooms={getAllRooms.data?.collection || []}
					loading={getAllRooms.loading}
					refetch={getAllRooms.fetch}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					sort={sort}
					setSort={setSort}
				/>

				<Stack justifyContent='center' px={2}>
					<GenericTablePagination
						totalPage={getAllRooms.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getAllRooms.loading}
					/>
				</Stack>
			</Stack>
		</Paper>
	)
}

export default RoomManagementPage

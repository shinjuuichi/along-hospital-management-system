import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import BuildingFilterSection from '@/pages/managers/buildingManagementPage/sections/BuildingFilterSection'
import BuildingManagementTableSection from '@/pages/managers/buildingManagementPage/sections/BuildingManagementTableSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const BuildingManagementPage = () => {
	const { t } = useTranslation()
	const [filters, setFilters] = useState({ name: '', location: '' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selectedIds, setSelectedIds] = useState([])

	const normalizedFilters = Object.fromEntries(
		Object.entries(filters).filter(([, v]) => v !== '' && v != null)
	)

	const getAllBuildings = useFetch(
		ApiUrls.BUILDING.MANAGEMENT.INDEX,
		{ ...normalizedFilters, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[filters, page, pageSize, sort]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('building.title.management')}</Typography>

				<BuildingFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getAllBuildings.loading}
				/>

				<BuildingManagementTableSection
					buildings={getAllBuildings.data?.collection}
					loading={getAllBuildings.loading}
					refetch={getAllBuildings.fetch}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					sort={sort}
					setSort={setSort}
				/>

				<Stack justifyContent='center' px={2}>
					<GenericTablePagination
						totalPage={getAllBuildings.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getAllBuildings.loading}
					/>
				</Stack>
			</Stack>
		</Paper>
	)
}

export default BuildingManagementPage

import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import RegionalWageManagementFormSection from './sections/RegionalWageManagementFormSection'
import RegionalWageManagementTableSection from './sections/RegionalWageManagementTableSection'

const RegionalWageManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()

	const getRegionalWages = useFetch(
		ApiUrls.REGIONAL_WAGE.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const regionalWagePut = useAxiosSubmit({
		url: ApiUrls.REGIONAL_WAGE.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await regionalWagePut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getRegionalWages.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('regional_wage.title.management')}</Typography>
				<RegionalWageManagementTableSection
					regionalWages={getRegionalWages.data?.collection}
					loading={getRegionalWages.loading}
					sort={sort}
					setSort={setSort}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
				/>
				<GenericTablePagination
					totalPage={getRegionalWages.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getRegionalWages.loading}
				/>
			</Stack>
			<RegionalWageManagementFormSection
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default RegionalWageManagementPage

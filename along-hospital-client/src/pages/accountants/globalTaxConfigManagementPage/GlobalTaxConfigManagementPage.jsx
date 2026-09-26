import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import GlobalTaxConfigManagementFormSection from './sections/GlobalTaxConfigManagementFormSection'
import GlobalTaxConfigManagementTableSection from './sections/GlobalTaxConfigManagementTableSection'

const GlobalTaxConfigManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()

	const getGlobalTaxConfigs = useFetch(
		ApiUrls.GLOBAL_TAX_CONFIG.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const globalTaxConfigPut = useAxiosSubmit({
		url: ApiUrls.GLOBAL_TAX_CONFIG.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await globalTaxConfigPut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getGlobalTaxConfigs.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>
					{t('global_tax_config.title.global_tax_config_management')}
				</Typography>
				<GlobalTaxConfigManagementTableSection
					globalTaxConfigs={getGlobalTaxConfigs.data?.collection}
					loading={getGlobalTaxConfigs.loading}
					sort={sort}
					setSort={setSort}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
				/>
				<GenericTablePagination
					totalPage={getGlobalTaxConfigs.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getGlobalTaxConfigs.loading}
				/>
			</Stack>
			<GlobalTaxConfigManagementFormSection
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default GlobalTaxConfigManagementPage

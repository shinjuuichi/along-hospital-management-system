import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import TaxBracketManagementFormSection from './sections/TaxBracketManagementFormSection'
import TaxBracketManagementTableSection from './sections/TaxBracketManagementTableSection'

const TaxBracketManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()

	const getTaxBrackets = useFetch(
		ApiUrls.TAX_BRACKET.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const taxBracketPut = useAxiosSubmit({
		url: ApiUrls.TAX_BRACKET.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await taxBracketPut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getTaxBrackets.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('tax_bracket.title.tax_bracket_management')}</Typography>
				<TaxBracketManagementTableSection
					taxBrackets={getTaxBrackets.data?.collection}
					loading={getTaxBrackets.loading}
					sort={sort}
					setSort={setSort}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
				/>
				<GenericTablePagination
					totalPage={getTaxBrackets.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getTaxBrackets.loading}
				/>
			</Stack>
			<TaxBracketManagementFormSection
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default TaxBracketManagementPage

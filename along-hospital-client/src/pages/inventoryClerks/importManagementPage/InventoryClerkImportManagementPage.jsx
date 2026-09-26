import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import ImportTableSection from './sections/ImportTableSection'

const InventoryClerkImportManagementPage = () => {
	const { t } = useTranslation()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })

	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'asc'}`, [sort])

	const fetchParams = useMemo(
		() => ({
			page,
			pageSize,
			sort: sortParam,
		}),
		[page, pageSize, sortParam]
	)

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(ApiUrls.IMPORT.MANAGEMENT.INDEX, fetchParams, [page, pageSize, sortParam])

	const totalPage = data?.totalPage ?? 1

	const updateImport = useAxiosSubmit({
		url: ApiUrls.IMPORT.MANAGEMENT.DETAIL(''),
		method: 'PUT',
	})

	const handleUpdateSubmit = useCallback(
		async (id, values) => {
			return await updateImport.submit({
				overrideUrl: ApiUrls.IMPORT.MANAGEMENT.DETAIL(id),
				overrideData: { note: values.note || null },
			})
		},
		[updateImport]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('import_management.title.main')}</Typography>
				<ImportTableSection
					imports={data?.collection || data?.items || []}
					loading={loading}
					sort={sort}
					setSort={setSort}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					totalPage={totalPage}
					onUpdateSubmit={handleUpdateSubmit}
					onSuccess={refetch}
				/>
			</Stack>
		</Paper>
	)
}

export default InventoryClerkImportManagementPage

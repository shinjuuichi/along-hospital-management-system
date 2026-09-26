import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import AllowanceTypeManagementFormSection from './sections/AllowanceTypeManagementFormSection'
import AllowanceTypeManagementTableSection from './sections/AllowanceTypeManagementTableSection'

const AllowanceTypeManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getAllowanceTypes = useFetch(
		ApiUrls.ALLOWANCE_TYPE.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const allowanceTypePost = useAxiosSubmit({
		url: ApiUrls.ALLOWANCE_TYPE.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const allowanceTypePut = useAxiosSubmit({
		url: ApiUrls.ALLOWANCE_TYPE.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const allowanceTypeDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const respond = await allowanceTypePost.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getAllowanceTypes.fetch()
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await allowanceTypePut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getAllowanceTypes.fetch()
		}
	}

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('allowance_type.title.delete'),
			description: `${t('allowance_type.title.delete_confirm')} ${row.name}?`,
		})

		if (isConfirmed) {
			await allowanceTypeDelete.submit({
				overrideUrl: ApiUrls.ALLOWANCE_TYPE.MANAGEMENT.DETAIL(row.id),
			})
			getAllowanceTypes.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('allowance_type.title.allowance_type_management')}</Typography>
				<AllowanceTypeManagementTableSection
					allowanceTypes={getAllowanceTypes.data?.collection}
					loading={getAllowanceTypes.loading}
					sort={sort}
					setSort={setSort}
					setOpenCreate={setOpenCreate}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
					onDelete={handleDelete}
				/>
				<GenericTablePagination
					totalPage={getAllowanceTypes.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getAllowanceTypes.loading}
				/>
			</Stack>
			<AllowanceTypeManagementFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onCreate={handleCreate}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default AllowanceTypeManagementPage

import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import DeductionTypeManagementFormSection from './sections/DeductionTypeManagementFormSection'
import DeductionTypeManagementTableSection from './sections/DeductionTypeManagementTableSection'

const DeductionTypeManagementPage = () => {
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getDeductionTypes = useFetch(
		ApiUrls.DEDUCTION_TYPE.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, page, pageSize },
		[sort, page, pageSize]
	)

	const deductionTypePost = useAxiosSubmit({
		url: ApiUrls.DEDUCTION_TYPE.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const deductionTypePut = useAxiosSubmit({
		url: ApiUrls.DEDUCTION_TYPE.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const deductionTypeDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const respond = await deductionTypePost.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getDeductionTypes.fetch()
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await deductionTypePut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getDeductionTypes.fetch()
		}
	}

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('deduction_type.title.delete'),
			description: `${t('deduction_type.title.delete_confirm')} ${row.name}?`,
		})

		if (isConfirmed) {
			await deductionTypeDelete.submit({
				overrideUrl: ApiUrls.DEDUCTION_TYPE.MANAGEMENT.DETAIL(row.id),
			})
			getDeductionTypes.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('deduction_type.title.deduction_type_management')}</Typography>
				<DeductionTypeManagementTableSection
					deductionTypes={getDeductionTypes.data?.collection}
					loading={getDeductionTypes.loading}
					sort={sort}
					setSort={setSort}
					setOpenCreate={setOpenCreate}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
					onDelete={handleDelete}
				/>
				<GenericTablePagination
					totalPage={getDeductionTypes.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getDeductionTypes.loading}
				/>
			</Stack>
			<DeductionTypeManagementFormSection
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

export default DeductionTypeManagementPage

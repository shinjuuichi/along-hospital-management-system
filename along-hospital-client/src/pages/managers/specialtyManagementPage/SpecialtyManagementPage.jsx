import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import SpecialtyManagementFilterSection from './sections/SpecialtyManagementFilterSection'
import SpecialtyManagementFormSection from './sections/SpecialtyManagementFormSection'
import SpecialtyManagementTableSection from './sections/SpecialtyManagementTableSection'

const SpecialtyManagementPage = () => {
	const [filters, setFilters] = useState({
		name: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getSpecialties = useFetch(
		ApiUrls.SPECIALTY.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const specialtyPost = useAxiosSubmit({
		url: ApiUrls.SPECIALTY.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const specialtyPut = useAxiosSubmit({
		url: ApiUrls.SPECIALTY.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const specialtyDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const respond = await specialtyPost.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getSpecialties.fetch()
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const respond = await specialtyPut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			getSpecialties.fetch()
		}
	}

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('specialty.title.delete'),
			description: `${t('specialty.title.delete_confirm')} ${row.name}?`,
		})

		if (isConfirmed) {
			await specialtyDelete.submit({
				overrideUrl: ApiUrls.SPECIALTY.MANAGEMENT.DETAIL(row.id),
			})
			getSpecialties.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('specialty.title.specialty_management')}</Typography>
				<SpecialtyManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getSpecialties.loading}
				/>
				<SpecialtyManagementTableSection
					specialties={getSpecialties.data?.collection}
					loading={getSpecialties.loading}
					sort={sort}
					setSort={setSort}
					setOpenCreate={setOpenCreate}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
					onDelete={handleDelete}
				/>
				<GenericTablePagination
					totalPage={getSpecialties.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getSpecialties.loading}
				/>
			</Stack>
			<SpecialtyManagementFormSection
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

export default SpecialtyManagementPage

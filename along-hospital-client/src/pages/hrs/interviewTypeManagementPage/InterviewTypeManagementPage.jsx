import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

import InterviewTypeManagementFilterSection from './sections/InterviewTypeManagementFilterSection'
import InterviewTypeManagementFormSection from './sections/InterviewTypeManagementFormSection'
import InterviewTypeManagementTableSection from './sections/InterviewTypeManagementTableSection'

const InterviewTypeManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [filters, setFilters] = useState({
		name: '',
	})

	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])

	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'asc'}`, [sort])

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(
		ApiUrls.INTERVIEW_TYPE.MANAGEMENT.INDEX,
		{
			sort: sortParam,
			...filters,
			page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const createInterviewType = useAxiosSubmit({
		url: ApiUrls.INTERVIEW_TYPE.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: () => {
			refetch()
		},
	})

	const updateInterviewType = useAxiosSubmit({
		url: selectedItem?.id ? ApiUrls.INTERVIEW_TYPE.MANAGEMENT.DETAIL(selectedItem.id) : undefined,
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const deleteInterviewType = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => {
			refetch()
		},
	})

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('interview_type.dialog.confirm_delete_title'),
			description: t('interview_type.dialog.confirm_delete_description', {
				name: row?.name ?? '',
			}),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteInterviewType.submit({
			overrideUrl: ApiUrls.INTERVIEW_TYPE.MANAGEMENT.DETAIL(row.id),
		})
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirm = await confirm({
			title: t('interview_type.dialog.confirm_delete_title'),
			description: t('text.confirm_delete'),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteInterviewType.submit({
			overrideUrl: ApiUrls.INTERVIEW_TYPE.MANAGEMENT.DELETE_SELECTED,
			overrideParam: { ids: selectedIds },
		})

		setSelectedIds([])
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('interview_type.title.interview_type_management')}</Typography>

			<InterviewTypeManagementFilterSection
				filters={filters}
				setFilters={setFilters}
				loading={loading}
			/>

			<InterviewTypeManagementTableSection
				data={data?.collection || data?.items || data || []}
				loading={loading}
				totalPage={data?.totalPage}
				sort={sort}
				setSort={setSort}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				onCreateClick={() => setOpenCreateForm(true)}
				onUpdateClick={(row) => {
					setSelectedItem(row)
					setOpenUpdateForm(true)
				}}
				onDeleteClick={handleDelete}
				onDeleteSelectedClick={handleDeleteSelected}
			/>

			<InterviewTypeManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={async ({ values, closeDialog }) => {
					await createInterviewType.submit({ overrideData: values })
					closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onUpdateSubmit={async ({ values, closeDialog }) => {
					if (!selectedItem?.id) return

					await updateInterviewType.submit({
						overrideData: values,
					})
					closeDialog()
				}}
			/>
		</Paper>
	)
}

export default InterviewTypeManagementPage

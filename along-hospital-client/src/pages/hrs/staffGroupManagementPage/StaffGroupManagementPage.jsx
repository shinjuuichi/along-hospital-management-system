import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setStaffsStore } from '@/redux/reducers/managementReducer'
import { Paper, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import StaffGroupManagementFilterSection from './sections/StaffGroupManagementFilterSection'
import StaffGroupManagementFormSection from './sections/StaffGroupManagementFormSection'
import StaffGroupManagementTableSection from './sections/StaffGroupManagementTableSection'

const StaffGroupManagementPage = () => {
	const [filters, setFilters] = useState({
		name: '',
	})

	const [sort, setSort] = useState({ key: 'creationDate', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getStaffGroups = useFetch(
		ApiUrls.STAFF_GROUP.MANAGEMENT.INDEX,
		{
			sort: `${sort.key} ${sort.direction}`,
			...filters,
			page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)
	const getStaffsStore = useReduxStore({
		selector: (state) => state.management.staffs,
		setStore: setStaffsStore,
	})

	const createStaffGroup = useAxiosSubmit({
		url: ApiUrls.STAFF_GROUP.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: () => {
			getStaffGroups.fetch()
		},
	})

	const updateStaffGroup = useAxiosSubmit({
		url: ApiUrls.STAFF_GROUP.MANAGEMENT.DETAIL(selectedItem?.id),
		method: 'PUT',
		onSuccess: () => {
			getStaffGroups.fetch()
		},
	})

	const deleteStaffGroup = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => {
			getStaffGroups.fetch()
		},
	})

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('staff_group.title.delete_title'),
			description: t('staff_group.title.delete_description', { name: row.name }),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			deleteStaffGroup.submit({
				overrideUrl: ApiUrls.STAFF_GROUP.MANAGEMENT.DETAIL(row.id),
			})
		}
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('staff_group.title.staff_group_management')}</Typography>
			<StaffGroupManagementFilterSection
				filters={filters}
				setFilters={setFilters}
				loading={getStaffGroups.loading}
			/>
			<StaffGroupManagementTableSection
				data={getStaffGroups.data?.collection}
				loading={getStaffGroups.loading}
				totalPage={getStaffGroups.data?.totalPage}
				sort={sort}
				setSort={setSort}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				onCreateClick={() => setOpenCreateForm(true)}
				onUpdateClick={(row) => {
					setSelectedItem(row)
					setOpenUpdateForm(true)
				}}
				onDeleteClick={handleDelete}
			/>
			<StaffGroupManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={({ values, closeDialog }) => {
					createStaffGroup.submit({ overrideData: values })
					closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onUpdateSubmit={({ values, closeDialog }) => {
					updateStaffGroup.submit({ overrideData: values })
					closeDialog()
				}}
				staffs={getStaffsStore.data}
			/>
		</Paper>
	)
}

export default StaffGroupManagementPage

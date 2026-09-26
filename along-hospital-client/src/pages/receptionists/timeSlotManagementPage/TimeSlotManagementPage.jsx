import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import TimeSlotManagementFilterSection from './sections/TimeSlotManagementFilterSection'
import TimeSlotManagementFormSection from './sections/TimeSlotManagementFormSection'
import TimeSlotManagementTableSection from './sections/TimeSlotManagementTableSection'

const TimeSlotManagementPage = () => {
	const [filters, setFilters] = useState({
		startTime: '',
		endTime: '',
	})

	const [sort, setSort] = useState({ key: 'time', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)

	const { t } = useTranslation()
	const confirm = useConfirm()

	const getTimeSlots = useFetch(
		ApiUrls.TIME_SLOT.MANAGEMENT.INDEX,
		{
			sort: `${sort.key} ${sort.direction}`,
			...filters,
			page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)

	const createTimeSlot = useAxiosSubmit({
		url: ApiUrls.TIME_SLOT.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: () => {
			getTimeSlots.fetch()
		},
	})

	const updateTimeSlot = useAxiosSubmit({
		url: ApiUrls.TIME_SLOT.MANAGEMENT.DETAIL(selectedItem?.id),
		method: 'PUT',
		onSuccess: () => {
			getTimeSlots.fetch()
		},
	})

	const deleteTimeSlot = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => {
			getTimeSlots.fetch()
		},
	})

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('time_slot.dialog.delete_title'),
			description: t('time_slot.dialog.delete_description', { time: row.time }),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			deleteTimeSlot.submit({
				overrideUrl: ApiUrls.TIME_SLOT.MANAGEMENT.DETAIL(row.id),
			})
		}
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('time_slot.title.time_slot_management')}</Typography>
			<TimeSlotManagementFilterSection
				filters={filters}
				setFilters={setFilters}
				loading={getTimeSlots.loading}
			/>
			<TimeSlotManagementTableSection
				data={getTimeSlots.data?.collection}
				loading={getTimeSlots.loading}
				totalPage={getTimeSlots.data?.totalPage}
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
			<TimeSlotManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={({ values, closeDialog }) => {
					createTimeSlot.submit({ overrideData: values })
					closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onUpdateSubmit={({ values, closeDialog }) => {
					updateTimeSlot.submit({ overrideData: values })
					closeDialog()
				}}
			/>
		</Paper>
	)
}

export default TimeSlotManagementPage

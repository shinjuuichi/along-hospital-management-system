import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import WorkScheduleTemplateManagementFormDialog from '@/pages/hrs/workScheduleTemplateManagementPage/sections/WorkScheduleTemplateFormDialog'
import WorkScheduleTemplateManagementTableSection from '@/pages/hrs/workScheduleTemplateManagementPage/sections/WorkScheduleTemplateManagementTableSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const WorkScheduleTemplateManagementPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const confirm = useConfirm()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selectedTemplate, setSelectedTemplate] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const getTemplates = useFetch(
		ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.INDEX,
		{ page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[page, pageSize, sort]
	)

	const createTemplate = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const updateTemplate = useAxiosSubmit({
		method: 'PUT',
	})

	const deleteTemplate = useAxiosSubmit({
		method: 'DELETE',
	})

	const duplicateTemplate = useAxiosSubmit({
		method: 'POST',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const response = await createTemplate.submit({ overrideData: values })
		if (!response) return

		closeDialog()
		await getTemplates.fetch()
		const newId = response?.data?.data?.id
		if (newId) {
			navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_TEMPLATE_MANAGEMENT.DETAIL(newId)))
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		if (!selectedTemplate?.id) return

		const response = await updateTemplate.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DETAIL(selectedTemplate.id),
			overrideData: values,
		})
		if (!response) return

		closeDialog()
		setSelectedTemplate(null)
		await getTemplates.fetch()
	}

	const handleDelete = async (item) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('work_schedule_template.dialog.delete_template_title'),
			description: t('work_schedule_template.dialog.delete_template_description', { name: item.name }),
		})

		if (!isConfirmed) return

		const response = await deleteTemplate.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DETAIL(item.id),
		})
		if (response) {
			await getTemplates.fetch()
		}
	}

	const handleDuplicate = async (item) => {
		const response = await duplicateTemplate.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DUPLICATE(item.id),
		})
		if (response) {
			await getTemplates.fetch()
		}
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('work_schedule_template.dialog.delete_template_title'),
			description: t('text.confirm_delete'),
		})

		if (!isConfirmed) return

		const response = await deleteTemplate.submit({
			overrideUrl: ApiUrls.WORK_SCHEDULE_TEMPLATE.MANAGEMENT.DELETE_SELECTED,
			overrideParam: { ids: selectedIds },
		})
		if (response) {
			setSelectedIds([])
			await getTemplates.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('work_schedule_template.title.template_management')}</Typography>
				<WorkScheduleTemplateManagementTableSection
					templates={getTemplates.data?.collection || []}
					loading={getTemplates.loading}
					sort={sort}
					setSort={setSort}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					onCreate={() => setOpenCreateDialog(true)}
					onEdit={(item) => {
						setSelectedTemplate(item)
						setOpenUpdateDialog(true)
					}}
					onDelete={handleDelete}
					onDuplicate={handleDuplicate}
					onDeleteSelected={handleDeleteSelected}
				/>

				<Stack justifyContent='center' px={2}>
					<GenericTablePagination
						totalPage={getTemplates.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getTemplates.loading}
					/>
				</Stack>
			</Stack>

			<WorkScheduleTemplateManagementFormDialog
				title={t('work_schedule_template.dialog.create_template')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={handleCreate}
			/>

			<WorkScheduleTemplateManagementFormDialog
				title={t('work_schedule_template.dialog.edit_template')}
				open={openUpdateDialog}
				onClose={() => {
					setOpenUpdateDialog(false)
					setSelectedTemplate(null)
				}}
				initialValues={selectedTemplate || {}}
				submitLabel={t('button.save')}
				submitButtonColor='info'
				onSubmit={handleUpdate}
				showActiveField
			/>
		</Paper>
	)
}

export default WorkScheduleTemplateManagementPage

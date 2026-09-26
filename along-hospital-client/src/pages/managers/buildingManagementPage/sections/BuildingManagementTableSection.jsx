import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useTranslation from '@/hooks/useTranslation'
import BuildingFormDialog from '@/pages/managers/buildingManagementPage/sections/BuildingFormDialog'
import { Button, Stack } from '@mui/material'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const BuildingManagementTableSection = ({
	buildings,
	loading,
	refetch,
	selectedIds,
	setSelectedIds,
	sort,
	setSort,
}) => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const navigate = useNavigate()

	const [selectedBuilding, setSelectedBuilding] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const createBuilding = useAxiosSubmit({
		url: ApiUrls.BUILDING.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await refetch()
		},
	})

	const updateBuilding = useAxiosSubmit({
		url: ApiUrls.BUILDING.MANAGEMENT.DETAIL(selectedBuilding?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedBuilding(null)
			await refetch()
		},
	})

	const deleteBuilding = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedBuilding(null)
			await refetch()
		},
	})

	const deleteSelectedBuildings = useAxiosSubmit({
		url: ApiUrls.BUILDING.MANAGEMENT.DELETE_SELECTED,
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedIds([])
			await refetch()
		},
	})

	useEffect(() => {
		setSelectedIds((prev) => {
			const buildingIdSet = new Set((buildings || []).map((b) => b.id))
			return prev.filter((id) => buildingIdSet.has(id))
		})
	}, [buildings, setSelectedIds])

	const tableFields = [
		{ key: 'id', title: 'ID', width: 10, sortable: true },
		{ key: 'name', title: t('building.field.name'), width: 30, sortable: true },
		{ key: 'location', title: t('building.field.location'), width: 45, sortable: true },
		{
			key: '',
			title: '',
			width: 15,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.detail'),
							onClick: () => {
								navigate(routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.BUILDING_MANAGEMENT.DETAIL(row.id)))
							},
						},
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedBuilding(row)
								setOpenUpdateDialog(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									confirmText: t('button.delete'),
									confirmColor: 'error',
									title: t('building.dialog.confirm_delete_title'),
									description: t('building.dialog.confirm_delete_description', { name: row.name }),
								})
								if (isConfirmed) {
									setSelectedIds((prev) => prev.filter((id) => id !== row.id))
									await deleteBuilding.submit({
										overrideUrl: ApiUrls.BUILDING.MANAGEMENT.DETAIL(row.id),
									})
								}
							},
						},
					]}
				/>
			),
		},
	]

	return (
		<>
			<Stack spacing={2}>
				<Stack direction='row' alignItems='center' justifyContent='flex-end' spacing={1}>
					<ConfirmationButton
						confirmationTitle={t('building.dialog.confirm_delete_selected_title')}
						confirmationDescription={t('building.dialog.confirm_delete_selected_description', {
							count: selectedIds.length,
						})}
						confirmButtonText={t('button.delete')}
						confirmButtonColor='error'
						onConfirm={async () => {
							await deleteSelectedBuildings.submit({ overrideParam: { ids: selectedIds } })
						}}
						variant='contained'
						color='error'
						disabled={!selectedIds.length}
					>
						{t('button.delete_selected')}
					</ConfirmationButton>
					<Button variant='contained' color='primary' onClick={() => setOpenCreateDialog(true)}>
						{t('button.create')}
					</Button>
				</Stack>

				<GenericTable
					data={buildings}
					fields={tableFields}
					rowKey='id'
					canSelectRows={true}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					loading={loading}
					sort={sort}
					setSort={setSort}
				/>
			</Stack>

			<BuildingFormDialog
				title={t('building.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createBuilding.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<BuildingFormDialog
				title={t('building.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				initialValues={selectedBuilding || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateBuilding.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</>
	)
}

export default BuildingManagementTableSection

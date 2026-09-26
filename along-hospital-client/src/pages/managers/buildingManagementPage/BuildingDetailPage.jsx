import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import EmptyPage from '@/components/placeholders/EmptyPage'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import FloorFormDialog from '@/pages/managers/buildingManagementPage/sections/FloorFormDialog'
import { Button, Card, CardContent, Grid, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'

const BuildingDetailPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const { id } = useParams()

	const [selectedIds, setSelectedIds] = useState([])

	const [selectedFloor, setSelectedFloor] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const getBuildingDetail = useFetch(ApiUrls.BUILDING.MANAGEMENT.DETAIL(id))
	const building = getBuildingDetail?.data
	const floors = useMemo(() => building?.floors || [], [building?.floors])

	useEffect(() => {
		setSelectedIds((prev) => {
			const floorIdSet = new Set(floors.map((f) => f.id))
			return prev.filter((id) => floorIdSet.has(id))
		})
	}, [floors])

	const createFloor = useAxiosSubmit({
		url: ApiUrls.FLOOR.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await getBuildingDetail.fetch()
		},
	})

	const updateFloor = useAxiosSubmit({
		url: ApiUrls.FLOOR.MANAGEMENT.DETAIL(selectedFloor?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedFloor(null)
			await getBuildingDetail.fetch()
		},
	})

	const deleteFloor = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedFloor(null)
			await getBuildingDetail.fetch()
		},
	})

	const deleteSelectedFloors = useAxiosSubmit({
		url: ApiUrls.FLOOR.MANAGEMENT.DELETE_SELECTED,
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedIds([])
			await getBuildingDetail.fetch()
		},
	})

	if (!building && !getBuildingDetail.loading) {
		return <EmptyPage title={t('text.placeholder.no_data')} />
	}

	const floorTableFields = [
		{ key: 'floorNumber', title: t('floor.field.floor_number'), width: 90, sortable: true },
		{
			key: '',
			title: '',
			width: 10,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedFloor(row)
								setOpenUpdateDialog(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									confirmText: t('button.delete'),
									confirmColor: 'error',
									title: t('floor.dialog.confirm_delete_title'),
									description: t('floor.dialog.confirm_delete_description', {
										floorNumber: row.floorNumber,
									}),
								})
								if (isConfirmed) {
									setSelectedIds((prev) => prev.filter((id) => id !== row.id))
									await deleteFloor.submit({
										overrideUrl: ApiUrls.FLOOR.MANAGEMENT.DETAIL(row.id),
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
		<Stack spacing={3} p={2}>
			<Typography variant='h5'>{t('building.title.management')}</Typography>

			{building && (
				<Card>
					<CardContent>
						<Grid container spacing={2}>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={1}>
									<Typography variant='subtitle2' color='textSecondary'>
										{t('building.field.name')}
									</Typography>
									<Typography variant='body1'>{building.name}</Typography>
								</Stack>
							</Grid>
							<Grid size={{ xs: 12, md: 6 }}>
								<Stack spacing={1}>
									<Typography variant='subtitle2' color='textSecondary'>
										{t('building.field.location')}
									</Typography>
									<Typography variant='body1'>{building.location}</Typography>
								</Stack>
							</Grid>
						</Grid>
					</CardContent>
				</Card>
			)}

			<Paper sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Stack direction='row' alignItems='center' justifyContent='space-between'>
						<Typography variant='h6'>{t('floor.title.management')}</Typography>
						<Stack direction='row' spacing={1}>
							<ConfirmationButton
								confirmationTitle={t('floor.dialog.confirm_delete_selected_title')}
								confirmationDescription={t('floor.dialog.confirm_delete_selected_description', {
									count: selectedIds.length,
								})}
								confirmButtonText={t('button.delete')}
								confirmButtonColor='error'
								onConfirm={async () => {
									await deleteSelectedFloors.submit({ overrideParam: { ids: selectedIds } })
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
					</Stack>

					<GenericTable
						data={floors}
						fields={floorTableFields}
						rowKey='id'
						canSelectRows={true}
						selectedRows={selectedIds}
						setSelectedRows={setSelectedIds}
						loading={getBuildingDetail.loading}
					/>
				</Stack>
			</Paper>

			<FloorFormDialog
				title={t('floor.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				buildingId={id}
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createFloor.submit({
						overrideData: { ...values, buildingId: id },
					})
					if (response) closeDialog()
				}}
			/>

			<FloorFormDialog
				title={t('floor.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				initialValues={selectedFloor || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				buildingId={id}
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateFloor.submit({
						overrideData: { ...values, buildingId: id },
					})
					if (response) closeDialog()
				}}
			/>
		</Stack>
	)
}

export default BuildingDetailPage

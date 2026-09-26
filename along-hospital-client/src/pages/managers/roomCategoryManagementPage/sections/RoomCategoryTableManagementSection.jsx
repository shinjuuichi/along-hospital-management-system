import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import RoomCategoryFormDialog from '@/pages/managers/roomCategoryManagementPage/sections/RoomCategoryFormDialog'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useState } from 'react'

const RoomCategoryTableManagementSection = ({
	roomCategories,
	loading,
	refetch,
	selectedIds,
	setSelectedIds,
	sort,
	setSort,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const confirm = useConfirm()

	const [selectedCategory, setSelectedCategory] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const createCategory = useAxiosSubmit({
		url: ApiUrls.ROOM_CATEGORY.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await refetch()
		},
	})

	const updateCategory = useAxiosSubmit({
		url: ApiUrls.ROOM_CATEGORY.MANAGEMENT.DETAIL(selectedCategory?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedCategory(null)
			await refetch()
		},
	})

	const deleteCategory = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedCategory(null)
			await refetch()
		},
	})

	const deleteSelectedCategories = useAxiosSubmit({
		url: ApiUrls.ROOM_CATEGORY.MANAGEMENT.DELETE_SELECTED,
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedIds([])
			await refetch()
		},
	})

	const tableFields = [
		{ key: 'id', title: 'ID', width: 10, sortable: true },
		{ key: 'name', title: t('room_category.field.name'), width: 25, sortable: true },
		{ key: 'description', title: t('room_category.field.description'), width: 30, sortable: true },
		{
			key: 'roles',
			title: t('room_category.field.roles'),
			width: 20,
			render: (roles) =>
				roles && roles.length > 0 ? (
					<Stack direction='row' spacing={0.5} flexWrap='wrap' useFlexGap>
						{roles.map((role) => (
							<Chip key={role} label={getEnumLabelByValue(_enum.roleOptions, role)} size='small' />
						))}
					</Stack>
				) : null,
		},
		{
			key: '',
			title: t(''),
			width: 15,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedCategory(row)
								setOpenUpdateDialog(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									confirmText: t('button.delete'),
									confirmColor: 'error',
									title: t('room_category.dialog.confirm_delete_title'),
									description: t('room_category.dialog.confirm_delete_description', { name: row.name }),
								})
								if (isConfirmed) {
									await deleteCategory.submit({
										overrideUrl: ApiUrls.ROOM_CATEGORY.MANAGEMENT.DETAIL(row.id),
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
						confirmationTitle={t('room_category.dialog.confirm_delete_selected_title')}
						confirmationDescription={t('room_category.dialog.confirm_delete_selected_description', {
							count: selectedIds.length,
						})}
						confirmButtonText={t('button.delete')}
						confirmButtonColor='error'
						onConfirm={async () => {
							await deleteSelectedCategories.submit({
								overrideParam: { ids: selectedIds },
							})
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
					data={roomCategories}
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

			<RoomCategoryFormDialog
				title={t('room_category.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createCategory.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<RoomCategoryFormDialog
				title={t('room_category.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				initialValues={selectedCategory || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateCategory.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</>
	)
}

export default RoomCategoryTableManagementSection

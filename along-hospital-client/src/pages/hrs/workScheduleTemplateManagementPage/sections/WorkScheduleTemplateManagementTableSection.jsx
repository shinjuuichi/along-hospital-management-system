import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { Button, Stack } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const WorkScheduleTemplateManagementTableSection = ({
	templates,
	loading,
	sort,
	setSort,
	selectedRows = [],
	setSelectedRows = () => {},
	onCreate,
	onEdit,
	onDelete,
	onDuplicate,
	onDeleteSelected = () => {},
}) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const tableFields = [
		{ key: 'id', title: t('work_schedule_template.table.id'), width: 10, sortable: true },
		{ key: 'name', title: t('work_schedule_template.field.name'), width: 20, sortable: true },
		{
			key: 'description',
			title: t('work_schedule_template.field.description'),
			width: 40,
		},
		{
			key: 'isActive',
			title: t('work_schedule_template.table.active'),
			width: 15,
			sortable: true,
			render: (value) =>
				t(value ? 'work_schedule_template.status.yes' : 'work_schedule_template.status.no'),
		},
		{
			key: '',
			title: t('work_schedule_template.table.actions'),
			width: 15,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('work_schedule_template.button.open'),
							onClick: () =>
								navigate(
									routeUrls.BASE_ROUTE.HR(routeUrls.HR.WORK_SCHEDULE_TEMPLATE_MANAGEMENT.DETAIL(row.id))
								),
						},
						{
							title: t('button.edit'),
							onClick: () => onEdit?.(row),
						},
						{
							title: t('work_schedule_template.button.duplicate'),
							onClick: () => onDuplicate?.(row),
						},
						{
							title: t('button.delete'),
							onClick: () => onDelete?.(row),
						},
					]}
				/>
			),
		},
	]

	return (
		<Stack spacing={2}>
			<Stack direction='row' alignItems='center' justifyContent='flex-end' spacing={1}>
				<Button variant='contained' color='primary' onClick={onCreate}>
					{t('work_schedule_template.button.new_template')}
				</Button>
				<ConfirmationButton
					confirmButtonColor='error'
					confirmButtonText={t('button.delete')}
					confirmationTitle={t('work_schedule_template.dialog.delete_template_title')}
					confirmationDescription={t('text.confirm_delete')}
					onConfirm={onDeleteSelected}
					color='error'
					variant='outlined'
					disabled={!selectedRows.length}
				>
					{t('button.delete_selected')}
				</ConfirmationButton>
			</Stack>

			<GenericTable
				data={templates}
				fields={tableFields}
				rowKey='id'
				loading={loading}
				sort={sort}
				setSort={setSort}
				canSelectRows
				selectedRows={selectedRows}
				setSelectedRows={setSelectedRows}
			/>
		</Stack>
	)
}

export default WorkScheduleTemplateManagementTableSection

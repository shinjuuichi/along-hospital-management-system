import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { defaultBooleanStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'

const ShiftManagementTableSection = ({
	shifts,
	loading,
	sort,
	setSort,
	selectedRows = [],
	setSelectedRows = () => {},
	onCreate,
	onEdit,
	onDelete,
	onDeleteSelected = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const tableFields = [
		{ key: 'id', title: t('shift.field.id'), width: 10, sortable: true },
		{ key: 'name', title: t('shift.field.name'), width: 30, sortable: true },
		{
			key: 'startTime',
			title: t('shift.field.start'),
			width: 15,
			sortable: true,
			render: (value) => formatTimeToHourMinute(value),
		},
		{
			key: 'endTime',
			title: t('shift.field.end'),
			width: 15,
			sortable: true,
			render: (value) => formatTimeToHourMinute(value),
		},
		{
			key: 'isOvertime',
			title: t('shift.field.is_overtime'),
			width: 20,
			sortable: true,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.booleanOptions, value)}
					color={defaultBooleanStyle(value)}
					size='small'
					variant='outlined'
				/>
			),
		},
		{
			key: '',
			title: t('shift.table.actions'),
			width: 10,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.edit'),
							onClick: () => onEdit?.(row),
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
					{t('shift.button.new_shift')}
				</Button>
				<ConfirmationButton
					confirmButtonColor='error'
					confirmButtonText={t('button.delete')}
					confirmationTitle={t('shift.dialog.delete_title')}
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
				data={shifts}
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

export default ShiftManagementTableSection

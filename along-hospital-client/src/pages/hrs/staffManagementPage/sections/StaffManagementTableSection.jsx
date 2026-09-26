import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { defaultStaffStatusStyle } from '@/configs/defaultStylesConfig'
import useTranslation from '@/hooks/useTranslation'
import useEnum from '@/hooks/useEnum'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useMemo } from 'react'

const StaffManagementTableSection = ({
	staffs,
	loading,
	sort,
	setSort,
	selectedIds = [],
	setSelectedIds = () => {},
	actionLoading = false,
	onActivate = () => {},
	onOnLeave = () => {},
	onTerminate = () => {},
	onCreate = () => {},
	onEdit = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const hasSelectedStaffs = selectedIds.length > 0

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('staff.field.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'name',
				title: t('staff.field.name'),
				width: 15,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'email',
				title: t('staff.field.email'),
				width: 14,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'phone',
				title: t('staff.field.phone'),
				width: 12,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'specialtyName',
				title: t('staff.field.specialty'),
				width: 12,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'qualificationName',
				title: t('staff.field.qualification'),
				width: 11,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'status',
				title: t('staff.field.status'),
				width: 10,
				sortable: true,
				render: (value) => (
					<Chip
						label={renderEmptyFallback(getEnumLabelByValue(_enum.staffStatusOptions, value) || value)}
						size='small'
						color={defaultStaffStatusStyle(value)}
					/>
				),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.edit'),
								onClick: () => onEdit(row),
							},
						]}
					/>
				),
			},
		],
		[t, onEdit, _enum.staffStatusOptions]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<ConfirmationButton
					confirmationTitle={t('staff.confirm.activate_selected_title')}
					confirmationDescription={t('staff.confirm.activate_selected_description', {
						number: selectedIds.length,
					})}
					confirmButtonColor='success'
					confirmButtonText={t('enum.staff_status.active')}
					color='success'
					disabled={!hasSelectedStaffs || actionLoading}
					onConfirm={onActivate}
				>
					{t('staff.button.activate_selected')}
				</ConfirmationButton>
				<ConfirmationButton
					confirmationTitle={t('staff.confirm.on_leave_selected_title')}
					confirmationDescription={t('staff.confirm.on_leave_selected_description', {
						number: selectedIds.length,
					})}
					confirmButtonColor='warning'
					confirmButtonText={t('enum.staff_status.on_leave')}
					color='warning'
					disabled={!hasSelectedStaffs || actionLoading}
					onConfirm={onOnLeave}
				>
					{t('staff.button.on_leave_selected')}
				</ConfirmationButton>
				<ConfirmationButton
					confirmationTitle={t('staff.confirm.terminate_selected_title')}
					confirmationDescription={t('staff.confirm.terminate_selected_description', {
						number: selectedIds.length,
					})}
					confirmButtonColor='error'
					confirmButtonText={t('enum.staff_status.terminated')}
					color='error'
					disabled={!hasSelectedStaffs || actionLoading}
					onConfirm={onTerminate}
				>
					{t('staff.button.terminate_selected')}
				</ConfirmationButton>
				<Button variant='contained' color='primary' onClick={onCreate}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={staffs}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				canSelectRows
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
		</>
	)
}

export default StaffManagementTableSection

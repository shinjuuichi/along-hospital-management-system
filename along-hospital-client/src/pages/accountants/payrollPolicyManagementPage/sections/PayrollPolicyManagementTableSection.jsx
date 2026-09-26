import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultPayrollPolicyStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useMemo, useState } from 'react'
import PayrollPolicyStaffDialogSection from './PayrollPolicyStaffDialogSection'

const PayrollPolicyManagementTableSection = ({
	payrollPolicies,
	loading,
	sort,
	setSort,
	setOpenCreate,
	setOpenUpdate,
	setSelectedRow,
	staffs,
}) => {
	const [selectedIds, setSelectedIds] = useState([])
	const [openStaffDialog, setOpenStaffDialog] = useState(false)
	const [dialogStaffs, setDialogStaffs] = useState([])
	const { t } = useTranslation()
	const _enum = useEnum()

	const staffMap = useMemo(() => {
		const map = new Map()
		;(staffs || []).forEach((staff) => {
			map.set(staff.id, staff)
		})
		return map
	}, [staffs])

	const fields = useMemo(
		() => [
			{ key: 'name', title: t('payroll_policy.field.name'), width: 15, sortable: true },
			{
				key: 'startDate',
				title: t('payroll_policy.field.start_date'),
				width: 10,
				sortable: true,
				render: (value) => formatDateBasedOnCurrentLanguage(value),
			},
			{
				key: 'endDate',
				title: t('payroll_policy.field.end_date'),
				width: 10,
				sortable: true,
				render: (value) => formatDateBasedOnCurrentLanguage(value),
			},
			{
				key: 'payrollPolicyStatus',
				title: t('payroll_policy.field.payroll_policy_status'),
				width: 10,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.payrollPolicyStatusOptions, value)}
						size='small'
						color={defaultPayrollPolicyStatusStyle(value)}
					/>
				),
			},
			{
				key: 'allowanceTypeName',
				title: t('payroll_policy.field.allowance_type_id'),
				width: 10,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'deductionTypeName',
				title: t('payroll_policy.field.deduction_type_id'),
				width: 10,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'staffIds',
				title: t('payroll_policy.field.staff_id'),
				width: 10,
				sortable: false,
				render: (value) => {
					const count = Array.isArray(value) ? value.length : 0
					if (!count) return renderEmptyFallback(null)
					return (
						<Button
							size='small'
							variant='outlined'
							onClick={() => {
								const staffList = (value || []).map((id) => staffMap.get(id)).filter(Boolean)
								setDialogStaffs(staffList)
								setOpenStaffDialog(true)
							}}
						>
							{t('button.detail')} ({count})
						</Button>
					)
				},
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (_, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.edit'),
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
						]}
					/>
				),
			},
		],
		[t, _enum.payrollPolicyStatusOptions, setSelectedRow, setOpenUpdate, staffMap]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={payrollPolicies}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
			<PayrollPolicyStaffDialogSection
				open={openStaffDialog}
				onClose={() => setOpenStaffDialog(false)}
				staffs={dialogStaffs}
			/>
		</>
	)
}

export default PayrollPolicyManagementTableSection

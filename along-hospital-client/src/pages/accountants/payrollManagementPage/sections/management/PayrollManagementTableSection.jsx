import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultPayrollStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useMemo } from 'react'

const PayrollManagementTableSection = ({
	payrolls,
	loading,
	selectedIds,
	setSelectedIds,
	sort,
	setSort,
	onOpenDetail,
	onPending,
	onApprove,
	pendingLoading,
	approveLoading,
	isAccountant,
	isManager,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('payroll.field.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'staffName',
				title: t('payroll.field.staff_name'),
				width: 36,
				sortable: true,
				minWidthPx: 240,
			},
			{ key: 'month', title: t('payroll.field.month'), width: 12, sortable: true },
			{ key: 'year', title: t('payroll.field.year'), width: 12, sortable: true },
			{
				key: 'status',
				title: t('payroll.field.status'),
				width: 20,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.payrollStatusOptions, value)}
						size='small'
						color={defaultPayrollStatusStyle(value)}
					/>
				),
			},
			{
				key: '',
				title: t('payroll.field.actions'),
				width: 10,
				sortable: false,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.detail'),
								onClick: () => onOpenDetail?.(row),
							},
						]}
					/>
				),
			},
		],
		[t, _enum.payrollStatusOptions, onOpenDetail]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				{isAccountant && (
					<Button
						variant='outlined'
						onClick={onPending}
						disabled={!selectedIds.length || pendingLoading}
					>
						{t('payroll.button.pending')}
					</Button>
				)}
				{isManager && (
					<Button
						variant='contained'
						onClick={onApprove}
						disabled={!selectedIds.length || approveLoading}
					>
						{t('payroll.button.approve')}
					</Button>
				)}
			</Stack>
			<GenericTable
				data={payrolls}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				canSelectRows={true}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
		</>
	)
}

export default PayrollManagementTableSection

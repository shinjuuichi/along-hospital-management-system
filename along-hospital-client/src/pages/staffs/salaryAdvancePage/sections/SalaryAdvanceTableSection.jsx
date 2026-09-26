import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultSalaryAdvanceStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Stack, Tooltip, Typography } from '@mui/material'
import { useMemo } from 'react'

const SalaryAdvanceTableSection = ({
	salaryAdvances,
	loading,
	sort,
	setSort,
	onCreate,
	onEdit,
	onCancel,
	actionLoading,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('salary_advance.field.id'), width: 8, sortable: true, fixedColumn: true },
			{
				key: 'amount',
				title: t('salary_advance.field.amount'),
				width: 15,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'reason',
				title: t('salary_advance.field.reason'),
				width: 22,
				sortable: true,
				render: (value) => {
					return (
						<Tooltip title={value} arrow placement='top' disableInteractive>
							<Typography
								variant='body2'
								noWrap
								sx={{ width: '100%', overflow: 'hidden', textOverflow: 'ellipsis' }}
							>
								{renderEmptyFallback(value)}
							</Typography>
						</Tooltip>
					)
				},
			},
			{
				key: 'status',
				title: t('salary_advance.field.status'),
				width: 14,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.salaryAdvanceStatusOptions, value)}
						color={defaultSalaryAdvanceStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'creationDate',
				title: t('salary_advance.field.creation_date'),
				width: 18,
				sortable: true,
				render: (value) => formatDatetimeStringBasedOnCurrentLanguage(value),
			},
			{
				key: 'payrollId',
				title: t('salary_advance.field.payroll_id'),
				width: 15,
				sortable: true,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: '',
				title: t('salary_advance.field.actions'),
				width: 8,
				render: (_, row) => (
					<ActionMenu
						actions={[
							String(row?.status || '').toLowerCase() === 'pending'
								? {
										title: t('button.edit'),
										onClick: () => onEdit(row),
										disabled: actionLoading,
									}
								: null,
							String(row?.status || '').toLowerCase() === 'pending'
								? {
										title: t('salary_advance.button.cancel_request'),
										onClick: () => onCancel(row),
										disabled: actionLoading,
									}
								: null,
						].filter(Boolean)}
					/>
				),
			},
		],
		[_enum.salaryAdvanceStatusOptions, actionLoading, onCancel, onEdit, t]
	)

	return (
		<Stack spacing={2}>
			<Stack direction='row' justifyContent='flex-end'>
				<Button variant='contained' color='primary' onClick={onCreate} disabled={actionLoading}>
					{t('salary_advance.button.create_request')}
				</Button>
			</Stack>
			<GenericTable
				data={salaryAdvances}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				loading={loading}
				stickyHeader
			/>
		</Stack>
	)
}

export default SalaryAdvanceTableSection

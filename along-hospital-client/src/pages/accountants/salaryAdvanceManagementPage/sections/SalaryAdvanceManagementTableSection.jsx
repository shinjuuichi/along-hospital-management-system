import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { defaultSalaryAdvanceStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Chip, Stack, Tooltip, Typography } from '@mui/material'
import { useMemo } from 'react'

const SalaryAdvanceManagementTableSection = ({
	salaryAdvances,
	loading,
	selectedIds,
	setSelectedIds,
	sort,
	setSort,
	onApproveSelected,
	onRejectSelected,
	actionLoading,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('salary_advance.field.id'), width: 8, sortable: true },
			{
				key: 'amount',
				title: t('salary_advance.field.amount'),
				width: 16,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'reason',
				title: t('salary_advance.field.reason'),
				width: 25,
				sortable: true,
				render: (value) => {
					const text = renderEmptyFallback(value)
					return (
						<Tooltip title={text} arrow placement='top' disableInteractive>
							<Typography
								variant='body2'
								noWrap
								sx={{ width: '100%', overflow: 'hidden', textOverflow: 'ellipsis' }}
							>
								{text}
							</Typography>
						</Tooltip>
					)
				},
			},
			{
				key: 'status',
				title: t('salary_advance.field.status'),
				width: 16,
				sortable: true,
				render: (value) => (
					<Chip
						label={renderEmptyFallback(
							getEnumLabelByValue(_enum.salaryAdvanceStatusOptions, value) || value
						)}
						color={defaultSalaryAdvanceStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'creationDate',
				title: t('salary_advance.field.creation_date'),
				width: 20,
				sortable: true,
				render: (value) =>
					renderEmptyFallback(value ? formatDatetimeStringBasedOnCurrentLanguage(value) : null),
			},
		],
		[_enum.salaryAdvanceStatusOptions, t]
	)

	return (
		<Stack spacing={2}>
			<Stack direction='row' justifyContent='flex-end' spacing={1}>
				<ConfirmationButton
					confirmationTitle={t('salary_advance.confirm.approve_title')}
					confirmationDescription={t('salary_advance.confirm.approve_description')}
					confirmButtonColor='success'
					confirmButtonText={t('salary_advance.button.approve')}
					color='success'
					disabled={!selectedIds.length || actionLoading}
					onConfirm={onApproveSelected}
				>
					{t('salary_advance.button.approve_selected')}
				</ConfirmationButton>
				<ConfirmationButton
					confirmationTitle={t('salary_advance.confirm.reject_title')}
					confirmationDescription={t('salary_advance.confirm.reject_description')}
					confirmButtonColor='error'
					confirmButtonText={t('salary_advance.button.reject')}
					color='error'
					disabled={!selectedIds.length || actionLoading}
					onConfirm={onRejectSelected}
				>
					{t('salary_advance.button.reject_selected')}
				</ConfirmationButton>
			</Stack>
			<GenericTable
				data={salaryAdvances}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				canSelectRows={true}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
		</Stack>
	)
}

export default SalaryAdvanceManagementTableSection

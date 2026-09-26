import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const RegionalWageManagementTableSection = ({
	regionalWages,
	loading,
	sort,
	setSort,
	setOpenUpdate,
	setSelectedRow,
}) => {
	const [selectedIds, setSelectedIds] = useState([])
	const { t } = useTranslation()

	const fields = useMemo(
		() => [
			{ key: 'code', title: t('regional_wage.field.code'), width: 15, sortable: true },
			{
				key: 'monthlyWage',
				title: t('regional_wage.field.monthly_wage'),
				width: 20,
				sortable: true,
				render: (value) => (
					<Typography variant='body2'>{formatCurrencyBasedOnCurrentLanguage(value)}</Typography>
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
		[t, setSelectedRow, setOpenUpdate]
	)

	return (
		<GenericTable
			data={regionalWages}
			fields={fields}
			sort={sort}
			setSort={setSort}
			rowKey='id'
			selectedRows={selectedIds}
			setSelectedRows={setSelectedIds}
			loading={loading}
		/>
	)
}

export default RegionalWageManagementTableSection

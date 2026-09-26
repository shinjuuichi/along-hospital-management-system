import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
} from '@/utils/formatNumberUtil'
import { useMemo, useState } from 'react'

const TaxBracketManagementTableSection = ({
	taxBrackets,
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
			{ key: 'id', title: t('tax_bracket.field.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'fromAmount',
				title: t('tax_bracket.field.from_amount'),
				width: 20,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'taxRate',
				title: t('tax_bracket.field.tax_rate'),
				width: 20,
				sortable: true,
				render: (value) => formatNumberToPercent(value),
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
			data={taxBrackets}
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

export default TaxBracketManagementTableSection

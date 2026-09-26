import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import {
	formatCurrencyBasedOnCurrentLanguage,
	formatNumberToPercent,
} from '@/utils/formatNumberUtil'
import { useMemo, useState } from 'react'

const GlobalTaxConfigManagementTableSection = ({
	globalTaxConfigs,
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
			{
				key: 'id',
				title: t('global_tax_config.field.id'),
				width: 10,
				sortable: true,
			},
			{
				key: 'personalDeductionAmount',
				title: t('global_tax_config.field.personal_deduction_amount'),
				width: 15,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'dependentDeductionAmount',
				title: t('global_tax_config.field.dependent_deduction_amount'),
				width: 15,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'referenceBaseSalary',
				title: t('global_tax_config.field.reference_base_salary'),
				width: 15,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'socialInsuranceRate',
				title: t('global_tax_config.field.social_insurance_rate'),
				width: 12,
				sortable: true,
				render: (value) => formatNumberToPercent(value),
			},
			{
				key: 'healthInsuranceRate',
				title: t('global_tax_config.field.health_insurance_rate'),
				width: 12,
				sortable: true,
				render: (value) => formatNumberToPercent(value),
			},
			{
				key: 'unemploymentInsuranceRate',
				title: t('global_tax_config.field.unemployment_insurance_rate'),
				width: 15,
				sortable: true,
				render: (value) => formatNumberToPercent(value),
			},
			{
				key: '',
				title: '',
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
			data={globalTaxConfigs}
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

export default GlobalTaxConfigManagementTableSection

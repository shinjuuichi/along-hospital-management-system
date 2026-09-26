import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'

const MedicineSkuTableSection = ({ skus = [], loading = false, t, sort, setSort, onOpenUpdate = () => {} }) => {
	const tableFields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{ key: 'skuCode', title: t('medicine_sku.field.sku_code'), width: 15, sortable: true },
		{
			key: 'price',
			title: t('medicine_sku.field.price'),
			width: 10,
			sortable: true,
			isNumeric: true,
		},
		{ key: 'medicine.name', title: t('medicine_sku.field.medicine'), width: 20, sortable: true },
		{
			key: 'name',
			title: t('medicine_sku.field.name'),
		},
		{
			key: 'inventory.quantity',
			title: t('medicine.field.quantity'),
			width: 10,
			render: (_, row) => row.inventory?.quantity ?? 0,
		},
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			render: (_, row) =>
				row.isActive === false ? t('medicine_sku.text.inactive') : t('medicine_sku.text.active'),
		},
		{
			key: 'actions',
			title: t('medicine_sku.field.actions'),
			width: 14,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => onOpenUpdate(row),
						},
					]}
				/>
			),
		},
	]

	return <GenericTable data={skus} fields={tableFields} rowKey='id' loading={loading} sort={sort} setSort={setSort} />
}

export default MedicineSkuTableSection

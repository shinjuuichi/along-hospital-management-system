import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { useMemo } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const OrderManagementTableSection = ({
	data,
	sort,
	setSort,
	page,
	setPage,
	pageSize,
	setPageSize,
	totalPage,
	loading,
	onViewDetail,
}) => {
	const { t } = useTranslation()

	const fields = useMemo(
		() => [
			{
				key: 'id',
				title: t('order_management.field.id'),
				width: 10,
				sortable: true,
				fixedColumn: true,
			},
			{
				key: 'orderDate',
				title: t('order_management.field.order_date'),
				width: 18,
				sortable: true,
				render: (v) => formatDatetimeStringBasedOnCurrentLanguage(v),
			},
			{
				key: 'deliveryDate',
				title: t('order_management.field.delivery_date'),
				width: 18,
				sortable: true,
				render: (v) => renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(v)),
			},
			{
				key: 'orderStatus',
				title: t('order_management.field.status'),
				width: 14,
				sortable: true,
				render: (v) => v || t('order_management.text.unknown'),
			},
			{
				key: 'finalPrice',
				title: t('order_management.field.final_price'),
				width: 15,
				isNumeric: true,
				sortable: true,
				render: (v) => formatCurrencyBasedOnCurrentLanguage(v),
			},
			{
				key: 'isPickupAtStore',
				title: t('order_management.field.is_pickup_at_store'),
				width: 10,
				render: (v) =>
					v ? t('order_management.text.pickup_at_store') : t('order_management.text.delivery'),
			},
			{
				key: '',
				title: '',
				width: 6,
				render: (_, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.view_detail'),
								onClick: () => onViewDetail(row),
							},
						]}
					/>
				),
			},
		],
		[t, onViewDetail]
	)

	return (
		<>
			<GenericTable
				data={data}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				loading={loading}
			/>
			<GenericTablePagination
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				loading={loading}
			/>
		</>
	)
}

export default OrderManagementTableSection

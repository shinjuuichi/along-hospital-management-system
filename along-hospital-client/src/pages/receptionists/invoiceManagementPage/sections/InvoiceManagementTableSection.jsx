import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultInvoiceStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Chip } from '@mui/material'

const InvoiceManagementTableSection = ({
	invoices,
	loading,
	sort,
	setSort,
	onPaymentInvoiceClick = (invoiceId) => Promise.resolve(invoiceId),
	onCompleteInvoiceClick = (invoiceId) => Promise.resolve(invoiceId),
	onCancelInvoiceClick = (invoiceId) => Promise.resolve(invoiceId),
	onOpenDetail = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = [
		{ key: 'invoiceNumber', title: t('invoice.field.invoice_number'), width: 24, sortable: true },
		{
			key: 'invoiceStatus',
			title: t('invoice.field.invoice_status'),
			width: 16,
			sortable: false,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.invoiceStatusOptions, value)}
					color={defaultInvoiceStatusStyle(value)}
					size='small'
				/>
			),
		},
		{
			key: 'creationDate',
			title: t('invoice.field.creation_date'),
			width: 17,
			sortable: true,
			render: (value) => renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(value)),
		},
		{
			key: 'paymentDate',
			title: t('invoice.field.payment_date'),
			width: 17,
			sortable: true,
			render: (value) => renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(value)),
		},
		{
			key: 'totalAmount',
			title: t('invoice.field.total_amount'),
			width: 18,
			sortable: true,
			render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
		},
		{
			key: 'action',
			title: '',
			width: 8,
			render: (_, row) => {
				const isPendingInvoice = row.invoiceStatus === EnumConfig.InvoiceStatus.Pending

				return (
					<ActionMenu
						actions={[
							{
								title: t('button.detail'),
								onClick: () => onOpenDetail(row),
							},
							isPendingInvoice && {
								title: t('invoice.button.payment'),
								onClick: () => onPaymentInvoiceClick(row.id),
							},
							isPendingInvoice && {
								title: t('invoice.button.complete'),
								onClick: () => onCompleteInvoiceClick(row.id),
							},
							isPendingInvoice && {
								title: t('button.cancel'),
								onClick: () => onCancelInvoiceClick(row.id),
							},
						].filter(Boolean)}
					/>
				)
			},
		},
	]

	return (
		<GenericTable
			fields={fields}
			data={invoices}
			rowKey='id'
			sort={sort}
			setSort={setSort}
			loading={loading}
		/>
	)
}

export default InvoiceManagementTableSection

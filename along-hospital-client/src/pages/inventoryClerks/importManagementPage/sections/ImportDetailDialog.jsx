import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useMemo } from 'react'

const ImportDetailDialog = ({ open, onClose, importData }) => {
	const { t } = useTranslation()

	const formFields = useMemo(
		() => [
			{
				key: 'id',
				title: t('import_management.table.id'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'importDate',
				title: t('import_management.field.import_date'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'supplierName',
				title: t('import_management.field.supplier'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'note',
				title: t('import_management.field.note'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled', multiline: true, rows: 2 },
			},
			{
				key: 'importDetails',
				title: t('import_management.field.import_details'),
				type: 'array',
				required: false,
				props: { readOnly: true },
				of: [
					{
						key: 'skuCode',
						title: t('import_request.field.sku_code'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'quantity',
						title: t('import_management.field.quantity'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'unitPrice',
						title: t('import_management.field.unit_price'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
				],
			},
		],
		[t]
	)

	if (!importData) return null

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('import_management.dialog.view.title')}
			fields={formFields}
			initialValues={importData ?? {}}
			maxWidth='md'
			showSubmit={false}
		/>
	)
}

export default ImportDetailDialog

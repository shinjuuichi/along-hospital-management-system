import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useMemo } from 'react'

const ImportRequestDetailDialog = ({ open, onClose, request, getStatusLabel, additionalButtons = [] }) => {
	const { t } = useTranslation()

	const statusLabel = useMemo(() => {
		if (!request) return ''
		return getStatusLabel ? getStatusLabel(request.status) : (request.status ?? '')
	}, [request, getStatusLabel])

	const details = Array.isArray(request?.details) ? request.details : []

	const initialValues = useMemo(() => {
		return {
			id: request?.id || '',
			requestDate: request?.requestDate || '',
			status: statusLabel,
			details: details.map((d) => ({
				skuCode: d.skuCode || '',
				medicineName: d.medicineName || '',
				requestQuantity: d.requestQuantity || '',
			})),
		}
	}, [request, statusLabel, details])

	const formFields = useMemo(
		() => [
			{
				key: 'id',
				title: t('import_request.table.id'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'requestDate',
				title: t('import_request.table.request_date'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'status',
				title: t('import_request.table.status'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'details',
				title: t('import_request.field.details'),
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
						key: 'medicineName',
						title: t('import_request.field.medicine_name'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'requestQuantity',
						title: t('import_request.field.request_quantity'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
				],
			},
		],
		[t]
	)

	if (!request) return null

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('import_request.dialog.view_title')}
			fields={formFields}
			initialValues={initialValues}
			maxWidth='md'
			showSubmit={false}
			additionalButtons={additionalButtons.map((btn) => ({
				...btn,
				onClick: ({ closeDialog }) => btn.onClick?.(closeDialog),
			}))}
		/>
	)
}

export default ImportRequestDetailDialog

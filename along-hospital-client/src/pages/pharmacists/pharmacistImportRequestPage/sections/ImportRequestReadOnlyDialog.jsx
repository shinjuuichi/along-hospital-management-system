import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'
import { useMemo } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const ImportRequestReadOnlyDialog = ({ open, onClose, request, getStatusLabel }) => {
	const { t } = useTranslation()

	const statusLabel = useMemo(() => {
		if (!request) return ''
		return getStatusLabel ? getStatusLabel(request.status) : (request.status ?? '')
	}, [request, getStatusLabel])

	const details = Array.isArray(request?.details) ? request.details : []
	const isCreated = request?.status === EnumConfig.ImportRequestStatus.Created

	const initialValues = useMemo(() => {
		return {
			id: request?.id || '',
			requestDate: request?.requestDate || '',
			status: statusLabel,
			details: details.map((d) => ({
				skuCode: d.skuCode || '',
				medicineName: d.medicineName || '',
				requestQuantity: d.requestQuantity || '',
				unitPrice: d.unitPrice ?? '',
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
				type: 'custom',
				required: false,
				render: ({ value }) => (
					<Stack spacing={1.5}>
						{(Array.isArray(value) ? value : []).map((item, idx) => (
							<Stack key={idx} direction='row' spacing={1}>
								<Typography variant='body2' sx={{ flex: 1 }}>
									{t('import_request.field.sku_code')}: {renderEmptyFallback(item.skuCode)}{item.medicineName ? ` - ${item.medicineName}` : ''}
								</Typography>
								<Typography variant='body2' sx={{ flex: 1 }}>
									{t('import_request.field.request_quantity')}: {renderEmptyFallback(item.requestQuantity)}
								</Typography>
								{isCreated && item.unitPrice != null && (
									<Typography variant='body2' sx={{ flex: 1 }}>
										{t('import_management.field.unit_price')}: {item.unitPrice}
									</Typography>
								)}
							</Stack>
						))}
					</Stack>
				),
			},
		],
		[t, isCreated]
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
		/>
	)
}

export default ImportRequestReadOnlyDialog

import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import CreateImportFromRequestDialog from '@/pages/inventoryClerks/importRequestPage/sections/CreateImportFromRequestDialog'
import ImportRequestReadOnlyDialog from '@/pages/pharmacists/pharmacistImportRequestPage/sections/ImportRequestReadOnlyDialog'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { isNumber } from '@/utils/validateUtil'
import { Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'

const renderMedicineOption = (value, label) => {
	const skuCode = typeof value === 'string' ? value : ''
	if (label && typeof label === 'object' && label !== null) {
		const medicineName = label.medicineName || label.name || ''
		return <span>{medicineName ? `${skuCode} - ${medicineName}` : skuCode}</span>
	}
	return <span>{typeof label === 'string' && label ? label : skuCode}</span>
}

const ImportRequestManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openDetail,
	setOpenDetail,
	openCreateImport,
	setOpenCreateImport,
	selectedRequest,
	setSelectedRequest,
	medicineSkuOptions,
	onCreateSubmit,
	onDetailSubmit,
	onCancel,
	onReject,
	onApprove,
	onCreateImport,
	onSuccess,
	role,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [createFormValues, setCreateFormValues] = useState({ details: [] })
	const [detailFormValues, setDetailFormValues] = useState(null)

	const remainSkuOptions = useMemo(
		() =>
			medicineSkuOptions.filter((medicine) =>
				createFormValues?.details
					? !createFormValues?.details?.some((detail) => detail.skuCode === medicine.value)
					: true
			),
		[medicineSkuOptions, createFormValues]
	)

	const remainSkuOptionsForDetail = useMemo(
		() =>
			medicineSkuOptions.filter((medicine) =>
				detailFormValues?.details
					? !detailFormValues?.details?.some((detail) => detail.skuCode === medicine.value)
					: true
			),
		[medicineSkuOptions, detailFormValues]
	)

	const isPending = selectedRequest?.status === EnumConfig.ImportRequestStatus.Pending
	const isApproved = selectedRequest?.status === EnumConfig.ImportRequestStatus.Approved
	const isCreated = selectedRequest?.status === EnumConfig.ImportRequestStatus.Created
	const isPharmacist = role === EnumConfig.Role.Pharmacist

	const getStatusLabel = useCallback(
		(status) => getEnumLabelByValue(_enum.importRequestStatusOptions, status) ?? '',
		[_enum]
	)

	const createFormFields = useMemo(
		() => [
			{
				key: 'details',
				title: t('import_request.field.details'),
				type: 'array',
				required: true,
				of: [
					{
						key: 'skuCode',
						title: t('import_request.field.sku_code'),
						type: 'select-dialog',
						options: medicineSkuOptions,
						remainOptions: remainSkuOptions,
						renderOption: renderMedicineOption,
					},
					{
						key: 'requestQuantity',
						title: t('import_request.field.request_quantity'),
						type: 'number',
					},
				],
			},
		],
		[t, medicineSkuOptions, remainSkuOptions]
	)

	const detailChildFields = useMemo(
		() => [
			{
				key: 'skuCode',
				title: t('import_request.field.sku_code'),
				type: 'select-dialog',
				required: true,
				options: medicineSkuOptions,
				remainOptions: remainSkuOptionsForDetail,
				renderOption: renderMedicineOption,
				props: { disabled: !isPending },
			},
			{
				key: 'requestQuantity',
				title: t('import_request.field.request_quantity'),
				type: 'number',
				required: true,
				validate: [isNumber()],
				props: { disabled: !isPending },
			},
		],
		[t, isPending, medicineSkuOptions, remainSkuOptionsForDetail]
	)

	const detailFormFields = useMemo(
		() => [
			{
				key: 'id',
				title: t('import_request.table.id'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'requestDate',
				title: t('import_request.table.request_date'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'status',
				title: t('import_request.table.status'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'details',
				title: t('import_request.field.details'),
				type: 'array',
				required: isPending,
				props: { readOnly: !isPending },
				of: detailChildFields,
			},
		],
		[t, isPending, detailChildFields]
	)

	const detailInitialValues = useMemo(() => {
		if (!selectedRequest) return {}
		return {
			id: selectedRequest.id,
			requestDate: selectedRequest.requestDate,
			status: getStatusLabel(selectedRequest.status),
			details: selectedRequest.details || [],
		}
	}, [selectedRequest, getStatusLabel])

	const detailReadOnlyFormFields = useMemo(
		() => [
			{
				key: 'id',
				title: t('import_request.table.id'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'requestDate',
				title: t('import_request.table.request_date'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'status',
				title: t('import_request.table.status'),
				type: 'text',
				required: false,
				props: { readOnly: true },
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

	const handleDetailSubmit = useCallback(
		async ({ values, closeDialog }) => {
			const { details } = values
			const ok = await onDetailSubmit(selectedRequest.id, { details })
			if (ok) {
				closeDialog()
				await onSuccess?.()
			}
		},
		[selectedRequest, onDetailSubmit, onSuccess]
	)

	const detailAdditionalButtons = useMemo(() => {
		if (isPending && isPharmacist) {
			return [
				{
					label: t('import_request.button.reject'),
					color: 'error',
					variant: 'contained',
					onClick: async () => {
						if (!selectedRequest?.id) return
						await onReject(selectedRequest.id)
						setOpenDetail(false)
						setSelectedRequest(null)
					},
				},
				{
					label: t('import_request.button.approve'),
					color: 'success',
					variant: 'contained',
					onClick: async () => {
						if (!selectedRequest?.id) return
						await onApprove(selectedRequest.id)
						setOpenDetail(false)
						setSelectedRequest(null)
					},
				},
			]
		}
		if (isPending && !isPharmacist) {
			return [
				{
					label: t('import_request.button.cancel'),
					color: 'error',
					variant: 'contained',
					onClick: async () => {
						if (!selectedRequest?.id) return
						await onCancel(selectedRequest.id)
						setOpenDetail(false)
						setSelectedRequest(null)
					},
				},
			]
		}
		if (isApproved && !isPharmacist) {
			return [
				{
					label: t('import_request.button.create_import'),
					color: 'success',
					variant: 'contained',
					onClick: () => {
						setOpenDetail(false)
						setOpenCreateImport(true)
					},
				},
			]
		}
		return []
	}, [
		isPending,
		isApproved,
		isPharmacist,
		t,
		onCancel,
		onReject,
		onApprove,
		selectedRequest,
		setOpenDetail,
		setOpenCreateImport,
	])

	const handleCreateImportSuccess = useCallback(async () => {
		setOpenCreateImport(false)
		setSelectedRequest(null)
		await onSuccess?.()
	}, [onSuccess, setOpenCreateImport, setSelectedRequest])

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={createFormFields}
				initialValues={{ details: [] }}
				submitLabel={t('button.create')}
				title={t('import_request.dialog.create_title')}
				maxWidth='md'
				onValuesChange={(values) => setCreateFormValues(values)}
				onSubmit={async ({ values, closeDialog }) => {
					const ok = await onCreateSubmit(values)
					if (ok) {
						closeDialog()
						await onSuccess?.()
					}
				}}
			/>

			<GenericFormDialog
				open={openDetail && isPending}
				onClose={() => {
					setOpenDetail(false)
					setSelectedRequest(null)
				}}
				fields={detailFormFields}
				initialValues={detailInitialValues}
				submitLabel={t('button.update')}
				title={t('import_request.dialog.view_title')}
				maxWidth='md'
				onValuesChange={(values) => setDetailFormValues(values)}
				onSubmit={handleDetailSubmit}
				additionalButtons={detailAdditionalButtons}
			/>

			{isPharmacist ? (
				<ImportRequestReadOnlyDialog
					open={openDetail && !isPending}
					onClose={() => {
						setOpenDetail(false)
						setSelectedRequest(null)
					}}
					request={selectedRequest}
					getStatusLabel={getStatusLabel}
				/>
			) : (
				<GenericFormDialog
					open={openDetail && !isPending}
					onClose={() => {
						setOpenDetail(false)
						setSelectedRequest(null)
					}}
					fields={detailReadOnlyFormFields}
					initialValues={detailInitialValues}
					title={t('import_request.dialog.view_title')}
					maxWidth='md'
					showSubmit={false}
					additionalButtons={detailAdditionalButtons}
				/>
			)}

			{!isPharmacist && (
				<CreateImportFromRequestDialog
					open={openCreateImport}
					onClose={() => {
						setOpenCreateImport(false)
						setSelectedRequest(null)
					}}
					request={selectedRequest}
					onCreateImport={onCreateImport}
					onSuccess={handleCreateImportSuccess}
				/>
			)}
		</>
	)
}

export default ImportRequestManagementFormSection

import ActionMenu from '@/components/generals/ActionMenu'
import DrawerInfoRow from '@/components/infoRows/DrawerInfoRow'
import {
	defaultClinicalOrderDetailStatusStyle,
	defaultInfusionMedicalOrderDetailExecutionStatusStyle,
	defaultMedicalOrderTypeStyle,
} from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getObjectValueFromStringPath } from '@/utils/handleObjectUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Close } from '@mui/icons-material'
import {
	Box,
	Button,
	Chip,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
	IconButton,
	Paper,
	Stack,
	TextField,
	Typography,
} from '@mui/material'
import { useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import {
	canPrintClinicalMedicalOrderInvoice,
	getMedicalOrderStatusInfo,
	isClinicalMedicalOrderDetailEditable,
	isInfusionMedicalOrderDetailEditable,
} from '../../helpers/medicalOrderHelper'

const renderFields = (item, fields) => {
	return fields.map((field, index) => {
		const resolvedValue =
			typeof field.value === 'function'
				? field.value(item)
				: getObjectValueFromStringPath(item, field.value)

		return (
			<DrawerInfoRow
				key={`${field.label}-${index}`}
				label={field.label}
				value={resolvedValue === null || resolvedValue === undefined ? renderEmptyFallback(null) : resolvedValue}
			/>
		)
	})
}

const MedicalHistoryDetailMedicalOrderDetailDialog = ({
	open = false,
	onClose = () => {},
	medicalOrder,
	readOnly = false,
	onClickUpdateInstructionMedicalOrder = () => {},
	// Clinical Medical Order actions
	onClickCancelClinicalMedicalOrder = () => {},
	onClickCompleteClinicalMedicalOrderDetail = () => {},
	onClickFailClinicalMedicalOrderDetail = () => {},
	onClickPrintRefundClinicalMedicalOrderDetail = () => {},
	// Infusion Medical Order actions
	onClickCompleteInfusionMedicalOrderDetail = () => {},
	onClickFailInfusionMedicalOrderDetail = () => {},
	// Instruction Medical Order actions
	onClickIssueInstructionMedicalOrder = () => {},
	onClickCancelInstructionMedicalOrder = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const confirm = useConfirm()
	const navigate = useNavigate()
	const { auth } = useAuth()

	const failReasonRef = useRef('')

	if (!medicalOrder) {
		return null
	}

	const statusInfo = getMedicalOrderStatusInfo(medicalOrder, _enum)

	const isRoleDoctorOrNurse =
		auth?.role === EnumConfig.Role.Doctor || auth?.role === EnumConfig.Role.Nurse
	const isClinical = medicalOrder.medicalOrderType === EnumConfig.MedicalOrderType.Clinical
	const isInfusion = medicalOrder.medicalOrderType === EnumConfig.MedicalOrderType.Infusion
	const isInstruction = medicalOrder.medicalOrderType === EnumConfig.MedicalOrderType.Instruction

	const renderActionButtons = () => {
		const actionButtonsMap = {
			[EnumConfig.MedicalOrderType.Clinical]: [
				canPrintClinicalMedicalOrderInvoice(medicalOrder) && {
					label: t('invoice.button.print_invoice'),
					variant: 'contained',
					color: 'primary',
					onClick: () => {
						navigate(routeUrls.HOME.INVOICE_PRINT(medicalOrder.pendingInvoiceId))
					},
				},
				medicalOrder.clinicalMedicalOrderStatus === EnumConfig.ClinicalMedicalOrderStatus.Pending && {
					label: t('button.cancel'),
					variant: 'contained',
					color: 'error',
					onClick: async () => {
						const isConfirmed = await confirm({
							title: t('medical_order.dialog.clinical.cancel_title'),
							description: t('medical_order.dialog.clinical.cancel_description'),
							confirmColor: 'error',
							confirmText: t('button.cancel'),
						})
						if (isConfirmed) {
							onClickCancelClinicalMedicalOrder(medicalOrder.id)
						}
					},
				},
			],
			[EnumConfig.MedicalOrderType.Infusion]: [],
			[EnumConfig.MedicalOrderType.Instruction]:
				medicalOrder.instructionMedicalOrderStatus === EnumConfig.InstructionMedicalOrderStatus.Draft
					? [
							{
								label: t('button.update'),
								variant: 'outlined',
								color: 'success',
								onClick: () => onClickUpdateInstructionMedicalOrder(medicalOrder.id),
							},
							{
								label: t('medical_order.button.issue'),
								variant: 'contained',
								color: 'primary',
								onClick: async () => {
									const isConfirmed = await confirm({
										title: t('medical_order.dialog.instruction.issue_title'),
										description: t('medical_order.dialog.instruction.issue_description'),
										confirmColor: 'primary',
										confirmText: t('medical_order.button.issue'),
									})

									if (isConfirmed) {
										onClickIssueInstructionMedicalOrder(medicalOrder.id)
									}
								},
							},
							{
								label: t('button.cancel'),
								variant: 'contained',
								color: 'error',
								onClick: async () => {
									const isConfirmed = await confirm({
										title: t('medical_order.dialog.instruction.cancel_title'),
										description: t('medical_order.dialog.instruction.cancel_description'),
										confirmColor: 'error',
										confirmText: t('button.cancel'),
									})
									if (isConfirmed) {
										onClickCancelInstructionMedicalOrder(medicalOrder.id)
									}
								},
							},
						]
					: [],
		}

		return actionButtonsMap[medicalOrder.medicalOrderType].filter(Boolean) || []
	}

	const commonFields = [
		{
			label: t('medical_order.field.medical_order_type'),
			value: () => getEnumLabelByValue(_enum.medicalOrderTypeOptions, medicalOrder.medicalOrderType),
		},
		{
			label: t('medical_order.field.creation_date'),
			value: () => formatDatetimeStringBasedOnCurrentLanguage(medicalOrder.creationDate),
		},
		{ label: t('medical_order.field.instruction'), value: 'instruction' },
	]

	const clinicalSummaryFields = [
		{
			label: t('medical_order.field.clinical_medical_order.status'),
			value: () => (
				<Chip
					size='small'
					color={statusInfo.color}
					label={getEnumLabelByValue(
						_enum.clinicalOrderStatusOptions,
						medicalOrder.clinicalMedicalOrderStatus
					)}
				/>
			),
		},
	]

	const clinicalDetailFields = [
		{ label: t('medical_order.field.clinical_medical_order.details.quantity'), value: 'quantity' },
		{
			label: t('medical_order.field.clinical_medical_order.details.status'),
			value: (item) => (
				<Chip
					size='small'
					color={defaultClinicalOrderDetailStatusStyle(item?.clinicalMedicalOrderDetailStatus)}
					label={getEnumLabelByValue(
						_enum.clinicalOrderDetailStatusOptions,
						item?.clinicalMedicalOrderDetailStatus
					)}
				/>
			),
		},
		{ label: t('medical_service.field.name'), value: 'medicalServiceSnapshot.name' },
		{ label: t('medical_service.field.description'), value: 'medicalServiceSnapshot.description' },
		{ label: t('medical_service.field.code'), value: 'medicalServiceSnapshot.code' },
	]

	const infusionDetailFields = [
		{ label: t('medical_order.field.infusion_medical_order.details.rate'), value: 'rate' },
		{ label: t('medical_order.field.infusion_medical_order.details.frequency'), value: 'frequency' },
		{ label: t('medical_order.field.infusion_medical_order.details.duration'), value: 'duration' },
		{
			label: t('medical_order.field.infusion_medical_order.details.execution_status'),
			value: (item) => (
				<Chip
					size='small'
					color={defaultInfusionMedicalOrderDetailExecutionStatusStyle(
						item?.infusionMedicalOrderDetailExecutionStatus
					)}
					label={getEnumLabelByValue(
						_enum.infusionMedicalOrderDetailExecutionStatusOptions,
						item?.infusionMedicalOrderDetailExecutionStatus
					)}
				/>
			),
		},
		{ label: t('medical_order.field.infusion_medical_order.details.note'), value: 'note' },
		{ label: t('medicine.field.name'), value: 'medicineSnapshot.name' },
		{ label: t('medicine.field.brand'), value: 'medicineSnapshot.brand' },
		{ label: t('medicine.field.unit'), value: 'medicineSnapshot.medicineUnit' },
		{ label: t('medicine.field.medicine_category.name'), value: 'medicineSnapshot.categoryName' },
	]

	const instructionSummaryFields = [
		{
			label: t('medical_order.field.instruction_medical_order.status'),
			value: () => (
				<Chip
					size='small'
					color={statusInfo.color}
					label={getEnumLabelByValue(
						_enum.instructionMedicalOrderStatusOptions,
						medicalOrder.instructionMedicalOrderStatus
					)}
				/>
			),
		},
	]

	const positionOrderFields = [
		{
			label: t('medical_order.field.instruction_medical_order.position_order.position_order_type'),
			value: (item) => getEnumLabelByValue(_enum.positionOrderTypeOptions, item?.positionOrderType),
		},
		{
			label: t('medical_order.field.instruction_medical_order.position_order.instruction'),
			value: 'instruction',
		},
	]

	const respiratoryOrderFields = [
		{
			label: t(
				'medical_order.field.instruction_medical_order.respiratory_support_order.respiratory_support_order_type'
			),
			value: (item) =>
				getEnumLabelByValue(
					_enum.respiratorySupportOrderTypeOptions,
					item?.respiratorySupportOrderType
				),
		},
		{
			label: t('medical_order.field.instruction_medical_order.respiratory_support_order.oxygen_flow'),
			value: 'oxygenFlow',
		},
		{
			label: t('medical_order.field.instruction_medical_order.respiratory_support_order.fio2'),
			value: 'fiO2',
		},
		{
			label: t('medical_order.field.instruction_medical_order.respiratory_support_order.instruction'),
			value: 'instruction',
		},
	]

	const nutritionOrderFields = [
		{
			label: t('medical_order.field.instruction_medical_order.nutrition_order.nutrition_order_type'),
			value: (item) => getEnumLabelByValue(_enum.nutritionOrderTypeOptions, item?.nutritionOrderType),
		},
		{
			label: t('medical_order.field.instruction_medical_order.nutrition_order.instruction'),
			value: 'instruction',
		},
	]

	const nursingCareOrderFields = [
		{
			label: t(
				'medical_order.field.instruction_medical_order.nursing_care_order.nursing_care_order_level'
			),
			value: (item) =>
				getEnumLabelByValue(_enum.nursingCareOrderLevelOptions, item?.nursingCareOrderLevel),
		},
		{
			label: t(
				'medical_order.field.instruction_medical_order.nursing_care_order.monitor_interval_hour'
			),
			value: 'monitorIntervalHour',
		},
	]

	const clinicalDetails = medicalOrder.clinicalMedicalOrderDetails || []
	const infusionDetails = medicalOrder.infusionMedicalOrderDetails || []

	const getClinicalDetailActions = (detail) => {
		const medicalServiceId = detail?.medicalServiceId

		return [
			isClinicalMedicalOrderDetailEditable(medicalOrder, detail) && {
				title: t('medical_order.button.complete'),
				onClick: async () => {
					const isConfirmed = await confirm({
						title: t('medical_order.dialog.clinical.detail.complete_title'),
						description: t('medical_order.dialog.clinical.detail.complete_description'),
						confirmColor: 'success',
						confirmText: t('button.confirm'),
					})

					if (isConfirmed && medicalServiceId) {
						onClickCompleteClinicalMedicalOrderDetail(medicalOrder.id, medicalServiceId)
					}
				},
			},
			isClinicalMedicalOrderDetailEditable(medicalOrder, detail) && {
				title: t('medical_order.button.fail'),
				onClick: async () => {
					failReasonRef.current = ''

					const isConfirmed = await confirm({
						title: t('medical_order.dialog.clinical.detail.fail_title'),
						description: (
							<Stack spacing={1}>
								<Typography>{t('medical_order.dialog.clinical.detail.fail_description')}</Typography>
								<TextField
									label={t('medical_order.field.clinical_medical_order.details.fail_reason')}
									multiline
									rows={4}
									defaultValue=''
									onChange={(e) => {
										failReasonRef.current = e.target.value
									}}
									fullWidth
								/>
							</Stack>
						),
						confirmColor: 'error',
						confirmText: t('button.confirm'),
					})

					if (isConfirmed && medicalServiceId) {
						onClickFailClinicalMedicalOrderDetail(
							medicalOrder.id,
							medicalServiceId,
							failReasonRef.current
						)
					}
				},
			},
			detail.clinicalMedicalOrderDetailStatus ===
				EnumConfig.ClinicalMedicalOrderDetailStatus.Failed && {
				title: t('medical_order.button.print_refund'),
				onClick: async () => {
					onClickPrintRefundClinicalMedicalOrderDetail(detail.id)
				},
			},
		].filter(Boolean)
	}

	const getInfusionDetailActions = (detail) => {
		const medicineId = detail?.medicineId

		return [
			{
				title: t('medical_order.button.complete'),
				onClick: async () => {
					const isConfirmed = await confirm({
						title: t('medical_order.dialog.infusion.detail.complete_title'),
						description: t('medical_order.dialog.infusion.detail.complete_description'),
						confirmColor: 'success',
						confirmText: t('button.confirm'),
					})

					if (isConfirmed && medicineId) {
						onClickCompleteInfusionMedicalOrderDetail(medicalOrder.id, medicineId)
					}
				},
			},
			{
				title: t('medical_order.button.fail'),
				onClick: async () => {
					const isConfirmed = await confirm({
						title: t('medical_order.dialog.infusion.detail.fail_title'),
						description: t('medical_order.dialog.infusion.detail.fail_description'),
						confirmColor: 'error',
						confirmText: t('button.confirm'),
					})

					if (isConfirmed && medicineId) {
						onClickFailInfusionMedicalOrderDetail(medicalOrder.id, medicineId)
					}
				},
			},
		]
	}

	return (
		<Dialog open={open} onClose={onClose} fullWidth maxWidth='md'>
			<DialogTitle>
				<Stack direction='row' alignItems='flex-start' justifyContent='space-between' spacing={2}>
					<Box>
						<Typography variant='h6'>{t('medical_order.title.detail')}</Typography>
						<Stack direction='row' spacing={1} sx={{ mt: 1, flexWrap: 'wrap' }}>
							<Chip
								size='small'
								color={defaultMedicalOrderTypeStyle(medicalOrder.medicalOrderType).color}
								label={getEnumLabelByValue(_enum.medicalOrderTypeOptions, medicalOrder.medicalOrderType)}
							/>
							{statusInfo.value && <Chip size='small' color={statusInfo.color} label={statusInfo.label} />}
						</Stack>
					</Box>
					<IconButton onClick={onClose} size='small'>
						<Close />
					</IconButton>
				</Stack>
			</DialogTitle>

			<Divider />

			<DialogContent>
				<Stack spacing={2} sx={{ pt: 1 }}>
					<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
						<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1.5 }}>
							{t('medical_order.title.general_information')}
						</Typography>
						<Stack spacing={1.25}>{renderFields(medicalOrder, commonFields)}</Stack>
					</Paper>

					{isClinical && (
						<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
							<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1.5 }}>
								{t('medical_order.title.clinical_medical_order_information')}
							</Typography>
							<Stack spacing={1.25}>{renderFields(medicalOrder, clinicalSummaryFields)}</Stack>

							<Divider sx={{ my: 1.5 }} />

							<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1.5 }}>
								{t('medical_order.field.clinical_medical_order.details.details')}
							</Typography>

							{clinicalDetails.length === 0 ? (
								<Typography color='text.secondary'>
									{t('medical_order.placeholder.no_clinical_order_details')}
								</Typography>
							) : (
								<Stack spacing={1.5}>
									{clinicalDetails.map((detail, index) => (
										<Paper
											key={`${detail?.id || 'clinical-detail'}-${index}`}
											variant='outlined'
											sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}
										>
											<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ mb: 1 }}>
												<Typography variant='subtitle2' sx={{ fontWeight: 700 }}>
													{t('medical_order.field.clinical_medical_order.details.detail')} #{index + 1}
												</Typography>

												{isRoleDoctorOrNurse && <ActionMenu actions={getClinicalDetailActions(detail)} />}
											</Stack>
											<Stack spacing={1}>{renderFields(detail, clinicalDetailFields)}</Stack>
										</Paper>
									))}
								</Stack>
							)}
						</Paper>
					)}

					{isInfusion && (
						<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
							<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1.5 }}>
								{t('medical_order.field.infusion_medical_order.details.details')}
							</Typography>

							{infusionDetails.length === 0 ? (
								<Typography color='text.secondary'>
									{t('medical_order.placeholder.no_infusion_order_details')}
								</Typography>
							) : (
								<Stack spacing={1.5}>
									{infusionDetails.map((detail, index) => (
										<Paper
											key={`${detail?.id || 'infusion-detail'}-${index}`}
											variant='outlined'
											sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}
										>
											<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ mb: 1 }}>
												<Typography variant='subtitle2' sx={{ fontWeight: 700 }}>
													{t('medical_order.field.infusion_medical_order.details.detail')} #{index + 1}
												</Typography>

												{isRoleDoctorOrNurse && isInfusionMedicalOrderDetailEditable(detail) && (
													<ActionMenu actions={getInfusionDetailActions(detail)} />
												)}
											</Stack>
											<Stack spacing={1}>{renderFields(detail, infusionDetailFields)}</Stack>
										</Paper>
									))}
								</Stack>
							)}
						</Paper>
					)}

					{isInstruction && (
						<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
							<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1.5 }}>
								{t('medical_order.title.instruction_medical_order_information')}
							</Typography>
							<Stack spacing={1.25}>{renderFields(medicalOrder, instructionSummaryFields)}</Stack>

							<Divider sx={{ my: 1.5 }} />

							<Stack spacing={1.5}>
								{medicalOrder.positionOrder && (
									<Paper variant='outlined' sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}>
										<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1 }}>
											{t('medical_order.field.instruction_medical_order.position_order.position_order')}
										</Typography>
										<Stack spacing={1}>{renderFields(medicalOrder.positionOrder, positionOrderFields)}</Stack>
									</Paper>
								)}

								{medicalOrder.respiratorySupportOrder && (
									<Paper variant='outlined' sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}>
										<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1 }}>
											{t(
												'medical_order.field.instruction_medical_order.respiratory_support_order.respiratory_support_order'
											)}
										</Typography>
										<Stack spacing={1}>
											{renderFields(medicalOrder.respiratorySupportOrder, respiratoryOrderFields)}
										</Stack>
									</Paper>
								)}

								{medicalOrder.nutritionOrder && (
									<Paper variant='outlined' sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}>
										<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1 }}>
											{t('medical_order.field.instruction_medical_order.nutrition_order.nutrition_order')}
										</Typography>
										<Stack spacing={1}>
											{renderFields(medicalOrder.nutritionOrder, nutritionOrderFields)}
										</Stack>
									</Paper>
								)}

								{medicalOrder.nursingCareOrder && (
									<Paper variant='outlined' sx={{ p: 1.5, borderRadius: 2, bgcolor: 'background.default' }}>
										<Typography variant='subtitle2' sx={{ fontWeight: 700, mb: 1 }}>
											{t(
												'medical_order.field.instruction_medical_order.nursing_care_order.nursing_care_order'
											)}
										</Typography>
										<Stack spacing={1}>
											{renderFields(medicalOrder.nursingCareOrder, nursingCareOrderFields)}
										</Stack>
									</Paper>
								)}

								{!medicalOrder.positionOrder &&
									!medicalOrder.respiratorySupportOrder &&
									!medicalOrder.nutritionOrder &&
									!medicalOrder.nursingCareOrder && (
										<Typography color='text.secondary'>
											{t('medical_order.placeholder.no_instruction_sub_order_data')}
										</Typography>
									)}
							</Stack>
						</Paper>
					)}
				</Stack>
			</DialogContent>

			<DialogActions>
				{!readOnly &&
					renderActionButtons().map((button, index) =>
						button?.label ? (
							<Button
								key={`${button?.label || 'action-button'}-${index}`}
								onClick={button?.onClick || (() => {})}
								variant={button?.variant || 'contained'}
								color={button?.color || 'primary'}
							>
								{button?.label}
							</Button>
						) : null
					)}
			</DialogActions>
		</Dialog>
	)
}

export default MedicalHistoryDetailMedicalOrderDetailDialog

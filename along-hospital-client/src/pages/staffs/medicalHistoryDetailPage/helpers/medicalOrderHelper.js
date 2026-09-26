import { defaultMedicalOrderStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'

export const getMedicalOrderStatusInfo = (medicalOrder, _enum) => {
	const getMedicalOrderStatusValue = (medicalOrder) => {
		switch (medicalOrder?.medicalOrderType) {
			case EnumConfig.MedicalOrderType.Clinical:
				return medicalOrder?.clinicalMedicalOrderStatus
			case EnumConfig.MedicalOrderType.Instruction:
				return medicalOrder?.instructionMedicalOrderStatus
			default:
				return null
		}
	}

	const getMedicalOrderStatusOptionsByType = (_enum, medicalOrderType) => {
		switch (medicalOrderType) {
			case EnumConfig.MedicalOrderType.Clinical:
				return _enum.clinicalOrderStatusOptions
			case EnumConfig.MedicalOrderType.Instruction:
				return _enum.instructionMedicalOrderStatusOptions
			default:
				return []
		}
	}

	const status = getMedicalOrderStatusValue(medicalOrder)
	const options = getMedicalOrderStatusOptionsByType(_enum, medicalOrder?.medicalOrderType)

	if (!status) {
		return {
			value: null,
			label: null,
			color: 'default',
		}
	}

	return {
		value: status,
		label: getEnumLabelByValue(options, status) || status,
		color: defaultMedicalOrderStatusStyle(medicalOrder?.medicalOrderType, status),
	}
}

export const getMedicalOrderActionButtons = (medicalOrder) => {
	const actionButtonsMap = {
		[EnumConfig.MedicalOrderType.Clinical]: [],
		[EnumConfig.MedicalOrderType.Infusion]: [],
		[EnumConfig.MedicalOrderType.Instruction]: [],
	}

	return actionButtonsMap[medicalOrder?.medicalOrderType] || []
}

export const getProcessedMedicalOrderDetailCountInfo = (medicalOrder) => {
	if (medicalOrder?.medicalOrderType === EnumConfig.MedicalOrderType.Clinical) {
		const details = medicalOrder?.clinicalMedicalOrderDetails || []
		const processed = details.filter(
			(detail) =>
				detail?.clinicalMedicalOrderDetailStatus !== EnumConfig.ClinicalMedicalOrderDetailStatus.Pending
		).length

		return {
			processed,
			total: details.length,
		}
	}

	if (medicalOrder?.medicalOrderType === EnumConfig.MedicalOrderType.Infusion) {
		const details = medicalOrder?.infusionMedicalOrderDetails || []
		const processed = details.filter(
			(detail) =>
				detail?.infusionMedicalOrderDetailExecutionStatus !==
				EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Pending
		).length

		return {
			processed,
			total: details.length,
		}
	}

	return null
}

export const canRenderMedicalOrderByRole = (medicalOrder, role) => {
	const isInstructionDraft =
		medicalOrder?.medicalOrderType === EnumConfig.MedicalOrderType.Instruction &&
		medicalOrder?.instructionMedicalOrderStatus === EnumConfig.InstructionMedicalOrderStatus.Draft

	if (!isInstructionDraft) {
		return true
	}

	return role === EnumConfig.Role.Doctor
}

export const canPrintClinicalMedicalOrderInvoice = (medicalOrder) => {
	return (
		medicalOrder?.medicalOrderType === EnumConfig.MedicalOrderType.Clinical &&
		medicalOrder?.clinicalMedicalOrderStatus === EnumConfig.ClinicalMedicalOrderStatus.Pending &&
		medicalOrder?.pendingInvoiceId
	)
}

export const isMedicalOrderEditable = (medicalHistory, role) => {
	const isDraftMedicalHistory =
		medicalHistory?.medicalHistoryStatus === EnumConfig.MedicalHistoryStatus.Draft
	const isDoctorRole = role === EnumConfig.Role.Doctor

	return isDraftMedicalHistory && isDoctorRole
}

export const isClinicalMedicalOrderDetailEditable = (
	clinicalMedicalOrder,
	clinicalMedicalOrderDetail
) => {
	const isPaidClinicalMedicalOrder =
		clinicalMedicalOrder?.clinicalMedicalOrderStatus === EnumConfig.ClinicalMedicalOrderStatus.Paid
	const isPendingClinicalMedicalOrderDetail =
		clinicalMedicalOrderDetail?.clinicalMedicalOrderDetailStatus ===
		EnumConfig.ClinicalMedicalOrderDetailStatus.Pending

	return isPaidClinicalMedicalOrder && isPendingClinicalMedicalOrderDetail
}

export const isInfusionMedicalOrderDetailEditable = (infusionMedicalOrderDetail) => {
	const isPendingInfusionMedicalOrderDetail =
		infusionMedicalOrderDetail?.infusionMedicalOrderDetailExecutionStatus ===
		EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Pending

	return isPendingInfusionMedicalOrderDetail
}

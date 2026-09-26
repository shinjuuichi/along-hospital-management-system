import { EnumConfig } from '@/configs/enumConfig'

export const MedicalHistorySectionKey = {
	Header: 'header',
	Prescription: 'prescription',
	Complaint: 'complaint',
	BedOccupancy: 'bedOccupancy',
	MedicalOrder: 'medicalOrder',
	Invoice: 'invoice',
}

const supportAllSectionsRoles = [EnumConfig.Role.HotlineAgent, EnumConfig.Role.Manager]

const sectionVisibleRoles = {
	[MedicalHistorySectionKey.Prescription]: [
		EnumConfig.Role.Nurse,
		EnumConfig.Role.Doctor,
		EnumConfig.Role.Patient,
	],
	[MedicalHistorySectionKey.Complaint]: [EnumConfig.Role.HotlineAgent, EnumConfig.Role.Patient],
	[MedicalHistorySectionKey.BedOccupancy]: [
		EnumConfig.Role.Nurse,
		EnumConfig.Role.Doctor,
		EnumConfig.Role.Patient,
	],
	[MedicalHistorySectionKey.MedicalOrder]: [
		EnumConfig.Role.Nurse,
		EnumConfig.Role.Doctor,
		EnumConfig.Role.Patient,
	],
	[MedicalHistorySectionKey.Invoice]: [
		EnumConfig.Role.Accountant,
		EnumConfig.Role.Doctor,
		EnumConfig.Role.Nurse,
		EnumConfig.Role.Patient,
		EnumConfig.Role.Receptionist,
	],
}

export const canViewMedicalHistorySection = (role, sectionKey) => {
	if (sectionKey === MedicalHistorySectionKey.Header) {
		return true
	}

	if (!role) {
		return false
	}

	if (supportAllSectionsRoles.includes(role)) {
		return true
	}

	return sectionVisibleRoles[sectionKey]?.includes(role) || false
}

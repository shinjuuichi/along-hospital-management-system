import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { createSlice } from '@reduxjs/toolkit'

const initState = {
	doctors: [],
	patients: [],
	staffs: [],
	staffCertificateTypes: [],
	shifts: [],
	medicineCategories: [],
	blogCategories: [],
	bedCategories: [],
	beds: [],
	specialties: [],
	medicalServices: [],
	medicines: [],
	infusionMedicines: [],
	medicineUnits: [],
	options: [],
	optionValues: [],
	suppliers: [],
	qualifications: [],
	floors: [],
	roomCategories: [],
	buildings: [],
	rooms: [],
	allowanceTypes: [],
	deductionTypes: [],
	teleRooms: [],
	medicineSkus: [],
	regionalWages: [],
	interviewTypes: [],
}

const managementSlice = createSlice({
	name: 'manager',
	initialState: initState,
	reducers: {
		setDoctorsStore: (state, action) => {
			state.doctors = action.payload
		},
		setPatientsStore: (state, action) => {
			state.patients = action.payload
		},
		setStaffsStore: (state, action) => {
			state.staffs = action.payload
		},
		setStaffCertificateTypesStore: (state, action) => {
			state.staffCertificateTypes = action.payload
		},
		setShiftsStore: (state, action) => {
			state.shifts = action.payload
		},
		setMedicineCategoriesStore: (state, action) => {
			state.medicineCategories = action.payload
		},
		setBlogCategoriesStore: (state, action) => {
			state.blogCategories = action.payload
		},
		setBedCategoriesStore: (state, action) => {
			state.bedCategories = action.payload
		},
		setBedsStore: (state, action) => {
			state.beds = action.payload
		},
		setSpecialtiesStore: (state, action) => {
			state.specialties = action.payload
		},
		setMedicalServicesStore: (state, action) => {
			state.medicalServices = action.payload
		},
		setMedicinesStore: (state, action) => {
			state.medicines = action.payload
		},
		setInfusionMedicinesStore: (state, action) => {
			state.infusionMedicines = action.payload
		},
		setMedicineUnitsStore: (state, action) => {
			state.medicineUnits = action.payload
		},
		setOptionsStore: (state, action) => {
			state.options = action.payload
		},
		setOptionValuesStore: (state, action) => {
			state.optionValues = action.payload
		},
		setSuppliersStore: (state, action) => {
			state.suppliers = action.payload
		},
		setQualificationsStore: (state, action) => {
			state.qualifications = action.payload
		},
		setRoomsStore: (state, action) => {
			state.rooms = action.payload
		},
		setFloorsStore: (state, action) => {
			state.floors = action.payload
		},
		setRoomCategoriesStore: (state, action) => {
			state.roomCategories = action.payload
		},
		setAllowanceTypesStore: (state, action) => {
			state.allowanceTypes = action.payload
		},
		setDeductionTypesStore: (state, action) => {
			state.deductionTypes = action.payload
		},
		setBuildingsStore: (state, action) => {
			state.buildings = action.payload
		},
		setTeleRoomsStore: (state, action) => {
			state.teleRooms = action.payload
		},
		setMedicineSkusStore: (state, action) => {
			state.medicineSkus = action.payload
		},
		setRegionalWagesStore: (state, action) => {
			state.regionalWages = action.payload
		},
		setInterviewTypesStore: (state, action) => {
			state.interviewTypes = action.payload
		},
	},
})

const {
	setDoctorsStore,
	setPatientsStore,
	setStaffsStore,
	setStaffCertificateTypesStore,
	setMedicineCategoriesStore,
	setBlogCategoriesStore,
	setBedCategoriesStore,
	setBedsStore,
	setSpecialtiesStore,
	setMedicalServicesStore,
	setMedicinesStore,
	setInfusionMedicinesStore,
	setMedicineUnitsStore,
	setOptionsStore,
	setOptionValuesStore,
	setSuppliersStore,
	setQualificationsStore,
	setRoomsStore,
	setFloorsStore,
	setRoomCategoriesStore,
	setAllowanceTypesStore,
	setDeductionTypesStore,
	setBuildingsStore,
	setShiftsStore,
	setTeleRoomsStore,
	setMedicineSkusStore,
	setRegionalWagesStore,
	setInterviewTypesStore,
} = managementSlice.actions

setDoctorsStore.defaultUrl = ApiUrls.STAFF.GET_BY_ROLE(EnumConfig.Role.Doctor)
setPatientsStore.defaultUrl = ApiUrls.PATIENT.MANAGEMENT.GET_ALL // Only manager can get this
setStaffsStore.defaultUrl = ApiUrls.STAFF.MANAGEMENT.GET_ALL
setStaffCertificateTypesStore.defaultUrl = ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.GET_ALL
setShiftsStore.defaultUrl = ApiUrls.SHIFT.GET_ALL
setSpecialtiesStore.defaultUrl = ApiUrls.SPECIALTY.MANAGEMENT.GET_ALL
setMedicalServicesStore.defaultUrl = ApiUrls.MEDICAL_SERVICE.MANAGEMENT.GET_ALL
setMedicinesStore.defaultUrl = ApiUrls.MEDICINE.GET_ALL
setInfusionMedicinesStore.defaultUrl = ApiUrls.MEDICINE.INFUSION.GET_ALL
setMedicineUnitsStore.defaultUrl = ApiUrls.MEDICINE_UNIT.MANAGEMENT.GET_ALL
setOptionsStore.defaultUrl = ApiUrls.OPTION.MANAGEMENT.GET_ALL
setOptionValuesStore.defaultUrl = ApiUrls.OPTION_VALUE.MANAGEMENT.GET_ALL
setMedicineCategoriesStore.defaultUrl = ApiUrls.MEDICINE_CATEGORY.GET_ALL
setBlogCategoriesStore.defaultUrl = ApiUrls.BLOG_CATEGORY.GET_ALL
setBedCategoriesStore.defaultUrl = ApiUrls.BED_CATEGORY.MANAGEMENT.GET_ALL
setBedsStore.defaultUrl = ApiUrls.BED.MANAGEMENT.GET_ALL
setSuppliersStore.defaultUrl = ApiUrls.SUPPLIER.MANAGEMENT.GET_ALL
setQualificationsStore.defaultUrl = ApiUrls.QUALIFICATION.MANAGEMENT.GET_ALL
setRoomsStore.defaultUrl = ApiUrls.ROOM.MANAGEMENT.GET_ALL
setFloorsStore.defaultUrl = ApiUrls.FLOOR.MANAGEMENT.GET_ALL
setRoomCategoriesStore.defaultUrl = ApiUrls.ROOM_CATEGORY.MANAGEMENT.GET_ALL
setAllowanceTypesStore.defaultUrl = ApiUrls.ALLOWANCE_TYPE.MANAGEMENT.GET_ALL
setDeductionTypesStore.defaultUrl = ApiUrls.DEDUCTION_TYPE.MANAGEMENT.GET_ALL
setBuildingsStore.defaultUrl = ApiUrls.BUILDING.MANAGEMENT.GET_ALL
setTeleRoomsStore.defaultUrl = ApiUrls.TELE_ROOM.GET_ALL
setMedicineSkusStore.defaultUrl = ApiUrls.MEDICINE_SKU.MANAGEMENT.INDEX
setRegionalWagesStore.defaultUrl = ApiUrls.REGIONAL_WAGE.MANAGEMENT.GET_ALL
setInterviewTypesStore.defaultUrl = ApiUrls.INTERVIEW_TYPE.MANAGEMENT.GET_ALL

export {
	setAllowanceTypesStore,
	setBedCategoriesStore,
	setBedsStore,
	setBlogCategoriesStore,
	setBuildingsStore,
	setDeductionTypesStore,
	setDoctorsStore,
	setFloorsStore,
	setMedicalServicesStore,
	setMedicineCategoriesStore,
	setInfusionMedicinesStore,
	setMedicineSkusStore,
	setMedicinesStore,
	setMedicineUnitsStore,
	setOptionsStore,
	setOptionValuesStore,
	setPatientsStore,
	setQualificationsStore,
	setRoomCategoriesStore,
	setRoomsStore,
	setShiftsStore,
	setSpecialtiesStore,
	setStaffCertificateTypesStore,
	setStaffsStore,
	setSuppliersStore,
	setTeleRoomsStore,
	setRegionalWagesStore,
	setInterviewTypesStore,
}

export default managementSlice.reducer

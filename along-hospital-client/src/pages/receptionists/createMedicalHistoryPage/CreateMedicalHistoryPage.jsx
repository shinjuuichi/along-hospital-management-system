import PatientInfoDialog from '@/components/dialogs/PatientInfoDialog'
import EmptyBox from '@/components/placeholders/EmptyBox'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setPatientsStore, setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { Box, Divider, Grid, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import CreateMedicalHistoryCardSection from './sections/CreateMedicalHistoryCardSection'
import { default as CreateMedicalHistoryDataSection } from './sections/CreateMedicalHistoryDataSection'
import CreateMedicalHistoryFooterSection from './sections/CreateMedicalHistoryFooterSection'
import CreateMedicalHistoryHeaderSection from './sections/CreateMedicalHistoryHeaderSection'
import CreateMedicalHistoryPatientSummarySection from './sections/CreateMedicalHistoryPatientSummarySection'
import CreateMedicalHistorySearchPatientSection from './sections/CreateMedicalHistorySearchPatientSection'

export default function CreateMedicalHistoryPage() {
	const { t } = useTranslation()
	const { auth } = useAuth()
	const _enum = useEnum()
	const navigate = useNavigate()
	const role = auth?.role
	const isReceptionist = role === EnumConfig.Role.Receptionist
	const isNurse = role === EnumConfig.Role.Nurse
	const fixedMedicalHistoryType = isReceptionist
		? EnumConfig.MedicalHistoryType.Outpatient
		: isNurse
			? EnumConfig.MedicalHistoryType.Inpatient
			: null

	const [createMedicalHistoryData, setCreateMedicalHistoryData] = useState({
		patientId: null,
		medicalHistoryType: fixedMedicalHistoryType,
		specialtyId: null,
		assignedDoctorId: null,
	})

	const [searchTerm, setSearchTerm] = useState('')
	const [openCreatePatientDialog, setOpenCreatePatientDialog] = useState(false)
	const [availableDoctors, setAvailableDoctors] = useState([])

	useEffect(() => {
		setCreateMedicalHistoryData((prev) => ({
			...prev,
			medicalHistoryType: fixedMedicalHistoryType || prev.medicalHistoryType,
			assignedDoctorId: isNurse ? prev.assignedDoctorId : null,
		}))
	}, [fixedMedicalHistoryType, isNurse])

	const patientsStore = useReduxStore({
		selector: (state) => state.management.patients,
		setStore: setPatientsStore,
	})

	const specialtiesStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	const patients = useMemo(() => {
		if (!searchTerm) return patientsStore.data || []
		const lowerSearchTerm = searchTerm.toLowerCase()
		return patientsStore.data.filter(
			(p) =>
				(p.name && p.name.toLowerCase().includes(lowerSearchTerm)) ||
				(p.phone && p.phone.toLowerCase().includes(lowerSearchTerm)) ||
				(p.email && p.email.toLowerCase().includes(lowerSearchTerm)) ||
				(p.medicalNumber && p.medicalNumber.toLowerCase().includes(lowerSearchTerm))
		)
	}, [patientsStore.data, searchTerm])

	const selectedPatient = useMemo(
		() => patients.find((p) => p.id === createMedicalHistoryData.patientId) || null,
		[patients, createMedicalHistoryData.patientId]
	)

	const createPatient = useAxiosSubmit({
		url: ApiUrls.PATIENT.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async (response) => {
			const newPatient = response.data
			patientsStore.resetStore((prev) => [...prev, newPatient])
		},
	})

	const createMedicalHistory = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.MANAGEMENT.INDEX,
		method: 'POST',
		data: createMedicalHistoryData,
		onSuccess: async (response) => {
			const medicalHistoryId = response.data.id
			navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(medicalHistoryId)))
		},
	})

	const getDoctorsByWorkSchedule = useAxiosSubmit({
		url: ApiUrls.WORK_SCHEDULE.ALL_DOCTOR,
		method: 'GET',
	})

	const canCreateMedicalHistory = useMemo(
		() =>
			Boolean(
				createMedicalHistoryData.patientId &&
				createMedicalHistoryData.medicalHistoryType &&
				createMedicalHistoryData.specialtyId &&
				(!isNurse || createMedicalHistoryData.assignedDoctorId)
			),
		[
			createMedicalHistoryData.patientId,
			createMedicalHistoryData.medicalHistoryType,
			createMedicalHistoryData.specialtyId,
			createMedicalHistoryData.assignedDoctorId,
			isNurse,
		]
	)

	const handleCreateMedicalHistoryClick = async () => {
		if (!canCreateMedicalHistory) {
			toast.error(t('error.fill_all_required'))
			return
		}

		await createMedicalHistory.submit()
	}

	const handleDataSectionChange = useCallback((nextValues) => {
		setCreateMedicalHistoryData((prev) => ({ ...prev, ...nextValues }))
	}, [])

	useEffect(() => {
		let active = true

		const loadDoctors = async () => {
			if (!isNurse || !createMedicalHistoryData.specialtyId) {
				setAvailableDoctors([])
				return
			}

			const response = await getDoctorsByWorkSchedule.submit({
				overrideParam: { specialtyId: createMedicalHistoryData.specialtyId },
			})

			if (!active) return
			setAvailableDoctors(response?.data || [])
		}

		loadDoctors()

		return () => {
			active = false
		}
	}, [isNurse, createMedicalHistoryData.specialtyId])

	return (
		<Box sx={{ p: 2, bgcolor: 'gradients.background', minHeight: '100vh' }}>
			<CreateMedicalHistoryHeaderSection />

			<Grid container spacing={2}>
				<Grid size={{ xs: 12, md: 7 }}>
					<CreateMedicalHistorySearchPatientSection
						searchTerm={searchTerm}
						setSearchTerm={setSearchTerm}
						onCreateNewPatientClick={() => setOpenCreatePatientDialog(true)}
					/>
					<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
						<Stack spacing={1.2}>
							<Typography variant='subtitle2'>{t('text.result')}:</Typography>
							<Divider />
							<Stack
								spacing={1.2}
								sx={{
									maxHeight: 500,
									overflowY: 'auto',
									pr: 1,
								}}
							>
								{patients.length === 0 ? (
									<EmptyBox />
								) : (
									patients.map((p) => (
										<CreateMedicalHistoryCardSection
											key={p.id}
											item={p}
											selected={createMedicalHistoryData.patientId === p.id}
											onSelect={() => setCreateMedicalHistoryData((prev) => ({ ...prev, patientId: p.id }))}
										/>
									))
								)}
							</Stack>
						</Stack>
					</Paper>
				</Grid>
				<Grid size={{ xs: 12, md: 5 }}>
					<Stack spacing={2}>
						<CreateMedicalHistoryPatientSummarySection patient={selectedPatient} />
						<CreateMedicalHistoryDataSection
							specialtyOptions={specialtiesStore.data}
							showDoctorField={isNurse}
							availableDoctors={availableDoctors}
							disableSpecialtyField={getDoctorsByWorkSchedule.loading}
							disableDoctorField={
								!createMedicalHistoryData.specialtyId || getDoctorsByWorkSchedule.loading
							}
							medicalHistoryTypeOptions={
								fixedMedicalHistoryType
									? _enum.medicalHistoryTypeOptions.filter(
											(option) => option.value === fixedMedicalHistoryType
										)
									: _enum.medicalHistoryTypeOptions
							}
							defaultType={fixedMedicalHistoryType}
							onChange={handleDataSectionChange}
						/>
					</Stack>
				</Grid>
			</Grid>

			<CreateMedicalHistoryFooterSection
				selectedPatient={selectedPatient}
				createButtonDisabled={!canCreateMedicalHistory}
				onCreateMedicalHistoryClick={handleCreateMedicalHistoryClick}
				onCancelClick={() =>
					navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.INDEX))
				}
				loading={createMedicalHistory.loading}
			/>

			<PatientInfoDialog
				open={openCreatePatientDialog}
				onClose={() => setOpenCreatePatientDialog(false)}
				isEditable={true}
				onSave={async (data) => await createPatient.submit({ overrideData: data })}
				loading={createPatient.loading}
			/>
		</Box>
	)
}

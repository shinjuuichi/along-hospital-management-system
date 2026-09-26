import DoctorInfoDialog from '@/components/dialogs/DoctorInfoDialog'
import PatientInfoDialog from '@/components/dialogs/PatientInfoDialog'
import EmptyPage from '@/components/placeholders/EmptyPage'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import {
	AssignBedDialogMode,
	BedOccupancyManagementRoles,
	BedOccupancyStoreRoles,
} from '@/constants/medicalHistoryConstants'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBedsStore, setRoomsStore } from '@/redux/reducers/managementReducer'
import { Box, Grid, Stack } from '@mui/material'
import { useState } from 'react'
import { Fade, Slide } from 'react-awesome-reveal'
import { useNavigate, useParams } from 'react-router-dom'
import AssignBedDialog from './dialogs/AssignBedDialog'
import CreateComplaintDialog from './dialogs/CreateComplaintDialog'
import CreateMedicalOrderDialog from './dialogs/medicalOrderDialogs/CreateMedicalOrderDialog'
import UpdateMedicalOrderDialog from './dialogs/medicalOrderDialogs/UpdateMedicalOrderDialog'
import RespondComplaintDialog from './dialogs/RespondComplaintDialog'
import UpdateMedicalHistoryDialog from './dialogs/UpdateMedicalHistoryDialog'
import UpsertPrescriptionDialog from './dialogs/UpsertPrescriptionDialog'
import {
	canViewMedicalHistorySection,
	MedicalHistorySectionKey,
} from './helpers/medicalHistoryHelper'
import { isMedicalOrderEditable } from './helpers/medicalOrderHelper'
import MedicalHistoryDetailBedSection from './sections/MedicalHistoryDetailBedSection'
import MedicalHistoryDetailComplaintSection from './sections/MedicalHistoryDetailComplaintSection'
import MedicalHistoryDetailFooterSection from './sections/MedicalHistoryDetailFooterSection'
import MedicalHistoryDetailHeaderInfoSection from './sections/MedicalHistoryDetailHeaderInfoSection'
import MedicalHistoryDetailInvoiceSection from './sections/MedicalHistoryDetailInvoiceSection'
import MedicalHistoryDetailMedicalOrderSection from './sections/MedicalHistoryDetailMedicalOrderSection'
import MedicalHistoryDetailPrescriptionSection from './sections/MedicalHistoryDetailPrescriptionSection'

const MedicalHistoryDetailPage = () => {
	const { id } = useParams()

	const { t } = useTranslation()
	const navigate = useNavigate()
	const confirm = useConfirm()

	const [openPatientInfo, setOpenPatientInfo] = useState(false)
	const [openDoctorInfo, setOpenDoctorInfo] = useState(false)

	const [openUpdateMedicalHistory, setOpenUpdateMedicalHistory] = useState(false)

	const [openCreateComplaint, setOpenCreateComplaint] = useState(false)
	const [openRespondComplaint, setOpenRespondComplaint] = useState(false)

	const [openCreatePrescription, setOpenCreatePrescription] = useState(false)
	const [openUpdatePrescription, setOpenUpdatePrescription] = useState(false)

	const [openCreateMedicalOrder, setOpenCreateMedicalOrder] = useState(false)
	const [openUpdateMedicalOrder, setOpenUpdateMedicalOrder] = useState(false)
	const [selectedMedicalOrderId, setSelectedMedicalOrderId] = useState(null)

	const [openAssignBed, setOpenAssignBed] = useState(false)
	const [openTransferBed, setOpenTransferBed] = useState(false)

	const { auth } = useAuth()
	const role = auth?.role

	const canViewPrescriptionSection = canViewMedicalHistorySection(
		role,
		MedicalHistorySectionKey.Prescription
	)
	const canViewComplaintSection = canViewMedicalHistorySection(
		role,
		MedicalHistorySectionKey.Complaint
	)
	const canViewBedOccupancySection = canViewMedicalHistorySection(
		role,
		MedicalHistorySectionKey.BedOccupancy
	)
	const canViewMedicalOrderSection = canViewMedicalHistorySection(
		role,
		MedicalHistorySectionKey.MedicalOrder
	)
	const canViewInvoiceSection = canViewMedicalHistorySection(role, MedicalHistorySectionKey.Invoice)
	const canSetBedOccupancyStore = BedOccupancyStoreRoles.includes(role)
	const canManageBedOccupancy = BedOccupancyManagementRoles.includes(role)
	const shouldFetchBedOccupancyResourcesDirectly = canManageBedOccupancy && !canSetBedOccupancyStore

	const {
		loading,
		data: medicalHistory,
		setData: setMedicalHistory,
		fetch: refetchMedicalHistory,
	} = useFetch(ApiUrls.MEDICAL_HISTORY.DETAIL(id), {}, [id])

	const {
		loading: loadingRoomsFromStore,
		data: roomsFromStore,
		fetch: refetchRoomsFromStore,
	} = useReduxStore({
		selector: (state) => state.management.rooms,
		setStore: canSetBedOccupancyStore ? setRoomsStore : null,
	})

	const {
		loading: loadingBedsFromStore,
		data: bedsFromStore,
		fetch: refetchBedsFromStore,
	} = useReduxStore({
		selector: (state) => state.management.beds,
		setStore: canSetBedOccupancyStore ? setBedsStore : null,
	})

	const {
		loading: loadingRoomsDirectly,
		data: roomsDirectly,
		fetch: refetchRoomsDirectly,
	} = useFetch(
		setRoomsStore.defaultUrl,
		{},
		[shouldFetchBedOccupancyResourcesDirectly],
		shouldFetchBedOccupancyResourcesDirectly
	)
	const {
		loading: loadingBedsDirectly,
		data: bedsDirectly,
		fetch: refetchBedsDirectly,
	} = useFetch(
		setBedsStore.defaultUrl,
		{},
		[shouldFetchBedOccupancyResourcesDirectly],
		shouldFetchBedOccupancyResourcesDirectly
	)

	const rooms = canSetBedOccupancyStore ? roomsFromStore : roomsDirectly
	const beds = canSetBedOccupancyStore ? bedsFromStore : bedsDirectly
	const loadingRooms = canSetBedOccupancyStore ? loadingRoomsFromStore : loadingRoomsDirectly
	const loadingBeds = canSetBedOccupancyStore ? loadingBedsFromStore : loadingBedsDirectly

	const refreshBedOccupancyResources = async () => {
		const refreshTasks = [refetchMedicalHistory()]

		if (canSetBedOccupancyStore) {
			refreshTasks.unshift(refetchRoomsFromStore(), refetchBedsFromStore())
		} else if (shouldFetchBedOccupancyResourcesDirectly) {
			refreshTasks.unshift(refetchRoomsDirectly(), refetchBedsDirectly())
		}

		await Promise.all(refreshTasks)
	}

	const isInpatientType =
		medicalHistory?.medicalHistoryType === EnumConfig.MedicalHistoryType.Inpatient
	const hasActiveInpatientOccupancy =
		isInpatientType &&
		medicalHistory?.bedOccupancy?.occupancyStatus === EnumConfig.OccupancyStatus.Active

	// API Operations for medical history
	const canUpdateMedicalHistory =
		role === EnumConfig.Role.Doctor &&
		medicalHistory?.doctor?.id === auth?.userId &&
		medicalHistory?.medicalHistoryStatus === EnumConfig.MedicalHistoryStatus.Draft

	const updateMedicalHistory = useAxiosSubmit({
		url: `${ApiUrls.MEDICAL_HISTORY.MANAGEMENT.DETAIL(medicalHistory?.id)}`,
		method: 'PUT',
	})
	const completeMedicalHistory = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.MANAGEMENT.COMPLETE(id),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const dischargeBed = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.MANAGEMENT.DISCHARGE_BED(id),
		method: 'PUT',
		onSuccess: refreshBedOccupancyResources,
	})

	// API Operations for Invoice
	const handlePrintInvoiceClick = (invoiceId) => {
		if (!invoiceId) {
			return
		}

		navigate(routeUrls.HOME.INVOICE_PRINT(invoiceId))
	}

	const handlePrintRefundClick = (invoiceId, chargeId) => {
		if (!invoiceId || !chargeId) {
			return
		}

		navigate(routeUrls.HOME.INVOICE_REFUND_PRINT(invoiceId, chargeId))
	}

	// API Operations for Complaint
	const createComplaint = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.CREATE_COMPLAINT(id),
		method: 'POST',
		onSuccess: (response) => {
			setOpenCreateComplaint(false)
			setMedicalHistory((prev) => ({
				...prev,
				complaint: response.data,
			}))
		},
	})
	const responseAsDraftComplaint = useAxiosSubmit({
		url: ApiUrls.COMPLAINT.MANAGEMENT.DRAFT(medicalHistory?.complaint?.id),
		method: 'PUT',
	})
	const responseAsResolveComplaint = useAxiosSubmit({
		url: ApiUrls.COMPLAINT.MANAGEMENT.RESOLVE(medicalHistory?.complaint?.id),
		method: 'PUT',
	})
	const closeComplaint = useAxiosSubmit({
		url: ApiUrls.COMPLAINT.MANAGEMENT.CLOSE(medicalHistory?.complaint?.id),
		method: 'PUT',
		onSuccess: () => {
			setOpenRespondComplaint(false)
			setMedicalHistory((prev) => ({
				...prev,
				complaint: {
					...prev.complaint,
					complaintResolveStatus: EnumConfig.ComplaintResolveStatus.Closed,
				},
			}))
		},
	})

	// API Operations for Prescription
	const createPrescription = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.MANAGEMENT.PRESCRIPTION(id),
		method: 'POST',
		onSuccess: (response) => {
			setOpenCreatePrescription(false)
			setMedicalHistory((prev) => ({
				...prev,
				prescription: response.data,
			}))
		},
	})
	const updatePrescription = useAxiosSubmit({
		url: ApiUrls.MEDICAL_HISTORY.MANAGEMENT.PRESCRIPTION(id),
		method: 'PUT',
		onSuccess: (response) => {
			setOpenUpdatePrescription(false)
			setMedicalHistory((prev) => ({
				...prev,
				prescription: response.data,
			}))
		},
	})

	//#region API Operations for Medical Order
	// Medical Order
	const createMedicalOrder = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INDEX,
		method: 'POST',
		onSuccess: (response) => {
			const data = response.data
			setOpenCreateMedicalOrder(false)
			setMedicalHistory((prev) => ({
				...prev,
				medicalOrders: [...prev.medicalOrders, data],
			}))
		},
	})
	const createInstructionMedicalOrderAsDraft = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DRAFT,
		method: 'POST',
		onSuccess: (response) => {
			const data = response.data
			setOpenCreateMedicalOrder(false)
			setMedicalHistory((prev) => ({
				...prev,
				medicalOrders: [...prev.medicalOrders, data],
			}))
		},
	})
	const updateInstructionMedicalOrder = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DETAIL.INDEX(selectedMedicalOrderId),
		method: 'PUT',
		onSuccess: (response) => {
			const data = response.data
			setOpenUpdateMedicalOrder(false)
			setMedicalHistory((prev) => ({
				...prev,
				medicalOrders: [...prev.medicalOrders.filter((order) => order.id !== data.id), data],
			}))
		},
	})
	// Clinical Medical Order
	const cancelClinicalMedicalOrder = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.CLINICAL.CANCEL(selectedMedicalOrderId),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const completeClinicalMedicalOrderDetail = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.CLINICAL.DETAIL.COMPLETE(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const failClinicalMedicalOrderDetail = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.CLINICAL.DETAIL.FAIL(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const getInvoiceIdAndChargeIdByClinicalMedicalOrderDetailId = useAxiosSubmit({
		url: ApiUrls.REFUND.INVOICE_CHARGE(),
		method: 'GET',
	})
	// Infusion Medical Order
	const completeInfusionMedicalOrderDetail = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INFUSION.DETAIL.COMPLETE(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const failInfusionMedicalOrderDetail = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INFUSION.DETAIL.FAIL(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	// Instruction Medical Order
	const issueInstructionMedicalOrder = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DETAIL.ISSUE(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	const cancelInstructionMedicalOrder = useAxiosSubmit({
		url: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DETAIL.CANCEL(),
		method: 'PUT',
		onSuccess: () => refetchMedicalHistory(),
	})
	//#endregion

	// API Operations for Bed Occupancy
	const assignBed = useAxiosSubmit({
		url: ApiUrls.BED_OCCUPANCY.ASSIGN,
		method: 'POST',
		onSuccess: async () => {
			setOpenAssignBed(false)
			await refreshBedOccupancyResources()
		},
	})
	const transferBed = useAxiosSubmit({
		url: ApiUrls.BED_OCCUPANCY.TRANSFER,
		method: 'POST',
		onSuccess: async () => {
			setOpenTransferBed(false)
			await refreshBedOccupancyResources()
		},
	})

	// API Operations for Patient Info
	const canUpdatePatientInfo = [EnumConfig.Role.Doctor, EnumConfig.Role.Nurse].includes(role)

	const updatePatientInfo = useAxiosSubmit({
		url: ApiUrls.PATIENT.MANAGEMENT.DETAIL(medicalHistory?.patient.id),
		method: 'PUT',
		onSuccess: (response) => {
			setMedicalHistory((prev) => ({
				...prev,
				patient: response.data,
			}))
		},
	})

	if (!loading && !medicalHistory) {
		return <EmptyPage showButton />
	}

	return (
		<Box sx={{ p: 3 }}>
			<Stack spacing={2}>
				<MedicalHistoryDetailHeaderInfoSection
					medicalHistory={medicalHistory}
					onClickPatientInfo={() => setOpenPatientInfo(true)}
					onClickDoctorInfo={() => setOpenDoctorInfo(true)}
					loading={loading}
				/>

				{(canViewPrescriptionSection || canViewComplaintSection) && (
					<Grid container spacing={2}>
						{canViewPrescriptionSection && (
							<Grid size={{ xs: 12, md: canViewComplaintSection ? 6 : 12 }}>
								<Slide direction='left' triggerOnce>
									<MedicalHistoryDetailPrescriptionSection
										prescription={medicalHistory?.prescription}
										medicalHistoryStatus={medicalHistory?.medicalHistoryStatus}
										role={role}
										loading={loading}
										onClickCreatePrescription={() => setOpenCreatePrescription(true)}
										onClickUpdatePrescription={() => setOpenUpdatePrescription(true)}
										onClickPrintPrescription={() =>
											navigate(routeUrls.HOME.PRESCRIPTION_PRINT(medicalHistory.id))
										}
									/>
								</Slide>
							</Grid>
						)}

						{canViewComplaintSection && (
							<Grid size={{ xs: 12, md: canViewPrescriptionSection ? 6 : 12 }}>
								<Slide direction='right' triggerOnce>
									<MedicalHistoryDetailComplaintSection
										complaint={medicalHistory?.complaint}
										role={role}
										loading={loading}
										onClickCreateComplaint={() => setOpenCreateComplaint(true)}
										onClickRespondComplaint={() => setOpenRespondComplaint(true)}
									/>
								</Slide>
							</Grid>
						)}
					</Grid>
				)}

				{isInpatientType && canViewBedOccupancySection && (
					<Fade direction='up' triggerOnce>
						<MedicalHistoryDetailBedSection
							bedOccupancy={medicalHistory?.bedOccupancy}
							bedOccupancies={medicalHistory?.bedOccupancies}
							medicalHistoryStatus={medicalHistory?.medicalHistoryStatus}
							dischargeDate={medicalHistory?.dischargeDate}
							role={role}
							loading={loading}
							loadingAction={assignBed.loading || transferBed.loading || dischargeBed.loading}
							onClickAssignBed={() => setOpenAssignBed(true)}
							onClickDischargeBed={async () => await dischargeBed.submit()}
							onClickTransferBed={() => setOpenTransferBed(true)}
						/>
					</Fade>
				)}

				{canViewMedicalOrderSection && (
					<Fade triggerOnce>
						<MedicalHistoryDetailMedicalOrderSection
							medicalOrders={medicalHistory?.medicalOrders}
							role={role}
							loading={loading}
							readOnly={!isMedicalOrderEditable(medicalHistory, role)}
							// Medical Order actions
							onClickCreateMedicalOrder={() => setOpenCreateMedicalOrder(true)}
							onClickUpdateInstructionMedicalOrder={(medicalOrderId) => {
								setSelectedMedicalOrderId(medicalOrderId)
								setOpenUpdateMedicalOrder(true)
							}}
							// Clinical Medical Order actions
							onClickCancelClinicalMedicalOrder={(medicalOrderId) =>
								cancelClinicalMedicalOrder.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.CLINICAL.CANCEL(medicalOrderId),
								})
							}
							onClickCompleteClinicalMedicalOrderDetail={(medicalOrderId, medicalServiceId) =>
								completeClinicalMedicalOrderDetail.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.CLINICAL.DETAIL.COMPLETE(
										medicalOrderId,
										medicalServiceId
									),
								})
							}
							onClickFailClinicalMedicalOrderDetail={(medicalOrderId, medicalServiceId, failReason) =>
								failClinicalMedicalOrderDetail.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.CLINICAL.DETAIL.FAIL(medicalOrderId, medicalServiceId),
									overrideData: { reason: failReason },
								})
							}
							onClickPrintRefundClinicalMedicalOrderDetail={async (clinicalMedicalOrderDetailId) => {
								const response = await getInvoiceIdAndChargeIdByClinicalMedicalOrderDetailId.submit({
									overrideUrl: ApiUrls.REFUND.INVOICE_CHARGE(clinicalMedicalOrderDetailId),
								})

								if (response?.data) {
									const { invoiceId, chargeId } = response.data
									navigate(routeUrls.HOME.INVOICE_REFUND_PRINT(invoiceId, chargeId))
								}
							}}
							// Infusion Medical Order actions
							onClickCompleteInfusionMedicalOrderDetail={(medicalOrderId, medicineId) =>
								completeInfusionMedicalOrderDetail.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.INFUSION.DETAIL.COMPLETE(medicalOrderId, medicineId),
								})
							}
							onClickFailInfusionMedicalOrderDetail={(medicalOrderId, medicineId) =>
								failInfusionMedicalOrderDetail.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.INFUSION.DETAIL.FAIL(medicalOrderId, medicineId),
								})
							}
							// Instruction Medical Order actions
							onClickIssueInstructionMedicalOrder={(medicalOrderId) =>
								issueInstructionMedicalOrder.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DETAIL.ISSUE(medicalOrderId),
								})
							}
							onClickCancelInstructionMedicalOrder={(medicalOrderId) =>
								cancelInstructionMedicalOrder.submit({
									overrideUrl: ApiUrls.MEDICAL_ORDER.INSTRUCTION.DETAIL.CANCEL(medicalOrderId),
								})
							}
						/>
					</Fade>
				)}

				{canViewInvoiceSection && (
					<Fade triggerOnce>
						<MedicalHistoryDetailInvoiceSection
							invoices={medicalHistory?.invoices}
							loading={loading}
							onPrintInvoiceClick={handlePrintInvoiceClick}
							onPrintRefundClick={handlePrintRefundClick}
						/>
					</Fade>
				)}

				{medicalHistory && (
					<MedicalHistoryDetailFooterSection
						canUpdateMedicalHistory={canUpdateMedicalHistory}
						hasActiveInpatientOccupancy={hasActiveInpatientOccupancy}
						onClickUpdateMedicalHistory={() => setOpenUpdateMedicalHistory(true)}
						onClickCompleteMedicalHistory={async () => await completeMedicalHistory.submit()}
					/>
				)}
			</Stack>

			{/* Patient and doctor info dialogs */}
			{medicalHistory?.patient && (
				<PatientInfoDialog
					open={openPatientInfo}
					onClose={() => setOpenPatientInfo(false)}
					patientInfo={medicalHistory?.patient}
					loading={updatePatientInfo.loading}
					isEditable={canUpdatePatientInfo}
					onSave={async (values) => await updatePatientInfo.submit({ overrideData: values })}
				/>
			)}
			{medicalHistory?.doctor && (
				<DoctorInfoDialog
					open={openDoctorInfo}
					onClose={() => setOpenDoctorInfo(false)}
					doctorInfo={medicalHistory?.doctor}
				/>
			)}

			{/* Medical History Dialog */}
			<UpdateMedicalHistoryDialog
				open={openUpdateMedicalHistory}
				onClose={() => setOpenUpdateMedicalHistory(false)}
				medicalHistory={medicalHistory}
				onSubmit={async (values) => {
					const response = await updateMedicalHistory.submit({ overrideData: values })
					if (response) {
						setOpenUpdateMedicalHistory(false)
						setMedicalHistory((prev) => ({
							...prev,
							diagnosis: values.diagnosis,
							followUpAppointmentDate: values.followUpAppointmentDate,
						}))
					}
				}}
			/>

			{/* Complaint Dialog */}
			<CreateComplaintDialog
				open={openCreateComplaint}
				onClose={() => setOpenCreateComplaint(false)}
				onSubmit={async (values) => await createComplaint.submit({ overrideData: values })}
			/>
			<RespondComplaintDialog
				open={openRespondComplaint}
				onClose={() => setOpenRespondComplaint(false)}
				initialResponse={medicalHistory?.complaint?.response}
				onSaveDraft={async (values) => {
					const response = await responseAsDraftComplaint.submit({ overrideData: values })
					if (response) {
						setOpenRespondComplaint(false)
						setMedicalHistory((prev) => ({
							...prev,
							complaint: {
								...prev.complaint,
								response: values.response,
								complaintResolveStatus: EnumConfig.ComplaintResolveStatus.Draft,
							},
						}))
					}
				}}
				onCloseComplaint={async () => {
					const isConfirmed = await confirm({
						title: t('complaint.dialog.confirm.close_complaint_title'),
						description: t('complaint.dialog.confirm.close_complaint_description'),
						confirmColor: 'error',
						confirmText: t('button.close'),
					})

					if (isConfirmed) {
						await closeComplaint.submit()
					}
				}}
				onSubmit={async (values) => {
					const response = await responseAsResolveComplaint.submit({ overrideData: values })
					if (response) {
						setOpenRespondComplaint(false)
						setMedicalHistory((prev) => ({
							...prev,
							complaint: {
								...prev.complaint,
								response: values.response,
								complaintResolveStatus: EnumConfig.ComplaintResolveStatus.Resolved,
							},
						}))
					}
				}}
			/>

			{/* Prescription Dialog */}
			{openCreatePrescription && (
				<UpsertPrescriptionDialog
					open={openCreatePrescription}
					onClose={() => setOpenCreatePrescription(false)}
					onSubmit={async (values) => await createPrescription.submit({ overrideData: values })}
				/>
			)}
			{openUpdatePrescription && medicalHistory?.prescription && (
				<UpsertPrescriptionDialog
					open={openUpdatePrescription}
					onClose={() => setOpenUpdatePrescription(false)}
					initialValues={medicalHistory?.prescription}
					onSubmit={async (values) => await updatePrescription.submit({ overrideData: values })}
				/>
			)}

			{/* Medical Order Dialog */}
			{openCreateMedicalOrder && (
				<CreateMedicalOrderDialog
					open={openCreateMedicalOrder}
					onClose={() => setOpenCreateMedicalOrder(false)}
					onSubmit={async (values) =>
						await createMedicalOrder.submit({
							overrideData: { ...values, medicalHistoryId: medicalHistory?.id },
						})
					}
					onCreateInstructionMedicalOrderAsDraft={async (values) =>
						await createInstructionMedicalOrderAsDraft.submit({
							overrideData: { ...values, medicalHistoryId: medicalHistory?.id },
						})
					}
				/>
			)}
			{openUpdateMedicalOrder && selectedMedicalOrderId && (
				<UpdateMedicalOrderDialog
					open={openUpdateMedicalOrder}
					onClose={() => setOpenUpdateMedicalOrder(false)}
					medicalOrder={medicalHistory?.medicalOrders?.find((mo) => mo.id === selectedMedicalOrderId)}
					onSubmit={async (values) =>
						await updateInstructionMedicalOrder.submit({ overrideData: values })
					}
				/>
			)}

			{isInpatientType && (
				<AssignBedDialog
					open={openAssignBed}
					onClose={() => setOpenAssignBed(false)}
					loading={assignBed.loading}
					medicalHistoryId={medicalHistory?.id}
					specialtyId={medicalHistory?.specialtyId}
					rooms={rooms || []}
					beds={beds || []}
					loadingRooms={loadingRooms}
					loadingBeds={loadingBeds}
					onSubmit={async (values) => await assignBed.submit({ overrideData: values })}
				/>
			)}

			{isInpatientType && (
				<AssignBedDialog
					open={openTransferBed}
					onClose={() => setOpenTransferBed(false)}
					loading={transferBed.loading}
					mode={AssignBedDialogMode.Transfer}
					medicalHistoryId={medicalHistory?.id}
					specialtyId={medicalHistory?.specialtyId}
					rooms={rooms || []}
					beds={beds || []}
					loadingRooms={loadingRooms}
					loadingBeds={loadingBeds}
					currentBedId={medicalHistory?.bedOccupancy?.bedId}
					currentBedCode={medicalHistory?.bedOccupancy?.bed?.code}
					onSubmit={async (values) => await transferBed.submit({ overrideData: values })}
				/>
			)}
		</Box>
	)
}

export default MedicalHistoryDetailPage

import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from './useTranslation'

export default function useEnum() {
	const { t } = useTranslation()

	return {
		// For boolean
		booleanOptions: [
			{ value: true, label: t('text.yes') },
			{ value: false, label: t('text.no') },
		],

		// For users
		genderOptions: [
			{ value: EnumConfig.Gender.Male, label: t('enum.gender.male') },
			{ value: EnumConfig.Gender.Female, label: t('enum.gender.female') },
			{ value: EnumConfig.Gender.Other, label: t('enum.gender.other') },
		],
		bloodTypeOptions: [
			{ value: EnumConfig.BloodType.Unknown, label: t('enum.blood_type.unknown') },
			{ value: EnumConfig.BloodType.O, label: t('enum.blood_type.o') },
			{ value: EnumConfig.BloodType.A, label: t('enum.blood_type.a') },
			{ value: EnumConfig.BloodType.B, label: t('enum.blood_type.b') },
			{ value: EnumConfig.BloodType.AB, label: t('enum.blood_type.ab') },
			{ value: EnumConfig.BloodType.Rare, label: t('enum.blood_type.rare') },
		],
		severityLevelOptions: [
			{ value: EnumConfig.SeverityLevel.Mild, label: t('enum.severity_level.mild') },
			{ value: EnumConfig.SeverityLevel.Moderate, label: t('enum.severity_level.moderate') },
			{ value: EnumConfig.SeverityLevel.Severe, label: t('enum.severity_level.severe') },
		],
		roleOptions: [
			{ value: EnumConfig.Role.Manager, label: t('enum.role.manager') },
			{ value: EnumConfig.Role.Patient, label: t('enum.role.patient') },
			{ value: EnumConfig.Role.Doctor, label: t('enum.role.doctor') },
			{ value: EnumConfig.Role.Nurse, label: t('enum.role.nurse') },
			{ value: EnumConfig.Role.HR, label: t('enum.role.hr') },
			{ value: EnumConfig.Role.Pharmacist, label: t('enum.role.pharmacist') },
			{ value: EnumConfig.Role.Accountant, label: t('enum.role.accountant') },
			{ value: EnumConfig.Role.Marketer, label: t('enum.role.marketer') },
			{ value: EnumConfig.Role.Receptionist, label: t('enum.role.receptionist') },
			{ value: EnumConfig.Role.HotlineAgent, label: t('enum.role.hotline_agent') },
			{ value: EnumConfig.Role.InventoryClerk, label: t('enum.role.inventory_clerk') },
		],
		jobPostingManagementStatusOptions: [
			{ value: EnumConfig.JobPostingStatus.Draft, label: t('job_posting.status.draft') },
			{ value: EnumConfig.JobPostingStatus.Open, label: t('job_posting.status.open') },
			{ value: EnumConfig.JobPostingStatus.Closed, label: t('job_posting.status.closed') },
		],
		employmentTypeOptions: [
			{ value: EnumConfig.EmploymentTypeEnum.FullTime, label: t('enum.employment_type.full_time') },
			{ value: EnumConfig.EmploymentTypeEnum.PartTime, label: t('enum.employment_type.part_time') },
			{ value: EnumConfig.EmploymentTypeEnum.Internship, label: t('enum.employment_type.internship') },
		],

		// Appointment
		appointmentStatusOptions: [
			{ value: EnumConfig.AppointmentStatus.Scheduled, label: t('enum.appointment_status.scheduled') },
			{ value: EnumConfig.AppointmentStatus.Completed, label: t('enum.appointment_status.completed') },
			{ value: EnumConfig.AppointmentStatus.Cancelled, label: t('enum.appointment_status.cancelled') },
		],
		appointmentPaymentStatusOptions: [
			{
				value: EnumConfig.AppointmentPaymentStatus.Pending,
				label: t('enum.appointment_payment_status.pending'),
			},
			{
				value: EnumConfig.AppointmentPaymentStatus.Completed,
				label: t('enum.appointment_payment_status.completed'),
			},
			{
				value: EnumConfig.AppointmentPaymentStatus.Failed,
				label: t('enum.appointment_payment_status.failed'),
			},
		],
		appointmentMeetingTypeOptions: [
			{
				value: EnumConfig.AppointmentMeetingType.InPerson,
				label: t('enum.appointment_meeting_type.in_person'),
			},
			{
				value: EnumConfig.AppointmentMeetingType.Telehealth,
				label: t('enum.appointment_meeting_type.telehealth'),
			},
		],

		// Medical History
		medicalHistoryStatusOptions: [
			{
				value: EnumConfig.MedicalHistoryStatus.PendingPayment,
				label: t('enum.medical_history_status.pending_payment'),
			},
			{ value: EnumConfig.MedicalHistoryStatus.Draft, label: t('enum.medical_history_status.draft') },
			{
				value: EnumConfig.MedicalHistoryStatus.Completed,
				label: t('enum.medical_history_status.completed'),
			},
			{
				value: EnumConfig.MedicalHistoryStatus.Cancelled,
				label: t('enum.medical_history_status.cancelled'),
			},
		],
		medicalHistoryTypeOptions: [
			{
				value: EnumConfig.MedicalHistoryType.Outpatient,
				label: t('enum.medical_history_type.outpatient'),
			},
			{
				value: EnumConfig.MedicalHistoryType.Inpatient,
				label: t('enum.medical_history_type.inpatient'),
			},
		],

		// Medical order
		medicalOrderTypeOptions: [
			{
				value: EnumConfig.MedicalOrderType.Clinical,
				label: t('enum.medical_order_type.clinical'),
			},
			{
				value: EnumConfig.MedicalOrderType.Infusion,
				label: t('enum.medical_order_type.infusion'),
			},
			{
				value: EnumConfig.MedicalOrderType.Instruction,
				label: t('enum.medical_order_type.instruction'),
			},
		],
		clinicalOrderStatusOptions: [
			{
				value: EnumConfig.ClinicalMedicalOrderStatus.Pending,
				label: t('enum.clinical_medical_order_status.pending'),
			},
			{
				value: EnumConfig.ClinicalMedicalOrderStatus.Paid,
				label: t('enum.clinical_medical_order_status.paid'),
			},
			{
				value: EnumConfig.ClinicalMedicalOrderStatus.Cancelled,
				label: t('enum.clinical_medical_order_status.cancelled'),
			},
		],
		clinicalOrderDetailStatusOptions: [
			{
				value: EnumConfig.ClinicalMedicalOrderDetailStatus.Pending,
				label: t('enum.clinical_medical_order_detail_status.pending'),
			},
			{
				value: EnumConfig.ClinicalMedicalOrderDetailStatus.Completed,
				label: t('enum.clinical_medical_order_detail_status.completed'),
			},
			{
				value: EnumConfig.ClinicalMedicalOrderDetailStatus.Failed,
				label: t('enum.clinical_medical_order_detail_status.failed'),
			},
		],
		infusionMedicalOrderDetailExecutionStatusOptions: [
			{
				value: EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Pending,
				label: t('enum.infusion_medical_order_detail_execution_status.pending'),
			},
			{
				value: EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Completed,
				label: t('enum.infusion_medical_order_detail_execution_status.completed'),
			},
			{
				value: EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Failed,
				label: t('enum.infusion_medical_order_detail_execution_status.failed'),
			},
		],
		instructionMedicalOrderStatusOptions: [
			{
				value: EnumConfig.InstructionMedicalOrderStatus.Draft,
				label: t('enum.instruction_medical_order_status.draft'),
			},
			{
				value: EnumConfig.InstructionMedicalOrderStatus.Issued,
				label: t('enum.instruction_medical_order_status.issued'),
			},
			{
				value: EnumConfig.InstructionMedicalOrderStatus.Cancelled,
				label: t('enum.instruction_medical_order_status.cancelled'),
			},
		],
		nursingCareOrderLevelOptions: [
			{
				value: EnumConfig.NursingCareOrderLevel.Level1,
				label: t('enum.nursing_care_order_level.level_1'),
			},
			{
				value: EnumConfig.NursingCareOrderLevel.Level2,
				label: t('enum.nursing_care_order_level.level_2'),
			},
			{
				value: EnumConfig.NursingCareOrderLevel.Level3,
				label: t('enum.nursing_care_order_level.level_3'),
			},
		],
		nutritionOrderTypeOptions: [
			{
				value: EnumConfig.NutritionOrderType.BreastMilk,
				label: t('enum.nutrition_order_type.breast_milk'),
			},
			{
				value: EnumConfig.NutritionOrderType.FormulaMilk,
				label: t('enum.nutrition_order_type.formula_milk'),
			},
			{
				value: EnumConfig.NutritionOrderType.Porridge,
				label: t('enum.nutrition_order_type.porridge'),
			},
			{
				value: EnumConfig.NutritionOrderType.SoftRice,
				label: t('enum.nutrition_order_type.soft_rice'),
			},
			{
				value: EnumConfig.NutritionOrderType.RegularDiet,
				label: t('enum.nutrition_order_type.regular_diet'),
			},
			{ value: EnumConfig.NutritionOrderType.Npo, label: t('enum.nutrition_order_type.npo') },
			{ value: EnumConfig.NutritionOrderType.Other, label: t('enum.nutrition_order_type.other') },
		],
		positionOrderTypeOptions: [
			{ value: EnumConfig.PositionOrderType.Supine, label: t('enum.position_order_type.supine') },
			{
				value: EnumConfig.PositionOrderType.Trendelenburg,
				label: t('enum.position_order_type.trendelenburg'),
			},
			{
				value: EnumConfig.PositionOrderType.HeadElevated30,
				label: t('enum.position_order_type.head_elevated_30'),
			},
			{
				value: EnumConfig.PositionOrderType.HeadElevated45,
				label: t('enum.position_order_type.head_elevated_45'),
			},
			{ value: EnumConfig.PositionOrderType.Other, label: t('enum.position_order_type.other') },
		],
		respiratorySupportOrderTypeOptions: [
			{
				value: EnumConfig.RespiratorySupportOrderType.RoomAir,
				label: t('enum.respiratory_support_order_type.room_air'),
			},
			{
				value: EnumConfig.RespiratorySupportOrderType.OxygenNasalCannula,
				label: t('enum.respiratory_support_order_type.oxygen_nasal_cannula'),
			},
			{
				value: EnumConfig.RespiratorySupportOrderType.OxygenMask,
				label: t('enum.respiratory_support_order_type.oxygen_mask'),
			},
			{
				value: EnumConfig.RespiratorySupportOrderType.EndotrachealIntubation,
				label: t('enum.respiratory_support_order_type.endotracheal_intubation'),
			},
			{
				value: EnumConfig.RespiratorySupportOrderType.Tracheostomy,
				label: t('enum.respiratory_support_order_type.tracheostomy'),
			},
			{
				value: EnumConfig.RespiratorySupportOrderType.Other,
				label: t('enum.respiratory_support_order_type.other'),
			},
		],

		// Invoice
		invoiceStatusOptions: [
			{ value: EnumConfig.InvoiceStatus.Pending, label: t('enum.invoice_status.pending') },
			{ value: EnumConfig.InvoiceStatus.Completed, label: t('enum.invoice_status.completed') },
			{ value: EnumConfig.InvoiceStatus.Cancelled, label: t('enum.invoice_status.cancelled') },
		],
		chargeTypeOptions: [
			{ value: EnumConfig.ChargeType.Invoice, label: t('enum.charge_type.invoice') },
			{ value: EnumConfig.ChargeType.Refund, label: t('enum.charge_type.refund') },
		],
		refundStatusOptions: [
			{ value: EnumConfig.RefundStatus.Pending, label: t('enum.refund_status.pending') },
			{ value: EnumConfig.RefundStatus.Approved, label: t('enum.refund_status.approved') },
			{ value: EnumConfig.RefundStatus.Cancelled, label: t('enum.refund_status.cancelled') },
		],

		// Queue
		queueStatusOptions: [
			{ value: EnumConfig.QueueStatus.Waiting, label: t('enum.queue_status.waiting') },
			{ value: EnumConfig.QueueStatus.Called, label: t('enum.queue_status.called') },
			{ value: EnumConfig.QueueStatus.InProgress, label: t('enum.queue_status.in_progress') },
			{
				value: EnumConfig.QueueStatus.AwaitingResults,
				label: t('enum.queue_status.awaiting_results'),
			},
			{ value: EnumConfig.QueueStatus.Completed, label: t('enum.queue_status.completed') },
			{ value: EnumConfig.QueueStatus.Cancelled, label: t('enum.queue_status.cancelled') },
		],

		// Complaint
		complaintTopicOptions: [
			{ value: EnumConfig.ComplaintTopic.Service, label: t('enum.complaint_topic.service') },
			{ value: EnumConfig.ComplaintTopic.Billing, label: t('enum.complaint_topic.billing') },
			{ value: EnumConfig.ComplaintTopic.Doctor, label: t('enum.complaint_topic.doctor') },
			{ value: EnumConfig.ComplaintTopic.Medicine, label: t('enum.complaint_topic.medicine') },
			{ value: EnumConfig.ComplaintTopic.Others, label: t('enum.complaint_topic.others') },
		],
		complaintTypeOptions: [
			{
				value: EnumConfig.ComplaintType.Neutral,
				label: t('enum.complaint_type.neutral'),
			},
			{
				value: EnumConfig.ComplaintType.Positive,
				label: t('enum.complaint_type.positive'),
			},
			{
				value: EnumConfig.ComplaintType.Negative,
				label: t('enum.complaint_type.negative'),
			},
		],
		complaintResolveStatusOptions: [
			{
				value: EnumConfig.ComplaintResolveStatus.Pending,
				label: t('enum.complaint_resolve_status.pending'),
			},
			{
				value: EnumConfig.ComplaintResolveStatus.Draft,
				label: t('enum.complaint_resolve_status.draft'),
			},
			{
				value: EnumConfig.ComplaintResolveStatus.Resolved,
				label: t('enum.complaint_resolve_status.resolved'),
			},
			{
				value: EnumConfig.ComplaintResolveStatus.Closed,
				label: t('enum.complaint_resolve_status.closed'),
			},
		],

		// For vouchers
		voucherStatusOptions: [
			{ value: EnumConfig.VoucherStatus.Active, label: t('enum.voucher_status.active') },
			{ value: EnumConfig.VoucherStatus.Expired, label: t('enum.voucher_status.expired') },
		],
		voucherTypeOptions: [
			{ value: EnumConfig.VoucherType.Patient, label: t('enum.voucher_type.patient') },
			{ value: EnumConfig.VoucherType.Medicine, label: t('enum.voucher_type.medicine') },
		],
		voucherDiscountTypeOptions: [
			{
				value: EnumConfig.VoucherDiscountType.Percentage,
				label: t('enum.voucher_discount_type.percentage'),
			},
			{
				value: EnumConfig.VoucherDiscountType.FixedAmount,
				label: t('enum.voucher_discount_type.fixed_amount'),
			},
		],

		//For Order
		paymentTypeOptions: [
			{ value: EnumConfig.PaymentType.PayOS, label: t('enum.payment_type.pay_os') },
			{ value: EnumConfig.PaymentType.SePay, label: t('enum.payment_type.se_pay') },
			{ value: EnumConfig.PaymentType.Cash, label: t('enum.payment_type.cash') },
		],
		orderStatusOptions: [
			{ value: EnumConfig.OrderStatus.Unpaid, label: t('enum.order_status.unpaid') },
			{ value: EnumConfig.OrderStatus.Paid, label: t('enum.order_status.paid') },
			{ value: EnumConfig.OrderStatus.Shipping, label: t('enum.order_status.shipping') },
			{ value: EnumConfig.OrderStatus.Completed, label: t('enum.order_status.completed') },
			{ value: EnumConfig.OrderStatus.Cancelled, label: t('enum.order_status.cancelled') },
		],

		//For Feedbacks
		feedbackReportStatusEnum: [
			{
				value: EnumConfig.FeedbackReportStatus.Pending,
				label: t('enum.feedback_report_status.pending'),
			},
			{
				value: EnumConfig.FeedbackReportStatus.Rejected,
				label: t('enum.feedback_report_status.rejected'),
			},
			{
				value: EnumConfig.FeedbackReportStatus.Resolved,
				label: t('enum.feedback_report_status.resolved'),
			},
		],
		feedbackReplyStatusOptions: [
			{
				value: EnumConfig.FeedbackReplyStatus.WaitingStaff,
				label: t('enum.feedback_reply_status.waiting_staff'),
			},
			{
				value: EnumConfig.FeedbackReplyStatus.Replied,
				label: t('enum.feedback_reply_status.replied'),
			},
		],

		attendanceLogTypeOptions: [
			{
				value: EnumConfig.AttendanceLogType.CheckIn,
				label: t('enum.attendance_log_type.check_in'),
			},
			{
				value: EnumConfig.AttendanceLogType.CheckOut,
				label: t('enum.attendance_log_type.check_out'),
			},
		],

		// For Leave Request
		leaveTypeOptions: [
			{ value: EnumConfig.LeaveType.Annual, label: t('enum.leave_type.annual') },
			{ value: EnumConfig.LeaveType.Sick, label: t('enum.leave_type.sick') },
			{ value: EnumConfig.LeaveType.Maternity, label: t('enum.leave_type.maternity') },
			{ value: EnumConfig.LeaveType.Personal, label: t('enum.leave_type.personal') },
			{ value: EnumConfig.LeaveType.Other, label: t('enum.leave_type.other') },
		],
		leaveUnitOptions: [
			{ value: EnumConfig.LeaveUnit.Day, label: t('enum.leave_unit.day') },
			{ value: EnumConfig.LeaveUnit.Shift, label: t('enum.leave_unit.shift') },
		],
		overtimeTypeOptions: [
			{ value: EnumConfig.OvertimeType.AfterShift, label: t('enum.overtime_type.after_shift') },
			{ value: EnumConfig.OvertimeType.Holiday, label: t('enum.overtime_type.holiday') },
		],
		leaveRequestStatusOptions: [
			{ value: EnumConfig.LeaveRequestStatus.Pending, label: t('enum.leave_request_status.pending') },
			{
				value: EnumConfig.LeaveRequestStatus.Approved,
				label: t('enum.leave_request_status.approved'),
			},
			{
				value: EnumConfig.LeaveRequestStatus.Rejected,
				label: t('enum.leave_request_status.rejected'),
			},
			{
				value: EnumConfig.LeaveRequestStatus.Canceled,
				label: t('enum.leave_request_status.canceled'),
			},
		],
		salaryAdvanceStatusOptions: [
			{
				value: EnumConfig.SalaryAdvanceStatus.Pending,
				label: t('enum.salary_advance_status.pending'),
			},
			{
				value: EnumConfig.SalaryAdvanceStatus.Approved,
				label: t('enum.salary_advance_status.approved'),
			},
			{
				value: EnumConfig.SalaryAdvanceStatus.Rejected,
				label: t('enum.salary_advance_status.rejected'),
			},
			{
				value: EnumConfig.SalaryAdvanceStatus.Cancelled,
				label: t('enum.salary_advance_status.cancelled'),
			},
			{
				value: EnumConfig.SalaryAdvanceStatus.Disbursed,
				label: t('enum.salary_advance_status.disbursed'),
			},
		],

		medicineStatusOptions: [
			{ value: EnumConfig.MedicineStatus.Draft, label: t('enum.medicine_status.draft') },
			{ value: EnumConfig.MedicineStatus.Active, label: t('enum.medicine_status.active') },
			{ value: EnumConfig.MedicineStatus.Inactive, label: t('enum.medicine_status.inactive') },
		],
		//For Staff
		staffStatusOptions: [
			{ value: EnumConfig.StaffStatus.Active, label: t('enum.staff_status.active') },
			{ value: EnumConfig.StaffStatus.OnLeave, label: t('enum.staff_status.on_leave') },
			{ value: EnumConfig.StaffStatus.Terminated, label: t('enum.staff_status.terminated') },
			{ value: EnumConfig.StaffStatus.Suspended, label: t('enum.staff_status.suspended') },
		],
		bankCodeOptions: [
			{ value: EnumConfig.BankCode.MBBank, label: t('enum.bank_code.mb_bank') },
			{ value: EnumConfig.BankCode.Vietcombank, label: t('enum.bank_code.vietcombank') },
			{ value: EnumConfig.BankCode.Techcombank, label: t('enum.bank_code.techcombank') },
			{ value: EnumConfig.BankCode.BIDV, label: t('enum.bank_code.bidv') },
			{ value: EnumConfig.BankCode.VietinBank, label: t('enum.bank_code.vietin_bank') },
			{ value: EnumConfig.BankCode.ACB, label: t('enum.bank_code.acb') },
			{ value: EnumConfig.BankCode.TPBank, label: t('enum.bank_code.tp_bank') },
			{ value: EnumConfig.BankCode.VPBank, label: t('enum.bank_code.vp_bank') },
			{ value: EnumConfig.BankCode.Agribank, label: t('enum.bank_code.agribank') },
			{ value: EnumConfig.BankCode.MSB, label: t('enum.bank_code.msb') },
			{ value: EnumConfig.BankCode.OCB, label: t('enum.bank_code.ocb') },
			{ value: EnumConfig.BankCode.KienlongBank, label: t('enum.bank_code.kienlong_bank') },
			{ value: EnumConfig.BankCode.Eximbank, label: t('enum.bank_code.eximbank') },
			{ value: EnumConfig.BankCode.HDBank, label: t('enum.bank_code.hd_bank') },
			{ value: EnumConfig.BankCode.Sacombank, label: t('enum.bank_code.sacombank') },
			{ value: EnumConfig.BankCode.VIB, label: t('enum.bank_code.vib') },
			{ value: EnumConfig.BankCode.ABBank, label: t('enum.bank_code.ab_bank') },
			{ value: EnumConfig.BankCode.LPBank, label: t('enum.bank_code.lp_bank') },
			{ value: EnumConfig.BankCode.BacABank, label: t('enum.bank_code.bac_a_bank') },
			{ value: EnumConfig.BankCode.SeABank, label: t('enum.bank_code.sea_bank') },
			{ value: EnumConfig.BankCode.SHB, label: t('enum.bank_code.shb') },
			{ value: EnumConfig.BankCode.NCB, label: t('enum.bank_code.ncb') },
			{ value: EnumConfig.BankCode.WooriBank, label: t('enum.bank_code.woori_bank') },
			{ value: EnumConfig.BankCode.VietABank, label: t('enum.bank_code.viet_a_bank') },
			{ value: EnumConfig.BankCode.VietBank, label: t('enum.bank_code.viet_bank') },
			{ value: EnumConfig.BankCode.NamABank, label: t('enum.bank_code.nam_a_bank') },
			{ value: EnumConfig.BankCode.PGBank, label: t('enum.bank_code.pg_bank') },
			{ value: EnumConfig.BankCode.PublicBank, label: t('enum.bank_code.public_bank') },
		],

		// For Job Application
		jobApplicationStatusOptions: [
			{ value: EnumConfig.JobApplicationStatus.Applied, label: t('job_application.status.applied') },
			{
				value: EnumConfig.JobApplicationStatus.Interviewing,
				label: t('job_application.status.interviewing'),
			},
			{ value: EnumConfig.JobApplicationStatus.Passed, label: t('job_application.status.passed') },
			{ value: EnumConfig.JobApplicationStatus.Failed, label: t('job_application.status.failed') },
		],
		interviewResultOptions: [
			{ value: EnumConfig.InterviewResult.Pending, label: t('interview.result.pending') },
			{ value: EnumConfig.InterviewResult.Passed, label: t('interview.result.passed') },
			{ value: EnumConfig.InterviewResult.Failed, label: t('interview.result.failed') },
			{ value: EnumConfig.InterviewResult.Cancelled, label: t('interview.result.cancelled') },
		],

		// For Staff Contract
		contractTypeOptions: [
			{ value: EnumConfig.ContractType.Probation, label: t('enum.contract_type.probation') },
			{ value: EnumConfig.ContractType.FixedTerm, label: t('enum.contract_type.fixed_term') },
			{ value: EnumConfig.ContractType.Indefinite, label: t('enum.contract_type.indefinite') },
			{ value: EnumConfig.ContractType.Internship, label: t('enum.contract_type.internship') },
		],
		staffContractStatusOptions: [
			{ value: EnumConfig.StaffContractStatus.Active, label: t('enum.staff_contract_status.active') },
			{
				value: EnumConfig.StaffContractStatus.Expired,
				label: t('enum.staff_contract_status.expired'),
			},
			{
				value: EnumConfig.StaffContractStatus.Terminated,
				label: t('enum.staff_contract_status.terminated'),
			},
		],

		//For InPatient Resource
		bedStatusOptions: [
			{ value: EnumConfig.BedStatus.Active, label: t('enum.bed_status.active') },
			{ value: EnumConfig.BedStatus.Maintenance, label: t('enum.bed_status.maintenance') },
			{ value: EnumConfig.BedStatus.Occupied, label: t('enum.bed_status.occupied') },
		],
		roomStatusOptions: [
			{ value: EnumConfig.RoomStatus.Active, label: t('enum.room_status.active') },
			{ value: EnumConfig.RoomStatus.Maintenance, label: t('enum.room_status.maintenance') },
			{ value: EnumConfig.RoomStatus.Closed, label: t('enum.room_status.closed') },
		],
		occupancyStatusOptions: [
			{ value: EnumConfig.OccupancyStatus.Active, label: t('enum.occupancy_status.active') },
			{ value: EnumConfig.OccupancyStatus.Discharged, label: t('enum.occupancy_status.discharged') },
			{ value: EnumConfig.OccupancyStatus.Transferred, label: t('enum.occupancy_status.transferred') },
		],

		// For Payroll
		insuranceSubjectOptions: [
			{ value: EnumConfig.InsuranceSubject.None, label: t('enum.insurance_subject.none') },
			{ value: EnumConfig.InsuranceSubject.Full, label: t('enum.insurance_subject.full') },
			{
				value: EnumConfig.InsuranceSubject.SocialAndHealth,
				label: t('enum.insurance_subject.social_and_health'),
			},
		],
		allowanceQuantitySourceOptions: [
			{
				value: EnumConfig.AllowanceQuantitySource.None,
				label: t('enum.allowance_quantity_source.none'),
			},
			{
				value: EnumConfig.AllowanceQuantitySource.OvertimeMinutes,
				label: t('enum.allowance_quantity_source.overtime_minutes'),
			},
		],
		deductionQuantitySourceOptions: [
			{
				value: EnumConfig.DeductionQuantitySource.None,
				label: t('enum.deduction_quantity_source.none'),
			},
			{
				value: EnumConfig.DeductionQuantitySource.LateMinutes,
				label: t('enum.deduction_quantity_source.late_minutes'),
			},
			{
				value: EnumConfig.DeductionQuantitySource.EarlyLeaveMinutes,
				label: t('enum.deduction_quantity_source.early_leave_minutes'),
			},
		],

		payrollPolicyStatusOptions: [
			{ value: EnumConfig.PayrollPolicyStatus.Active, label: t('enum.payroll_policy_status.active') },
			{
				value: EnumConfig.PayrollPolicyStatus.Inactive,
				label: t('enum.payroll_policy_status.inactive'),
			},
		],

		payrollStatusOptions: [
			{ value: EnumConfig.PayrollStatus.Draft, label: t('enum.payroll_status.draft') },
			{ value: EnumConfig.PayrollStatus.Pending, label: t('enum.payroll_status.pending') },
			{ value: EnumConfig.PayrollStatus.Approved, label: t('enum.payroll_status.approved') },
			{ value: EnumConfig.PayrollStatus.Paid, label: t('enum.payroll_status.paid') },
			{ value: EnumConfig.PayrollStatus.Terminated, label: t('enum.payroll_status.terminated') },
		],
		// Work Schedule
		workScheduleStatusOptions: [
			{ value: EnumConfig.WorkScheduleStatus.Draft, label: t('enum.work_schedule_status.draft') },
			{
				value: EnumConfig.WorkScheduleStatus.Published,
				label: t('enum.work_schedule_status.published'),
			},
			{ value: EnumConfig.WorkScheduleStatus.Locked, label: t('enum.work_schedule_status.locked') },
			{
				value: EnumConfig.WorkScheduleStatus.Finalized,
				label: t('enum.work_schedule_status.finalized'),
			},
		],
		locationTypeOptions: [
			{ value: EnumConfig.LocationType.Room, label: t('enum.location_type.room') },
			{ value: EnumConfig.LocationType.TeleRoom, label: t('enum.location_type.tele_room') },
		],
		dayOfWeekOptions: [
			{ value: EnumConfig.DayOfWeek.Monday, label: t('enum.day_of_week.monday') },
			{ value: EnumConfig.DayOfWeek.Tuesday, label: t('enum.day_of_week.tuesday') },
			{ value: EnumConfig.DayOfWeek.Wednesday, label: t('enum.day_of_week.wednesday') },
			{ value: EnumConfig.DayOfWeek.Thursday, label: t('enum.day_of_week.thursday') },
			{ value: EnumConfig.DayOfWeek.Friday, label: t('enum.day_of_week.friday') },
			{ value: EnumConfig.DayOfWeek.Saturday, label: t('enum.day_of_week.saturday') },
			{ value: EnumConfig.DayOfWeek.Sunday, label: t('enum.day_of_week.sunday') },
		],
		workStatusOptions: [
			{ value: EnumConfig.WorkStatus.Worked, label: t('enum.work_status.worked') },
			{ value: EnumConfig.WorkStatus.Absent, label: t('enum.work_status.absent') },
			{ value: EnumConfig.WorkStatus.Overtime, label: t('enum.work_status.overtime') },
		],
		workStatusReasonOptions: [
			{
				value: EnumConfig.WorkStatusReason.WorkedFromAttendance,
				label: t('enum.work_status_reason.worked_from_attendance'),
			},
			{
				value: EnumConfig.WorkStatusReason.ApprovedDayLeave,
				label: t('enum.work_status_reason.approved_day_leave'),
			},
			{
				value: EnumConfig.WorkStatusReason.ApprovedShiftLeave,
				label: t('enum.work_status_reason.approved_shift_leave'),
			},
			{
				value: EnumConfig.WorkStatusReason.NoAttendanceLog,
				label: t('enum.work_status_reason.no_attendance_log'),
			},
			{
				value: EnumConfig.WorkStatusReason.MissingCheckIn,
				label: t('enum.work_status_reason.missing_check_in'),
			},
			{
				value: EnumConfig.WorkStatusReason.MissingCheckOut,
				label: t('enum.work_status_reason.missing_check_out'),
			},
			{
				value: EnumConfig.WorkStatusReason.InvalidAttendanceRange,
				label: t('enum.work_status_reason.invalid_attendance_range'),
			},
			{
				value: EnumConfig.WorkStatusReason.LateArrival,
				label: t('enum.work_status_reason.late_arrival'),
			},
			{
				value: EnumConfig.WorkStatusReason.EarlyLeave,
				label: t('enum.work_status_reason.early_leave'),
			},
		],
		// For Import Request
		importRequestStatusOptions: [
			{
				value: EnumConfig.ImportRequestStatus.Pending,
				label: t('enum.import_request_status.pending'),
			},
			{
				value: EnumConfig.ImportRequestStatus.Approved,
				label: t('enum.import_request_status.approved'),
			},
			{
				value: EnumConfig.ImportRequestStatus.Rejected,
				label: t('enum.import_request_status.rejected'),
			},
			{
				value: EnumConfig.ImportRequestStatus.Cancelled,
				label: t('enum.import_request_status.cancelled'),
			},
			{
				value: EnumConfig.ImportRequestStatus.Created,
				label: t('enum.import_request_status.created'),
			},
		],

		// For Staff Certificate
		staffCertificateStatusOptions: [
			{
				value: EnumConfig.StaffCertificateStatus.Valid,
				label: t('enum.staff_certificate_status.valid'),
			},
			{
				value: EnumConfig.StaffCertificateStatus.Expired,
				label: t('enum.staff_certificate_status.expired'),
			},
			{
				value: EnumConfig.StaffCertificateStatus.Suspended,
				label: t('enum.staff_certificate_status.suspended'),
			},
		],
	}
}

// Usage Example:

// const _enum = useEnum();
// options: _enum.genderOptions,
// options: _enum.bloodTypeOptions,
// options: _enum.severityLevelOptions,

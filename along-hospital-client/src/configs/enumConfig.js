export const EnumConfig = {
	// For users
	AuthStage: {
		PatientProfilePendingWithPhone: 'PatientProfilePendingWithPhone',
		PatientProfilePendingWithoutPhone: 'PatientProfilePendingWithoutPhone',
		Done: 'Done',
	},
	Role: {
		Manager: 'Manager',
		Patient: 'Patient',
		Doctor: 'Doctor',
		Nurse: 'Nurse',
		HR: 'HR',
		Pharmacist: 'Pharmacist',
		Accountant: 'Accountant',
		Marketer: 'Marketer',
		Receptionist: 'Receptionist',
		HotlineAgent: 'HotlineAgent',
		InventoryClerk: 'InventoryClerk',
	},
	Gender: {
		Male: 'Male',
		Female: 'Female',
		Other: 'Other',
	},

	AllergySeverity: {
		Mild: 'Mild',
		Moderate: 'Moderate',
		Severe: 'Severe',
	},
	BloodType: {
		Unknown: 'Unknown',
		O: 'O',
		A: 'A',
		B: 'B',
		AB: 'AB',
		Rare: 'Rare',
	},
	SeverityLevel: {
		Mild: 'Mild',
		Moderate: 'Moderate',
		Severe: 'Severe',
	},
	VerificationDeliveryMethod: {
		Email: 'Email',
		Sms: 'Sms',
	},

	// Appointment
	AppointmentStatus: {
		Scheduled: 'Scheduled',
		Completed: 'Completed',
		Cancelled: 'Cancelled',
	},
	AppointmentPaymentStatus: {
		Pending: 'Pending',
		Completed: 'Completed',
		Failed: 'Failed',
	},
	AppointmentMeetingType: {
		InPerson: 'InPerson',
		Telehealth: 'Telehealth',
	},

	// Medical history
	MedicalHistoryStatus: {
		PendingPayment: 'PendingPayment',
		Draft: 'Draft',
		Completed: 'Completed',
		Cancelled: 'Cancelled',
	},
	MedicalHistoryType: {
		Outpatient: 'Outpatient',
		Inpatient: 'Inpatient',
	},

	// Medical Order
	MedicalOrderType: {
		Clinical: 'Clinical',
		Infusion: 'Infusion',
		Instruction: 'Instruction',
	},
	ClinicalMedicalOrderStatus: {
		Pending: 'Pending',
		Paid: 'Paid',
		Cancelled: 'Cancelled',
	},
	ClinicalMedicalOrderDetailStatus: {
		Pending: 'Pending',
		Completed: 'Completed',
		Failed: 'Failed',
	},
	InfusionMedicalOrderDetailExecutionStatus: {
		Pending: 'Pending',
		Completed: 'Completed',
		Failed: 'Failed',
	},
	InstructionMedicalOrderStatus: {
		Draft: 'Draft',
		Issued: 'Issued',
		Cancelled: 'Cancelled',
	},
	NursingCareOrderLevel: {
		Level1: 'Level1',
		Level2: 'Level2',
		Level3: 'Level3',
	},
	NutritionOrderType: {
		BreastMilk: 'BreastMilk',
		FormulaMilk: 'FormulaMilk',
		Porridge: 'Porridge',
		SoftRice: 'SoftRice',
		RegularDiet: 'RegularDiet',
		Npo: 'Npo',
		Other: 'Other',
	},
	PositionOrderType: {
		Supine: 'Supine',
		Trendelenburg: 'Trendelenburg',
		HeadElevated30: 'HeadElevated30',
		HeadElevated45: 'HeadElevated45',
		Other: 'Other',
	},
	RespiratorySupportOrderType: {
		RoomAir: 'RoomAir',
		OxygenNasalCannula: 'OxygenNasalCannula',
		OxygenMask: 'OxygenMask',
		EndotrachealIntubation: 'EndotrachealIntubation',
		Tracheostomy: 'Tracheostomy',
		Other: 'Other',
	},

	// Invoice
	InvoiceStatus: {
		Pending: 'Pending',
		Completed: 'Completed',
		Cancelled: 'Cancelled',
	},
	ChargeType: {
		Invoice: 'Invoice',
		Refund: 'Refund',
	},
	RefundStatus: {
		Pending: 'Pending',
		Approved: 'Approved',
		Cancelled: 'Cancelled',
	},

	// Queue
	QueueStatus: {
		Waiting: 'Waiting',
		Called: 'Called',
		InProgress: 'InProgress',
		AwaitingResults: 'AwaitingResults',
		Completed: 'Completed',
		Cancelled: 'Cancelled',
	},

	// Complaint
	ComplaintTopic: {
		Service: 'Service',
		Billing: 'Billing',
		Doctor: 'Doctor',
		Medicine: 'Medicine',
		Others: 'Others',
	},
	ComplaintType: {
		Neutral: 'Neutral',
		Positive: 'Positive',
		Negative: 'Negative',
	},
	ComplaintResolveStatus: {
		Pending: 'Pending',
		Draft: 'Draft',
		Resolved: 'Resolved',
		Closed: 'Closed',
	},

	// Attendance
	AttendanceLogType: {
		CheckIn: 'CheckIn',
		CheckOut: 'CheckOut',
	},

	// For medicines
	MedicineStatus: {
		Draft: 'Draft',
		Active: 'Active',
		Inactive: 'Inactive',
	},

	//Voucher
	VoucherStatus: {
		Active: 'Active',
		Expired: 'Expired',
	},
	VoucherType: {
		Patient: 'Patient',
		Medicine: 'Medicine',
	},
	VoucherDiscountType: {
		Percentage: 'Percentage',
		FixedAmount: 'FixedAmount',
	},

	//Order
	PaymentType: {
		PayOS: 'PayOS',
		SePay: 'SePay',
		Cash: 'Cash',
	},
	OrderStatus: {
		Unpaid: 'Unpaid',
		Paid: 'Paid',
		Shipping: 'Shipping',
		Completed: 'Completed',
		Cancelled: 'Cancelled',
	},

	//Feedback
	FeedbackStatus: {
		Active: 'Active',
		Hidden: 'Hidden',
	},
	FeedbackReportStatus: {
		Pending: 'Pending',
		Resolved: 'Resolved',
		Rejected: 'Rejected',
	},
	FeedbackReplyStatus: {
		WaitingStaff: 'WaitingStaff',
		Replied: 'Replied',
	},

	//Staff
	StaffStatus: {
		Active: 'Active',
		OnLeave: 'OnLeave',
		Terminated: 'Terminated',
		Suspended: 'Suspended',
	},
	BankCode: {
		MBBank: 'MBBank',
		Vietcombank: 'Vietcombank',
		Techcombank: 'Techcombank',
		BIDV: 'BIDV',
		VietinBank: 'VietinBank',
		ACB: 'ACB',
		TPBank: 'TPBank',
		VPBank: 'VPBank',
		Agribank: 'Agribank',
		MSB: 'MSB',
		OCB: 'OCB',
		KienlongBank: 'KienlongBank',
		Eximbank: 'Eximbank',
		HDBank: 'HDBank',
		Sacombank: 'Sacombank',
		VIB: 'VIB',
		ABBank: 'ABBank',
		LPBank: 'LPBank',
		BacABank: 'BacABank',
		SeABank: 'SeABank',
		SHB: 'SHB',
		NCB: 'NCB',
		WooriBank: 'WooriBank',
		VietABank: 'VietABank',
		VietBank: 'VietBank',
		NamABank: 'NamABank',
		PGBank: 'PGBank',
		PublicBank: 'PublicBank',
	},

	// Staff Contract
	ContractType: {
		Probation: 'Probation',
		FixedTerm: 'FixedTerm',
		Indefinite: 'Indefinite',
		Internship: 'Internship',
	},
	StaffContractStatus: {
		Active: 'Active',
		Expired: 'Expired',
		Terminated: 'Terminated',
	},

	// Leave Request
	LeaveType: {
		Annual: 'Annual',
		Sick: 'Sick',
		Maternity: 'Maternity',
		Personal: 'Personal',
		Other: 'Other',
	},
	LeaveUnit: {
		Day: 'Day',
		Shift: 'Shift',
	},
	OvertimeType: {
		AfterShift: 'AfterShift',
		Holiday: 'Holiday',
	},
	LeaveRequestStatus: {
		Pending: 'Pending',
		Approved: 'Approved',
		Rejected: 'Rejected',
		Canceled: 'Canceled',
	},
	SalaryAdvanceStatus: {
		Pending: 'Pending',
		Approved: 'Approved',
		Rejected: 'Rejected',
		Cancelled: 'Cancelled',
		Disbursed: 'Disbursed',
	},

	// InPatient Resource
	BedStatus: {
		Active: 'Active',
		Maintenance: 'Maintenance',
		Occupied: 'Occupied',
	},
	RoomStatus: {
		Active: 'Active',
		Maintenance: 'Maintenance',
		Closed: 'Closed',
	},

	// For Payroll
	InsuranceSubject: {
		None: 'None',
		Full: 'Full',
		SocialAndHealth: 'SocialAndHealth',
	},
	AllowanceQuantitySource: {
		None: 'None',
		OvertimeMinutes: 'OvertimeMinutes',
	},
	DeductionQuantitySource: {
		None: 'None',
		LateMinutes: 'LateMinutes',
		EarlyLeaveMinutes: 'EarlyLeaveMinutes',
	},

	PayrollPolicyStatus: {
		Active: 'Active',
		Inactive: 'Inactive',
	},

	PayrollStatus: {
		Draft: 'Draft',
		Pending: 'Pending',
		Approved: 'Approved',
		Paid: 'Paid',
		Terminated: 'Terminated',
	},

	OccupancyStatus: {
		Active: 'Active',
		Discharged: 'Discharged',
		Transferred: 'Transferred',
	},

	// Work Schedule
	WorkScheduleStatus: {
		Draft: 'Draft',
		Published: 'Published',
		Locked: 'Locked',
		Finalized: 'Finalized',
	},
	LocationType: {
		Room: 'Room',
		TeleRoom: 'TeleRoom',
	},
	DayOfWeek: {
		Sunday: 0,
		Monday: 1,
		Tuesday: 2,
		Wednesday: 3,
		Thursday: 4,
		Friday: 5,
		Saturday: 6,
	},
	WorkStatus: {
		Worked: 'Worked',
		Absent: 'Absent',
		Overtime: 'Overtime',
	},
	WorkStatusReason: {
		WorkedFromAttendance: 'WorkedFromAttendance',
		ApprovedDayLeave: 'ApprovedDayLeave',
		ApprovedShiftLeave: 'ApprovedShiftLeave',
		NoAttendanceLog: 'NoAttendanceLog',
		MissingCheckIn: 'MissingCheckIn',
		MissingCheckOut: 'MissingCheckOut',
		InvalidAttendanceRange: 'InvalidAttendanceRange',
		LateArrival: 'LateArrival',
		EarlyLeave: 'EarlyLeave',
	},
	// Import Request
	ImportRequestStatus: {
		Pending: 'Pending',
		Approved: 'Approved',
		Rejected: 'Rejected',
		Cancelled: 'Cancelled',
		Created: 'Created',
	},

	// Job Posting
	JobPostingStatus: {
		Draft: 'Draft',
		Open: 'Open',
		Closed: 'Closed',
	},
	EmploymentTypeEnum: {
		FullTime: 'FullTime',
		PartTime: 'PartTime',
		Internship: 'Internship',
	},
	// Staff Certificate
	StaffCertificateStatus: {
		Valid: 'Valid',
		Expired: 'Expired',
		Suspended: 'Suspended',
	},
	// Job Application
	JobApplicationStatus: {
		Applied: 'Applied',
		Interviewing: 'Interviewing',
		Passed: 'Passed',
		Failed: 'Failed',
	},
	InterviewResult: {
		Pending: 'Pending',
		Passed: 'Passed',
		Failed: 'Failed',
		Cancelled: 'Cancelled',
	},
}

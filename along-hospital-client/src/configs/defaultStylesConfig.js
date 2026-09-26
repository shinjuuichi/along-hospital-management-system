import { AssignmentOutlined, ScienceOutlined, VaccinesOutlined } from '@mui/icons-material'
import { EnumConfig } from './enumConfig'

export const defaultAppointmentStatusStyle = (theme, status) => {
	const map = {
		[EnumConfig.AppointmentStatus.Scheduled]: theme.palette.info,
		[EnumConfig.AppointmentStatus.Confirmed]: theme.palette.primary,
		[EnumConfig.AppointmentStatus.Completed]: theme.palette.success,
		[EnumConfig.AppointmentStatus.Cancelled]: theme.palette.error,
		[EnumConfig.AppointmentStatus.Refused]: theme.palette.warning,
	}
	const p = map[status] || theme.palette.primary
	return { bg: p.softBg || p.main + '1A', border: p.softBorder || p.main + '33', color: p.main }
}

export const defaultAppointmentPaymentStatusStyle = (status) => {
	const map = {
		[EnumConfig.AppointmentPaymentStatus.None]: 'default',
		[EnumConfig.AppointmentPaymentStatus.Pending]: 'warning',
		[EnumConfig.AppointmentPaymentStatus.Completed]: 'success',
		[EnumConfig.AppointmentPaymentStatus.Failed]: 'error',
	}
	return map[status] || 'default'
}

export const defaultAllergySeverityStyle = (severity) => {
	const map = {
		[EnumConfig.AllergySeverity.Mild]: 'info.dark',
		[EnumConfig.AllergySeverity.Moderate]: 'warning.dark',
		[EnumConfig.AllergySeverity.Severe]: 'error.dark',
	}
	return map[severity] || 'primary'
}

export const defaultMedicalHistoryStatusStyle = (status) => {
	const map = {
		[EnumConfig.MedicalHistoryStatus.PendingPayment]: 'warning',
		[EnumConfig.MedicalHistoryStatus.Draft]: 'info',
		[EnumConfig.MedicalHistoryStatus.Completed]: 'success',
		[EnumConfig.MedicalHistoryStatus.Cancelled]: 'error',
	}
	return map[status] || 'primary'
}

export const defaultMedicalHistoryTypeStyle = (type) => {
	const map = {
		[EnumConfig.MedicalHistoryType.Outpatient]: 'warning',
		[EnumConfig.MedicalHistoryType.Inpatient]: 'success',
	}
	return map[type] || 'primary'
}

export const defaultMedicalOrderTypeStyle = (type) => {
	const map = {
		[EnumConfig.MedicalOrderType.Clinical]: { icon: ScienceOutlined, color: 'info' },
		[EnumConfig.MedicalOrderType.Infusion]: { icon: VaccinesOutlined, color: 'secondary' },
		[EnumConfig.MedicalOrderType.Instruction]: { icon: AssignmentOutlined, color: 'warning' },
	}
	return map[type] || { icon: AssignmentOutlined, color: 'primary' }
}

export const defaultMedicalOrderStatusStyle = (medicalOrderType, status) => {
	const map = {
		[EnumConfig.MedicalOrderType.Clinical]: {
			[EnumConfig.ClinicalMedicalOrderStatus.Pending]: 'warning',
			[EnumConfig.ClinicalMedicalOrderStatus.Paid]: 'success',
			[EnumConfig.ClinicalMedicalOrderStatus.Cancelled]: 'error',
		},
		[EnumConfig.MedicalOrderType.Instruction]: {
			[EnumConfig.InstructionMedicalOrderStatus.Draft]: 'warning',
			[EnumConfig.InstructionMedicalOrderStatus.Issued]: 'info',
			[EnumConfig.InstructionMedicalOrderStatus.Cancelled]: 'error',
		},
		[EnumConfig.MedicalOrderType.Infusion]: {},
	}

	return map[medicalOrderType]?.[status] || 'default'
}

export const defaultClinicalOrderDetailStatusStyle = (status) => {
	const map = {
		[EnumConfig.ClinicalMedicalOrderDetailStatus.Pending]: 'warning',
		[EnumConfig.ClinicalMedicalOrderDetailStatus.Completed]: 'success',
		[EnumConfig.ClinicalMedicalOrderDetailStatus.Failed]: 'error',
	}
	return map[status] || 'default'
}

export const defaultInfusionMedicalOrderDetailExecutionStatusStyle = (status) => {
	const map = {
		[EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Pending]: 'warning',
		[EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Completed]: 'success',
		[EnumConfig.InfusionMedicalOrderDetailExecutionStatus.Failed]: 'error',
	}
	return map[status] || 'default'
}

export const defaultInvoiceStatusStyle = (status) => {
	const map = {
		[EnumConfig.InvoiceStatus.Pending]: 'warning',
		[EnumConfig.InvoiceStatus.Completed]: 'success',
		[EnumConfig.InvoiceStatus.Cancelled]: 'error',
	}
	return map[status] || 'primary'
}

export const defaultChargeTypeStyle = (type) => {
	const map = {
		[EnumConfig.ChargeType.Invoice]: 'primary',
		[EnumConfig.ChargeType.Refund]: 'secondary',
	}
	return map[type] || 'primary'
}

export const defaultOrderStatusStyle = (status) => {
	const map = {
		[EnumConfig.OrderStatus.Unpaid]: 'warning',
		[EnumConfig.OrderStatus.Paid]: 'info',
		[EnumConfig.OrderStatus.Shipping]: 'primary',
		[EnumConfig.OrderStatus.Completed]: 'success',
		[EnumConfig.OrderStatus.Cancelled]: 'error',
	}
	return map[status] || 'default'
}

export const defaultOrderStatusThemeColor = (status) => {
	const map = {
		[EnumConfig.OrderStatus.Unpaid]: { bg: 'warning.light', color: 'warning.dark' },
		[EnumConfig.OrderStatus.Paid]: { bg: 'primary.light', color: 'primary.dark' },
		[EnumConfig.OrderStatus.Shipping]: { bg: 'secondary.light', color: 'secondary.dark' },
		[EnumConfig.OrderStatus.Completed]: { bg: 'success.light', color: 'success.dark' },
		[EnumConfig.OrderStatus.Cancelled]: { bg: 'error.light', color: 'error.dark' },
	}
	return map[status] || { bg: 'grey.200', color: 'grey.600' }
}

export const defaultRefundStatusStyle = (status) => {
	const map = {
		[EnumConfig.RefundStatus.Pending]: 'warning',
		[EnumConfig.RefundStatus.Approved]: 'success',
		[EnumConfig.RefundStatus.Cancelled]: 'error',
	}
	return map[status] || 'primary'
}

export const defaultQueueStatusStyle = (status) => {
	const map = {
		[EnumConfig.QueueStatus.Waiting]: 'info',
		[EnumConfig.QueueStatus.Called]: 'warning',
		[EnumConfig.QueueStatus.InProgress]: 'primary',
		[EnumConfig.QueueStatus.AwaitingResults]: 'warning',
		[EnumConfig.QueueStatus.Completed]: 'success',
		[EnumConfig.QueueStatus.Cancelled]: 'error',
	}
	return map[status] || 'primary'
}

export const defaultQueueStatusThemeColor = (theme, status) => {
	const map = {
		total: theme.palette.primary.main,
		[EnumConfig.QueueStatus.Waiting]: theme.palette.info.main,
		[EnumConfig.QueueStatus.Called]: theme.palette.warning.main,
		[EnumConfig.QueueStatus.InProgress]: theme.palette.primary.main,
		[EnumConfig.QueueStatus.AwaitingResults]: theme.palette.warning.dark,
		[EnumConfig.QueueStatus.Completed]: theme.palette.success.main,
		[EnumConfig.QueueStatus.Cancelled]: theme.palette.error.main,
	}
	return map[status] || theme.palette.primary.main
}

export const defaultComplaintTypeStyle = (type) => {
	const map = {
		[EnumConfig.ComplaintType.Neutral]: 'info',
		[EnumConfig.ComplaintType.Positive]: 'success',
		[EnumConfig.ComplaintType.Negative]: 'error',
	}
	return map[type] || 'primary'
}

export const defaultComplaintResolveStatusStyle = (status) => {
	const map = {
		[EnumConfig.ComplaintResolveStatus.Pending]: 'warning',
		[EnumConfig.ComplaintResolveStatus.Draft]: 'info',
		[EnumConfig.ComplaintResolveStatus.Resolved]: 'success',
		[EnumConfig.ComplaintResolveStatus.Closed]: 'error',
	}
	return map[status] || 'primary'
}

export const defaultLineClampStyle = (lines = 2) => ({
	overflow: 'hidden',
	textOverflow: 'ellipsis',
	width: '100%',
	display: '-webkit-box',
	WebkitLineClamp: lines,
	lineClamp: lines,
	WordBreak: 'break-word',
	WebkitBoxOrient: 'vertical',
})

export const defaultVoucherStatusStyle = (status) => {
	const map = {
		[EnumConfig.VoucherStatus.Active]: 'success',
		[EnumConfig.VoucherStatus.Expired]: 'error',
	}
	return map[status] || 'default'
}

export const defaultFeedbackReportStatusStyle = (status) => {
	const map = {
		[EnumConfig.FeedbackReportStatus.Pending]: 'warning',
		[EnumConfig.FeedbackReportStatus.Resolved]: 'success',
		[EnumConfig.FeedbackReportStatus.Rejected]: 'error',
	}
	return map[status] || 'default'
}

export const defaultVoucherTypeStyle = (type) => {
	const map = {
		[EnumConfig.VoucherType.Patient]: 'primary',
		[EnumConfig.VoucherType.Medicine]: 'secondary',
	}
	return map[type] || 'default'
}

export const defaultAttendanceLogTypeStyle = (type) => {
	const map = {
		[EnumConfig.AttendanceLogType.CheckIn]: 'success',
		[EnumConfig.AttendanceLogType.CheckOut]: 'error',
	}
	return map[type] || 'default'
}

export const defaultRoomStatusStyle = (status) => {
	const map = {
		[EnumConfig.RoomStatus.Active]: 'success',
		[EnumConfig.RoomStatus.Maintenance]: 'warning',
		[EnumConfig.RoomStatus.Closed]: 'error',
	}
	return map[status] || 'default'
}

export const defaultMedicineStatusStyle = (status) => {
	const map = {
		[EnumConfig.MedicineStatus.Active]: 'success',
		[EnumConfig.MedicineStatus.Inactive]: 'default',
		[EnumConfig.MedicineStatus.Draft]: 'warning',
	}
	return map[status] || 'default'
}

export const defaultBedStatusStyle = (status) => {
	const map = {
		[EnumConfig.BedStatus.Active]: 'success',
		[EnumConfig.BedStatus.Maintenance]: 'warning',
		[EnumConfig.BedStatus.Occupied]: 'info',
	}
	return map[status] || 'default'
}

export const defaultInsuranceSubjectStyle = (value) => {
	const map = {
		[EnumConfig.InsuranceSubject.None]: 'default',
		[EnumConfig.InsuranceSubject.Full]: 'success',
		[EnumConfig.InsuranceSubject.SocialAndHealth]: 'info',
	}
	return map[value] ?? 'default'
}

export const defaultAllowanceQuantitySourceStyle = (value) => {
	const map = {
		[EnumConfig.AllowanceQuantitySource.OvertimeMinutes]: 'primary',
	}
	return map[value] ?? 'default'
}

export const defaultDeductionQuantitySourceStyle = (value) => {
	const map = {
		[EnumConfig.DeductionQuantitySource.LateMinutes]: 'warning',
		[EnumConfig.DeductionQuantitySource.EarlyLeaveMinutes]: 'error',
	}
	return map[value] ?? 'default'
}

export const defaultPayrollPolicyStatusStyle = (value) => {
	const map = {
		[EnumConfig.PayrollPolicyStatus.Active]: 'success',
		[EnumConfig.PayrollPolicyStatus.Inactive]: 'default',
	}
	return map[value] ?? 'default'
}

export const defaultPayrollStatusStyle = (value) => {
	const map = {
		[EnumConfig.PayrollStatus.Draft]: 'default',
		[EnumConfig.PayrollStatus.Pending]: 'warning',
		[EnumConfig.PayrollStatus.Approved]: 'info',
		[EnumConfig.PayrollStatus.Paid]: 'success',
		[EnumConfig.PayrollStatus.Terminated]: 'error',
	}
	return map[value] ?? 'default'
}

export const defaultOccupancyStatusStyle = (status) => {
	const map = {
		[EnumConfig.OccupancyStatus.Active]: 'success',
		[EnumConfig.OccupancyStatus.Discharged]: 'info',
		[EnumConfig.OccupancyStatus.Transferred]: 'warning',
	}
	return map[status] || 'default'
}

export const defaultFeedbackReplyStatusStyle = (status) => {
	const map = {
		[EnumConfig.FeedbackReplyStatus.Replied]: 'success',
		[EnumConfig.FeedbackReplyStatus.WaitingStaff]: 'Warning',
	}
	return map[status] || 'default'
}

export const defaultImportRequestStatusStyle = (status) => {
	const map = {
		[EnumConfig.ImportRequestStatus.Pending]: 'warning',
		[EnumConfig.ImportRequestStatus.Approved]: 'success',
		[EnumConfig.ImportRequestStatus.Rejected]: 'error',
		[EnumConfig.ImportRequestStatus.Cancelled]: 'default',
		[EnumConfig.ImportRequestStatus.Created]: 'info',
	}
	return map[status] || 'default'
}

export const defaultLeaveRequestStatusStyle = (status) => {
	const map = {
		[EnumConfig.LeaveRequestStatus.Pending]: 'warning',
		[EnumConfig.LeaveRequestStatus.Approved]: 'success',
		[EnumConfig.LeaveRequestStatus.Rejected]: 'error',
		[EnumConfig.LeaveRequestStatus.Canceled]: 'default',
	}
	return map[String(status || '')] || 'default'
}

export const defaultSalaryAdvanceStatusStyle = (status) => {
	const map = {
		[EnumConfig.SalaryAdvanceStatus.Pending]: 'warning',
		[EnumConfig.SalaryAdvanceStatus.Approved]: 'success',
		[EnumConfig.SalaryAdvanceStatus.Rejected]: 'error',
		[EnumConfig.SalaryAdvanceStatus.Cancelled]: 'default',
		[EnumConfig.SalaryAdvanceStatus.Disbursed]: 'info',
	}
	return map[String(status || '')] || 'default'
}

export const defaultJobPostingStatusStyle = (status) => {
	const map = {
		[EnumConfig.JobPostingStatus.Draft]: 'default',
		[EnumConfig.JobPostingStatus.Open]: 'success',
		[EnumConfig.JobPostingStatus.Closed]: 'error',
	}
	return map[status] || 'default'
}
export const defaultStaffContractStatusStyle = (status) => {
	const map = {
		[EnumConfig.StaffContractStatus.Active]: 'success',
		[EnumConfig.StaffContractStatus.Expired]: 'warning',
		[EnumConfig.StaffContractStatus.Terminated]: 'error',
	}
	return map[String(status || '')] || 'default'
}

export const defaultStaffStatusStyle = (status) => {
	const map = {
		[EnumConfig.StaffStatus.Active]: 'success',
		[EnumConfig.StaffStatus.OnLeave]: 'warning',
		[EnumConfig.StaffStatus.Terminated]: 'error',
		[EnumConfig.StaffStatus.Suspended]: 'default',
	}
	return map[String(status || '')] || 'default'
}

export const defaultBooleanStyle = (value) => (value ? 'success' : 'default')
export const defaultStaffCertificateStatusStyle = (status) => {
	const map = {
		[EnumConfig.StaffCertificateStatus.Valid]: 'success',
		[EnumConfig.StaffCertificateStatus.Expired]: 'error',
		[EnumConfig.StaffCertificateStatus.Suspended]: 'info',
	}
	return map[String(status || '')] || 'default'
}

export const defaultWorkScheduleStatusStyle = (status) => {
	const map = {
		[EnumConfig.WorkScheduleStatus.Draft]: 'warning',
		[EnumConfig.WorkScheduleStatus.Published]: 'success',
		[EnumConfig.WorkScheduleStatus.Locked]: 'info',
		[EnumConfig.WorkScheduleStatus.Finalized]: 'secondary',
	}
	return map[status] || 'default'
}

export const defaultWorkStatusStyle = (status) => {
	const map = {
		[EnumConfig.WorkStatus.Worked]: 'success',
		[EnumConfig.WorkStatus.Absent]: 'error',
		[EnumConfig.WorkStatus.Overtime]: 'info',
	}
	return map[status] || 'default'
}

export const defaultJobApplicationStatusStyle = (status) => {
	const map = {
		[EnumConfig.JobApplicationStatus.Applied]: 'info',
		[EnumConfig.JobApplicationStatus.Interviewing]: 'warning',
		[EnumConfig.JobApplicationStatus.Passed]: 'success',
		[EnumConfig.JobApplicationStatus.Failed]: 'error',
	}
	return map[status] || 'default'
}

export const defaultInterviewResultStyle = (result) => {
	const map = {
		[EnumConfig.InterviewResult.Pending]: 'warning',
		[EnumConfig.InterviewResult.Passed]: 'success',
		[EnumConfig.InterviewResult.Failed]: 'error',
		[EnumConfig.InterviewResult.Cancelled]: 'default',
	}
	return map[result] || 'default'
}

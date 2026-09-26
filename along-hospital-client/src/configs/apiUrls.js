export const ApiUrls = {
	AUTH: {
		LOGIN: '/auth/login',
		LOGIN_GOOGLE: '/auth/login/google',
		REGISTER: '/auth/register',
		REGISTER_RESEND_OPTIONS: '/auth/register/resend/options',
		REGISTER_RESEND: '/auth/register/resend',
		REGISTER_VERIFY: '/auth/register/verify',
		REFRESH: '/auth/refresh',
		FORGOT_PASSWORD_OPTIONS: '/auth/forgot-password/options',
		FORGOT_PASSWORD: '/auth/forgot-password',
		FORGOT_PASSWORD_VERIFY: '/auth/forgot-password/verify',
		FORGOT_PASSWORD_RESET: '/auth/forgot-password/reset',
		LOGOUT: '/auth/logout',
		CURRENT_ACCOUNT: '/auth/me',
		REFRESH_TOKEN: '/auth/refresh',
		COMPLETE_PROFILE: '/user/complete-profile',
		CHANGE_PASSWORD: '/auth/change-password',
	},
	HUB: {
		CHATBOT: '/hub/chatbot',
		QUEUE: (roomId) => `/hub/queue${roomId ? `?roomId=${roomId}` : ''}`,
	},
	USER: {
		INDEX: `/user`,
		PROFILE: `/user/profile`,
	},
	PATIENT: {
		MANAGEMENT: {
			INDEX: `/patient-management`,
			DETAIL: (id) => `/patient-management/${id}`,
			GET_ALL: `/patient-management/all`,
		},
	},
	STAFF: {
		INDEX: `/staff`,
		GET_ALL: `/staff/all`,
		GET_BY_ROLE: (role) => `/staff/role/${role}`,
		MANAGEMENT: {
			INDEX: `/staff-management`,
			GET_ALL: `/staff-management/all`,
			CREATE: `/staff-management/staff-account`,
			DETAIL: (id) => `/staff-management/staff-account/${id}`,
			ACTIVATE: `/staff-management/activate`,
			ON_LEAVE: `/staff-management/onleave`,
			TERMINATE: `/staff-management/terminate`,
		},
	},
	STAFF_GROUP: {
		MANAGEMENT: {
			INDEX: `/staff-group-management`,
			GET_ALL: `/staff-group-management/all`,
			DETAIL: (id) => `/staff-group-management/${id}`,
		},
	},
	STAFF_CONTRACT: {
		MANAGEMENT: {
			INDEX: `/staff-contract-management`,
			GET_ALL: `/staff-contract-management/all`,
			DETAIL: (id) => `/staff-contract-management/${id}`,
			TERMINATE: (id) => `/staff-contract-management/terminate/${id}`,
			RENEW: (id) => `/staff-contract-management/renew/${id}`,
			SIGN: (id) => `/staff-contract-management/sign/${id}`,
		},
	},
	SPECIALTY: {
		INDEX: `/specialty`,
		GET_ALL: `/specialty/all`,
		MANAGEMENT: {
			INDEX: `/specialty-management`,
			GET_ALL: `/specialty-management/all`,
			DETAIL: (id) => `/specialty-management/${id}`,
		},
	},
	ALLOWANCE_TYPE: {
		MANAGEMENT: {
			INDEX: `/allowance-type-management`,
			GET_ALL: `/allowance-type-management/all`,
			DETAIL: (id) => `/allowance-type-management/${id}`,
		},
	},
	DEDUCTION_TYPE: {
		MANAGEMENT: {
			INDEX: `/deduction-type-management`,
			GET_ALL: `/deduction-type-management/all`,
			DETAIL: (id) => `/deduction-type-management/${id}`,
		},
	},
	ALLOWANCE: {
		MANAGEMENT: {
			INDEX: `/allowance-management`,
			DETAIL: (id) => `/allowance-management/${id}`,
		},
	},
	DEDUCTION: {
		MANAGEMENT: {
			INDEX: `/deduction-management`,
			DETAIL: (id) => `/deduction-management/${id}`,
		},
	},
	TAX_BRACKET: {
		MANAGEMENT: {
			INDEX: `/tax-bracket-management`,
			GET_ALL: `/tax-bracket-management/all`,
			DETAIL: (id) => `/tax-bracket-management/${id}`,
		},
	},
	GLOBAL_TAX_CONFIG: {
		MANAGEMENT: {
			INDEX: `/global-tax-config-management`,
			GET_ALL: `/global-tax-config-management/all`,
			DETAIL: (id) => `/global-tax-config-management/${id}`,
		},
	},
	PAYROLL_POLICY: {
		MANAGEMENT: {
			INDEX: `/payroll-policy-management`,
			GET_ALL: `/payroll-policy-management/all`,
			DETAIL: (id) => `/payroll-policy-management/${id}`,
		},
	},
	PAYROLL: {
		MANAGEMENT: {
			INDEX: `/payroll-management`,
			GET_ALL: `/payroll-management/all`,
			DETAIL: (id) => `/payroll-management/${id}`,
			PENDING: `/payroll-management/pending`,
			APPROVE: `/payroll-management/approve`,
		},
	},
	REGIONAL_WAGE: {
		MANAGEMENT: {
			INDEX: `/regional-wage-management`,
			GET_ALL: `/regional-wage-management/all`,
			DETAIL: (id) => `/regional-wage-management/${id}`,
		},
	},
	MEDICINE: {
		INDEX: `/medicine`,
		GET_BY_ID: (id) => `/medicine/${id}`,
		GET_ALL: `/medicine/all`,
		INFUSION: {
			GET_ALL: `/medicine/infusion/all`,
		},
		MANAGEMENT: {
			INDEX: `/medicine-management`,
			GET_ALL: `/medicine-management/all`,
			GET_BY_ID: (id) => `/medicine-management/${id}`,
			CREATE: `/medicine-management`,
			UPDATE: (id) => `/medicine-management/${id}`,
			DELETE: (id) => `/medicine-management/${id}`,
		},
	},
	MEDICINE_CATEGORY: {
		INDEX: `/medicine-category`,
		GET_ALL: `/medicine-category/all`,
		CREATE: `/medicine-category`,
		UPDATE: (id) => `/medicine-category/${id}`,
		DELETE: (id) => `/medicine-category/${id}`,
	},
	OPTION: {
		INDEX: `/option`,
		MANAGEMENT: {
			INDEX: `/option-management`,
			GET_ALL: `/option-management/all`,
			DETAIL: (id) => `/option-management/${id}`,
		},
	},
	OPTION_VALUE: {
		INDEX: `/option-value`,
		MANAGEMENT: {
			INDEX: `/option-value-management`,
			GET_ALL: `/option-value-management/all`,
			DETAIL: (id) => `/option-value-management/${id}`,
		},
	},
	MEDICINE_UNIT: {
		INDEX: `/medicine-unit`,
		GET_ALL: `/medicine-unit/all`,
		MANAGEMENT: {
			INDEX: `/medicine-unit-management`,
			GET_ALL: `/medicine-unit-management/all`,
			DETAIL: (id) => `/medicine-unit-management/${id}`,
		},
	},
	MEDICINE_UNIT_OPTION: {
		MANAGEMENT: {
			UPDATE_STATUS: `/medicine-unit-option-management`,
		},
	},
	MEDICINE_SKU: {
		INDEX: `/medicine-sku`,
		MANAGEMENT: {
			INDEX: `/medicine-sku-management`,
			GET_ALL: `/medicine-sku-management/all`,
			DETAIL: (id) => `/medicine-sku-management/${id}`,
		},
	},
	IMPORT: {
		MANAGEMENT: {
			INDEX: `/import-management`,
			GET_ALL: `/import-management/all`,
			DETAIL: (id) => `/import-management/${id}`,
			DELETE_SELECTED: `/import-management/selected`,
			BULK_IMPORT_FROM_EXCEL: `/import-management/bulk-import-from-excel`,
			FROM_APPROVED_REQUEST: `/import-management/from-approved-request`,
		},
	},
	IMPORT_REQUEST: {
		MANAGEMENT: {
			INDEX: `/import-request-management`,
			DETAIL: (id) => `/import-request-management/${id}`,
			APPROVE: (id) => `/import-request-management/approve/${id}`,
			REJECT: (id) => `/import-request-management/reject/${id}`,
			CANCEL: (id) => `/import-request-management/cancel/${id}`,
		},
	},
	MEDICAL_SERVICE: {
		INDEX: `/medical-service`,
		GET_ALL: `/medical-service/all`,
		DETAIL: (id) => `/medical-service/${id}`,
		MANAGEMENT: {
			INDEX: `/medical-service-management`,
			GET_ALL: `/medical-service-management/all`,
			DETAIL: (id) => `/medical-service-management/${id}`,
		},
	},
	APPOINTMENT: {
		INDEX: `/appointment`,
		GET_PAYMENT_URL: (id) => `/appointment/${id}/payment-url`,
		CANCEL: (id) => `/appointment/cancel/${id}`,
		MEETING_ROOM: `/appointment/meeting-room`,
		MEETING_ROOM_TOKEN: (id) => `/appointment/meeting-room-token/${id}`,
		MANAGEMENT: {
			INDEX: `/appointment-management`,
		},
	},
	TIME_SLOT: {
		AVAILABLE: `/time-slot/available`,
		MANAGEMENT: {
			INDEX: `/time-slot-management`,
			GET_ALL: `/time-slot-management/all`,
			DETAIL: (id) => `/time-slot-management/${id}`,
		},
	},
	COMPLAINT: {
		MANAGEMENT: {
			INDEX: `/complaint-management`,
			SUMMARY: `/complaint-management/summary`,
			DRAFT: (id) => `/complaint-management/draft/${id}`,
			RESOLVE: (id) => `/complaint-management/resolve/${id}`,
			CLOSE: (id) => `/complaint-management/close/${id}`,
			CLASSIFY: (id) => `/complaint-management/classify/${id}`,
		},
	},
	MEDICAL_HISTORY: {
		INDEX: `/medical-history`,
		DETAIL: (id) => `/medical-history/${id}`,
		CREATE_COMPLAINT: (medicalHistoryId) => `/medical-history/${medicalHistoryId}/complaint`,
		MANAGEMENT: {
			INDEX: `/medical-history-management`,
			DETAIL: (medicalHistoryId) => `/medical-history-management/${medicalHistoryId}`,
			GET_ALL_BY_CURRENT_DOCTOR: `/medical-history-management/doctor`,
			GET_ALL_PENDING: `/medical-history-management/all/pending`,
			COMPLETE: (medicalHistoryId) => `/medical-history-management/complete/${medicalHistoryId}`,
			DISCHARGE_BED: (medicalHistoryId) =>
				`/medical-history-management/discharge-bed/${medicalHistoryId}`,
			CANCEL: (medicalHistoryId) => `/medical-history-management/cancel/${medicalHistoryId}`,
			PRESCRIPTION: (medicalHistoryId) =>
				`/medical-history-management/${medicalHistoryId}/prescription`,
		},
	},
	MEDICAL_ORDER: {
		INDEX: '/medical-order',
		DETAIL: (id) => `/medical-order/${id}`,
		CLINICAL: {
			CANCEL: (id) => `/medical-order/clinical/cancel/${id}`,
			DETAIL: {
				COMPLETE: (medicalOrderId, medicalServiceId) =>
					`/medical-order/clinical/${medicalOrderId}/detail/complete/${medicalServiceId}`,
				FAIL: (medicalOrderId, medicalServiceId) =>
					`/medical-order/clinical/${medicalOrderId}/detail/fail/${medicalServiceId}`,
			},
		},
		INFUSION: {
			DETAIL: {
				COMPLETE: (medicalOrderId, medicineId) =>
					`/medical-order/infusion/${medicalOrderId}/detail/complete/${medicineId}`,
				FAIL: (medicalOrderId, medicineId) =>
					`/medical-order/infusion/${medicalOrderId}/detail/fail/${medicineId}`,
			},
		},
		INSTRUCTION: {
			DRAFT: `/medical-order/instruction/draft`,
			DETAIL: {
				INDEX: (medicalOrderId) => `/medical-order/instruction/${medicalOrderId}`,
				ISSUE: (medicalOrderId) => `/medical-order/instruction/issue/${medicalOrderId}`,
				CANCEL: (medicalOrderId) => `/medical-order/instruction/cancel/${medicalOrderId}`,
			},
		},
	},
	INVOICE: {
		INDEX: `/invoice`,
		DETAIL: (id) => `/invoice/${id}`,
		COMPLETE: (id) => `/invoice/complete/${id}`,
		CANCEL: (id) => `/invoice/cancel/${id}`,
		PAYMENT_URL: (id) => `/invoice/${id}/payment-url`,
	},
	REFUND: {
		INVOICE_CHARGE: (clinicalMedicalOrderDetailId) =>
			`/refund/invoice-charge/${clinicalMedicalOrderDetailId}`,
		APPROVE: (chargeId) => `/refund/approve/${chargeId}`,
	},
	QUEUE: {
		INDEX: `/queue`,
		GET_ALL: `/queue/all`,
		QR: `/queue/qr`,
		MANAGEMENT: {
			ASSIGN_MEDICAL_HISTORY: (queueId) => `/queue-management/assign-medical-history/${queueId}`,
			CALL: (queueId) => `/queue-management/call/${queueId}`,
			UNCALL: (queueId) => `/queue-management/uncall/${queueId}`,
			START: (queueId) => `/queue-management/start/${queueId}`,
			AWAIT_RESULTS: (queueId) => `/queue-management/await-results/${queueId}`,
			COMPLETE: (queueId) => `/queue-management/complete/${queueId}`,
			CANCEL: (queueId) => `/queue-management/cancel/${queueId}`,
		},
	},
	BLOG: {
		INDEX: `/blog`,
		MANAGEMENT: {
			INDEX: `/blog-management`,
			DETAIL: (id) => `/blog-management/${id}`,
			DELETE_SELECTED: `/blog-management/selected`,
		},
	},
	BLOG_CATEGORY: {
		INDEX: `/blog-category`,
		GET_ALL: `/blog-category/all`,
		CREATE: `/blog-category`,
		UPDATE: (id) => `/blog-category/${id}`,
		DELETE: (id) => `/blog-category/${id}`,
		DELETE_SELECTED: `/blog-category/selected`,
		DETAIL: (id) => `/blog-category/${id}`,
	},
	TELE_SESSION: {
		DETAIL: (id) => `/tele-session/${id}`,
	},
	TELE_ROOM: {
		GET_ALL: `/tele-room/all`,
		GET_TELE_ROOM_FOR_DOCTOR: `/tele-room/room`,
		MANAGEMENT: {
			INDEX: `/tele-room-management`,
			GET_ALL: `/tele-room-management/all`,
			DETAIL: (id) => `/tele-room-management/${id}`,
		},
	},
	CART: {
		INDEX: `/cart`,
		CHECKOUT: `/cart/checkout`,
		ADD_TO_CART: `/cart/add-to-cart`,
		UPDATE: `/cart/update`,
		DELETE: (id) => `/cart/delete/${id}`,
	},
	VOUCHER: {
		COLLECTIBLE: `/voucher/collectible`,
		COLLECT: `/voucher/collect`,
		MY_VOUCHERS: `/voucher/my-vouchers`,
		MY_ALL_VOUCHERS: `/voucher/my-vouchers/all`,
		MANAGEMENT: {
			INDEX: `/voucher-management`,
			DETAIL: (id) => `/voucher-management/${id}`,
		},
	},
	PATIENT_VOUCHER: {
		INDEX: `/patient-voucher`,
		DETAIL: (id) => `/patient-voucher/${id}`,
	},
	SUPPLIER: {
		MANAGEMENT: {
			INDEX: `/supplier-management`,
			GET_ALL: `/supplier-management/all`,
			DETAIL: (id) => `/supplier-management/${id}`,
		},
	},
	FEEDBACK: {
		INDEX: `/feedback`,
		DETAIL: (id) => `/feedback/${id}`,
		GET_FEEDBACK_BY_MEDICINE: (medicineId) => `/feedback/medicine/${medicineId}`,
		MANAGEMENT: {
			INDEX: `/feedback-management`,
			DETAIL: (id) => `/feedback-management/${id}`,
		},
	},
	FEEDBACK_RESPOND: {
		INDEX: `/feedback-respond`,
		DETAIL: (id) => `/feedback-respond/${id}`,
	},
	REPORT: {
		MANAGER_DASHBOARD_STATISTICS: `/report/manager-dashboard/statistics`,
		ACCOUNTANT_DASHBOARD_STATISTICS: `/report/accountant-dashboard/statistics`,
		HR_DASHBOARD_STATISTICS: `/report/hr-dashboard/statistics`,
		INVENTORY_CLERK_DASHBOARD_STATISTICS: `/report/inventory-clerk-dashboard/statistics`,
		PHARMACIST_DASHBOARD_STATISTICS: `/report/pharmacist-dashboard/statistics`,
		MANAGER_DASHBOARD_EXPORT: `/report/manager-dashboard/export`,
		ACCOUNTANT_DASHBOARD_EXPORT: `/report/accountant-dashboard/export`,
		HR_DASHBOARD_EXPORT: `/report/hr-dashboard/export`,
		INVENTORY_CLERK_DASHBOARD_EXPORT: `/report/inventory-clerk-dashboard/export`,
		PHARMACIST_DASHBOARD_EXPORT: `/report/pharmacist-dashboard/export`,
	},
	FEEDBACK_REPORT: {
		INDEX: `/feedback-report`,
		DETAIL: (id) => `/feedback-report/${id}`,
		ME: `/feedback-report/me`,
		MANAGEMENT: {
			INDEX: `/feedback-report-management`,
			RESOLVE: (id) => `/feedback-report-management/resolve/${id}`,
			REJECT: (id) => `/feedback-report-management/reject/${id}`,
		},
	},
	ORDER_HISTORY: {
		INDEX: `/order/all`,
		DETAIL: (id) => `/order/${id}`,
		CANCEL: (id) => `/order/cancel/${id}`,
		REPAY: (id) => `/order/repay/${id}`,
	},
	PAYMENT: {
		CANCEL_PAYMENT: (id) => `/pay-os/cancel-payment/${id}`,
		COMPLETE_PAYMENT: (id) => `/pay-os/complete-payment/${id}`,
	},
	ATTENDANCE: {
		MANAGEMENT: {
			INDEX: `/attendance-management`,
		},
		STAFF_ATTENDANCE: {
			INDEX: `/staff-attendance`,
			STATS: `/staff-attendance/stats`,
			CHECK_IN: `/staff-attendance/check-in`,
			CHECK_OUT: `/staff-attendance/check-out`,
			CHECK_IDENTIFICATION: `/staff-attendance/check-identification`,
			RESET_IDENTIFICATION: `/staff-attendance/reset-identification`,
			ENROLL: `/staff-attendance/enroll`,
		},
	},
	SHIFT: {
		GET_ALL: `/shift/all`,
		MANAGEMENT: {
			INDEX: `/shift-management`,
			GET_ALL: `/shift-management/all`,
			DETAIL: (id) => `/shift-management/${id}`,
			DELETE_SELECTED: `/shift-management/selected`,
		},
	},
	WORK_SCHEDULE: {
		STAFF: `/work-schedule/staff`,
		ALL_DOCTOR: `/work-schedule/all/doctor`,
		MANAGEMENT: {
			INDEX: `/work-schedule-management`,
			DATE_RANGE: `/work-schedule-management/date-range`,
			GET_ALL: `/work-schedule-management/all`,
			DETAIL: (id) => `/work-schedule-management/${id}`,
			PUBLISH: `/work-schedule-management/publish`,
			LOCK: `/work-schedule-management/lock`,
			FINALIZE: `/work-schedule-management/finalize`,
		},
	},
	WORK_SCHEDULE_ASSIGNMENT: {
		MANAGEMENT: {
			INDEX: `/work-schedule-assignment-management`,
			DETAIL: (id) => `/work-schedule-assignment-management/${id}`,
		},
	},
	WORK_SEGMENT: {
		MANAGEMENT: {
			DETAIL: (id) => `/work-segment-management/${id}`,
		},
	},
	WORK_SCHEDULE_TEMPLATE: {
		MANAGEMENT: {
			INDEX: `/work-schedule-template-management`,
			GET_ALL: `/work-schedule-template-management/all`,
			DETAIL: (id) => `/work-schedule-template-management/${id}`,
			DELETE_SELECTED: `/work-schedule-template-management/selected`,
			DUPLICATE: (id) => `/work-schedule-template-management/duplicate/${id}`,
			DAY_SHIFTS: (templateId) => `/work-schedule-template-day-shift-management/${templateId}`,
			ROOM_ASSIGNMENTS: (id) => `/work-schedule-template-assignment-management/${id}/rooms`,
			TELE_ROOM_ASSIGNMENTS: (id) => `/work-schedule-template-assignment-management/${id}/tele-rooms`,
		},
	},
	LEAVE_REQUEST: {
		INDEX: `/leave-request`,
		DETAIL: (id) => `/leave-request/${id}`,
		CANCEL: (id) => `/leave-request/cancel/${id}`,
		MANAGEMENT: {
			INDEX: `/leave-request-management`,
			DETAIL: (id) => `/leave-request-management/${id}`,
			APPROVE: (id) => `/leave-request-management/approve/${id}`,
			REJECT: (id) => `/leave-request-management/reject/${id}`,
		},
	},
	SALARY_ADVANCE: {
		INDEX: `/salary-advance`,
		DETAIL: (id) => `/salary-advance/${id}`,
		MY_REQUESTS: `/salary-advance/my-requests`,
		CANCEL: (id) => `/salary-advance/cancel/${id}`,
		MANAGEMENT: {
			INDEX: `/salary-advance-management`,
			DETAIL: (id) => `/salary-advance-management/${id}`,
			APPROVE: `/salary-advance-management/approve`,
			REJECT: `/salary-advance-management/reject`,
		},
	},
	OVERTIME_REQUEST: {
		INDEX: `/overtime-request`,
		DETAIL: (id) => `/overtime-request/${id}`,
		CANCEL: (id) => `/overtime-request/cancel/${id}`,
		MANAGEMENT: {
			INDEX: `/overtime-request-management`,
			DETAIL: (id) => `/overtime-request-management/${id}`,
			APPROVE: (id) => `/overtime-request-management/approve/${id}`,
			REJECT: (id) => `/overtime-request-management/reject/${id}`,
		},
	},
	ROOM: {
		MANAGEMENT: {
			INDEX: `/room-management`,
			GET_ALL: `/room-management/all`,
			DETAIL: (id) => `/room-management/${id}`,
			DELETE_SELECTED: `/room-management/selected`,
		},
	},
	ROOM_CATEGORY: {
		MANAGEMENT: {
			INDEX: `/room-category-management`,
			GET_ALL: `/room-category-management/all`,
			DETAIL: (id) => `/room-category-management/${id}`,
			DELETE_SELECTED: `/room-category-management/selected`,
		},
	},
	BUILDING: {
		MANAGEMENT: {
			INDEX: `/building-management`,
			GET_ALL: `/building-management/all`,
			DETAIL: (id) => `/building-management/${id}`,
			DELETE_SELECTED: `/building-management/selected`,
		},
	},
	FLOOR: {
		MANAGEMENT: {
			INDEX: `/floor-management`,
			GET_ALL: `/floor-management/all`,
			DETAIL: (id) => `/floor-management/${id}`,
			DELETE_SELECTED: `/floor-management/selected`,
		},
	},
	ORDER: {
		MANAGEMENT: {
			INDEX: `/order-management`,
			CREATE: `/order-management`,
			SHIPPING: (id) => `/order-management/shipping/${id}`,
			PAID: (id) => `/order-management/paid/${id}`,
			COMPLETE: (id) => `/order-management/complete/${id}`,
		},
	},
	QUALIFICATION: {
		MANAGEMENT: {
			INDEX: `/qualification-management`,
			GET_ALL: `/qualification-management/all`,
			DETAIL: (id) => `/qualification-management/${id}`,
		},
	},
	BED: {
		MANAGEMENT: {
			INDEX: `/bed-management`,
			GET_ALL: `/bed-management/all`,
			DETAIL: (id) => `/bed-management/${id}`,
			DELETE_SELECTED: `/bed-management/selected`,
		},
	},
	BED_CATEGORY: {
		MANAGEMENT: {
			INDEX: `/bed-category-management`,
			GET_ALL: `/bed-category-management/all`,
			DETAIL: (id) => `/bed-category-management/${id}`,
			DELETE_SELECTED: `/bed-category-management/selected`,
		},
	},
	BED_OCCUPANCY: {
		ROOMS: `/bed-occupancy/rooms`,
		ASSIGN: `/bed-occupancy/assign`,
		TRANSFER: `/bed-occupancy/transfer`,
	},
	INTERVIEW_TYPE: {
		MANAGEMENT: {
			INDEX: `/interview-type-management`,
			GET_ALL: `/interview-type-management/all`,
			DETAIL: (id) => `/interview-type-management/${id}`,
			DELETE_SELECTED: `/interview-type-management/selected`,
		},
	},
	INTERVIEW: {
		MANAGEMENT: {
			INDEX: `/interview-management`,
			DETAIL: (id) => `/interview-management/${id}`,
			DELETE: (id) => `/interview-management/${id}`,
			BY_JOB_APPLICATION: (jobApplicationId) =>
				`/interview-management/by-job-application/${jobApplicationId}`,
		},
	},
	JOB_POSTING: {
		INDEX: `/job-posting`,
		DETAIL: (id) => `/job-posting/${id}`,
		MANAGEMENT: {
			INDEX: `/job-posting-management`,
			DETAIL: (id) => `/job-posting-management/${id}`,
			DELETE_SELECTED: `/job-posting-management/selected`,
			OPEN: (id) => `/job-posting-management/open/${id}`,
			CLOSE: (id) => `/job-posting-management/close/${id}`,
		},
	},
	JOB_APPLICATION: {
		MANAGEMENT: {
			INDEX: `/job-application-management`,
			DETAIL: (id) => `/job-application-management/${id}`,
			PASS: (id) => `/job-application-management/pass/${id}`,
			FAIL: (id) => `/job-application-management/fail/${id}`,
			REJECT_ALL: (jobPostingId) => `/job-application-management/reject-all/${jobPostingId}`,
		},
	},
	HOLIDAY: {
		MANAGEMENT: {
			INDEX: `/holiday-management`,
			GET_ALL: `/holiday-management/all`,
			DETAIL: (id) => `/holiday-management/${id}`,
			DELETE_SELECTED: `/holiday-management/selected`,
			RANGE: `/holiday-management/range`,
		},
	},
	STAFF_CERTIFICATE_TYPE: {
		MANAGEMENT: {
			INDEX: `/staff-certificate-type-management`,
			GET_ALL: `/staff-certificate-type-management/all`,
			DETAIL: (id) => `/staff-certificate-type-management/${id}`,
			DELETE_SELECTED: `/staff-certificate-type-management/selected`,
		},
	},
	STAFF_CERTIFICATE: {
		MANAGEMENT: {
			INDEX: `/staff-certificate-management`,
			GET_ALL: `/staff-certificate-management/all`,
			DETAIL: (id) => `/staff-certificate-management/${id}`,
			DELETE_SELECTED: `/staff-certificate-management/selected`,
			SUSPEND: (id) => `/staff-certificate-management/suspend/${id}`,
			ACTIVE: (id) => `/staff-certificate-management/active/${id}`,
			APPROVE: (id) => `/staff-certificate-management/approve/${id}`,
		},
	},
	UPLOAD: {
		SINGLE_IMAGE: (folder) => `/upload?folder=${folder}`,
	},
}

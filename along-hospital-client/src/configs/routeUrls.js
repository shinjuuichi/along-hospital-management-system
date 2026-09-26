export const routeUrls = {
	BASE_ROUTE: {
		AUTH: (route = '') => `/auth${route}`,
		STAFF: (route = '') => `/staff${route}`,
		MANAGER: (route = '') => `/manager${route}`,
		PATIENT: (route = '') => `/patient${route}`,
		DOCTOR: (route = '') => `/doctor${route}`,
		NURSE: (route = '') => `/nurse${route}`,
		HR: (route = '') => `/hr${route}`,
		PHARMACIST: (route = '') => `/pharmacist${route}`,
		ACCOUNTANT: (route = '') => `/accountant${route}`,
		MARKETER: (route = '') => `/marketer${route}`,
		RECEPTIONIST: (route = '') => `/receptionist${route}`,
		HOTLINE_AGENT: (route = '') => `/hotline-agent${route}`,
		INVENTORY_CLERK: (route = '') => `/inventory-clerk${route}`,
	},
	HOME: {
		INDEX: '/',
		MEDICAL_SERVICE: '/medical-service',
		MEDICINE: '/medicine',
		SPECIALTY: '/specialty',
		DOCTOR: '/doctors',
		BLOG: '/blog',
		ABOUT_US: '/about-us',
		CONTACT: '/contact',
		TERMS_OF_SERVICE: '/terms-of-service',
		PRIVACY_POLICY: '/privacy-policy',
		CAREER: '/career',
		FAQ: '/faq',
		VOUCHERS: '/vouchers',
		INVOICE_PRINT: (id) => `/invoice/print/${id}`,
		INVOICE_REFUND_PRINT: (invoiceId, chargeId) => `/invoice/print/${invoiceId}/refund/${chargeId}`,
		PRESCRIPTION_PRINT: (id) => `/prescription/print/${id}`,
		PAYMENT: {
			RETURN: '/payment/return',
			CANCEL: '/payment/cancel',
			SEPAY: '/payment/sepay',
		},
		QUEUE: {
			INDEX: '/queue',
			CREATE: '/queue/create',
			CREATE_FROM_QR: '/queue/create-qr',
		},
		JOB_POSTING: {
			INDEX: '/job-posting',
			DETAIL: (id) => `/job-posting/${id}`,
		},
	},
	AUTH: {
		LOGIN: '/login',
		REGISTER: '/register',
		FORGOT_PASSWORD: '/forgot-password',
		FORGOT_PASSWORD_METHOD: '/forgot-password/method',
		VERIFY: '/verify',
		RESEND: '/resend',
		RESEND_LINK: '/resend',
		RESET_PASSWORD: '/reset-password',
		VERIFY_RESET_PASSWORD: '/verify-reset-password',
		COMPLETE_PROFILE: '/complete-profile',
	},
	STAFF: {
		ATTENDANCE: '/attendance',
		WORK_SCHEDULE: '/work-schedule',
		SALARY_ADVANCE: '/salary-advance',
		PROFILE: '/profile',
		MEDICAL_HISTORY: {
			INDEX: '/medical-history',
			DETAIL: (id) => `/medical-history/${id}`,
			CREATE: '/medical-history/create',
		},
		QUEUE_MANAGEMENT: {
			INDEX: '/queue-management',
			DETAIL: (roomId) => `/queue-management/${roomId}`,
		},
		IDENTIFICATION: {
			ENROLL: '/identification/enroll',
		},
	},
	PATIENT: {
		CART: '/cart',
		CHECKOUT: '/checkout',
		PROFILE: '/profile',
		FEEDBACK: '/feedback',
		MEDICAL_HISTORY: {
			INDEX: '/medical-history',
			DETAIL: (id) => `/medical-history/${id}`,
		},
		ORDER_HISTORY: {
			INDEX: '/order-history',
			DETAIL: (id) => `/order-history/${id}`,
		},
		APPOINTMENT: {
			JOIN_MEETING_ROOM: `/appointments/join-meeting-room`,
			MEETING_ROOM_TOKEN: (id) => `/appointments/meeting-room-token/${id}`,
			MEETING_ROOM_COMPLETE: (id) => `/appointments/meeting-room-token/${id}/complete`,
			INDEX: '/appointment',
			CREATE: '/appointment/create',
		},
		VIDEO_CONSULTATION: '/video-consultation',
		VOUCHER: {
			MY_VOUCHERS: '/my-vouchers',
		},
	},
	DOCTOR: {
		DASHBOARD: '/',
		APPOINTMENT_MANAGEMENT: '/appointment',
		MEDICAL_HISTORY: '/medical-history',
		APPOINTMENT: {
			JOIN_MEETING_ROOM: `/appointments/join-meeting-room`,
		},
	},
	NURSE: {
		DASHBOARD: '/',
		BED_OCCUPANCY_MANAGEMENT: {
			INDEX: '/bed-occupancy-management',
		},
	},
	HR: {
		DASHBOARD: '/',
		PROFILE: '/profile',
		STAFF_MANAGEMENT: '/staff',
		STAFF_GROUP_MANAGEMENT: '/staff-group',
		STAFF_CONTRACT_MANAGEMENT: '/staff-contract',
		LEAVE_REQUEST_MANAGEMENT: '/leave-request-management',
		SHIFT_MANAGEMENT: '/shift-management',
		WORK_SCHEDULE_MANAGEMENT: {
			INDEX: '/work-schedule',
			DETAIL: (id) => `/work-schedule/${id}`,
		},
		WORK_SCHEDULE_TEMPLATE_MANAGEMENT: {
			INDEX: '/work-schedule-template',
			DETAIL: (id) => `/work-schedule-template/${id}`,
		},
		HOLIDAY_MANAGEMENT: '/holiday-management',
		INTERVIEW_TYPE_MANAGEMENT: '/interview-type-management',
		STAFF_CERTIFICATE_TYPE_MANAGEMENT: '/staff-certificate-type',
		STAFF_CERTIFICATE_MANAGEMENT: '/staff-certificate',
		JOB_POSTING: {
			INDEX: '/job-posting',
			CREATE: '/job-posting/create',
			UPDATE: (id) => `/job-posting/edit/${id}`,
			DETAIL: (id) => `/job-posting/${id}`,
			APPLICATION_DETAIL: (jobPostingId = ':jobPostingId', applicationId = ':applicationId') =>
				`/job-posting/${jobPostingId}/application/${applicationId}`,
		},
	},
	PHARMACIST: {
		DASHBOARD: '/',
		MEDICINE_MANAGEMENT: {
			INDEX: '/medicine-management',
			DETAIL: (id) => `/medicine-management/${id}`,
		},
		MEDICINE_SKU_MANAGEMENT: {
			INDEX: '/medicine-sku',
		},
		MEDICINE_UNIT_MANAGEMENT: {
			INDEX: '/medicine-unit',
			DETAIL: (id) => `/medicine-unit/${id}`,
		},
		MEDICINE_CATEGORY_MANAGEMENT: {
			INDEX: '/medicine-category',
		},
		OPTION_MANAGEMENT: {
			INDEX: '/option',
		},
		OPTION_VALUE_MANAGEMENT: {
			INDEX: '/option-value',
		},
		IMPORT_REQUEST: {
			INDEX: '/import-request',
		},
		ORDER_MANAGEMENT: '/order-management',
	},
	ACCOUNTANT: {
		DASHBOARD: '/',
		INVOICE: {
			INDEX: '/invoice',
		},
		SALARY_ADVANCE_MANAGEMENT: '/salary-advance-management',
		ALLOWANCE_TYPE: '/allowance-type',
		DEDUCTION_TYPE: '/deduction-type',
		TAX_BRACKET: '/tax-bracket',
		GLOBAL_TAX_CONFIG: '/global-tax-config',
		PAYROLL_POLICY: '/payroll-policy',
		PAYROLL: {
			INDEX: '/payroll',
			DETAIL: (id) => `/payroll/${id}`,
			PRINT: (id) => `/payroll/print/${id}`,
		},
		REGIONAL_WAGE_MANAGEMENT: '/regional-wage',
	},
	MARKETER: {
		DASHBOARD: '/',
		BLOG: {
			INDEX: '/blogs',
			CREATE: '/blogs/create',
			UPDATE: (blogId = ':id') => `/blogs/edit/${blogId}`,
		},
		BLOG_CATEGORY_MANAGEMENT: {
			INDEX: '/blog-category',
		},
	},
	RECEPTIONIST: {
		DASHBOARD: '/',
		TIME_SLOT_MANAGEMENT: '/time-slot',
	},
	HOTLINE_AGENT: {
		DASHBOARD: '/',
		FEEDBACK_MANAGEMENT: '/feedback',
		FEEDBACK_REPORT_MANAGEMENT: '/feedback-report',
		COMPLAINT_MANAGEMENT: '/complaint',
	},
	INVENTORY_CLERK: {
		DASHBOARD: '/',
		SUPPLIER_MANAGEMENT: '/supplier',
		IMPORT_REQUEST: {
			INDEX: '/import-request',
		},
		IMPORT_MANAGEMENT: {
			INDEX: '/import-management',
		},
	},
	MANAGER: {
		DASHBOARD: '/',
		PAYROLL: {
			INDEX: '/payroll',
		},
		BLOG: {
			INDEX: '/blogs',
			CREATE: '/blogs/create',
			UPDATE: (blogId = ':id') => `/blogs/edit/${blogId}`,
		},
		BLOG_CATEGORY_MANAGEMENT: {
			INDEX: '/blog-category',
		},

		SPECIALTY_MANAGEMENT: '/specialty',
		MEDICAL_SERVICE_MANAGEMENT: '/medical-service',
		VOUCHER_MANAGEMENT: '/voucher',
		ATTENDANCE_MANAGEMENT: '/attendance',
		TELE_ROOM_MANAGEMENT: '/tele-room',

		STAFF_CONTRACT_MANAGEMENT: '/staff-contract',

		QUALIFICATION_MANAGEMENT: '/qualification',
		BED_MANAGEMENT: {
			INDEX: '/bed',
		},
		BED_CATEGORY_MANAGEMENT: {
			INDEX: '/bed-category',
		},
		ROOM_MANAGEMENT: {
			INDEX: '/room',
			DETAIL: (id) => `/room/${id}`,
		},
		ROOM_CATEGORY_MANAGEMENT: {
			INDEX: '/room-category',
		},
		BUILDING_MANAGEMENT: {
			INDEX: '/building',
			DETAIL: (id) => `/building/${id}`,
		},
		LEAVE_REQUEST_MANAGEMENT: '/leave-request-management',
		SALARY_ADVANCE_MANAGEMENT: '/salary-advance-management',
	},
}

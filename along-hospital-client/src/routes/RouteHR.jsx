import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutHR from '@/layouts/LayoutHR'
import AllowanceTypeManagementPage from '@/pages/accountants/allowanceTypeManagementPage/AllowanceTypeManagementPage'
import DeductionTypeManagementPage from '@/pages/accountants/deductionTypeManagementPage/DeductionTypeManagementPage'
import GlobalTaxConfigManagementPage from '@/pages/accountants/globalTaxConfigManagementPage/GlobalTaxConfigManagementPage'
import PayrollManagementPage from '@/pages/accountants/payrollManagementPage/PayrollManagementPage'
import PayrollPrintPage from '@/pages/accountants/payrollManagementPage/PayrollPrintPage'
import PayrollPolicyManagementPage from '@/pages/accountants/payrollPolicyManagementPage/PayrollPolicyManagementPage'
import TaxBracketManagementPage from '@/pages/accountants/taxBracketManagementPage/TaxBracketManagementPage'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import ProfilePage from '@/pages/commons/profile/ProfilePage'
import HolidayManagementPage from '@/pages/hrs/holidayManagementPage/HolidayManagementPage'
import HRDashboardPage from '@/pages/hrs/hrDashboardPage/HRDashboardPage'
import InterviewTypeManagementPage from '@/pages/hrs/interviewTypeManagementPage/InterviewTypeManagementPage'
import JobPostingManagementPage from '@/pages/hrs/jobPostingManagementPage/JobPostingManagementPage'
import JobPostingUpsertPage from '@/pages/hrs/jobPostingManagementPage/JobPostingUpsertPage'
import LeaveRequestManagementPage from '@/pages/hrs/leaveRequestManagementPage/LeaveRequestManagementPage'
import ShiftManagementPage from '@/pages/hrs/shiftManagementPage/ShiftManagementPage'
import StaffCertificateManagementPage from '@/pages/hrs/staffCertificateManagementPage/StaffCertificateManagementPage'
import StaffCertificateTypeManagementPage from '@/pages/hrs/staffCertificateTypeManagementPage/StaffCertificateTypeManagementPage'
import HRStaffContractManagementPage from '@/pages/hrs/staffContractManagementPage/HRStaffContractManagementPage'
import StaffGroupManagementPage from '@/pages/hrs/staffGroupManagementPage/StaffGroupManagementPage'
import StaffManagementPage from '@/pages/hrs/staffManagementPage/StaffManagementPage'
import WorkScheduleManagementDetailPage from '@/pages/hrs/workScheduleManagementPage/WorkScheduleManagementDetailPage'
import WorkScheduleManagementPage from '@/pages/hrs/workScheduleManagementPage/WorkScheduleManagementPage'
import WorkScheduleTemplateManagementDetailPage from '@/pages/hrs/workScheduleTemplateManagementPage/WorkScheduleTemplateManagementDetailPage'
import WorkScheduleTemplateManagementPage from '@/pages/hrs/workScheduleTemplateManagementPage/WorkScheduleTemplateManagementPage'
import JobApplicationDetailPage from '@/pages/hrs/jobPostingManagementPage/JobApplicationDetailPage'
import JobPostingDetailPage from '@/pages/hrs/jobPostingManagementPage/JobPostingDetailPage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteHR = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.HR]} />}>
				<Route element={<LayoutHR />}>
					<Route path={routeUrls.HR.DASHBOARD} element={<HRDashboardPage />} />
					<Route path={routeUrls.ACCOUNTANT.ALLOWANCE_TYPE} element={<AllowanceTypeManagementPage />} />
					<Route path={routeUrls.ACCOUNTANT.DEDUCTION_TYPE} element={<DeductionTypeManagementPage />} />
					<Route path={routeUrls.ACCOUNTANT.TAX_BRACKET} element={<TaxBracketManagementPage />} />
					<Route
						path={routeUrls.ACCOUNTANT.GLOBAL_TAX_CONFIG}
						element={<GlobalTaxConfigManagementPage />}
					/>
					<Route path={routeUrls.ACCOUNTANT.PAYROLL_POLICY} element={<PayrollPolicyManagementPage />} />
					<Route path={routeUrls.ACCOUNTANT.PAYROLL.INDEX} element={<PayrollManagementPage />} />
					<Route path={routeUrls.ACCOUNTANT.PAYROLL.PRINT(':id')} element={<PayrollPrintPage />} />
					<Route path={routeUrls.HR.PROFILE} element={<ProfilePage />} />
					<Route path={routeUrls.HR.STAFF_MANAGEMENT} element={<StaffManagementPage />} />
					<Route path={routeUrls.HR.STAFF_GROUP_MANAGEMENT} element={<StaffGroupManagementPage />} />
					<Route
						path={routeUrls.HR.STAFF_CONTRACT_MANAGEMENT}
						element={<HRStaffContractManagementPage />}
					/>
					<Route path={routeUrls.HR.LEAVE_REQUEST_MANAGEMENT} element={<LeaveRequestManagementPage />} />
					<Route path={routeUrls.HR.SHIFT_MANAGEMENT} element={<ShiftManagementPage />} />
					<Route
						path={routeUrls.HR.WORK_SCHEDULE_TEMPLATE_MANAGEMENT.INDEX}
						element={<WorkScheduleTemplateManagementPage />}
					/>
					<Route
						path={routeUrls.HR.WORK_SCHEDULE_TEMPLATE_MANAGEMENT.DETAIL(':id')}
						element={<WorkScheduleTemplateManagementDetailPage />}
					/>
					<Route
						path={routeUrls.HR.WORK_SCHEDULE_MANAGEMENT.INDEX}
						element={<WorkScheduleManagementPage />}
					/>
					<Route
						path={routeUrls.HR.WORK_SCHEDULE_MANAGEMENT.DETAIL(':id')}
						element={<WorkScheduleManagementDetailPage />}
					/>
					<Route path={routeUrls.HR.HOLIDAY_MANAGEMENT} element={<HolidayManagementPage />} />
					<Route
						path={routeUrls.HR.INTERVIEW_TYPE_MANAGEMENT}
						element={<InterviewTypeManagementPage />}
					/>
					<Route path={routeUrls.HR.JOB_POSTING.INDEX} element={<JobPostingManagementPage />} />
					<Route path={routeUrls.HR.JOB_POSTING.CREATE} element={<JobPostingUpsertPage />} />
					<Route path={routeUrls.HR.JOB_POSTING.UPDATE(':id')} element={<JobPostingUpsertPage />} />
					<Route
						path={routeUrls.HR.STAFF_CERTIFICATE_TYPE_MANAGEMENT}
						element={<StaffCertificateTypeManagementPage />}
					/>
					<Route
						path={routeUrls.HR.STAFF_CERTIFICATE_MANAGEMENT}
						element={<StaffCertificateManagementPage />}
					/>
					<Route path={routeUrls.HR.JOB_POSTING.DETAIL(':id')} element={<JobPostingDetailPage />} />
					<Route
						path={routeUrls.HR.JOB_POSTING.APPLICATION_DETAIL(':jobPostingId', ':applicationId')}
						element={<JobApplicationDetailPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteHR

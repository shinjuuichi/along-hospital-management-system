import { EnumConfig } from '@/configs/enumConfig'
import { getLayoutByRole } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import ProfilePage from '@/pages/commons/profile/ProfilePage'
import CreateMedicalHistoryPage from '@/pages/receptionists/createMedicalHistoryPage/CreateMedicalHistoryPage'
import MedicalHistoryDetailPage from '@/pages/staffs/medicalHistoryDetailPage/MedicalHistoryDetailPage'
import QueueDetailManagementPage from '@/pages/staffs/queues/queueDetailManagementPage/QueueDetailManagementPage'
import QueueManagementPage from '@/pages/staffs/queues/queueManagementPage/QueueManagementPage'
import SalaryAdvancePage from '@/pages/staffs/salaryAdvancePage/SalaryAdvancePage'
import StaffAttendancePage from '@/pages/staffs/staffAttendancePage/StaffAttendancePage'
import StaffIdentificationEnrollPage from '@/pages/staffs/staffIdentificationEnrollPage/StaffIdentificationEnrollPage'
import StaffMedicalHistoryManagementPage from '@/pages/staffs/staffMedicalHistoryManagementPage/StaffMedicalHistoryManagementPage'
import StaffWorkSchedulePage from '@/pages/staffs/staffWorkSchedulePage/StaffWorkSchedulePage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteStaff = () => {
	const { auth } = useAuth()
	const Layout = getLayoutByRole(auth.role)
	const { Guest, Patient, ...staffRole } = EnumConfig.Role

	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[...Object.values(staffRole)]} />}>
				<Route element={<Layout />}>
					<Route path={routeUrls.STAFF.ATTENDANCE} element={<StaffAttendancePage />} />
					<Route path={routeUrls.STAFF.WORK_SCHEDULE} element={<StaffWorkSchedulePage />} />
					<Route path={routeUrls.STAFF.SALARY_ADVANCE} element={<SalaryAdvancePage />} />
					<Route path={routeUrls.STAFF.PROFILE} element={<ProfilePage />} />
					<Route
						path={routeUrls.STAFF.IDENTIFICATION.ENROLL}
						element={<StaffIdentificationEnrollPage />}
					/>
					<Route
						path={routeUrls.STAFF.MEDICAL_HISTORY.INDEX}
						element={<StaffMedicalHistoryManagementPage />}
					/>
					<Route
						element={
							<ProtectedRoute allowRoles={[EnumConfig.Role.Receptionist, EnumConfig.Role.Nurse]} />
						}
					>
						<Route path={routeUrls.STAFF.MEDICAL_HISTORY.CREATE} element={<CreateMedicalHistoryPage />} />
					</Route>
					<Route path={routeUrls.STAFF.QUEUE_MANAGEMENT.INDEX} element={<QueueManagementPage />} />
					<Route
						path={routeUrls.STAFF.QUEUE_MANAGEMENT.DETAIL(':roomId')}
						element={<QueueDetailManagementPage />}
					/>
					<Route
						path={routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(':id')}
						element={<MedicalHistoryDetailPage />}
					/>
				</Route>
			</Route>
			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteStaff

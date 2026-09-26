import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutManager from '@/layouts/LayoutManager'
import PayrollManagementPage from '@/pages/accountants/payrollManagementPage/PayrollManagementPage'
import SalaryAdvanceManagementPage from '@/pages/accountants/salaryAdvanceManagementPage/SalaryAdvanceManagementPage'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import ProfilePage from '@/pages/commons/profile/ProfilePage'
import LeaveRequestManagementPage from '@/pages/hrs/leaveRequestManagementPage/LeaveRequestManagementPage'
import BedCategoryManagementPage from '@/pages/managers/bedCategoryManagementPage/BedCategoryManagementPage'
import BuildingDetailPage from '@/pages/managers/buildingManagementPage/BuildingDetailPage'
import BuildingManagementPage from '@/pages/managers/buildingManagementPage/BuildingManagementPage'
import ManagerAttendanceManagementPage from '@/pages/managers/managerAttendanceManagementPage/ManagerAttendanceManagementPage'
import ManagerBedManagementPage from '@/pages/managers/managerBedManagementPage/ManagerBedManagementPage'
import ManagerDashboardPage from '@/pages/managers/managerDashboardPage/ManagerDashboardPage'
import ManagerMedicalServiceManagementPage from '@/pages/managers/managerMedicalServiceManagementPage/ManagerMedicalServiceManagemnetPage'
import QualificationManagementPage from '@/pages/managers/qualificationManagementPage/QualificationManagementPage'
import RoomCategoryManagementPage from '@/pages/managers/roomCategoryManagementPage/RoomCategoryManagementPage'
import RoomDetailPage from '@/pages/managers/roomManagementPage/RoomDetailPage'
import RoomManagementPage from '@/pages/managers/roomManagementPage/RoomManagementPage'
import SpecialtyManagementPage from '@/pages/managers/specialtyManagementPage/SpecialtyManagementPage'
import ManagerStaffContractManagementPage from '@/pages/managers/staffContractManagementPage/ManagerStaffContractManagementPage'
import TeleRoomManagementPage from '@/pages/managers/teleRoomManagementPage/TeleRoomManagementPage'
import VoucherManagementPage from '@/pages/managers/voucherManagementPage/VoucherManagementPage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteManager = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Manager]} />}>
				<Route element={<LayoutManager />}>
					<Route path={routeUrls.MANAGER.DASHBOARD} element={<ManagerDashboardPage />} />
					<Route path={routeUrls.MANAGER.PROFILE} element={<ProfilePage />} />
					<Route path={routeUrls.MANAGER.BED_MANAGEMENT.INDEX} element={<ManagerBedManagementPage />} />
					<Route
						path={routeUrls.MANAGER.BED_CATEGORY_MANAGEMENT.INDEX}
						element={<BedCategoryManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.MEDICAL_SERVICE_MANAGEMENT}
						element={<ManagerMedicalServiceManagementPage />}
					/>
					<Route path={routeUrls.MANAGER.SPECIALTY_MANAGEMENT} element={<SpecialtyManagementPage />} />

					<Route path={routeUrls.MANAGER.VOUCHER_MANAGEMENT} element={<VoucherManagementPage />} />
					<Route
						path={routeUrls.MANAGER.ATTENDANCE_MANAGEMENT}
						element={<ManagerAttendanceManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.STAFF_CONTRACT_MANAGEMENT}
						element={<ManagerStaffContractManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.QUALIFICATION_MANAGEMENT}
						element={<QualificationManagementPage />}
					/>
					<Route path={routeUrls.MANAGER.ROOM_MANAGEMENT.INDEX} element={<RoomManagementPage />} />
					<Route path={routeUrls.MANAGER.ROOM_MANAGEMENT.DETAIL(':id')} element={<RoomDetailPage />} />
					<Route path={routeUrls.MANAGER.TELE_ROOM_MANAGEMENT} element={<TeleRoomManagementPage />} />
					<Route
						path={routeUrls.MANAGER.ROOM_CATEGORY_MANAGEMENT.INDEX}
						element={<RoomCategoryManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.BUILDING_MANAGEMENT.INDEX}
						element={<BuildingManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.BUILDING_MANAGEMENT.DETAIL(':id')}
						element={<BuildingDetailPage />}
					/>
					<Route
						path={routeUrls.MANAGER.LEAVE_REQUEST_MANAGEMENT}
						element={<LeaveRequestManagementPage />}
					/>
					<Route
						path={routeUrls.MANAGER.SALARY_ADVANCE_MANAGEMENT}
						element={<SalaryAdvanceManagementPage />}
					/>
					<Route path={routeUrls.MANAGER.PAYROLL.INDEX} element={<PayrollManagementPage />} />
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteManager

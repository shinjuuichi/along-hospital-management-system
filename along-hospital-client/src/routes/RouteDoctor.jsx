import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutDoctor from '@/layouts/LayoutDoctor'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import DoctorAppointmentManagementPage from '@/pages/doctors/doctorAppointmentManagementPage/DoctorAppointmentManagementPage'
import DoctorMedicalHistoryManagementPage from '@/pages/doctors/doctorMedicalHistoryManagementPage/DoctorMedicalHistoryManagementPage'
import DoctorMeetingRoomPage from '@/pages/doctors/doctorMeetingRoomPage/DoctorMeetingRoomPage'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteDoctor = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Doctor]} />}>
				<Route element={<LayoutDoctor />}>
					<Route
						path='/'
						element={
							<Navigate
								to={routeUrls.BASE_ROUTE.DOCTOR(routeUrls.DOCTOR.APPOINTMENT_MANAGEMENT)}
								replace
							/>
						}
					/>
					<Route
						path={routeUrls.DOCTOR.APPOINTMENT_MANAGEMENT}
						element={<DoctorAppointmentManagementPage />}
					/>
					<Route
						path={routeUrls.DOCTOR.MEDICAL_HISTORY}
						element={<DoctorMedicalHistoryManagementPage />}
					/>
				</Route>
				<Route
					path={routeUrls.DOCTOR.APPOINTMENT.JOIN_MEETING_ROOM}
					element={<DoctorMeetingRoomPage />}
				/>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteDoctor

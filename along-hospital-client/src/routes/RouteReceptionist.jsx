import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutReceptionist from '@/layouts/LayoutReceptionist'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import TimeSlotManagementPage from '@/pages/receptionists/timeSlotManagementPage/TimeSlotManagementPage'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteReceptionist = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Receptionist]} />}>
				<Route element={<LayoutReceptionist />}>
					<Route
						path='/'
						element={
							<Navigate
								to={routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.INDEX)}
								replace
							/>
						}
					/>
					<Route
						path={routeUrls.RECEPTIONIST.TIME_SLOT_MANAGEMENT}
						element={<TimeSlotManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteReceptionist

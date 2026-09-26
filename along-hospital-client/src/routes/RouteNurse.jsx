import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import BedOccupancyManagementPage from '@/pages/nurses/bedOccupancyManagementPage/BedOccupancyManagementPage'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

import LayoutNurse from '@/layouts/LayoutNurse'

const RouteNurse = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Nurse]} />}>
				<Route element={<LayoutNurse />}>
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
						path={routeUrls.NURSE.BED_OCCUPANCY_MANAGEMENT.INDEX}
						element={<BedOccupancyManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteNurse

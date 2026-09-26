import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import MedicineCategoryManagementPage from '@/pages/pharmacists/medicineCategoryManagementPage/MedicineCategoryManagementPage'
import PharmacistDashboardPage from '@/pages/pharmacists/pharmacistDashboardPage/PharmacistDashboardPage'
import MedicineDetailPage from '@/pages/pharmacists/medicineDetailPage/MedicineDetailPage'
import MedicineManagementPage from '@/pages/pharmacists/medicineManagementPage/MedicineManagementPage'
import MedicineSkuManagementPage from '@/pages/pharmacists/medicineSkuManagementPage/MedicineSkuManagementPage'
import OrderManagementPage from '@/pages/pharmacists/orderManagementPage/OrderManagementPage'
import MedicineUnitDetailPage from '@/pages/pharmacists/medicineUnitDetailPage/MedicineUnitDetailPage'
import MedicineUnitManagementPage from '@/pages/pharmacists/medicineUnitManagementPage/MedicineUnitManagementPage'
import OptionManagementPage from '@/pages/pharmacists/optionManagementPage/OptionManagementPage'
import OptionValueManagementPage from '@/pages/pharmacists/optionValueManagementPage/OptionValueManagementPage'
import PharmacistImportRequestPage from '@/pages/pharmacists/pharmacistImportRequestPage/PharmacistImportRequestPage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

import LayoutPharmacist from '@/layouts/LayoutPharmacist'

const RoutePharmacist = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Pharmacist]} />}>
				<Route element={<LayoutPharmacist />}>
					<Route path='/' element={<PharmacistDashboardPage />} />
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_MANAGEMENT.INDEX}
						element={<MedicineManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_MANAGEMENT.DETAIL(':id')}
						element={<MedicineDetailPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_SKU_MANAGEMENT.INDEX}
						element={<MedicineSkuManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_UNIT_MANAGEMENT.INDEX}
						element={<MedicineUnitManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_UNIT_MANAGEMENT.DETAIL(':id')}
						element={<MedicineUnitDetailPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.MEDICINE_CATEGORY_MANAGEMENT.INDEX}
						element={<MedicineCategoryManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.OPTION_MANAGEMENT.INDEX}
						element={<OptionManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.OPTION_VALUE_MANAGEMENT.INDEX}
						element={<OptionValueManagementPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.IMPORT_REQUEST.INDEX}
						element={<PharmacistImportRequestPage />}
					/>
					<Route
						path={routeUrls.PHARMACIST.ORDER_MANAGEMENT}
						element={<OrderManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RoutePharmacist

import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutInventoryClerk from '@/layouts/LayoutInventoryClerk'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import InventoryClerkDashboardPage from '@/pages/inventoryClerks/inventoryClerkDashboardPage/InventoryClerkDashboardPage'
import InventoryClerkImportManagementPage from '@/pages/inventoryClerks/importManagementPage/InventoryClerkImportManagementPage'
import InventoryClerkImportRequestPage from '@/pages/inventoryClerks/importRequestPage/InventoryClerkImportRequestPage'
import SupplierManagementPage from '@/pages/inventoryClerks/supplierManagementPage/SupplierManagementPage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteInventoryClerk = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.InventoryClerk]} />}>
				<Route element={<LayoutInventoryClerk />}>
					<Route path='/' element={<InventoryClerkDashboardPage />} />
					<Route
						path={routeUrls.INVENTORY_CLERK.SUPPLIER_MANAGEMENT}
						element={<SupplierManagementPage />}
					/>
					<Route
						path={routeUrls.INVENTORY_CLERK.IMPORT_REQUEST.INDEX}
						element={<InventoryClerkImportRequestPage />}
					/>
					<Route
						path={routeUrls.INVENTORY_CLERK.IMPORT_MANAGEMENT.INDEX}
						element={<InventoryClerkImportManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteInventoryClerk

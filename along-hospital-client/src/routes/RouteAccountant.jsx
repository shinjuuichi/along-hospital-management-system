import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutAccountant from '@/layouts/LayoutAccountant'
import AccountantDashboardPage from '@/pages/accountants/accountantDashboardPage/AccountantDashboardPage'
import AllowanceTypeManagementPage from '@/pages/accountants/allowanceTypeManagementPage/AllowanceTypeManagementPage'
import DeductionTypeManagementPage from '@/pages/accountants/deductionTypeManagementPage/DeductionTypeManagementPage'
import GlobalTaxConfigManagementPage from '@/pages/accountants/globalTaxConfigManagementPage/GlobalTaxConfigManagementPage'
import PayrollManagementPage from '@/pages/accountants/payrollManagementPage/PayrollManagementPage'
import PayrollPrintPage from '@/pages/accountants/payrollManagementPage/PayrollPrintPage'
import PayrollPolicyManagementPage from '@/pages/accountants/payrollPolicyManagementPage/PayrollPolicyManagementPage'
import RegionalWageManagementPage from '@/pages/accountants/regionalWageManagementPage/RegionalWageManagementPage'
import SalaryAdvanceManagementPage from '@/pages/accountants/salaryAdvanceManagementPage/SalaryAdvanceManagementPage'
import TaxBracketManagementPage from '@/pages/accountants/taxBracketManagementPage/TaxBracketManagementPage'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import InvoiceManagementPage from '@/pages/receptionists/invoiceManagementPage/InvoiceManagementPage'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteAccountant = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Accountant]} />}>
				<Route element={<LayoutAccountant />}>
					<Route path={routeUrls.ACCOUNTANT.DASHBOARD} element={<AccountantDashboardPage />} />
					<Route path={routeUrls.ACCOUNTANT.INVOICE.INDEX} element={<InvoiceManagementPage />} />
					<Route
						path={routeUrls.ACCOUNTANT.SALARY_ADVANCE_MANAGEMENT}
						element={<SalaryAdvanceManagementPage />}
					/>
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
					<Route
						path={routeUrls.ACCOUNTANT.REGIONAL_WAGE_MANAGEMENT}
						element={<RegionalWageManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteAccountant

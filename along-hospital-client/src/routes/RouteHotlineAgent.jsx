import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import LayoutHotlineAgent from '@/layouts/LayoutHotlineAgent'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import ComplaintManagementPage from '@/pages/hotlineAgents/complaintManagementPage/ComplaintManagementPage'
import FeedbackReportManagementPage from '@/pages/hotlineAgents/feedbackReportManagementPage/FeedbackReportManagementPage'
import FeedbackManagementPage from '@/pages/managers/feedbackManagementPage/FeedbackManagementPage'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteHotlineAgent = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.HotlineAgent]} />}>
				<Route element={<LayoutHotlineAgent />}>
					<Route
						path='/'
						element={
							<Navigate
								to={routeUrls.BASE_ROUTE.HOTLINE_AGENT(routeUrls.HOTLINE_AGENT.FEEDBACK_MANAGEMENT)}
								replace
							/>
						}
					/>
					<Route
						path={routeUrls.HOTLINE_AGENT.FEEDBACK_MANAGEMENT}
						element={<FeedbackManagementPage />}
					/>
					<Route
						path={routeUrls.HOTLINE_AGENT.FEEDBACK_REPORT_MANAGEMENT}
						element={<FeedbackReportManagementPage />}
					/>
					<Route
						path={routeUrls.HOTLINE_AGENT.COMPLAINT_MANAGEMENT}
						element={<ComplaintManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteHotlineAgent

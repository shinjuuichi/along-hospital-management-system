import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import BlogCategoryManagementPage from '@/pages/marketers/blogCategoryManagementPage/BlogCategoryManagementPage'
import BlogManagementPage from '@/pages/marketers/blogManagementPage/BlogManagementPage'
import BlogUpsertPage from '@/pages/marketers/blogUpsertPage/BlogUpsertPage'
import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

import LayoutMarketer from '@/layouts/LayoutMarketer'

const RouteMarketer = () => {
	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[EnumConfig.Role.Marketer]} />}>
				<Route element={<LayoutMarketer />}>
					<Route
						path='/'
						element={
							<Navigate
								to={routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX)}
								replace
							/>
						}
					/>
					<Route path={routeUrls.MARKETER.BLOG.INDEX} element={<BlogManagementPage />} />
					<Route path={routeUrls.MARKETER.BLOG.CREATE} element={<BlogUpsertPage />} />
					<Route path={routeUrls.MARKETER.BLOG.UPDATE(':id')} element={<BlogUpsertPage />} />
					<Route
						path={routeUrls.MARKETER.BLOG_CATEGORY_MANAGEMENT.INDEX}
						element={<BlogCategoryManagementPage />}
					/>
				</Route>
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteMarketer

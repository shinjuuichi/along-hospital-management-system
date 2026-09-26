import SkeletonLoadingPage from '@/components/skeletons/SkeletonLoadingPage'
import { EnumConfig } from '@/configs/enumConfig'
import { getReturnUrlByRole } from '@/configs/roleBasedConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import { Navigate, Outlet, useLocation } from 'react-router-dom'

const ProtectedRoute = ({
	allowRoles = [],
	redirectPath = routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN),
}) => {
	const location = useLocation()
	const completeProfilePath = routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.COMPLETE_PROFILE)

	const { auth, initialized } = useAuth()

	const hasRole = (roles) => {
		if (!auth?.role) return false
		return roles.map((r) => String(r).toLowerCase()).includes(auth.role.toLowerCase())
	}

	if (!initialized) {
		return <SkeletonLoadingPage />
	}

	if (
		auth.stage &&
		auth.stage !== EnumConfig.AuthStage.Done &&
		location.pathname !== completeProfilePath
	) {
		return <Navigate to={completeProfilePath} replace />
	}

	if (allowRoles.length === 0) return <Outlet />

	if (!auth?.role) {
		return <Navigate to={redirectPath} replace state={{ from: location }} />
	}

	if (!hasRole(allowRoles)) {
		return <Navigate to={getReturnUrlByRole(auth?.role)} replace />
	}

	return <Outlet />
}

export default ProtectedRoute

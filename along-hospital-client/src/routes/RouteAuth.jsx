import { routeUrls } from '@/configs/routeUrls'
import LayoutAuth from '@/layouts/LayoutAuth'
import CompleteProfilePage from '@/pages/auths/CompleteProfilePage/CompleteProfilePage'
import ForgotPasswordMethodPage from '@/pages/auths/ForgotPasswordPage/ForgotPasswordMethodPage'
import ForgotPasswordPage from '@/pages/auths/ForgotPasswordPage/ForgotPasswordPage'
import ResetPasswordPage from '@/pages/auths/ForgotPasswordPage/ResetPasswordPage/ResetPasswordPage'
import VerifyResetPasswordPage from '@/pages/auths/ForgotPasswordPage/ResetPasswordPage/VerifyResetPasswordPage'
import LoginPage from '@/pages/auths/LoginPage/LoginPage'
import RegisterPage from '@/pages/auths/RegisterPage/RegisterPage'
import ResendVerificationPage from '@/pages/auths/RegisterPage/VerifyAccountPage/ResendVerificationPage'
import VerifyLinkPage from '@/pages/auths/RegisterPage/VerifyAccountPage/VerifyAccountPage'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import { Route, Routes } from 'react-router-dom'

const RouteAuth = () => {
	return (
		<Routes>
			<Route element={<LayoutAuth />}>
				<Route path={routeUrls.AUTH.LOGIN} element={<LoginPage />} />
				<Route path={routeUrls.AUTH.REGISTER} element={<RegisterPage />} />
				<Route path={routeUrls.AUTH.VERIFY} element={<VerifyLinkPage />} />
				<Route path={routeUrls.AUTH.RESEND} element={<ResendVerificationPage />} />
				<Route path={routeUrls.AUTH.FORGOT_PASSWORD} element={<ForgotPasswordPage />} />
				<Route path={routeUrls.AUTH.FORGOT_PASSWORD_METHOD} element={<ForgotPasswordMethodPage />} />
				<Route path={routeUrls.AUTH.RESET_PASSWORD} element={<ResetPasswordPage />} />
				<Route path={routeUrls.AUTH.VERIFY_RESET_PASSWORD} element={<VerifyResetPasswordPage />} />
				<Route path={routeUrls.AUTH.COMPLETE_PROFILE} element={<CompleteProfilePage />} />
			</Route>
			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteAuth

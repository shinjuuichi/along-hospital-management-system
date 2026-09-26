/* eslint-disable react-refresh/only-export-components */
/* eslint-disable react-hooks/exhaustive-deps */
import useReduxStore from '@/hooks/useReduxStore'
import { useLocalStorage } from '@/hooks/useStorage'
import { resetAuthStore, setAuthStore } from '@/redux/reducers/authReducer'
import { resetPatientStore } from '@/redux/reducers/patientReducer'
import { createContext, useEffect, useMemo } from 'react'
import { useDispatch } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { ApiUrls } from './apiUrls'
import axiosConfig from './axiosConfig'
import { routeUrls } from './routeUrls'

export const AuthContext = createContext(null)

const AuthProvider = ({ children }) => {
	const dispatch = useDispatch()
	const navigate = useNavigate()
	const [accessToken, setAccessToken, removeAccessToken] = useLocalStorage('accessToken')

	useEffect(() => {
		const fetchToken = async () => {
			if (accessToken) await authStore.fetch()
		}
		fetchToken()
	}, [accessToken])

	const authStore = useReduxStore({
		selector: (s) => s.auth,
		setStore: setAuthStore,
	})

	const login = async (accessToken) => {
		setAccessToken(accessToken)
	}

	const logout = async () => {
		try {
			await axiosConfig.post(ApiUrls.AUTH.LOGOUT)
		} finally {
			removeAccessToken()
			dispatch(resetAuthStore())
			dispatch(resetPatientStore())
			navigate(routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN))
		}
	}

	const initialized = useMemo(() => {
		if (accessToken === undefined) {
			return true
		}

		return Boolean(authStore.error) || Boolean(authStore.data.role) || authStore.initialized
	}, [accessToken, authStore.error, authStore.data.role, authStore.initialized])

	const value = useMemo(
		() => ({ auth: authStore.data, login, logout, initialized }),
		[authStore.data, initialized]
	)
	return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export default AuthProvider

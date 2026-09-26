import { AuthContext } from '@/configs/AuthProvider'
import { useContext } from 'react'

/**
 * @typedef {import('@/redux/store').RootState} RootState
 */

/**
 * @returns {{
 *  auth: RootState['auth'],
 * 	login: (accessToken: string) => Promise<void>,
 * 	logout: () => void,
 *  initialized: boolean
 * }}
 */
export default function useAuth() {
	const ctx = useContext(AuthContext)
	if (!ctx) throw new Error('useAuth must be used within AuthProvider')
	return ctx
}

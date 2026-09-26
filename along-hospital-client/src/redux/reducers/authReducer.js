import { ApiUrls } from '@/configs/apiUrls'
import { createSlice } from '@reduxjs/toolkit'

const initialState = {
	authId: 0,
	userId: 0,
	role: null,
	stage: null,
}

const authSlice = createSlice({
	name: 'auth',
	initialState,
	reducers: {
		setAuthStore: (state, action) => {
			return { ...state, ...action.payload }
		},
		setIsIdentificationExistAuthStore: (state, action) => {
			state.isIdentificationExist = action.payload
		},
		resetAuthStore: () => initialState,
	},
})

const { setAuthStore, setIsIdentificationExistAuthStore, resetAuthStore } = authSlice.actions

setAuthStore.defaultUrl = ApiUrls.AUTH.CURRENT_ACCOUNT

export { resetAuthStore, setAuthStore, setIsIdentificationExistAuthStore }
export default authSlice.reducer

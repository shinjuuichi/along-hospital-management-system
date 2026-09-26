import { ApiUrls } from '@/configs/apiUrls'
import { createSlice } from '@reduxjs/toolkit'

const initState = {
	cart: {},
	profile: {},
	orders: [],
	vouchers: [],
	doctors: [],
	specialties: [],
}

const patientSlice = createSlice({
	name: 'patient',
	initialState: initState,
	reducers: {
		setCartStore: (state, action) => {
			state.cart = action.payload
		},
		setProfileStore: (state, action) => {
			state.profile = action.payload
		},
		setOrdersStore: (state, action) => {
			state.orders = action.payload
		},
		setVouchersStore: (state, action) => {
			state.vouchers = action.payload
		},
		setDoctorsStore: (state, action) => {
			state.doctors = action.payload || []
		},
		setSpecialtiesStore: (state, action) => {
			state.specialties = action.payload || []
		},
		resetPatientStore: () => initState,
	},
})

const {
	setCartStore,
	setProfileStore,
	setOrdersStore,
	setVouchersStore,
	setDoctorsStore,
	setSpecialtiesStore,
	resetPatientStore,
} = patientSlice.actions

setCartStore.defaultUrl = ApiUrls.CART.INDEX
setProfileStore.defaultUrl = ApiUrls.USER.PROFILE
setDoctorsStore.defaultUrl = ApiUrls.STAFF.GET_BY_ROLE('doctor')
setSpecialtiesStore.defaultUrl = ApiUrls.SPECIALTY.GET_ALL

export {
	resetPatientStore,
	setCartStore,
	setDoctorsStore,
	setOrdersStore,
	setProfileStore,
	setSpecialtiesStore,
	setVouchersStore,
}

export default patientSlice.reducer

import { combineReducers } from '@reduxjs/toolkit'
import authReducer from './reducers/authReducer'
import managementReducer from './reducers/managementReducer'
import patientReducer from './reducers/patientReducer'

const rootReducer = combineReducers({
	auth: authReducer,
	patient: patientReducer,
	management: managementReducer,
})

export default rootReducer

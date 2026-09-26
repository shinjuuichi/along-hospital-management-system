import ManagementMedicalHistoryBasePage from '@/components/basePages/manageMedicalHistoryBasePage/ManagementMedicalHistoryBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import { setDoctorsStore, setPatientsStore } from '@/redux/reducers/managementReducer'
import { useState } from 'react'

const ManagerMedicalHistoryManagementPage = () => {
	const [filters, setFilters] = useState({
		medicalHistoryStatus: '',
		doctor: '',
		startDate: '',
		endDate: '',
		patientName: '',
		doctorName: '',
	})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedItem, setSelectedItem] = useState(null)

	const getMedicalHistories = useFetch(
		ApiUrls.MEDICAL_HISTORY.MANAGEMENT.INDEX,
		{
			...filters,
			page,
			pageSize,
		},
		[filters, page, pageSize]
	)
	const getPatientStore = useReduxStore({
		selector: (state) => state.management.patients,
		setStore: setPatientsStore,
		dataToGet: (patient) => patient.name,
	})
	const getDoctorStore = useReduxStore({
		selector: (state) => state.management.doctors,
		setStore: setDoctorsStore,
		dataToGet: (doctor) => doctor.name,
	})

	return (
		<ManagementMedicalHistoryBasePage
			items={getMedicalHistories.data?.collection}
			totalPage={getMedicalHistories.data?.totalPage}
			patientNames={getPatientStore.data || []}
			doctorNames={getDoctorStore.data || []}
			loading={getMedicalHistories.loading}
			filters={filters}
			setFilters={setFilters}
			page={page}
			setPage={setPage}
			pageSize={pageSize}
			setPageSize={setPageSize}
			selectedItem={selectedItem}
			setSelectedItem={setSelectedItem}
		/>
	)
}

export default ManagerMedicalHistoryManagementPage

import ManagementMedicalHistoryBasePage from '@/components/basePages/manageMedicalHistoryBasePage/ManagementMedicalHistoryBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import { setPatientsStore } from '@/redux/reducers/managementReducer'
import { useState } from 'react'

const StaffMedicalHistoryManagementPage = () => {
	const [filters, setFilters] = useState({
		medicalHistoryStatus: '',
		doctor: '',
		startDate: '',
		endDate: '',
		patientName: '',
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
		dataToGet: (patients) => patients.map((patient) => patient.name),
	})

	return (
		<ManagementMedicalHistoryBasePage
			items={getMedicalHistories.data?.collection}
			totalPage={getMedicalHistories.data?.totalPage}
			patientNames={getPatientStore.data || []}
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

export default StaffMedicalHistoryManagementPage

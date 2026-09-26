import ManagementMedicalHistoryBasePage from '@/components/basePages/manageMedicalHistoryBasePage/ManagementMedicalHistoryBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import { setDoctorsStore } from '@/redux/reducers/managementReducer'
import { Box } from '@mui/material'
import { useState } from 'react'

const PatientMedicalHistoryPage = () => {
	const [filters, setFilters] = useState({
		medicalHistoryStatus: '',
		doctor: '',
		startDate: '',
		endDate: '',
		doctorName: '',
	})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedItem, setSelectedItem] = useState(null)

	const getMedicalHistories = useFetch(
		ApiUrls.MEDICAL_HISTORY.INDEX,
		{
			...filters,
			page,
			pageSize,
		},
		[filters, page, pageSize]
	)
	const getDoctorStore = useReduxStore({
		selector: (state) => state.management.doctors,
		setStore: setDoctorsStore,
		dataToGet: (doctor) => doctor.name,
	})

	return (
		<Box my={{ xs: 2, md: 4 }}>
			<ManagementMedicalHistoryBasePage
				items={getMedicalHistories.data?.collection}
				totalPage={getMedicalHistories.data?.totalPage}
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
		</Box>
	)
}

export default PatientMedicalHistoryPage

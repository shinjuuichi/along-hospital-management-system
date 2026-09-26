import ManageAppointmentBasePage from '@/components/basePages/manageAppointmentBasePage/ManageAppointmentBasePage'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { useState } from 'react'

const DoctorAppointmentManagementPage = () => {
	const [selectedAppointment, setSelectedAppointment] = useState(null)
	const [filters, setFilters] = useState({})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(6)

	const { t } = useTranslation()

	const getAppointments = useFetch(
		ApiUrls.APPOINTMENT.MANAGEMENT.INDEX,
		{ ...filters, page, pageSize },
		[filters, page, pageSize]
	)
	const specialtiesStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	return (
		<ManageAppointmentBasePage
			headerTitle={t('appointment.title.appointment_management')}
			filters={filters}
			setFilters={setFilters}
			page={page}
			setPage={setPage}
			pageSize={pageSize}
			setPageSize={setPageSize}
			selectedAppointment={selectedAppointment}
			setSelectedAppointment={setSelectedAppointment}
			totalPage={getAppointments.data?.totalPage || 1}
			appointments={getAppointments.data?.collection || []}
			specialties={specialtiesStore.data || []}
			loading={getAppointments.loading}
		/>
	)
}

export default DoctorAppointmentManagementPage

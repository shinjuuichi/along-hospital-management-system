import ManageAppointmentBasePage from '@/components/basePages/manageAppointmentBasePage/ManageAppointmentBasePage'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { Box, Button, Stack } from '@mui/material'
import { useState } from 'react'

const PatientAppointmentHistoryPage = () => {
	const [selectedAppointment, setSelectedAppointment] = useState(null)
	const [filters, setFilters] = useState({})
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(6)

	const { t } = useTranslation()

	const getAppointments = useFetch(
		ApiUrls.APPOINTMENT.INDEX,
		{
			...filters,
			page,
			pageSize,
		},
		[filters, page, pageSize]
	)

	const getPaymentUrl = useAxiosSubmit({
		url: ApiUrls.APPOINTMENT.GET_PAYMENT_URL(selectedAppointment?.id),
		method: 'GET',
		onSuccess: async (response) => {
			window.open(response.data, '_blank')
		},
	})

	const specialtiesStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	const cancelAppointment = useAxiosSubmit({
		url: ApiUrls.APPOINTMENT.CANCEL(selectedAppointment?.id),
		method: 'PUT',
		onSuccess: async () => {
			setSelectedAppointment(null)
			await getAppointments.fetch()
		},
	})

	return (
		<Box my={{ xs: 2, md: 4 }}>
			<ManageAppointmentBasePage
				headerTitle={t('appointment.title.appointment_history')}
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
				drawerButtons={
					selectedAppointment?.appointmentStatus === EnumConfig.AppointmentStatus.Scheduled && (
						<Stack direction='row' gap={2}>
							<ConfirmationButton
								confirmButtonText={t('appointment.button.cancel_appointment')}
								confirmButtonColor='error'
								confirmationTitle={t('appointment.dialog.confirm_cancel_title')}
								confirmationDescription={t('appointment.dialog.confirm_cancel_description')}
								onConfirm={async () => await cancelAppointment.submit()}
								color='error'
								variant='contained'
							>
								{t('appointment.button.cancel_appointment')}
							</ConfirmationButton>
							{selectedAppointment?.appointmentPaymentStatus ===
								EnumConfig.AppointmentPaymentStatus.Pending && (
								<Button variant='outlined' onClick={getPaymentUrl.submit}>
									{t('appointment.button.pay')}
								</Button>
							)}
						</Stack>
					)
				}
			/>
		</Box>
	)
}

export default PatientAppointmentHistoryPage

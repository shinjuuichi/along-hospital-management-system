import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import CreateAppointmentSuccessfullyDialog from '@/pages/patients/createAppointmentPage/sections/CreateAppointmentSuccessfullyDialog'
import LeftCreateAppointmentSection from '@/pages/patients/createAppointmentPage/sections/LeftCreateAppointmentSection'
import RightCreateAppointmentSection from '@/pages/patients/createAppointmentPage/sections/RightCreateAppointmentSection'
import { setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { setProfileStore } from '@/redux/reducers/patientReducer'
import { Grid, Paper } from '@mui/material'
import { useState } from 'react'

const CreateAppointmentPage = () => {
	const [timeSlotFilter, setTimeSlotFilter] = useState({
		date: '',
		specialtyId: '',
	})
	const [successDialog, setSuccessDialog] = useState({
		open: false,
		paymentUrl: '',
		appointmentMeetingType: '',
	})

	const getSpecialtyStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})
	const userProfileStore = useReduxStore({
		selector: (state) => state.patient.profile,
		setStore: setProfileStore,
	})

	const getAvailableTimeSlots = useFetch(
		ApiUrls.TIME_SLOT.AVAILABLE,
		timeSlotFilter,
		[timeSlotFilter],
		false
	)

	const postAppointment = useAxiosSubmit({
		url: ApiUrls.APPOINTMENT.INDEX,
		method: 'POST',
		onSuccess: (response) => {
			const { data } = response
			if (data) {
				setSuccessDialog({
					open: true,
					paymentUrl: data.paymentUrl,
					appointmentMeetingType: data.appointmentMeetingType,
				})
			}
		},
	})

	const handleValuesChange = (newValues) => {
		if (
			newValues.date &&
			newValues.specialtyId &&
			(newValues.date !== timeSlotFilter.date || newValues.specialtyId !== timeSlotFilter.specialtyId)
		) {
			setTimeSlotFilter({
				date: newValues.date,
				specialtyId: newValues.specialtyId,
			})
		}
	}

	return (
		<Paper
			sx={{
				bgcolor: (t) => t.palette.background.paper,
				borderRadius: 3,
				my: 3,
				overflow: 'hidden',
			}}
		>
			<Grid container spacing={2}>
				<Grid size={{ xs: 12, md: 8 }} my={2} px={4} py={2}>
					<LeftCreateAppointmentSection
						specialties={getSpecialtyStore.data}
						userProfile={userProfileStore.data}
						timeSlots={getAvailableTimeSlots.data}
						loadingTimeSlots={getAvailableTimeSlots.loading}
						onSubmit={async (values) => await postAppointment.submit({ overrideData: values })}
						onValuesChange={handleValuesChange}
						loadingGet={userProfileStore.loading || getSpecialtyStore.loading}
						loadingSubmit={postAppointment.loading}
					/>
				</Grid>
				<Grid size={{ xs: 0, md: 4 }}>
					<RightCreateAppointmentSection />
				</Grid>
			</Grid>
			<CreateAppointmentSuccessfullyDialog
				open={successDialog.open}
				onClose={() =>
					setSuccessDialog((prev) => ({
						...prev,
						open: false,
					}))
				}
				paymentUrl={successDialog.paymentUrl}
				appointmentMeetingType={successDialog.appointmentMeetingType}
			/>
		</Paper>
	)
}

export default CreateAppointmentPage

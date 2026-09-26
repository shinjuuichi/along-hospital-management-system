import MeetingRoomTeleSession from '@/components/meetingRoom/MeetingRoomTeleSession'
import { ApiUrls } from '@/configs/apiUrls'
import { getReturnUrlByRole } from '@/configs/roleBasedConfig'
import useAuth from '@/hooks/useAuth'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const DoctorMeetingRoomTeleSessionSection = () => {
	const { t } = useTranslation()
	const { auth } = useAuth()

	const navigate = useNavigate()

	const {
		data: room,
		error: roomError,
		loading,
	} = useFetch(ApiUrls.TELE_ROOM.GET_TELE_ROOM_FOR_DOCTOR, {}, [])

	return (
		<MeetingRoomTeleSession
			credentials={room?.credentials}
			roomCode={room?.roomCode}
			isCaller={false}
			fetchError={roomError}
			loading={loading}
			header={
				<Typography variant='h6' sx={{ mb: 1 }}>
					{t('meeting_room.title.room_name')}: {room?.roomDisplayName}
				</Typography>
			}
			onAfterEndCall={async () => {
				navigate(getReturnUrlByRole(auth?.role), {
					replace: true,
				})
			}}
		/>
	)
}

export default DoctorMeetingRoomTeleSessionSection

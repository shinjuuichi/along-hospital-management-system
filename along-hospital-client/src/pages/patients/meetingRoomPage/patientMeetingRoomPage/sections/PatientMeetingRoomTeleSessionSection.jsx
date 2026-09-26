import MeetingRoomTeleSession from '@/components/meetingRoom/MeetingRoomTeleSession'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Button, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const resolvePatientStatusContent = ({ message }, t) => {
	if (message === 'Tele session has not started yet.') {
		return {
			title: t('telehealth.status.not_started_title'),
			description: t('telehealth.status.not_started_description'),
		}
	}

	if (message === 'Tele session has expired.') {
		return {
			title: t('telehealth.status.expired_title'),
			description: t('telehealth.status.expired_description'),
		}
	}

	if (message === 'User is not authorized to access this tele session.') {
		return {
			title: t('telehealth.status.forbidden_title'),
			description: t('telehealth.status.forbidden_description'),
		}
	}

	if (message === 'Appointment payment is not completed.') {
		return {
			title: t('telehealth.status.payment_pending_title'),
			description: t('telehealth.status.payment_pending_description'),
		}
	}

	return {
		title: t('telehealth.status.unavailable_title'),
		description: message || t('telehealth.error.session_not_ready'),
	}
}

const PatientMeetingRoomTeleSessionSection = ({ transactionId }) => {
	const { t } = useTranslation()
	const nav = useNavigate()

	const {
		data: session,
		error: sessionError,
		loading,
	} = useFetch(ApiUrls.TELE_SESSION.DETAIL(transactionId), {}, [transactionId])

	return (
		<MeetingRoomTeleSession
			credentials={session}
			transactionId={transactionId}
			isCaller
			enablePatientRecording
			fetchError={sessionError}
			loading={loading}
			header={
				session?.roomDisplayName ? (
					<Typography variant='h6' sx={{ mb: 1 }}>
						{t('meeting_room.title.room_name')}: {session.roomDisplayName}
					</Typography>
				) : null
			}
			expiringNotice={t('telehealth.notice.expiring_soon')}
			resolveStatusContent={(errorState, translate) => {
				const status = resolvePatientStatusContent(errorState, translate)

				return {
					...status,
					description: (
						<>
							{status.description}
							<Button
								variant='contained'
								sx={{ mt: 2, display: 'block' }}
								onClick={() =>
									nav(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.JOIN_MEETING_ROOM))
								}
							>
								{translate('meeting_room.title.join_meeting')}
							</Button>
						</>
					),
				}
			}}
			onAfterEndCall={async () => {
				nav(
					routeUrls.BASE_ROUTE.PATIENT(
						routeUrls.PATIENT.APPOINTMENT.MEETING_ROOM_COMPLETE(transactionId)
					)
				)
			}}
			onAfterSessionExpired={() => {
				nav(
					routeUrls.BASE_ROUTE.PATIENT(
						routeUrls.PATIENT.APPOINTMENT.MEETING_ROOM_COMPLETE(transactionId)
					),
					{ replace: true }
				)
			}}
		/>
	)
}

export default PatientMeetingRoomTeleSessionSection

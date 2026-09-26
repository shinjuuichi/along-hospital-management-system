import useTranslation from '@/hooks/useTranslation'
import { MeetingRoom } from '@mui/icons-material'
import { IconButton, Tooltip } from '@mui/material'

const MeetingRoomButton = ({ onClick }) => {
	const { t } = useTranslation()

	return (
		<Tooltip title={t('tooltip.meeting_room')}>
			<IconButton onClick={onClick}>
				<MeetingRoom />
			</IconButton>
		</Tooltip>
	)
}

export default MeetingRoomButton

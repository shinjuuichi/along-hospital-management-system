import useTranslation from '@/hooks/useTranslation'
import { Typography } from '@mui/material'

const WaitingNote = () => {
	const { t } = useTranslation()
	return (
		<Typography variant='caption' color='text.secondary' sx={{ mt: 1, textAlign: 'center' }}>
			{t('meeting_room.title.note')}
		</Typography>
	)
}

export default WaitingNote

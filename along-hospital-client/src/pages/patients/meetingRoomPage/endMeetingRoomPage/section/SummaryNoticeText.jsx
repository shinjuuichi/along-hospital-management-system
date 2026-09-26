import useTranslation from '@/hooks/useTranslation'
import { Typography } from '@mui/material'

const SummaryNoticeText = ({ noticeKey = 'meeting_room.summary_notice' }) => {
	const { t } = useTranslation()
	return (
		<Typography
			variant='caption'
			color='text.secondary'
			sx={{ mt: 1.5, display: 'block', textAlign: 'center' }}
		>
			{t(noticeKey)}
		</Typography>
	)
}

export default SummaryNoticeText

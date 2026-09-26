import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { Feed } from '@mui/icons-material'
import { IconButton, Tooltip } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const ViewDetailMedicalHistoryButton = ({ medicalHistoryId }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	return (
		<Tooltip title={t('tooltip.view_detail_medical_history')}>
			<IconButton
				onClick={() =>
					navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(medicalHistoryId)))
				}
			>
				<Feed />
			</IconButton>
		</Tooltip>
	)
}

export default ViewDetailMedicalHistoryButton

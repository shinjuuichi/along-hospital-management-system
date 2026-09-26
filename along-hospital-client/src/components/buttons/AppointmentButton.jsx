import useTranslation from '@/hooks/useTranslation'
import { AddAlarm } from '@mui/icons-material'
import { IconButton, Tooltip } from '@mui/material'

const AppointmentButton = ({ onClick }) => {
	const { t } = useTranslation()

	return (
		<Tooltip title={t('tooltip.create_appointment')}>
			<IconButton onClick={onClick}>
				<AddAlarm />
			</IconButton>
		</Tooltip>
	)
}

export default AppointmentButton

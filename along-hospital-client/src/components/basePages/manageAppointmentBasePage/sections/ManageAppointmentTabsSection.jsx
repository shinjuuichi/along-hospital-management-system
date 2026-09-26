import GenericTabs from '@/components/generals/GenericTabs'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'

const ManageAppointmentTabsSection = ({ filters, setFilters, loading }) => {
	const _enum = useEnum()
	const { t } = useTranslation()

	const statusTabs = [
		{ key: '', title: t('text.all') },
		..._enum.appointmentStatusOptions.map((status) => ({ key: status.value, title: status.label })),
	]

	return (
		<Stack spacing={2} direction={'row'} alignItems='center'>
			<Typography minWidth={'max-content'} variant='subtitle1'>
				{t('appointment.field.status')}:
			</Typography>
			<GenericTabs
				tabs={statusTabs}
				currentTab={filters.status}
				setCurrentTab={(tab) => setFilters({ ...filters, status: tab.key, page: 1 })}
				loading={loading}
			/>
		</Stack>
	)
}

export default ManageAppointmentTabsSection

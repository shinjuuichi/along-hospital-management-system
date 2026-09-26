/* eslint-disable no-unused-vars */
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import ManagementMedicalHistoryCardSection from './sections/ManagementMedicalHistoryCardSection'
import ManagementMedicalHistoryDetailDrawerSection from './sections/ManagementMedicalHistoryDetailDrawerSection'
import ManagementMedicalHistoryFilterSection from './sections/ManagementMedicalHistoryFilterSection'

const ManagementMedicalHistoryBasePage = ({
	items,
	loading,
	patientNames,
	doctorNames,
	totalPage,
	filters,
	setFilters,
	page,
	setPage,
	pageSize,
	setPageSize,
	selectedItem,
	setSelectedItem,
}) => {
	const [drawerOpen, setDrawerOpen] = useState(false)

	const { t } = useTranslation()
	const navigate = useNavigate()
	const { auth } = useAuth()
	const isReceptionist = auth?.role === EnumConfig.Role.Receptionist
	const isNurse = auth?.role === EnumConfig.Role.Nurse

	const handleOpenDrawer = (item) => {
		setSelectedItem(item)
		setDrawerOpen(true)
	}

	return (
		<Paper sx={{ p: 2, bgcolor: 'background.paper' }}>
			<Stack spacing={4}>
				<Stack spacing={2}>
					<Typography variant='h6'>{t('medical_history.title.medical_history_management')}</Typography>
					{(isReceptionist || isNurse) && (
						<Box>
							<Button
								variant='contained'
								color='primary'
								onClick={() => navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.CREATE))}
							>
								{t('medical_history.button.create_medical_history')}
							</Button>
						</Box>
					)}
				</Stack>
				<ManagementMedicalHistoryFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={loading}
					patientNames={patientNames}
					doctorNames={doctorNames}
				/>
				<ManagementMedicalHistoryCardSection
					items={items}
					onOpen={handleOpenDrawer}
					loading={loading}
				/>
				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={loading}
				/>
			</Stack>
			<ManagementMedicalHistoryDetailDrawerSection
				open={drawerOpen}
				onClose={() => setDrawerOpen(false)}
				item={selectedItem}
			/>
		</Paper>
	)
}

export default ManagementMedicalHistoryBasePage

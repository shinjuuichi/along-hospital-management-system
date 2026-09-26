import ManageAppointmentDetailDrawerSection from '@/components/basePages/manageAppointmentBasePage/sections/ManageAppointmentDetailDrawerSection'
import ManageAppointmentFilterBarSection from '@/components/basePages/manageAppointmentBasePage/sections/ManageAppointmentFilterBarSection'
import ManageAppointmentListItemSection from '@/components/basePages/manageAppointmentBasePage/sections/ManageAppointmentListItemSection'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { Grid, Paper, Stack, Typography } from '@mui/material'
import React, { useEffect, useState } from 'react'
import EmptyBox from '../../placeholders/EmptyBox'
import SkeletonBox from '../../skeletons/SkeletonBox'
import ManageAppointmentTabsSection from './sections/ManageAppointmentTabsSection'

const ManageAppointmentBasePage = ({
	headerTitle = 'Manage Appointments',
	appointments = [],
	totalPage = 1,
	specialties = [],
	filters,
	setFilters,
	page,
	setPage,
	pageSize,
	setPageSize,
	selectedAppointment,
	setSelectedAppointment,
	loading = false,
	drawerButtons = <React.Fragment />,
}) => {
	const [drawerOpen, setDrawerOpen] = useState(false)

	const onOpenDrawer = (appt) => {
		setSelectedAppointment(appt)
		setDrawerOpen(true)
	}

	useEffect(() => {
		if (page !== 1) setPage(1)
	}, [filters, page, setPage])

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{headerTitle}</Typography>

				<ManageAppointmentFilterBarSection
					filters={filters}
					setFilters={setFilters}
					specialties={specialties}
					loading={loading}
				/>

				<Stack spacing={2} sx={{ width: '100%' }}>
					<ManageAppointmentTabsSection filters={filters} setFilters={setFilters} loading={loading} />
					{loading ? (
						<Grid container spacing={2} alignItems={'stretch'}>
							<Grid size={{ sm: 12, md: 6 }}>
								<SkeletonBox numberOfBoxes={2} rounded heights={[150]} />
							</Grid>
							<Grid size={{ sm: 12, md: 6 }}>
								<SkeletonBox numberOfBoxes={2} rounded heights={[150]} />
							</Grid>
						</Grid>
					) : appointments.length === 0 ? (
						<EmptyBox minHeight={300} />
					) : (
						<Grid container spacing={2} alignItems={'stretch'}>
							{appointments.map((appt, index) => (
								<Grid size={{ sm: 12, md: 6 }} key={appt?.id || index}>
									<ManageAppointmentListItemSection
										key={appt?.id || index}
										appointment={appt}
										onClick={() => onOpenDrawer(appt)}
									/>
								</Grid>
							))}
						</Grid>
					)}
					<Stack justifyContent={'center'} px={2}>
						<GenericTablePagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							pageSize={pageSize}
							setPageSize={setPageSize}
							pageSizeOptions={[6, 12, 20]}
							loading={loading}
						/>
					</Stack>
				</Stack>

				<ManageAppointmentDetailDrawerSection
					appointment={selectedAppointment}
					open={drawerOpen}
					onClose={() => setDrawerOpen(false)}
					buttons={drawerButtons}
				/>
			</Stack>
		</Paper>
	)
}

export default ManageAppointmentBasePage

import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import ManageStaffContractFilterSection from './sections/ManageStaffContractFilterSection'
import ManageStaffContractFormSection from './sections/ManageStaffContractFormSection'
import ManageStaffContractTableSection from './sections/ManageStaffContractTableSection'

const ManageStaffContractBasePage = ({
	headerTitle,
	staffContracts = [],
	totalPage = 1,
	loading = false,
	filters,
	setFilters,
	page,
	setPage,
	pageSize,
	setPageSize,
	sort,
	setSort,
	onCreateSubmit,
	onUpdateSubmit,
	onRenewSubmit,
	onSignSubmit,
	onDeleteClick,
	onTerminateClick,
	staffs = [],
	regionalWages = [],
	role,
}) => {
	const { t } = useTranslation()
	const isHR = role === EnumConfig.Role.HR

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [openRenewForm, setOpenRenewForm] = useState(false)
	const [openSignForm, setOpenSignForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)

	const handleSignClick = (row) => {
		setSelectedItem(row)
		setOpenSignForm(true)
	}

	const handleDetailClick = (row) => {
		setSelectedItem(row)
		setOpenSignForm(true)
	}

	const handleSignSubmit = async ({ values, closeDialog }) => {
		if (onSignSubmit) {
			await onSignSubmit({ values, closeDialog, contractId: selectedItem?.id })
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{headerTitle || t('staff_contract.title.staff_contract_management')}</Typography>

				<ManageStaffContractFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={loading}
					staffs={staffs}
					regionalWages={regionalWages}
					role={role}
				/>

				<ManageStaffContractTableSection
					data={staffContracts}
					loading={loading}
					totalPage={totalPage}
					sort={sort}
					setSort={setSort}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					isHR={isHR}
					onCreateClick={() => setOpenCreateForm(true)}
					onUpdateClick={(row) => {
						setSelectedItem(row)
						setOpenUpdateForm(true)
					}}
					onDeleteClick={onDeleteClick}
					onTerminateClick={onTerminateClick}
					onRenewClick={(row) => {
						setSelectedItem(row)
						setOpenRenewForm(true)
					}}
					onSignClick={handleSignClick}
					onDetailClick={handleDetailClick}
				/>

				<ManageStaffContractFormSection
					openCreateForm={openCreateForm}
					setOpenCreateForm={setOpenCreateForm}
					onCreateSubmit={onCreateSubmit}
					openUpdateForm={openUpdateForm}
					setOpenUpdateForm={setOpenUpdateForm}
					selectedItem={selectedItem}
					onUpdateSubmit={onUpdateSubmit}
					openRenewForm={openRenewForm}
					setOpenRenewForm={setOpenRenewForm}
					onRenewSubmit={onRenewSubmit}
					openSignForm={openSignForm}
					setOpenSignForm={setOpenSignForm}
					onSignSubmit={handleSignSubmit}
					staffs={staffs}
					regionalWages={regionalWages}
					isAlreadySigned={!!selectedItem?.signatureImage}
				/>
			</Stack>
		</Paper>
	)
}

export default ManageStaffContractBasePage

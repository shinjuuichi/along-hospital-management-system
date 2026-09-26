import ImportRequestManagementFilterSection from './sections/ImportRequestManagementFilterSection'
import ImportRequestManagementTableSection from './sections/ImportRequestManagementTableSection'
import ImportRequestManagementFormSection from './sections/ImportRequestManagementFormSection'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const ImportRequestManagementBasePage = ({
	headerTitle = 'Import Request',
	importRequests = [],
	totalPage = 1,
	filters,
	setFilters,
	page,
	setPage,
	pageSize,
	setPageSize,
	sort,
	setSort,
	medicineSkuOptions = [],
	loading = false,
	onSuccess,
	onApprove,
	onReject,
	onCancel,
	onCreateImport,
	onCreateRequest,
	onUpdateRequest,
	role,
}) => {
	const { t } = useTranslation()

	const [openCreate, setOpenCreate] = useState(false)
	const [openDetail, setOpenDetail] = useState(false)
	const [openCreateImport, setOpenCreateImport] = useState(false)
	const [selectedRequest, setSelectedRequest] = useState(null)

	const handleRowClick = (row) => {
		setSelectedRequest(row)
		setOpenDetail(true)
	}

	const handleDetailClose = () => {
		setOpenDetail(false)
		setSelectedRequest(null)
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{headerTitle}</Typography>

				<ImportRequestManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={loading}
				/>

				<ImportRequestManagementTableSection
					importRequests={importRequests}
					loading={loading}
					sort={sort}
					setSort={setSort}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					totalPage={totalPage}
					onCreateClick={() => setOpenCreate(true)}
					onRowClick={handleRowClick}
				/>

				<ImportRequestManagementFormSection
					openCreate={openCreate}
					setOpenCreate={setOpenCreate}
					openDetail={openDetail}
					setOpenDetail={setOpenDetail}
					openCreateImport={openCreateImport}
					setOpenCreateImport={setOpenCreateImport}
					selectedRequest={selectedRequest}
					setSelectedRequest={setSelectedRequest}
					medicineSkuOptions={medicineSkuOptions}
					onCreateSubmit={onCreateRequest}
					onDetailSubmit={onUpdateRequest}
					onCancel={onCancel}
					onReject={onReject}
					onApprove={onApprove}
					onCreateImport={onCreateImport}
					onSuccess={onSuccess}
					role={role}
				/>
			</Stack>
		</Paper>
	)
}

export default ImportRequestManagementBasePage

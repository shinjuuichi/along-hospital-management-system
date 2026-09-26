import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { defaultStaffContractStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Avatar, Box, Button, Chip, Stack } from '@mui/material'
import { useMemo } from 'react'

const ManageStaffContractTableSection = ({
	data,
	loading,
	sort,
	setSort,
	totalPage,
	page,
	setPage,
	pageSize,
	setPageSize,
	isHR = false,
	onCreateClick = () => {},
	onUpdateClick = (row) => Promise.resolve(row),
	onTerminateClick = (row) => Promise.resolve(row),
	onRenewClick = (row) => Promise.resolve(row),
	onSignClick = (row) => Promise.resolve(row),
	onDetailClick = (row) => Promise.resolve(row),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const tableFields = useMemo(() => {
		const actionField = isHR
			? {
					key: '',
					title: '',
					width: 6,
					render: (_, row) => (
						<ActionMenu
							actions={[
								{
									title: t('button.update'),
									onClick: () => onUpdateClick(row),
								},
								...(row.status === EnumConfig.StaffContractStatus.Active
									? [
											{
												title: t('staff_contract.button.terminate'),
												onClick: () => onTerminateClick(row),
											},
										]
									: []),
								...(row.status === EnumConfig.StaffContractStatus.Expired
									? [
											{
												title: t('staff_contract.button.renew'),
												onClick: () => onRenewClick(row),
											},
										]
									: []),
							]}
						/>
					),
				}
			: {
					key: '',
					title: '',
					width: 6,
					render: (_, row) => (
						<Stack direction='row' gap={1}>
							{row.signatureImage ? (
								<Button
									size='small'
									variant='contained'
									disableElevation
									onClick={() => onDetailClick(row)}
								>
									{t('staff_contract.button.detail')}
								</Button>
							) : (
								<Button
									size='small'
									variant='contained'
									color='primary'
									disableElevation
									onClick={() => onSignClick(row)}
								>
									{t('staff_contract.button.sign')}
								</Button>
							)}
						</Stack>
					),
				}

		return [
			{
				key: 'contractCode',
				title: t('staff_contract.field.contract_code'),
				width: 10,
				sortable: true,
			},
			{
				key: 'staffName',
				title: t('staff_contract.field.staff_name'),
				width: 10,
			},
			{
				key: 'staffImage',
				title: t('staff_contract.field.staff_image'),
				width: 6,
				sortable: false,
				render: (_, row) => {
					if (row.staffImage === undefined) return null
					const imageUrl = getImageFromCloud(row.staffImage)
					return (
						<Avatar src={imageUrl} alt={row.staff?.name || 'staff'} sx={{ width: 50, height: 50 }} />
					)
				},
			},
			{
				key: 'contractType',
				title: t('staff_contract.field.contract_type'),
				width: 10,
				sortable: true,
				render: (value) => getEnumLabelByValue(_enum.contractTypeOptions, value),
			},
			{
				key: 'hourlyRate',
				title: t('staff_contract.field.hourly_rate'),
				width: 10,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'status',
				title: t('staff_contract.field.status'),
				width: 8,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.staffContractStatusOptions, value)}
						color={defaultStaffContractStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'signatureImage',
				title: t('staff_contract.field.signing_status'),
				width: 8,
				sortable: false,
				render: (value) => (
					<Chip
						label={value ? t('staff_contract.button.signed') : t('staff_contract.button.unsigned')}
						color={value ? 'success' : 'default'}
						size='small'
					/>
				),
			},
			actionField,
		]
	}, [isHR, t, _enum, onUpdateClick, onTerminateClick, onRenewClick, onDetailClick, onSignClick])

	return (
		<>
			{isHR && (
				<Box>
					<Button variant='contained' sx={{ float: 'right' }} onClick={() => onCreateClick()}>
						{t('button.create')}
					</Button>
				</Box>
			)}
			<GenericTable
				fields={tableFields}
				data={data}
				rowKey='id'
				loading={loading}
				sort={sort}
				setSort={setSort}
			/>
			<GenericTablePagination
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				pageSizeOptions={[5, 10, 20]}
				loading={loading}
			/>
		</>
	)
}

export default ManageStaffContractTableSection

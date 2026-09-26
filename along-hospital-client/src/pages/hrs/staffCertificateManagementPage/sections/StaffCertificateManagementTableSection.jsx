import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { defaultStaffCertificateStatusStyle } from '@/configs/defaultStylesConfig'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Stack } from '@mui/material'
import { useCallback, useMemo } from 'react'

const StaffCertificateManagementTableSection = ({
	data,
	loading,
	sort,
	setSort,
	totalPage,
	page,
	setPage,
	pageSize,
	setPageSize,
	onCreateClick = () => {},
	onUpdateClick = (row) => Promise.resolve(row),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const getStatusChip = useCallback(
		(status) => {
			const color = defaultStaffCertificateStatusStyle(status)
			const label = getEnumLabelByValue(_enum.staffCertificateStatusOptions, status) ?? status
			return (
				<Chip
					label={label}
					color={color}
					size='small'
					sx={{ fontWeight: 500 }}
				/>
			)
		},
		[_enum]
	)

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('staff_certificate.table.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'certificateNo',
				title: t('staff_certificate.table.certificate_no'),
				width: 15,
				sortable: true,
			},
			{
				key: 'staffCertificateType.name',
				title: t('staff_certificate.table.certificate_type'),
				width: 15,
				sortable: false,
			},
			{
				key: 'staffName',
				title: t('staff_certificate.table.staff'),
				width: 15,
				sortable: false,
			},
			{
				key: 'issuedDate',
				title: t('staff_certificate.table.issued_date'),
				width: 12,
				sortable: true,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'expiredDate',
				title: t('staff_certificate.table.expired_date'),
				width: 12,
				sortable: true,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'status',
				title: t('staff_certificate.table.status'),
				width: 12,
				sortable: true,
				render: (value) => getStatusChip(value),
			},
			{
				key: '',
				title: t('staff_certificate.table.actions'),
				width: 10,
				render: (_, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.update'),
								onClick: () => onUpdateClick(row),
							},
						]}
					/>
				),
			},
		],
		[t, getStatusChip, onUpdateClick]
	)

	return (
		<>
			<Box>
				<Stack direction='row' justifyContent='flex-end' alignItems='center' spacing={2}>
					<Button variant='contained' color='primary' onClick={() => onCreateClick()}>
						{t('button.create')}
					</Button>
				</Stack>
			</Box>
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

export default StaffCertificateManagementTableSection

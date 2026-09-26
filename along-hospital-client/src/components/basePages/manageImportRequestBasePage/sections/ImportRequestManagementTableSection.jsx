import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { defaultImportRequestStatusStyle } from '@/configs/defaultStylesConfig'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Preview } from '@mui/icons-material'
import { Box, Button, Chip, Stack } from '@mui/material'
import { useCallback, useMemo } from 'react'

const ImportRequestManagementTableSection = ({
	importRequests = [],
	loading = false,
	sort,
	setSort,
	page,
	setPage,
	pageSize,
	setPageSize,
	totalPage = 1,
	onCreateClick,
	onRowClick,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const getStatusChip = useCallback(
		(status) => {
			const color = defaultImportRequestStatusStyle(status)
			const label = getEnumLabelByValue(_enum.importRequestStatusOptions, status) ?? status
			return (
				<Chip
					label={label}
					color={color}
					size='small'
					sx={{ fontWeight: 500 }}
				/>
			)
		},
		[t, _enum]
	)

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('import_request.table.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'requestDate',
				title: t('import_request.table.request_date'),
				width: 15,
				sortable: true,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'status',
				title: t('import_request.table.status'),
				width: 15,
				sortable: true,
				render: (value) => getStatusChip(value),
			},
			{
				key: 'details',
				title: t('import_request.table.items_count'),
				width: 15,
				sortable: false,
				render: (value) => (Array.isArray(value) ? value.length : 0),
			},
			{
				key: '',
				title: t('import_request.table.actions'),
				width: 15,
				sortable: false,
				render: (_, row) => (
					<Button
						variant='outlined'
						size='small'
						startIcon={<Preview />}
						onClick={() => onRowClick?.(row)}
					>
						{t('import_request.button.view')}
					</Button>
				),
			},
		],
		[t, getStatusChip, onRowClick]
	)

	return (
		<Stack spacing={2}>
			<Box sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center', gap: 2 }}>
				{onCreateClick && (
					<Button variant='contained' color='primary' onClick={onCreateClick}>
						{t('import_request.button.create')}
					</Button>
				)}
			</Box>

			<GenericTable
				data={importRequests}
				fields={tableFields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				loading={loading}
				sx={{ minHeight: 400 }}
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
	)
}

export default ImportRequestManagementTableSection

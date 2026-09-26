import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultBooleanStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Stack, Tooltip, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const SpecialtyManagementTableSection = ({
	specialties,
	loading,
	sort,
	setSort,
	setOpenCreate,
	setOpenUpdate,
	setSelectedRow,
	onDelete,
}) => {
	const [selectedIds, setSelectedIds] = useState([])
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('specialty.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'name', title: t('specialty.field.name'), width: 20, sortable: true },
			{
				key: 'description',
				title: t('specialty.field.description'),
				width: 40,
				sortable: false,
				render: (value) => {
					const text = value ?? ''
					return (
						<Tooltip title={text} arrow placement='top' disableInteractive>
							<Typography
								variant='body2'
								noWrap
								sx={{ width: '100%', overflow: 'hidden', textOverflow: 'ellipsis' }}
							>
								{renderEmptyFallback(text)}
							</Typography>
						</Tooltip>
					)
				},
			},
			{
				key: 'isMedical',
				title: t('specialty.field.is_medical'),
				width: 12,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.booleanOptions, value)}
						size='small'
						color={defaultBooleanStyle(value)}
					/>
				),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.edit'),
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
							{
								title: t('button.delete'),
								onClick: () => onDelete(row),
							},
						]}
					/>
				),
			},
		],
		[t, setSelectedRow, setOpenUpdate, onDelete, _enum.booleanOptions]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={specialties}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
		</>
	)
}

export default SpecialtyManagementTableSection

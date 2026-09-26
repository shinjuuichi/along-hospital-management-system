import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultBooleanStyle, defaultInsuranceSubjectStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'
import { useMemo, useState } from 'react'

const AllowanceTypeManagementTableSection = ({
	allowanceTypes,
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
			{ key: 'id', title: t('allowance_type.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'name', title: t('allowance_type.field.name'), width: 15, sortable: true },
			{
				key: 'price',
				title: t('allowance_type.field.price'),
				width: 10,
				sortable: true,
				render: (value) => formatCurrencyBasedOnCurrentLanguage(value),
			},
			{
				key: 'insuranceSubject',
				title: t('allowance_type.field.insurance_subject'),
				width: 10,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.insuranceSubjectOptions, value)}
						size='small'
						color={defaultInsuranceSubjectStyle(value)}
					/>
				),
			},
			{
				key: 'isTaxable',
				title: t('allowance_type.field.is_taxable'),
				width: 10,
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
				key: 'isPercentage',
				title: t('allowance_type.field.is_percentage'),
				width: 15,
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
				render: (_, row) => (
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
		[t, _enum.insuranceSubjectOptions, setSelectedRow, setOpenUpdate, onDelete, _enum.booleanOptions]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={allowanceTypes}
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

export default AllowanceTypeManagementTableSection

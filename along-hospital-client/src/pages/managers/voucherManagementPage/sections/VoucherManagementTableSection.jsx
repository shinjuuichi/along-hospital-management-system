/* eslint-disable react-hooks/exhaustive-deps */
import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultVoucherStatusStyle, defaultVoucherTypeStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useConfirm from '@/hooks/useConfirm'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage, formatDateToSqlDate } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Stack } from '@mui/material'
import { useMemo, useState } from 'react'
import VoucherDetailDialog from './VoucherDetailDialog'
import VoucherFormDialog from './VoucherFormDialog'

const VoucherManagementTableSection = ({
	vouchers,
	loading,
	medicineOptions = [],
	onCreateSubmit,
	onUpdateSubmit,
	onDelete,
}) => {
	const [selectedRow, setSelectedRow] = useState({})
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [openDetail, setOpenDetail] = useState(false)
	const [detailVoucher, setDetailVoucher] = useState(null)

	const confirm = useConfirm()
	const { t } = useTranslation()

	const createInitialValues = useMemo(
		() => ({
			name: '',
			description: '',
			discountValue: '',
			minPurchaseAmount: '',
			maxDiscount: '',
			quantity: '',
			expireDate: '',
			voucherType: EnumConfig.VoucherType.Patient,
			discountType: EnumConfig.VoucherDiscountType.Percentage,
			image: null,
			medicineIds: [],
		}),
		[]
	)

	const handleUpdateSubmit = async ({ values, closeDialog }) => {
		await onUpdateSubmit?.({
			rowId: selectedRow.id,
			values,
			closeDialog,
		})
	}

	const handleCreateSubmit = async ({ values, closeDialog }) => {
		await onCreateSubmit?.({
			values,
			closeDialog,
		})
	}

	const handleDeleteClick = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('voucher.title.delete'),
			description: `${t('voucher.title.delete_confirm')} ${row.name}?`,
		})

		if (isConfirmed) {
			await onDelete?.(row.id)
		}
	}

	const fields = useMemo(
		() => [
			{ key: 'code', title: t('voucher.field.code'), width: 10, fixedColumn: true },
			{ key: 'name', title: t('voucher.field.name'), width: 12 },
			{
				key: 'discountValue',
				title: t('voucher.field.discount_value'),
				width: 8,
				render: (value, row) => {
					if (row.discountType === EnumConfig.VoucherDiscountType.Percentage) {
						return `${value}%`
					}
					return formatCurrencyBasedOnCurrentLanguage(value)
				},
			},
			{
				key: 'expireDate',
				title: t('voucher.field.expire_date'),
				width: 10,
				render: (value) => formatDateBasedOnCurrentLanguage(value),
			},
			{
				key: 'voucherStatus',
				title: t('voucher.field.voucher_status'),
				width: 8,
				render: (value) => (
					<Chip
						label={t(`voucher.status.${value}`)}
						color={defaultVoucherStatusStyle(value)}
						size='small'
						sx={{ fontWeight: 500 }}
					/>
				),
			},
			{
				key: 'voucherType',
				title: t('voucher.field.voucher_type'),
				width: 8,
				render: (value) => (
					<Chip
						label={t(`voucher.type.${value}`)}
						color={defaultVoucherTypeStyle(value)}
						size='small'
						sx={{ fontWeight: 500 }}
					/>
				),
			},
			{
				key: 'medicines',
				title: t('voucher.field.medicines'),
				width: 12,
				render: (value, row) => {
					if (row.voucherType === EnumConfig.VoucherType.Medicine) {
						if (value?.length > 0) {
							return (
								<Stack direction='row' spacing={1} alignItems='center'>
									<Chip
										label={`${value.length} ${t('voucher.field.medicine').toLowerCase()}`}
										size='small'
										color='info'
										variant='outlined'
									/>
								</Stack>
							)
						}
						return renderEmptyFallback(null)
					}
					return renderEmptyFallback(null)
				},
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.view_detail'),
								onClick: () => {
									setDetailVoucher(row)
									setOpenDetail(true)
								},
							},
							{
								title: t('button.edit'),
								onClick: () => {
									const formattedRow = { ...row }
									if (row.medicineIds && Array.isArray(row.medicineIds)) {
										formattedRow.medicineIds = row.medicineIds
											.map((item) => item?.medicineId ?? item)
											.filter((id) => id !== null && id !== undefined && id !== '')
									} else if (row.medicines && Array.isArray(row.medicines)) {
										formattedRow.medicineIds = row.medicines
											.map((medicine) => medicine.id)
											.filter((id) => id !== null && id !== undefined && id !== '')
									}
									if (
										formattedRow.voucherType === EnumConfig.VoucherType.Medicine &&
										(!formattedRow.medicineIds || formattedRow.medicineIds.length === 0)
									) {
										formattedRow.medicineIds = []
									}
									if (row.expireDate) {
										formattedRow.expireDate = formatDateToSqlDate(row.expireDate)
									}
									setSelectedRow(formattedRow)
									setOpenUpdate(true)
								},
							},
							{
								title: t('button.delete'),
								onClick: () => handleDeleteClick(row),
							},
						]}
					/>
				),
			},
		],
		[t]
	)

	return (
		<>
			<Box sx={{ py: 1, px: 2, mt: 2 }}>
				<Stack
					spacing={2}
					direction='row'
					alignItems='center'
					justifyContent='flex-end'
					ml={2}
					mb={1.5}
				>
					<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
						{t('button.create')}
					</Button>
				</Stack>
				<GenericTable data={vouchers} fields={fields} rowKey='id' loading={loading} stickyHeader />
			</Box>
			<VoucherFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={createInitialValues}
				medicineOptions={medicineOptions}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('voucher.title.create')}
				onSubmit={handleCreateSubmit}
				isUpdate={false}
			/>
			<VoucherFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				initialValues={selectedRow}
				medicineOptions={medicineOptions}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('voucher.title.update')}
				onSubmit={handleUpdateSubmit}
				isUpdate={true}
			/>
			<VoucherDetailDialog
				open={openDetail}
				onClose={() => {
					setOpenDetail(false)
					setDetailVoucher(null)
				}}
				voucher={detailVoucher}
			/>
		</>
	)
}

export default VoucherManagementTableSection

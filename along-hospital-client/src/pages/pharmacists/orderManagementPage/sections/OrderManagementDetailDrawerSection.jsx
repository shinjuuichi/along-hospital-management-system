import GenericDrawer from '@/components/generals/GenericDrawer'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultOrderStatusThemeColor } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Avatar, Button, Chip, Stack, Typography } from '@mui/material'

const OrderManagementDetailDrawerSection = ({
	open,
	onClose,
	order,
	shippingSubmit,
	paidSubmit,
	completeSubmit,
	onSuccess,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const confirm = useConfirm()

	if (!order) return null

	const statusLabel = getEnumLabelByValue(_enum.orderStatusOptions, order.orderStatus)

	const statusStyle = defaultOrderStatusThemeColor(order.orderStatus)

	const buttons = []

	if (order.orderStatus === EnumConfig.OrderStatus.Paid && order.isPickupAtStore === false) {
		buttons.push(
			<Button
				key='shipping'
				variant='contained'
				color='info'
				onClick={async () => {
					const isConfirmed = await confirm({
						title: t('order_management.confirm.shipping_title'),
						description: t('order_management.confirm.shipping_description'),
						confirmText: t('order_management.button.shipping'),
						confirmColor: 'info',
					})
					if (!isConfirmed) return
					const response = await shippingSubmit.submit({
						overrideUrl: ApiUrls.ORDER.MANAGEMENT.SHIPPING(order.id),
					})
					if (response) {
						await onSuccess?.()
						onClose()
					}
				}}
			>
				{t('order_management.button.shipping')}
			</Button>
		)
	}

	if (order.orderStatus === EnumConfig.OrderStatus.Unpaid) {
		buttons.push(
			<Button
				key='paid'
				variant='contained'
				color='success'
				onClick={async () => {
					const isConfirmed = await confirm({
						title: t('order_management.confirm.paid_title'),
						description: t('order_management.confirm.paid_description'),
						confirmText: t('order_management.button.paid'),
						confirmColor: 'success',
					})
					if (!isConfirmed) return
					const response = await paidSubmit.submit({
						overrideUrl: ApiUrls.ORDER.MANAGEMENT.PAID(order.id),
					})
					if (response) {
						await onSuccess?.()
						onClose()
					}
				}}
			>
				{t('order_management.button.paid')}
			</Button>
		)
	}

	if (
		(order.orderStatus === EnumConfig.OrderStatus.Paid && order.isPickupAtStore === true) ||
		(order.orderStatus === EnumConfig.OrderStatus.Shipping && order.isPickupAtStore === false)
	) {
		buttons.push(
			<Button
				key='completed'
				variant='contained'
				color='success'
				onClick={async () => {
					const isConfirmed = await confirm({
						title: t('order_management.confirm.completed_title'),
						description: t('order_management.confirm.completed_description'),
						confirmText: t('order_management.button.completed'),
						confirmColor: 'success',
					})
					if (!isConfirmed) return
					const response = await completeSubmit.submit({
						overrideUrl: ApiUrls.ORDER.MANAGEMENT.COMPLETE(order.id),
					})
					if (response) {
						await onSuccess?.()
						onClose()
					}
				}}
			>
				{t('order_management.button.completed')}
			</Button>
		)
	}

	const orderDetails = (order.orderDetails || []).map((d) => {
		const snapshot = d.medicineSnapshot || {}
		return {
			...d,
			medicineName: renderEmptyFallback(snapshot.medicineName || d.skuCode),
			medicineBrand: renderEmptyFallback(snapshot.medicineBrand),
			medicineUnit: renderEmptyFallback(snapshot.medicineUnit),
			medicineImages: snapshot.medicineImages || [],
		}
	})

	const fields = [
		{
			title: (
				<Stack direction='row' alignItems='center' justifyContent='space-between' spacing={1}>
					<Typography variant='subtitle1'>{t('order_management.title.order_info')}</Typography>
					<Chip
						label={statusLabel}
						size='small'
						sx={{
							bgcolor: statusStyle.bg,
							color: statusStyle.color,
						}}
					/>
				</Stack>
			),
			of: [
				{ label: t('order_management.field.id'), value: order.id },
				{ label: t('order_management.field.order_date'), value: formatDateBasedOnCurrentLanguage(order.orderDate) },
				{
					label: t('order_management.field.delivery_date'),
					value: renderEmptyFallback(
						order.deliveryDate ? formatDateBasedOnCurrentLanguage(order.deliveryDate) : null
					),
				},
				{
					label: t('order_management.field.is_pickup_at_store'),
					value: order.isPickupAtStore
						? t('order_management.text.pickup_at_store')
						: t('order_management.text.delivery'),
				},
				{
					label: t('order_management.field.paid_date'),
					value: renderEmptyFallback(order.paidDate ? formatDateBasedOnCurrentLanguage(order.paidDate) : null),
				},
				{
					label: t('order_management.field.voucher'),
					value: renderEmptyFallback(order.voucherCode),
				},
			],
		},
		{
			title: t('order_management.title.patient_info'),
			of: [
				{ label: t('order_management.field.patient_name'), value: renderEmptyFallback(order.patientName) },
				{ label: t('order_management.field.patient_phone'), value: renderEmptyFallback(order.patientPhone) },
				{ label: t('order_management.field.patient_email'), value: renderEmptyFallback(order.patientEmail) },
				{ label: t('order_management.field.patient_address'), value: renderEmptyFallback(order.patientAddress) },
			],
		},
		{
			title: t('order_management.text.order_details'),
			of: orderDetails.map((d) => ({
				label: (
					<Stack direction='row' alignItems='center' spacing={1.5}>
						{d.medicineImages.length > 0 ? (
							<Avatar
								src={getImageFromCloud(d.medicineImages[0])}
								variant='rounded'
								sx={{ width: 44, height: 44 }}
							/>
						) : (
							<Avatar variant='rounded' sx={{ width: 44, height: 44, bgcolor: 'action.hover' }}>
								?
							</Avatar>
						)}
						<Stack spacing={0.3}>
							<Typography variant='subtitle2'>{d.medicineName}</Typography>
							<Typography variant='caption' color='text.secondary'>
								{d.medicineBrand}
							</Typography>
						</Stack>
					</Stack>
				),
				value: (
					<Stack spacing={0.5} alignItems='flex-end'>
						<Typography variant='caption'>
							{d.medicineUnit} | x{d.quantity} | {formatCurrencyBasedOnCurrentLanguage(d.unitPrice)}
						</Typography>
						{d.discountAmount > 0 && (
							<Typography variant='caption' color='error'>
								-{formatCurrencyBasedOnCurrentLanguage(d.discountAmount)}
							</Typography>
						)}
					</Stack>
				),
				fullWidth: false,
			})),
		},
		{
			title: t('order_management.title.price_summary'),
			of: [
				{
					label: t('order_management.field.origin_price'),
					value: formatCurrencyBasedOnCurrentLanguage(order.originPrice),
				},
				{
					label: t('order_management.field.total_discount'),
					value: `-${formatCurrencyBasedOnCurrentLanguage(order.totalDiscountAmount)}`,
				},
				{
					label: t('order_management.field.final_price'),
					value: (
						<Typography variant='subtitle1' fontWeight={800} color='primary'>
							{formatCurrencyBasedOnCurrentLanguage(order.finalPrice)}
						</Typography>
					),
				},
			],
		},
	].filter(Boolean)

	return (
		<GenericDrawer
			open={open}
			onClose={onClose}
			title={t('order_management.title.detail')}
			fields={fields}
			buttons={
				<Stack direction='row' spacing={1} justifyContent='flex-end' width='100%'>
					{buttons}
				</Stack>
			}
		/>
	)
}

export default OrderManagementDetailDrawerSection

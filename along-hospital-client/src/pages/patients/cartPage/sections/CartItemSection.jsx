import useDebounce from '@/hooks/useDebounce'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Add, Delete as DeleteIcon, Remove } from '@mui/icons-material'
import {
	Avatar,
	Box,
	Button,
	Card,
	Chip,
	IconButton,
	Stack,
	TextField,
	Typography,
} from '@mui/material'
import { useMemo, useRef, useState } from 'react'

const CartItemSection = ({ validDetails, drafts, setDraft, onUpdate, onRemove, isSubmitting }) => {
	const details = useMemo(() => validDetails || [], [validDetails])
	const { t } = useTranslation()
	const [submitting, setSubmitting] = useState(false)
	const pendingUpdateRef = useRef(null)

	useDebounce(
		() => {
			if (pendingUpdateRef.current) {
				const { skuCode, quantity } = pendingUpdateRef.current
				pendingUpdateRef.current = null
				onUpdate(skuCode, quantity, () => setSubmitting(false))
			}
		},
		500,
		[pendingUpdateRef.current]
	)

	const handleQuantityChange = (draftKey, value) => {
		const numValue = Math.floor(Number(value))
		if (!Number.isFinite(numValue) || numValue < 0) {
			setDraft(draftKey, undefined)
			return
		}

		setDraft(draftKey, String(numValue))
		setSubmitting(true)

		if (numValue <= 0) {
			onUpdate(draftKey, numValue, () => setSubmitting(false))
			return
		}

		pendingUpdateRef.current = { skuCode: draftKey, quantity: numValue }
	}

	return (
		<Box sx={{ maxHeight: '80vh', overflowY: 'auto', pr: 1 }}>
			<Stack spacing={2}>
				{details.map((item) => {
					const skuCode = item.medicineSKU?.skuCode || item.skuCode
					if (!skuCode) return null

					const draftKey = String(skuCode)
					const draftValue = drafts[draftKey] ?? String(item.quantity ?? '')

					const displayName = item.medicineSKU?.medicineName || ''
					const displayBrand = item.medicineSKU?.medicineBrand || ''
					const displayUnit = item.medicineSKU?.medicineUnit || ''
					const optionValues = (item.medicineSKU?.skuValues || [])
						.map((sv) => sv.valueName || '')
						.filter(Boolean)
						.join(', ')

					const images = item.medicineSKU?.medicineImages ?? []
					const unitPrice = item.discountPrice ?? item.originPrice ?? item.medicineSKU?.price ?? 0
					const maxQuantity = item.medicineSKU?.quantity ?? 0
					const isOutOfStock = item.quantity > maxQuantity

					const preview = images.length > 0 ? getImageFromCloud(images[0]) : null

					return (
						<Card
							key={skuCode}
							sx={{ display: 'flex', overflow: 'visible', width: '100%', minHeight: 180 }}
						>
							<Box
								sx={{
									width: 180,
									height: 180,
									bgcolor: 'background.default',
									flexShrink: 0,
									display: 'flex',
									alignItems: 'center',
									justifyContent: 'center',
								}}
							>
								{preview ? (
									<Avatar
										variant='rounded'
										src={preview}
										alt={displayName}
										sx={{ width: '100%', height: '100%' }}
									/>
								) : (
									<Typography color='text.secondary'>{t('cart.no_image')}</Typography>
								)}
							</Box>

							<Box sx={{ p: 2, flex: 1 }}>
								<Stack spacing={2}>
									<Stack direction='row' justifyContent='space-between'>
										<Box flex={1}>
											<Typography variant='h6' sx={{ fontWeight: 700 }}>
												{displayName || t('cart.no_name')}
											</Typography>
											<Typography variant='body2' color='text.secondary'>
												{t('cart.brand')}: {renderEmptyFallback(displayBrand)}
											</Typography>
											<Typography variant='body2' color='text.secondary'>
												{t('cart.unit')}: {renderEmptyFallback(displayUnit)}
											</Typography>
											<Typography variant='body2' color='text.secondary'>
												{t('medicine_sku.field.option_values')}: {renderEmptyFallback(optionValues)}
											</Typography>
											{isOutOfStock && (
												<Chip
													label={t('cart.out_of_stock')}
													color='error'
													size='small'
													sx={{ width: 'fit-content', mt: 0.5 }}
												/>
											)}
											<Stack direction='row' spacing={1} alignItems='center' sx={{ mt: 1 }}>
												{unitPrice < (item.originPrice ?? item.medicineSKU?.price ?? 0) && (
													<Typography
														variant='body2'
														color='text.disabled'
														sx={{ textDecoration: 'line-through' }}
													>
														{formatCurrencyBasedOnCurrentLanguage(
															item.originPrice ?? item.medicineSKU?.price ?? 0
														)}
													</Typography>
												)}
												<Typography variant='body2' sx={{ fontWeight: 600 }}>
													{formatCurrencyBasedOnCurrentLanguage(unitPrice)}
												</Typography>
												<Typography component='span' variant='body2' color='text.secondary'>
													x {item.quantity}
												</Typography>
											</Stack>
										</Box>

										<Stack direction='row' alignItems='center' spacing={2}>
											<Stack
												direction='row'
												alignItems='center'
												sx={{ border: '1px solid', borderColor: 'divider', borderRadius: 1 }}
											>
												<IconButton
													size='small'
													disabled={submitting || isSubmitting}
													onClick={() => handleQuantityChange(draftKey, item.quantity - 1)}
												>
													<Remove fontSize='small' />
												</IconButton>

												<TextField
													value={draftValue}
													onChange={(e) => {
														const raw = e.target.value
														if (!/^\d*$/.test(raw)) return
														handleQuantityChange(draftKey, raw)
													}}
													size='small'
													sx={{ width: 50 }}
													inputProps={{
														min: 0,
														inputMode: 'numeric',
														pattern: '[0-9]*',
														style: { textAlign: 'center' },
													}}
												/>

												<IconButton
													size='small'
													disabled={submitting || isSubmitting || Number(draftValue) >= maxQuantity}
													onClick={() => handleQuantityChange(draftKey, item.quantity + 1)}
												>
													<Add fontSize='small' />
												</IconButton>
											</Stack>

											<Button
												startIcon={<DeleteIcon />}
												color='error'
												size='small'
												disabled={submitting || isSubmitting}
												onClick={() => onRemove(skuCode)}
											>
												{t('button.delete')}
											</Button>
										</Stack>
									</Stack>
								</Stack>
							</Box>
						</Card>
					)
				})}
			</Stack>
		</Box>
	)
}

export default CartItemSection

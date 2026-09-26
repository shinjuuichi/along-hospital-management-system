import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Box, Button, Chip, Divider, Stack, Typography } from '@mui/material'
import { useTheme } from '@mui/material/styles'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const MedicineDetailInfoSection = ({ medicine, quantity, setQuantity, onAddToCart, loading }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const [selectedSkuId, setSelectedSkuId] = useState(null)

	const activeSkus = useMemo(
		() =>
			(medicine?.skus || [])
				.filter((sku) => sku?.isActive !== false)
				.map((sku) => ({
					id: sku.id,
					skuCode: sku.skuCode,
					name: sku.name,
					origionPrice: sku.origionPrice || 0,
					price: sku.price || 0,
					finalPrice: sku.finalPrice || sku.price || 0,
					discountAmount: sku.discountAmount || 0,
					label: sku.skuValues?.[0]?.optionValue?.valueName || '',
					skuValues: sku.skuValues || [],
					inventory: sku.inventory,
					isOutOfStock: !sku?.inventory?.quantity || sku?.inventory?.quantity <= 0,
				})),
		[medicine]
	)

	const inStockSkus = useMemo(
		() => activeSkus.filter((sku) => !sku.isOutOfStock),
		[activeSkus]
	)

	const isAllOutOfStock = inStockSkus.length === 0

	useEffect(() => {
		if (!selectedSkuId && activeSkus.length > 0) {
			setSelectedSkuId(activeSkus[0].id)
		}
	}, [activeSkus, selectedSkuId])

	const selectedSku = useMemo(() => {
		if (!activeSkus?.length) return null

		if (selectedSkuId) {
			const found = activeSkus.find((sku) => sku.id === selectedSkuId)
			if (found) return found
		}

		return activeSkus[0]
	}, [activeSkus, selectedSkuId])

	return (
		<Stack spacing={3}>
			{medicine.medicineCategory && (
				<Chip
					label={medicine.medicineCategory.name}
					sx={{
						bgcolor: theme.palette.info.light,
						color: theme.palette.info.contrastText,
						fontWeight: 600,
						width: 'fit-content',
						fontSize: '0.85rem',
					}}
				/>
			)}

			<Box>
				<Typography variant='h4' sx={{ fontWeight: 700, mb: 1, lineHeight: 1.3 }}>
					{renderEmptyFallback(medicine.name)}
				</Typography>
				<Typography variant='body2' sx={{ color: theme.palette.text.secondary, mb: 2 }}>
					{medicine.medicineCategory?.description}
				</Typography>
			</Box>

			<Divider />

			<Stack spacing={2}>
				<Stack direction='row' spacing={4}>
					{medicine.brand && (
						<Stack direction='row' spacing={1}>
							<Typography sx={{ fontWeight: 600, color: theme.palette.text.secondary }}>
								{t('medicine.field.brand')}:
							</Typography>
							<Typography sx={{ fontWeight: 500 }}>{medicine.brand}</Typography>
						</Stack>
					)}
					{medicine.medicineUnit?.name && (
						<Stack direction='row' spacing={1}>
							<Typography sx={{ fontWeight: 600, color: theme.palette.text.secondary }}>
								{t('medicine.field.unit')}:
							</Typography>
							<Typography sx={{ fontWeight: 500 }}>{medicine.medicineUnit.name}</Typography>
						</Stack>
					)}
				</Stack>

				{selectedSku && activeSkus.length > 0 && (
					<Stack spacing={0.5}>
						{selectedSku.skuValues?.length > 0 && (
							<Typography variant='body2' fontWeight='bold' color='secondary.main'>
								{t('shop.button.option_values')}: {selectedSku.skuValues.map((sv) => sv.optionValue?.valueName).filter(Boolean).join(' , ')}
							</Typography>
						)}

						<Box
							sx={{
								display: 'inline-flex',
								alignItems: 'center',
								backgroundColor: selectedSku.isOutOfStock ? 'error.light' : 'success.light',
								color: selectedSku.isOutOfStock ? 'error.dark' : 'success.dark',
								px: 1,
								py: 0.25,
								borderRadius: 1,
								fontWeight: 'bold',
								fontSize: '0.75rem',
								width: 'fit-content',
							}}
						>
							{t('medicine.field.quantity')}: {selectedSku.inventory?.quantity ?? 0}
						</Box>
					</Stack>
				)}

				{activeSkus.length > 0 && (
					<Stack direction='row' spacing={1} flexWrap='wrap'>
						{activeSkus.map((sku, idx) => {
							const isSelected = selectedSku?.id === sku.id
							const firstValueName = sku.name
							return (
								<Button
									key={sku.id || idx}
									variant={isSelected ? 'contained' : 'outlined'}
									size='small'
									disabled={sku.isOutOfStock}
									onClick={() => setSelectedSkuId(sku.id)}
									sx={(theme) => ({
										minWidth: 'auto',
										px: 2,
										py: 0.75,
										fontSize: '0.875rem',
										textTransform: 'none',
										borderRadius: 1,
										borderColor: isSelected ? theme.palette.secondary.main : theme.palette.divider,
										backgroundColor: isSelected ? theme.palette.secondary.main : 'transparent',
										color: isSelected ? theme.palette.secondary.contrastText : theme.palette.text.secondary,
										'&:hover': {
											backgroundColor: isSelected ? theme.palette.secondary.dark : theme.palette.action.hover,
											borderColor: theme.palette.secondary.main,
										},
									})}
								>
									{firstValueName}
								</Button>
							)
						})}
					</Stack>
				)}
			</Stack>

			<Divider />

			<Box
				sx={{
					bgcolor: theme.palette.background.paper,
					p: 2,
					borderRadius: 1.5,
					border: `1px solid ${theme.palette.divider}`,
				}}
			>
				<Stack spacing={1}>
					<Typography sx={{ color: theme.palette.text.secondary, fontSize: '0.9rem' }}>
						{t('medicine.field.price')}
					</Typography>
					<Stack direction='row' spacing={2} alignItems='center' flexWrap='wrap'>
						{selectedSku ? (
							selectedSku.isOutOfStock ? (
								<Typography sx={{ fontSize: '1.1rem', color: theme.palette.error.main }}>
									{t('shop.text.out_of_stock')}
								</Typography>
							) : selectedSku.origionPrice && selectedSku.origionPrice > selectedSku.price ? (
								<>
									<Typography
										sx={{
											textDecoration: 'line-through',
											color: theme.palette.text.disabled,
											fontSize: '1.1rem',
										}}
									>
										{formatCurrencyBasedOnCurrentLanguage(selectedSku.origionPrice)}
									</Typography>
									<Typography
										sx={{
											fontSize: '1.8rem',
											fontWeight: 700,
											color: theme.palette.primary.main,
										}}
									>
										{formatCurrencyBasedOnCurrentLanguage(selectedSku.price)}
									</Typography>
									{selectedSku.discountAmount > 0 && (
										<Chip
											label={`-${formatCurrencyBasedOnCurrentLanguage(selectedSku.discountAmount)}`}
											sx={{
												bgcolor: theme.palette.primary.light,
												color: theme.palette.primary.main,
												fontWeight: 700,
												ml: 'auto',
											}}
										/>
									)}
								</>
							) : (
								<Typography
									sx={{ fontSize: '1.8rem', fontWeight: 700, color: theme.palette.secondary.main }}
								>
									{formatCurrencyBasedOnCurrentLanguage(selectedSku.price)}
								</Typography>
							)
						) : (
							<Typography sx={{ fontSize: '1.1rem', color: theme.palette.text.secondary }}>
								{t('shop.text.select_full_variant_to_view_price')}
							</Typography>
						)}
					</Stack>
				</Stack>
			</Box>

			{!isAllOutOfStock && (
				<Box>
					<Typography sx={{ fontWeight: 600, mb: 1.5 }}>{t('medicine.field.quantity')}</Typography>
					<Box
						sx={{
							display: 'flex',
							alignItems: 'center',
							border: `1px solid ${theme.palette.divider}`,
							borderRadius: 1,
							width: 'fit-content',
							bgcolor: theme.palette.background.paper,
						}}
					>
						<Button size='small' onClick={() => setQuantity(Math.max(1, quantity - 1))}>
							−
						</Button>
						<Divider orientation='vertical' flexItem />
						<Typography sx={{ px: 2, minWidth: 50, textAlign: 'center', fontWeight: 600 }}>
							{quantity}
						</Typography>
						<Divider orientation='vertical' flexItem />
						<Button size='small' onClick={() => setQuantity(quantity + 1)}>
							+
						</Button>
					</Box>
				</Box>
			)}

			{isAllOutOfStock ? (
				<Button
					variant='outlined'
					size='large'
					disabled
					sx={{ mt: 2, py: 1.5, fontWeight: 600, borderRadius: 1 }}
				>
					{t('shop.text.out_of_stock')}
				</Button>
			) : (
				<Button
					variant='contained'
					size='large'
					onClick={() => {
						if (!selectedSku) return
						onAddToCart(selectedSku.skuCode)
					}}
					disabled={loading || selectedSku?.isOutOfStock}
					sx={{ mt: 2, py: 1.5, fontWeight: 600, borderRadius: 1 }}
				>
					{loading ? t('shop.button.adding_to_cart') : t('shop.button.add_to_cart')}
				</Button>
			)}
		</Stack>
	)
}

export default MedicineDetailInfoSection

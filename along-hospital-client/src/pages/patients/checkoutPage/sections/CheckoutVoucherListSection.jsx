import useDebounce from '@/hooks/useDebounce'
import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined'
import { Box, Chip, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const CheckoutVoucherListSection = ({ allVouchers, appliedVoucher, onApply, isApplying }) => {
	const { t } = useTranslation()
	const [pendingApply, setPendingApply] = useState(null)

	const handleApply = (code) => {
		setPendingApply(code)
	}

	useDebounce(() => {
		if (pendingApply) {
			onApply(pendingApply)
			setPendingApply(null)
		}
	}, 500, [pendingApply])

	return (
		<Paper
			sx={(theme) => ({
				p: 3,
				borderRadius: 2,
				border: `1px solid ${theme.palette.divider}`,
				backgroundColor: theme.palette.background.paper,
			})}
		>
			<Stack direction='row' alignItems='center' spacing={1} mb={2}>
				<LocalShippingOutlinedIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
				<Typography
					variant='h6'
					sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
				>
					{t('cart.summary.your_vouchers')}
				</Typography>
			</Stack>

			<Box sx={{ maxHeight: 400, overflowY: 'auto', pr: 0.5 }}>
				<Stack spacing={1.5}>
					{allVouchers.length === 0 && (
						<Typography variant='body2' sx={(theme) => ({ color: theme.palette.text.secondary })}>
							{t('cart.summary.no_voucher')}
						</Typography>
					)}

					{allVouchers.map((v) => {
					const isActive = appliedVoucher?.toLowerCase() === v.code?.toLowerCase()
					const isDisabled = !v.isAvailable
					return (
						<Paper
							key={v.id || v.code}
							sx={(theme) => ({
								p: 2,
								borderRadius: 2,
								border: `2px solid ${isActive ? theme.palette.primary.main : theme.palette.divider}`,
								backgroundColor: isActive ? theme.palette.action.hover : isDisabled ? theme.palette.action.disabledBackground : theme.palette.background.paper,
								cursor: isDisabled ? 'not-allowed' : 'pointer',
								opacity: isDisabled ? 0.5 : 1,
								transition: 'all 0.2s ease',
							})}
							onClick={() => !isApplying && !isDisabled && handleApply(v.code)}
						>
							<Stack direction='row' alignItems='flex-start' spacing={2}>
								{v.image ? (
									<Box
										component='img'
										src={v.image}
										alt={v.name || v.code}
										sx={{
											width: 80,
											height: 80,
											objectFit: 'cover',
											borderRadius: 1,
											backgroundColor: '#f5f5f5',
										}}
									/>
								) : (
									<Box
										sx={{
											width: 80,
											height: 80,
											display: 'flex',
											alignItems: 'center',
											justifyContent: 'center',
											backgroundColor: 'primary.main',
											borderRadius: 1,
											color: 'white',
											fontWeight: 800,
											fontSize: 24,
										}}
									>
										{v.discountType === 'Percentage' ? '%' : '$'}
									</Box>
								)}

								<Box flex={1}>
									<Stack direction='row' alignItems='center' spacing={1} flexWrap='wrap' useFlexGap>
										<Typography
											variant='subtitle1'
											sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
										>
											{v.name || v.code}
										</Typography>
										<Chip
											size='small'
											label={v.valueLabel}
											sx={(theme) => ({
												backgroundColor: theme.palette.primary.main,
												color: theme.palette.primary.contrastText,
												fontWeight: 700,
											})}
										/>
									</Stack>

									<Typography variant='body2' sx={(theme) => ({ color: theme.palette.text.secondary, mt: 0.5 })}>
										{v.description}
									</Typography>

									<Stack direction='row' spacing={2} mt={1} alignItems='center' flexWrap='wrap' useFlexGap>
										{v.minPrice ? (
											<Typography variant='caption' sx={(theme) => ({ color: theme.palette.text.secondary })}>
												{t('cart.summary.min_order')}: {formatCurrencyBasedOnCurrentLanguage(v.minPrice)}
											</Typography>
										) : null}

										{v.expiry ? (
											<Typography variant='caption' sx={(theme) => ({ color: theme.palette.text.secondary })}>
												{t('cart.summary.expiry')}: {new Date(v.expiry).toLocaleDateString()}
											</Typography>
										) : null}

										{v.maxDiscount ? (
											<Typography variant='caption' sx={(theme) => ({ color: theme.palette.text.secondary })}>
												Max: {formatCurrencyBasedOnCurrentLanguage(v.maxDiscount)}
											</Typography>
										) : null}
									</Stack>
								</Box>

								{isActive ? (
									<CheckCircleIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
								) : null}
							</Stack>
						</Paper>
					)
				})}
				</Stack>
			</Box>
		</Paper>
	)
}

export default CheckoutVoucherListSection

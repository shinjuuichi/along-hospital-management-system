import { defaultVoucherTypeStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { AccessTime, ContentCopy, Inventory2Rounded, LocalOffer } from '@mui/icons-material'
import {
	Box,
	Button,
	Card,
	CardActions,
	CardContent,
	CardMedia,
	Chip,
	CircularProgress,
	IconButton,
	Stack,
	Tooltip,
	Typography,
	useTheme,
} from '@mui/material'
import { useMemo } from 'react'

const VoucherCardSection = ({
	voucher,
	mode = 'collectible',
	onCollect,
	onCopy,
	onUse,
	loading = false,
	userRole,
}) => {
	const theme = useTheme()
	const { t } = useTranslation()

	const formattedExpireDate = useMemo(
		() => (voucher?.expireDate ? formatDateBasedOnCurrentLanguage(voucher.expireDate) : null),
		[voucher?.expireDate]
	)

	const isExpired = useMemo(() => {
		if (!voucher?.expireDate) return false
		return new Date() > new Date(voucher.expireDate)
	}, [voucher?.expireDate])

	const discountDisplay = useMemo(() => {
		if (!voucher) return ''
		return voucher.discountType === EnumConfig.VoucherDiscountType.Percentage
			? `−${voucher.discountValue}%`
			: `−${formatCurrencyBasedOnCurrentLanguage(voucher.discountValue)}`
	}, [voucher])
	return (
		<Card
			sx={{
				display: 'flex',
				flexDirection: 'row',
				position: 'relative',
				overflow: 'hidden',
				transition: 'all 0.3s ease',
				height: '100%',
				'&:hover': {
					transform: 'translateY(-2px)',
					boxShadow: theme.shadows[8],
				},
			}}
		>
			<CardMedia
				component='img'
				sx={{
					width: 120,
					minWidth: 120,
					objectFit: 'cover',
					bgcolor: theme.palette.grey[200],
				}}
				image={getImageFromCloud(voucher?.image)}
				alt={voucher?.name || 'Voucher'}
			/>

			<Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
				<CardContent sx={{ flex: 1, pb: 1 }}>
					<Chip
						label={t('voucher.type.Patient')}
						size='small'
						color={defaultVoucherTypeStyle(voucher?.voucherType)}
						variant='outlined'
						sx={{ fontWeight: 600, fontSize: '0.7rem', mb: 1.5 }}
					/>

					<Typography
						variant='h6'
						component='h3'
						gutterBottom
						sx={{ fontWeight: 700, fontSize: '1rem', mb: 1 }}
					>
						{voucher?.name || 'Voucher Name'}
					</Typography>

					{voucher?.description && (
						<Typography
							variant='body2'
							color='text.secondary'
							sx={{
								mb: 1.5,
								display: '-webkit-box',
								WebkitLineClamp: 2,
								WebkitBoxOrient: 'vertical',
								overflow: 'hidden',
							}}
						>
							{voucher.description}
						</Typography>
					)}

					<Box
						sx={{
							display: 'inline-flex',
							alignItems: 'center',
							gap: 0.5,
							mb: 1.5,
							px: 1.5,
							py: 0.5,
							bgcolor: theme.palette.primary.main,
							color: 'white',
							borderRadius: 1,
						}}
					>
						<Typography variant='body2' component='div' sx={{ fontWeight: 700 }}>
							{discountDisplay}
						</Typography>
					</Box>

					<Stack spacing={0.5} sx={{ minHeight: '80px' }}>
						{formattedExpireDate && (
							<Stack direction='row' alignItems='center' spacing={0.5}>
								<AccessTime sx={{ fontSize: '0.875rem', color: 'text.secondary' }} />
								<Typography
									variant='caption'
									color={isExpired ? 'error' : 'text.secondary'}
									sx={{ wordBreak: 'break-word', fontWeight: isExpired ? 600 : 400 }}
								>
									{t('voucher.label.expire_date')}: {formattedExpireDate}
								</Typography>
							</Stack>
						)}

						{mode === 'collectible' && voucher?.quantity !== undefined && (
							<Stack direction='row' alignItems='center' spacing={0.5}>
								<Inventory2Rounded sx={{ fontSize: '0.875rem', color: 'text.secondary' }} />
								<Typography variant='caption' color='text.secondary' sx={{ wordBreak: 'break-word' }}>
									{t('voucher.label.remaining_quantity', { quantity: voucher.quantity })}
								</Typography>
							</Stack>
						)}

						{voucher?.minPurchaseAmount > 0 && (
							<Stack direction='row' alignItems='center' spacing={0.5}>
								<LocalOffer sx={{ fontSize: '0.875rem', color: 'text.secondary' }} />
								<Typography variant='caption' color='text.secondary' sx={{ wordBreak: 'break-word' }}>
									{t('voucher.label.min_purchase', {
										amount: formatCurrencyBasedOnCurrentLanguage(voucher.minPurchaseAmount),
									})}
								</Typography>
							</Stack>
						)}
						{voucher?.maxDiscount > 0 && (
							<Stack direction='row' alignItems='center' spacing={0.5}>
								<LocalOffer sx={{ fontSize: '0.875rem', color: 'text.secondary' }} />
								<Typography variant='caption' color='text.secondary' sx={{ wordBreak: 'break-word' }}>
									{t('voucher.label.max_discount', {
										amount: formatCurrencyBasedOnCurrentLanguage(voucher.maxDiscount),
									})}
								</Typography>
							</Stack>
						)}

						{mode === 'my' && voucher?.code && (
							<Stack
								direction='row'
								alignItems='center'
								spacing={0.5}
								sx={{
									mt: 0.5,
									p: 0.75,
									bgcolor: theme.palette.grey[100],
									borderRadius: 1,
									border: `1px dashed ${theme.palette.divider}`,
								}}
							>
								<Typography
									variant='caption'
									sx={{ fontFamily: 'monospace', fontWeight: 600, flexGrow: 1 }}
								>
									{voucher.code}
								</Typography>
								<Tooltip title={t('voucher.button.copy_code')}>
									<IconButton size='small' onClick={() => onCopy?.(voucher.code)} sx={{ p: 0.5 }}>
										<ContentCopy sx={{ fontSize: '1rem' }} />
									</IconButton>
								</Tooltip>
							</Stack>
						)}
					</Stack>
				</CardContent>

				<CardActions sx={{ p: 1.5, pt: 0 }}>
					{mode === 'collectible' ? (
						userRole === 'Patient' && (
							<Button
								variant='contained'
								color='primary'
								size='small'
								disabled={isExpired || loading}
								onClick={() => onCollect?.(voucher)}
								startIcon={loading ? <CircularProgress size={16} color='inherit' /> : <LocalOffer />}
							>
								{t('voucher.button.collect')}
							</Button>
						)
					) : (
						<Button
							variant='contained'
							color='primary'
							size='small'
							disabled={isExpired || loading}
							onClick={() => onUse?.(voucher)}
							startIcon={loading && <CircularProgress size={16} color='inherit' />}
						>
							{t('voucher.button.use_now')}
						</Button>
					)}
				</CardActions>
			</Box>
		</Card>
	)
}

export default VoucherCardSection

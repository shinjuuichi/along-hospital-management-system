import useTranslation from '@/hooks/useTranslation'
import LocalOfferOutlinedIcon from '@mui/icons-material/LocalOfferOutlined'
import { Button, Paper, Stack, TextField, Typography } from '@mui/material'
import { useEffect, useState } from 'react'

const CheckoutVoucherInputSection = ({
	voucherDraft,
	appliedVoucher,
	onChangeDraft,
	onApply,
	onCancel,
	isApplying,
	setIsTyping,
}) => {
	const { t } = useTranslation()
	const [localDraft, setLocalDraft] = useState(voucherDraft)
	const [isTyping, setIsTypingLocal] = useState(false)

	useEffect(() => {
		if (!isTyping) {
			setLocalDraft(voucherDraft)
		}
	}, [voucherDraft, isTyping])

	const handleChange = (e) => {
		setLocalDraft(e.target.value)
		onChangeDraft(e.target.value)
		setIsTypingLocal(true)
		setIsTyping?.(true)
	}

	const handleBlur = () => {
		setIsTypingLocal(false)
		setIsTyping?.(false)
	}

	const isApplied = appliedVoucher !== null

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
				<LocalOfferOutlinedIcon sx={(theme) => ({ color: theme.palette.primary.main })} />
				<Typography
					variant='h6'
					sx={(theme) => ({ fontWeight: 800, color: theme.palette.text.primary })}
				>
					{t('cart.summary.voucher_code')}
				</Typography>
			</Stack>

			<Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5}>
				<TextField
					fullWidth
					placeholder={t('cart.summary.voucher_placeholder')}
					value={localDraft}
					onChange={handleChange}
					onBlur={handleBlur}
					size='small'
				/>

				{isApplied ? (
					<Button
						variant='outlined'
						color='error'
						sx={{ minWidth: 140 }}
						onClick={onCancel}
						disabled={isApplying}
					>
						{t('cart.summary.cancel_voucher') || 'Cancel'}
					</Button>
				) : (
					<Button
						variant='contained'
						sx={(theme) => ({
							minWidth: 140,
							backgroundColor: theme.palette.primary.main,
							'&:hover': { backgroundColor: theme.palette.primary.dark },
						})}
						onClick={() => onApply(localDraft)}
						disabled={isApplying || !localDraft.trim()}
					>
						{t('cart.summary.apply')}
					</Button>
				)}
			</Stack>
		</Paper>
	)
}

export default CheckoutVoucherInputSection

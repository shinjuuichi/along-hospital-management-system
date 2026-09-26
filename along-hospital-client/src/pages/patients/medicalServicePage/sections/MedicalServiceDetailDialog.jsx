import useTranslation from '@/hooks/useTranslation'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { Close } from '@mui/icons-material'
import {
	Box,
	Button,
	Chip,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	IconButton,
	Stack,
	Typography,
} from '@mui/material'

const MedicalServiceDetailDialog = ({ open, service, onClose, onBook }) => {
	const { t } = useTranslation()
	const description = service?.description
	const price = service?.price

	return (
		<Dialog
			open={open}
			onClose={onClose}
			maxWidth='md'
			fullWidth
			PaperProps={{ sx: { overflowX: 'hidden' } }}
		>
			<DialogTitle sx={{ pr: 6, position: 'relative' }}>
				{service?.name}
				<IconButton
					edge='end'
					aria-label='close'
					onClick={onClose}
					sx={{ position: 'absolute', right: 8, top: 8 }}
				>
					<Close />
				</IconButton>
			</DialogTitle>
			<DialogContent sx={{ overflowX: 'hidden' }}>
				<Stack spacing={2}>
					<Box>
						<Typography variant='subtitle2' fontWeight={600} gutterBottom>
							{t('medical_service.label.price')}
						</Typography>
						<Chip
							label={formatCurrencyBasedOnCurrentLanguage(price)}
							color='primary'
							variant='outlined'
							sx={{ fontSize: '1rem', fontWeight: 600 }}
						/>
					</Box>

					<Box>
						<Typography variant='subtitle2' fontWeight={600} gutterBottom>
							{t('medical_service.label.description')}
						</Typography>
						<Typography variant='body1' color='text.secondary' sx={{ whiteSpace: 'pre-line' }}>
							{description}
						</Typography>
					</Box>

					<Box
						sx={(theme) => ({
							p: 2,
							backgroundColor: theme.palette.action.hover,
							borderRadius: 1,
						})}
					>
						<Typography variant='caption' color='text.secondary'>
							{t('medical_service.note.price_note')}
						</Typography>
					</Box>
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose}>{t('button.close')}</Button>
				<Button variant='contained' onClick={onBook}>
					{t('medical_service.button.book_appointment')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default MedicalServiceDetailDialog

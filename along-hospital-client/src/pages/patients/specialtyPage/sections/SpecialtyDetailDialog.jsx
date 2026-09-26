import useTranslation from '@/hooks/useTranslation'
import { Close } from '@mui/icons-material'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	IconButton,
	Typography,
} from '@mui/material'

const SpecialtyDetailDialog = ({ open, specialty, onClose, onBook }) => {
	const { t } = useTranslation()
	const description = specialty?.description

	return (
		<Dialog
			open={open}
			onClose={onClose}
			maxWidth='md'
			fullWidth
			PaperProps={{ sx: { overflowX: 'hidden' } }}
		>
			<DialogTitle sx={{ pr: 6 }}>
				{specialty?.name}
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
				<Typography variant='body1' color='text.secondary' sx={{ whiteSpace: 'pre-line' }}>
					{description}
				</Typography>
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose}>{t('button.close')}</Button>
				<Button variant='contained' onClick={onBook}>
					{t('specialty.button.book_appointment')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default SpecialtyDetailDialog

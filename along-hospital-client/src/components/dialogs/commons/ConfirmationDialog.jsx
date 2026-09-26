import useTranslation from '@/hooks/useTranslation'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle,
	Slide,
} from '@mui/material'
import { forwardRef, useMemo } from 'react'

const Down = forwardRef(function Down(props, ref) {
	return <Slide direction='down' ref={ref} {...props} />
})
const Up = forwardRef(function Up(props, ref) {
	return <Slide direction='up' ref={ref} {...props} />
})

/**
 * @param {Object} props
 * @param {boolean} props.open
 * @param {Function} props.onClose
 * @param {Function} [props.onCancel=props.onClose]
 * @param {Function} props.onConfirm
 * @param {string} [props.title]
 * @param {React.ReactNode} [props.description]
 * @param {string} [props.confirmButtonText='OK']
 * @param {'primary'|'error'|'warning'|'info'|'success'} [props.confirmButtonColor='primary']
 * @param {boolean} [props.confirmButtonLoading=false]
 * @param {'xs'|'sm'|'md'|'lg'|'xl'} [props.dialogWidth='xs']
 * @param {'top' | 'bottom' | 'center'} [props.dialogPosition='top']
 */
const ConfirmationDialog = ({
	open,
	onClose,
	onCancel = onClose,
	onConfirm,
	title,
	description,
	confirmButtonText = 'OK',
	confirmButtonColor = 'primary',
	confirmButtonLoading = false,
	dialogWidth = 'xs',
	dialogPosition = 'top',
	...props
}) => {
	const { t } = useTranslation()

	const transition = useMemo(() => {
		if (dialogPosition === 'top') return Down
		if (dialogPosition === 'bottom') return Up
		return undefined
	}, [dialogPosition])

	const containerSx = useMemo(() => {
		if (dialogPosition === 'top') {
			return { '& .MuiDialog-container': { alignItems: 'flex-start', justifyContent: 'center' } }
		}
		if (dialogPosition === 'bottom') {
			return { '& .MuiDialog-container': { alignItems: 'flex-end', justifyContent: 'center' } }
		}
		return {}
	}, [dialogPosition])

	return (
		<Dialog
			open={open}
			onClose={onClose}
			fullWidth
			maxWidth={dialogWidth}
			scroll='paper'
			slots={{ transition }}
			sx={containerSx}
			{...props}
		>
			{title ? <DialogTitle>{title}</DialogTitle> : null}
			{description ? (
				<DialogContent>
					{typeof description === 'string' ? (
						<DialogContentText>{description}</DialogContentText>
					) : (
						description
					)}
				</DialogContent>
			) : null}
			<DialogActions>
				<Button onClick={onCancel} color='inherit'>
					{t('button.cancel')}
				</Button>
				<Button
					onClick={onConfirm}
					variant='contained'
					color={confirmButtonColor}
					loading={Boolean(confirmButtonLoading)}
				>
					{confirmButtonText}
					{confirmButtonLoading && '...'}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default ConfirmationDialog

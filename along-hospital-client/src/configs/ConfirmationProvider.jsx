import ConfirmationDialog from '@/components/dialogs/commons/ConfirmationDialog'
import useTranslation from '@/hooks/useTranslation'
import { createContext, useCallback, useMemo, useRef, useState } from 'react'

export const ConfirmationContext = createContext(
	/** @type {null | ((opts: ConfirmOptions)=>Promise<boolean>)} */ (null)
)

export default function ConfirmationProvider({ children }) {
	const { t } = useTranslation()
	const [open, setOpen] = useState(false)
	const resolverRef = useRef(null)
	const [opts, setOpts] = useState({
		title: '',
		description: '',
		confirmColor: 'primary',
		confirmText: 'OK',
		width: 'xs',
		position: 'top',
	})

	const confirm = useCallback(
		(options = {}) => {
			return new Promise((resolve) => {
				setOpts((prev) => ({
					title: options.title ?? prev.title,
					description: options.description ?? prev.description,
					confirmColor: options.confirmColor ?? 'primary',
					confirmText: options.confirmText ?? 'OK',
					width: options.width ?? 'xs',
					position: options.position ?? 'top',
				}))
				resolverRef.current = resolve
				setOpen(true)
			})
		},
		[t]
	)

	const handleClose = useCallback((result) => {
		setOpen(false)
		if (resolverRef.current) {
			resolverRef.current(result)
			resolverRef.current = null
		}
	}, [])

	const onClose = useCallback(
		(_, reason) => {
			if (opts.disableBackdropClose && (reason === 'backdropClick' || reason === 'escapeKeyDown'))
				return
			handleClose(false)
		},
		[handleClose]
	)

	const ctxValue = useMemo(() => confirm, [confirm])

	return (
		<ConfirmationContext.Provider value={ctxValue}>
			{children}
			<ConfirmationDialog
				open={open}
				onClose={onClose}
				onCancel={() => handleClose(false)}
				onConfirm={() => handleClose(true)}
				title={opts.title}
				description={opts.description}
				confirmButtonText={opts.confirmText}
				confirmButtonColor={opts.confirmColor}
				dialogWidth={opts.width}
				dialogPosition={opts.position}
			/>
		</ConfirmationContext.Provider>
	)
}

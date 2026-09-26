import { ConfirmationContext } from '@/configs/ConfirmationProvider'
import { useContext } from 'react'

/**
 * @typedef {Object} ConfirmOptions
 * @property {string} title
 * @property {string|import('react').ReactNode} description
 * @property {import('@mui/material').ButtonProps['color']} confirmColor
 * @property {string} confirmText
 * @property {import('@mui/material').DialogProps['maxWidth']} width
 * @property {'top'|'bottom'|'center'} position
 */

/**
 * @returns {(options: ConfirmOptions) => Promise<boolean>}
 */
export default function useConfirm() {
	const ctx = useContext(ConfirmationContext)
	if (!ctx) throw new Error('useConfirm must be used within <ConfirmationProvider/>')

	return (options) => ctx(options)
}

import useConfirm from '@/hooks/useConfirm'
import { Button } from '@mui/material'

/**
 * @typedef {Object} CustomProps
 * @property {string} props.confirmationTitle
 * @property {string} props.confirmationDescription
 * @property {import('@mui/material').DialogProps['maxWidth']} props.dialogWidth
 * @property {'top' | 'bottom' | 'center'} props.dialogPosition
 * @property {string} props.confirmButtonText
 * @property {import('@mui/material').ButtonProps['color']} props.confirmButtonColor
 * @property {function} props.onConfirm
 * /

/**
 * @param {import('@mui/material').ButtonProps & CustomProps} props
 */
const ConfirmationButton = ({
	confirmationTitle,
	confirmationDescription,
	confirmButtonColor = 'primary',
	confirmButtonText = 'OK',
	dialogWidth = 'xs',
	dialogPosition = 'top',
	onConfirm,
	children,
	...props
}) => {
	const confirm = useConfirm()

	const handleClick = async () => {
		const isConfirmed = await confirm({
			title: confirmationTitle,
			description: confirmationDescription,
			confirmColor: confirmButtonColor,
			confirmText: confirmButtonText,
			width: dialogWidth,
			position: dialogPosition,
		})

		if (isConfirmed && onConfirm) {
			onConfirm()
		}
	}

	return (
		<Button variant='contained' {...props} onClick={handleClick}>
			{children}
		</Button>
	)
}
export default ConfirmationButton

/* <ConfirmationButton
	confirmationTitle='Title'
	confirmationDescription='Descripion'
	confirmButtonText='Delete'
	confirmButtonColor='error'
	onConfirm={() => alert('Hello world!')}
>
	Hello world
</ConfirmationButton> 
*/

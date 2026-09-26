import { isPasswordStrong, maxLen } from '@/utils/validateUtil'
import { Visibility, VisibilityOff } from '@mui/icons-material'
import { IconButton, InputAdornment } from '@mui/material'
import React, { forwardRef, useState } from 'react'
import ValidationTextField from './ValidationTextField'

/**
 * @typedef {Object} CustomProps
 * @property {string} label
 * @property {string} name
 * @property {string} value
 * @property {function} onChange
 */

/**
 * @param {import('@mui/material').TextFieldProps & CustomProps} props
 * @param {React.Ref} ref
 */
const PasswordTextField = ({ label, name, value, onChange, validate, ...props }, ref) => {
	const [showPassword, setShowPassword] = useState(false)

	return (
		<ValidationTextField
			ref={ref}
			label={label}
			type={showPassword ? 'text' : 'password'}
			name={name}
			value={value}
			onChange={onChange}
			validate={validate ?? [maxLen(50), isPasswordStrong()]}
			slotProps={{
				input: {
					endAdornment: (
						<InputAdornment position='end'>
							<IconButton onClick={() => setShowPassword(!showPassword)} edge='end' color='default'>
								{showPassword ? <Visibility /> : <VisibilityOff />}
							</IconButton>
						</InputAdornment>
					),
				},
			}}
			{...props}
		/>
	)
}

export default forwardRef(PasswordTextField)

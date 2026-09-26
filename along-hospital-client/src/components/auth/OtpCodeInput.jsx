import { Box, Stack, TextField, Typography } from '@mui/material'
import { forwardRef, useCallback, useEffect, useImperativeHandle, useMemo, useRef, useState } from 'react'

const createEmptyDigits = (length) => Array.from({ length }, () => '')
const normalizeOtpValue = (value, length) => String(value ?? '').replace(/\D/g, '').slice(0, length)
const toDigits = (value, length) => {
	const digits = createEmptyDigits(length)
	normalizeOtpValue(value, length)
		.split('')
		.forEach((digit, index) => {
			digits[index] = digit
		})
	return digits
}

const OtpCodeInput = (
	{
		label,
		value = '',
		onChange,
		length = 6,
		disabled = false,
		errorText = '',
		helperText = '',
		invalidText = '',
		autoFocus = false,
	},
	ref
) => {
	const inputRefs = useRef([])
	const [digits, setDigits] = useState(() => toDigits(value, length))
	const [focusedIndex, setFocusedIndex] = useState(-1)
	const [internalErrorText, setInternalErrorText] = useState('')

	const currentValue = useMemo(() => normalizeOtpValue(digits.join(''), length), [digits, length])
	const normalizedValue = useMemo(() => normalizeOtpValue(value, length), [value, length])
	const resolvedErrorText = errorText || internalErrorText
	const hasError = Boolean(resolvedErrorText)

	useEffect(() => {
		if (normalizedValue !== currentValue) {
			setDigits(toDigits(normalizedValue, length))
		}
	}, [currentValue, length, normalizedValue])

	useEffect(() => {
		if (!autoFocus || disabled) return

		const timer = window.setTimeout(() => {
			inputRefs.current[0]?.focus()
		}, 0)

		return () => window.clearTimeout(timer)
	}, [autoFocus, disabled])

	const focusInput = useCallback(
		(index) => {
			const safeIndex = Math.max(0, Math.min(index, length - 1))
			const nextInput = inputRefs.current[safeIndex]
			if (nextInput) {
				nextInput.focus()
				nextInput.select()
			}
		},
		[length]
	)

	const commitDigits = useCallback(
		(nextDigits, nextFocusIndex = null) => {
			setDigits(nextDigits)
			onChange?.(nextDigits.join(''))

			if (nextFocusIndex !== null) {
				window.setTimeout(() => focusInput(nextFocusIndex), 0)
			}
		},
		[focusInput, onChange]
	)

	const clearInternalError = useCallback(() => {
		setInternalErrorText('')
	}, [])

	const clearInput = useCallback(
		(nextErrorText = '') => {
			setInternalErrorText(nextErrorText)
			commitDigits(createEmptyDigits(length), 0)
		},
		[commitDigits, length]
	)

	const validateInput = useCallback(
		(nextErrorText = invalidText || helperText) => {
			if (currentValue.length === length) {
				clearInternalError()
				return true
			}

			setInternalErrorText(nextErrorText)
			focusInput(0)
			return false
		},
		[clearInternalError, currentValue, focusInput, helperText, invalidText, length]
	)

	useImperativeHandle(
		ref,
		() => ({
			clear: (nextErrorText = '') => {
				clearInput(nextErrorText)
			},
			focus: (index = 0) => {
				focusInput(index)
			},
			getValue: () => currentValue,
			setError: (nextErrorText = '') => {
				setInternalErrorText(nextErrorText)
			},
			validate: (nextErrorText) => validateInput(nextErrorText),
		}),
		[clearInput, currentValue, focusInput, validateInput]
	)

	const handleDigitChange = useCallback(
		(index, nextValue) => {
			const numericValue = nextValue.replace(/\D/g, '')
			const nextDigits = [...digits]
			clearInternalError()

			if (!numericValue) {
				nextDigits[index] = ''
				commitDigits(nextDigits)
				return
			}

			let cursor = index
			numericValue.split('').forEach((digit) => {
				if (cursor >= length) return
				nextDigits[cursor] = digit
				cursor += 1
			})

			const nextFocusIndex = cursor >= length ? length - 1 : cursor
			commitDigits(nextDigits, nextFocusIndex)
		},
		[clearInternalError, commitDigits, digits, length]
	)

	const handleKeyDown = useCallback(
		(index, event) => {
			if (event.key === 'Backspace') {
				event.preventDefault()

				const nextDigits = [...digits]
				clearInternalError()
				if (nextDigits[index]) {
					nextDigits[index] = ''
					commitDigits(nextDigits, index > 0 ? index - 1 : 0)
					return
				}

				if (index > 0) {
					nextDigits[index - 1] = ''
					commitDigits(nextDigits, index - 1)
				}
				return
			}

			if (event.key === 'Delete') {
				event.preventDefault()
				const nextDigits = [...digits]
				clearInternalError()
				nextDigits[index] = ''
				commitDigits(nextDigits)
				return
			}

			if (event.key === 'ArrowLeft') {
				event.preventDefault()
				focusInput(index - 1)
				return
			}

			if (event.key === 'ArrowRight') {
				event.preventDefault()
				focusInput(index + 1)
			}
		},
		[clearInternalError, commitDigits, digits, focusInput]
	)

	const handlePaste = useCallback(
		(index, event) => {
			event.preventDefault()
			const pastedDigits = event.clipboardData.getData('text').replace(/\D/g, '')
			if (!pastedDigits) return

			const nextDigits = [...digits]
			clearInternalError()
			let cursor = index
			pastedDigits.split('').forEach((digit) => {
				if (cursor >= length) return
				nextDigits[cursor] = digit
				cursor += 1
			})

			const nextFocusIndex = cursor >= length ? length - 1 : cursor
			commitDigits(nextDigits, nextFocusIndex)
		},
		[clearInternalError, commitDigits, digits, length]
	)

	return (
		<Box>
			{label ? (
				<Typography variant='subtitle2' sx={{ mb: 1, fontWeight: 700 }}>
					{label}
				</Typography>
			) : null}

			<Stack direction='row' spacing={{ xs: 1, sm: 1.25 }} justifyContent='center'>
				{Array.from({ length }, (_, index) => (
					<TextField
						key={index}
						value={digits[index] ?? ''}
						disabled={disabled}
						error={hasError}
						onChange={(event) => handleDigitChange(index, event.target.value)}
						onKeyDown={(event) => handleKeyDown(index, event)}
						onPaste={(event) => handlePaste(index, event)}
						onFocus={(event) => {
							setFocusedIndex(index)
							event.target.select()
						}}
						onBlur={() => setFocusedIndex(-1)}
						inputRef={(element) => {
							inputRefs.current[index] = element
						}}
						inputProps={{
							inputMode: 'numeric',
							pattern: '[0-9]*',
							maxLength: 1,
							autoComplete: index === 0 ? 'one-time-code' : 'off',
							'aria-label': label ? `${label} ${index + 1}` : `OTP digit ${index + 1}`,
						}}
						sx={{
							width: { xs: 44, sm: 54 },
							'& .MuiOutlinedInput-root': {
								borderRadius: 2.5,
								bgcolor: 'background.paper',
								boxShadow:
									focusedIndex === index
										? '0 0 0 4px rgba(25, 118, 210, 0.12)'
										: '0 10px 24px rgba(15, 23, 42, 0.04)',
							},
							'& .MuiOutlinedInput-input': {
								p: { xs: '12px 0', sm: '14px 0' },
								textAlign: 'center',
								fontSize: { xs: '1.1rem', sm: '1.4rem' },
								fontWeight: 700,
							},
						}}
					/>
				))}
			</Stack>

			<Typography
				variant='caption'
				color={hasError ? 'error.main' : 'text.secondary'}
				sx={{
					display: 'block',
					mt: 1,
					textAlign: 'center',
					minHeight: 20,
				}}
			>
				{resolvedErrorText || helperText}
			</Typography>
		</Box>
	)
}

export default forwardRef(OtpCodeInput)

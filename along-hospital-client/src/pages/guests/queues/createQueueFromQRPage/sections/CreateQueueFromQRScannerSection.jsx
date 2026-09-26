import useTranslation from '@/hooks/useTranslation'
import { QrCode2Rounded, UploadFileRounded } from '@mui/icons-material'
import { Box, Button, Paper, Stack, Typography, useTheme } from '@mui/material'
import { alpha, keyframes } from '@mui/material/styles'
import { Scanner } from '@yudiel/react-qr-scanner'
import { useCallback, useRef, useState } from 'react'
import { toast } from 'react-toastify'

const CreateQueueFromQRScannerSection = ({ onScanCode = () => {} }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const [hasScannerError, setHasScannerError] = useState(false)
	const [isDecodingImage, setIsDecodingImage] = useState(false)
	const fileInputRef = useRef(null)

	const handleScan = useCallback(
		(detectedCodes = []) => {
			const scannedValue = detectedCodes[0]?.rawValue
			if (!scannedValue) return

			onScanCode(scannedValue)
			setHasScannerError(false)
		},
		[onScanCode]
	)

	const handleError = useCallback(() => {
		setHasScannerError(true)
	}, [])

	const handleOpenImagePicker = () => {
		fileInputRef.current?.click()
	}

	const handleSelectImage = useCallback(
		async (event) => {
			const selectedFile = event.target.files?.[0]
			event.target.value = ''
			if (!selectedFile) return

			if (typeof window === 'undefined' || typeof window.BarcodeDetector !== 'function') {
				toast.error(t('queue.guest.error.image_scan_not_supported'), {
					toastId: 'queue-image-scan-not-supported',
				})
				return
			}

			setIsDecodingImage(true)
			try {
				const detector = new window.BarcodeDetector({ formats: ['qr_code'] })
				const imageBitmap = await window.createImageBitmap(selectedFile)
				const detectedCodes = await detector.detect(imageBitmap)
				imageBitmap.close()

				const scannedValue = detectedCodes?.[0]?.rawValue
				if (!scannedValue) {
					toast.error(t('queue.guest.error.invalid_qr_format'), {
						toastId: 'queue-invalid-qr-format-image',
					})
					return
				}

				onScanCode(scannedValue)
				setHasScannerError(false)
			} catch {
				toast.error(t('queue.guest.error.invalid_qr_format'), {
					toastId: 'queue-invalid-qr-format-image',
				})
			} finally {
				setIsDecodingImage(false)
			}
		},
		[onScanCode, t]
	)

	return (
		<Stack alignItems='center' spacing={1.5} sx={{ width: '100%' }}>
			<Paper
				elevation={0}
				sx={{
					width: 'min(100%, 440px)',
					p: { xs: 1, md: 1.1 },
					borderRadius: { xs: 3, md: 4 },
					border: `2px solid ${alpha(theme.palette.primary.main, 0.2)}`,
					bgcolor: alpha(theme.palette.background.paper, 0.9),
				}}
			>
				<Box
					sx={{
						position: 'relative',
						width: '100%',
						aspectRatio: '1 / 1',
						borderRadius: { xs: 2.8, md: 3 },
						overflow: 'hidden',
						bgcolor: alpha(theme.palette.background.default, 0.75),
					}}
				>
					<Scanner
						onScan={handleScan}
						onError={handleError}
						formats={['qr_code']}
						scanDelay={900}
						allowMultiple
						constraints={{
							facingMode: 'environment',
							aspectRatio: 1,
						}}
						styles={{
							container: { width: '100%', height: '100%' },
							video: { width: '100%', height: '100%', objectFit: 'cover' },
						}}
					/>

					<Box
						sx={{
							position: 'absolute',
							left: 0,
							right: 0,
							top: '16%',
							height: 4,
							bgcolor: theme.palette.primary.main,
							boxShadow: `0 0 16px ${alpha(theme.palette.primary.main, 0.5)}`,
							animation: `${keyframes` 0% { top: 0%; opacity: 0.35; }
								50% { top: 100%; opacity: 1;} 
								100% {top: 0%;opacity: 0.35;}`} 2.6s ease-in-out infinite`,
							pointerEvents: 'none',
						}}
					/>

					<Box
						sx={{
							position: 'absolute',
							inset: 0,
							border: `2px solid ${alpha(theme.palette.primary.main, 0.92)}`,
							borderRadius: { xs: 2.8, md: 3 },
							pointerEvents: 'none',
						}}
					/>
				</Box>
			</Paper>

			{hasScannerError && (
				<Stack direction={'row'} alignItems='center' spacing={0.4}>
					<QrCode2Rounded sx={{ color: alpha(theme.palette.text.secondary, 0.6) }} />
					<Typography variant='body2' color='text.secondary' align='center'>
						{t('queue.guest.text.scanner_error')}
					</Typography>
				</Stack>
			)}

			<Button
				variant='outlined'
				startIcon={<UploadFileRounded />}
				onClick={handleOpenImagePicker}
				disabled={isDecodingImage}
				sx={{ borderRadius: 99, textTransform: 'none' }}
			>
				{isDecodingImage ? t('commons.text.loading') : t('queue.guest.button.select_qr_image')}
			</Button>

			<Box
				component='input'
				type='file'
				accept='image/*'
				ref={fileInputRef}
				onChange={handleSelectImage}
				sx={{ display: 'none' }}
			/>
		</Stack>
	)
}

export default CreateQueueFromQRScannerSection

import useTranslation from '@/hooks/useTranslation'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import PhotoCameraIcon from '@mui/icons-material/PhotoCamera'
import RestartAltIcon from '@mui/icons-material/RestartAlt'
import {
	Box,
	Button,
	CardMedia,
	CircularProgress,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Stack,
	Typography,
} from '@mui/material'
import { useEffect, useRef, useState } from 'react'

const FaceCaptureDialog = ({
	open,
	onClose,
	mode = 'single',
	durationSeconds = 5,
	onCaptureComplete,
}) => {
	const { t } = useTranslation()

	const videoRef = useRef(null)
	const canvasRef = useRef(null)
	const streamRef = useRef(null)
	const captureIntervalRef = useRef(null)
	const countdownIntervalRef = useRef(null)

	const [isCameraOn, setIsCameraOn] = useState(false)
	const [isCapturing, setIsCapturing] = useState(false)
	const [countdown, setCountdown] = useState(0)
	const [capturedImages, setCapturedImages] = useState([])
	const [previewUrl, setPreviewUrl] = useState('')
	const [error, setError] = useState('')
	const [isConfirming, setIsConfirming] = useState(false)

	const isBurstMode = mode === 'burst'
	const showPreviewPhase = !!previewUrl && !isCapturing

	const openCamera = async () => {
		setError('')
		try {
			const mediaStream = await navigator.mediaDevices.getUserMedia({ video: true })
			streamRef.current = mediaStream
			if (videoRef.current) videoRef.current.srcObject = mediaStream
			setIsCameraOn(true)
		} catch {
			setError(t('attendance.face_enrollment.error.no_camera_access'))
		}
	}

	const stopCamera = () => {
		if (streamRef.current) {
			streamRef.current.getTracks().forEach((t) => t.stop())
			streamRef.current = null
		}
		if (videoRef.current) videoRef.current.srcObject = null
		setIsCameraOn(false)
	}

	const clearIntervals = () => {
		if (captureIntervalRef.current) clearInterval(captureIntervalRef.current)
		if (countdownIntervalRef.current) clearInterval(countdownIntervalRef.current)
		captureIntervalRef.current = null
		countdownIntervalRef.current = null
	}

	const resetState = () => {
		clearIntervals()
		stopCamera()
		setIsCapturing(false)
		setCountdown(0)
		setError('')
		capturedImages.forEach((x) => x.url && URL.revokeObjectURL(x.url))
		setCapturedImages([])
		setPreviewUrl('')
		setIsConfirming(false)
	}

	const internalClose = () => {
		resetState()
		onClose && onClose()
	}

	const captureFrame = () => {
		if (!videoRef.current || !canvasRef.current) return
		const video = videoRef.current
		const canvas = canvasRef.current
		const ctx = canvas.getContext('2d')
		if (!video.videoWidth || !video.videoHeight) return
		canvas.width = video.videoWidth
		canvas.height = video.videoHeight
		ctx.drawImage(video, 0, 0, canvas.width, canvas.height)
		canvas.toBlob(
			(blob) => {
				if (!blob) return
				const url = URL.createObjectURL(blob)
				setCapturedImages((p) => [...p, { blob, url }])
			},
			'image/jpeg',
			0.9
		)
	}

	const finishCaptureBurst = () => {
		setIsCapturing(false)
		clearIntervals()
		stopCamera()
		setCapturedImages((p) => {
			if (p.length > 0) setPreviewUrl(p[p.length - 1].url)
			return p
		})
	}

	const startBurstCapture = () => {
		if (!isCameraOn) {
			setError(t('attendance.face_enrollment.error.enable_camera_first'))
			return
		}
		setError('')
		capturedImages.forEach((x) => x.url && URL.revokeObjectURL(x.url))
		setCapturedImages([])
		setPreviewUrl('')
		setIsCapturing(true)
		setCountdown(durationSeconds)
		captureIntervalRef.current = setInterval(captureFrame, 200)
		countdownIntervalRef.current = setInterval(() => {
			setCountdown((prev) => {
				const next = prev - 1
				if (next <= 0) {
					finishCaptureBurst()
					return 0
				}
				return next
			})
		}, 1000)
	}

	const startSingleCapture = () => {
		if (!isCameraOn) {
			setError(t('attendance.face_enrollment.error.enable_camera_first'))
			return
		}
		setError('')
		capturedImages.forEach((x) => x.url && URL.revokeObjectURL(x.url))
		setCapturedImages([])
		setPreviewUrl('')
		setIsCapturing(true)
		captureFrame()
		setTimeout(() => {
			setIsCapturing(false)
			stopCamera()
			setCapturedImages((p) => {
				if (p.length > 0) setPreviewUrl(p[p.length - 1].url)
				return p
			})
		}, 200)
	}

	const handlePrimaryAction = () => {
		if (isBurstMode) {
			if (!isCapturing) startBurstCapture()
		} else {
			if (!isCapturing) startSingleCapture()
		}
	}

	const handleConfirm = async () => {
		if (capturedImages.length === 0) {
			setError('No captured images to process.')
			return
		}
		setError('')
		setIsConfirming(true)
		try {
			const blobs = capturedImages.map((x) => x.blob)
			onCaptureComplete && (await onCaptureComplete(blobs))
			internalClose()
		} catch {
			setError(t('attendance.face_enrollment.error.enrollment_failed'))
		} finally {
			setIsConfirming(false)
		}
	}

	const handleRetake = () => {
		resetState()
		openCamera()
	}

	useEffect(() => {
		if (open) openCamera()
		else resetState()
	}, [open])

	return (
		<Dialog open={open} onClose={internalClose} maxWidth='sm' fullWidth>
			<DialogTitle>{t('attendance.face_enrollment.title.face_capture')}</DialogTitle>
			<DialogContent>
				<Box
					sx={{
						position: 'relative',
						bgcolor: 'black',
						borderRadius: 2,
						overflow: 'hidden',
						height: 320,
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						mb: 2,
					}}
				>
					{!showPreviewPhase ? (
						<video
							ref={videoRef}
							autoPlay
							playsInline
							style={{ width: '100%', height: '100%', objectFit: 'cover' }}
						/>
					) : (
						<CardMedia
							component='img'
							image={previewUrl}
							alt='face-preview'
							sx={{ width: '100%', height: '100%', objectFit: 'cover' }}
						/>
					)}
					<canvas ref={canvasRef} style={{ display: 'none' }} />
					{isBurstMode && isCapturing && (
						<Box
							sx={{
								position: 'absolute',
								top: 16,
								right: 16,
								bgcolor: 'rgba(0,0,0,0.6)',
								color: 'white',
								borderRadius: 999,
								px: 2,
								py: 0.5,
							}}
						>
							<Typography variant='body2'>Capturing... {countdown}s</Typography>
						</Box>
					)}
					{!isCameraOn && !showPreviewPhase && (
						<Box
							sx={{
								position: 'absolute',
								inset: 0,
								display: 'flex',
								alignItems: 'center',
								justifyContent: 'center',
								flexDirection: 'column',
								color: 'white',
								bgcolor: 'rgba(0,0,0,0.6)',
							}}
						>
							<PhotoCameraIcon sx={{ fontSize: 40, mb: 1 }} />
							<Typography variant='body2' textAlign='center'>
								{t('attendance.face_enrollment.description.prompt.opening_camera')}
							</Typography>
						</Box>
					)}
				</Box>
				<Stack spacing={1}>
					{!showPreviewPhase && (
						<Typography variant='body2' color='text.secondary'>
							{isBurstMode
								? t('attendance.face_enrollment.description.prompt.start_burst_mode', {
										durationSeconds: durationSeconds,
								  })
								: t('attendance.face_enrollment.description.prompt.start_single_mode')}
						</Typography>
					)}
					{showPreviewPhase && (
						<Typography variant='body2' color='text.secondary'>
							{t('attendance.face_enrollment.description.prompt.preview')}
						</Typography>
					)}
					{error && (
						<Typography variant='body2' color='error'>
							{error}
						</Typography>
					)}
				</Stack>
			</DialogContent>
			<DialogActions>
				{showPreviewPhase ? (
					<>
						<Button
							variant='outlined'
							color='secondary'
							startIcon={<RestartAltIcon />}
							onClick={handleRetake}
							disabled={isConfirming}
						>
							{t('attendance.button.retake')}
						</Button>
						<Button
							variant='contained'
							color='success'
							startIcon={<CheckCircleIcon />}
							onClick={handleConfirm}
							disabled={isConfirming}
						>
							{isConfirming ? <CircularProgress size={20} /> : t('button.confirm')}
						</Button>
					</>
				) : (
					<>
						<Button onClick={internalClose}>{t('button.cancel')}</Button>
						<Button
							variant='contained'
							startIcon={<PhotoCameraIcon />}
							onClick={handlePrimaryAction}
							disabled={!isCameraOn || isCapturing}
						>
							{isBurstMode
								? isCapturing
									? t('attendance.button.capturing')
									: t('attendance.button.start_enrollment')
								: t('attendance.button.capture')}
						</Button>
					</>
				)}
			</DialogActions>
		</Dialog>
	)
}

export default FaceCaptureDialog

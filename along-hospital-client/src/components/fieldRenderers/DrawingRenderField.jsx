import useTranslation from '@/hooks/useTranslation'
import { Delete } from '@mui/icons-material'
import { Box, Button, Stack, Typography } from '@mui/material'
import Signature from '@uiw/react-signature'
import { useRef, useState } from 'react'

const MIN_CANVAS_WIDTH = 400
const MIN_CANVAS_HEIGHT = 200
const EXPORT_PADDING = 20

const DrawingRenderField = ({ field, showError, onDrawingChange }) => {
	const { t } = useTranslation()
	const containerRef = useRef(null)
	const drawingRef = useRef(null)
	const [hasDrawing, setHasDrawing] = useState(false)

	const drawingOptions = {
		size: 6,
		smoothing: 0.46,
		thinning: 0.73,
		streamline: 0.5,
		easing: (t) => t,
		simulatePressure: true,
		last: true,
		start: { cap: true, taper: 0, easing: (t) => t },
		end: { cap: true, taper: 0, easing: (t) => t },
	}

	const handleClear = () => {
		drawingRef.current?.clear?.()
		setHasDrawing(false)
		onDrawingChange?.(null)
	}

	const exportToFile = async () => {
		const svgElement = containerRef.current?.querySelector('svg')
		if (!svgElement) {
			onDrawingChange?.(null)
			return null
		}

		const bbox = svgElement.getBBox()

		const contentWidth = bbox.width + EXPORT_PADDING * 2
		const contentHeight = bbox.height + EXPORT_PADDING * 2
		const width = Math.max(contentWidth, MIN_CANVAS_WIDTH)
		const height = Math.max(contentHeight, MIN_CANVAS_HEIGHT)

		const canvas = document.createElement('canvas')
		canvas.width = width
		canvas.height = height
		const ctx = canvas.getContext('2d')

		ctx.fillStyle = 'white'
		ctx.fillRect(0, 0, width, height)

		const svgClone = svgElement.cloneNode(true)
		svgClone.setAttribute('width', bbox.width)
		svgClone.setAttribute('height', bbox.height)
		svgClone.setAttribute('viewBox', `${bbox.x} ${bbox.y} ${bbox.width} ${bbox.height}`)

		const svgData = new XMLSerializer().serializeToString(svgClone)
		const svgBlob = new Blob([svgData], { type: 'image/svg+xml;charset=utf-8' })
		const svgUrl = URL.createObjectURL(svgBlob)

		return new Promise((resolve) => {
			const img = new Image()
			img.onload = () => {
				const drawX = (width - bbox.width) / 2
				const drawY = (height - bbox.height) / 2
				ctx.drawImage(img, drawX, drawY, bbox.width, bbox.height)
				URL.revokeObjectURL(svgUrl)
				canvas.toBlob((blob) => {
					if (blob) {
						const file = new File([blob], 'drawing.png', { type: 'image/png' })
						resolve(file)
					} else {
						resolve(null)
					}
				}, 'image/png')
			}
			img.onerror = () => {
				URL.revokeObjectURL(svgUrl)
				resolve(null)
			}
			img.src = svgUrl
		})
	}

	const handlePointerUp = async () => {
		setHasDrawing(true)
		await new Promise((resolve) => requestAnimationFrame(resolve))
		const file = await exportToFile()
		if (file) {
			onDrawingChange?.(file)
		}
	}

	return (
		<Stack spacing={1.25}>
			<Typography variant='subtitle2'>
				{field.title}
				{(field.required ?? true) && ' *'}
			</Typography>

			<Box
				ref={containerRef}
				sx={{
					border: (theme) => `1px solid ${showError ? theme.palette.error.main : theme.palette.divider}`,
					borderRadius: 1,
					overflow: 'hidden',
					backgroundColor: 'white',
				}}
			>
				<Signature
					ref={drawingRef}
					options={drawingOptions}
					onPointerUp={handlePointerUp}
					style={{ width: '100%', height: field.height || 200, touchAction: 'none' }}
				/>
			</Box>

			<Stack direction='row' spacing={1} alignItems='center'>
				<Button
					variant='outlined'
					color='error'
					size='small'
					startIcon={<Delete />}
					onClick={handleClear}
					disabled={!hasDrawing}
				>
					{t('button.clear')}
				</Button>
			</Stack>

			{showError && (
				<Typography variant='caption' color='error'>
					{t('error.required')}
				</Typography>
			)}
		</Stack>
	)
}

export default DrawingRenderField

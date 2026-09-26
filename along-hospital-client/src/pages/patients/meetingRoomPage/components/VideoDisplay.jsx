import { MicOff, VideocamOff } from '@mui/icons-material'
import { Box, Paper } from '@mui/material'

const VideoDisplay = ({ videoRef, isMicOn, isCamOn, isLocal = false, sx = {} }) => {
	const defaultSx = isLocal
		? {
				p: 0.5,
				position: 'absolute',
				bottom: 16,
				right: 16,
				width: { xs: '120px', md: '180px' },
				height: { xs: '80px', md: '120px' },
				borderRadius: 2,
				backgroundColor: 'black',
				zIndex: 1,
		  }
		: {
				position: 'relative',
				p: 1,
				borderRadius: 2,
				height: { xs: '400px', md: '600px' },
				backgroundColor: 'black',
		  }

	return (
		<Paper variant='outlined' sx={{ ...defaultSx, ...sx }}>
			{!isMicOn && (
				<Box
					sx={{
						position: 'absolute',
						top: isLocal ? 6 : 12,
						right: isLocal ? 6 : 12,
						zIndex: isLocal ? 3 : 2,
						bgcolor: 'error.main',
						color: 'common.white',
						width: isLocal ? 24 : 32,
						height: isLocal ? 24 : 32,
						borderRadius: '50%',
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						boxShadow: 2,
					}}
				>
					<MicOff sx={{ fontSize: isLocal ? 16 : 'small' }} />
				</Box>
			)}

			{!isCamOn && (
				<Box
					sx={{
						position: 'absolute',
						inset: isLocal ? 4 : 8,
						borderRadius: isLocal ? 1 : 1.5,
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						backgroundColor: 'action.hover',
						zIndex: isLocal ? 2 : 1,
					}}
				>
					<VideocamOff sx={{ fontSize: isLocal ? 32 : 64, color: 'text.disabled' }} />
				</Box>
			)}

			<video
				ref={videoRef}
				autoPlay
				playsInline
				muted={isLocal}
				style={{
					display: isCamOn ? 'block' : 'none',
					width: '100%',
					height: '100%',
					borderRadius: isLocal ? 6 : 8,
					objectFit: isLocal ? 'cover' : 'contain',
				}}
			/>
		</Paper>
	)
}

export default VideoDisplay

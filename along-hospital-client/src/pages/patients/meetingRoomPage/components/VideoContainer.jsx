import { Box } from '@mui/material'
import VideoDisplay from './VideoDisplay'

const VideoContainer = ({
	remoteVideoRef,
	localVideoRef,
	remoteMicOn,
	remoteCamOn,
	isMicOn,
	isCamOn,
}) => {
	return (
		<Box sx={{ position: 'relative', mb: 2 }}>
			<VideoDisplay
				videoRef={remoteVideoRef}
				isMicOn={remoteMicOn}
				isCamOn={remoteCamOn}
				isLocal={false}
			/>

			<VideoDisplay videoRef={localVideoRef} isMicOn={isMicOn} isCamOn={isCamOn} isLocal={true} />
		</Box>
	)
}

export default VideoContainer

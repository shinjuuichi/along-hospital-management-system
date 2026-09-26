import SearchBar from '@/components/generals/SearchBar'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { Search } from '@mui/icons-material'
import { Box, Button, Container, Grid, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'

const JoinMeetingRoomPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const [roomCode, setRoomCode] = useState('')

	useEffect(() => {
		const code = searchParams.get('code') || ''
		if (code) setRoomCode(code)
	}, [searchParams])

	const handleJoin = () => {
		const code = roomCode.trim()
		if (!code) return
		// Điều hướng tới route meeting room với transaction ID
		navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.MEETING_ROOM_TOKEN(code)))
	}

	return (
		<Container maxWidth='lg' sx={{ py: { xs: 4, md: 6 } }}>
			<Stack alignItems='center' spacing={1.5} sx={{ textAlign: 'center', mb: 3 }}>
				<Typography
					variant='h3'
					sx={{
						fontWeight: 700,
						letterSpacing: 0.2,
					}}
				>
					{t('meeting_room.title.join_meeting')}
				</Typography>
				<Typography color='text.secondary'>{t('meeting_room.subtitle.enter_meeting')}</Typography>
			</Stack>

			<Box px={{ xs: 2, md: 10 }}>
				<Paper
					elevation={5}
					sx={{
						p: { xs: 2.5, md: 3 },
						borderRadius: 3,
						border: '1px solid',
						borderColor: 'divider',
						bgcolor: (theme) => theme.palette.background.paper,
						mx: 'auto',
						width: '100%',
					}}
				>
					<Grid container spacing={2} alignItems='center'>
						<Grid size={{ xs: 12, md: 9 }}>
							<SearchBar
								value={roomCode}
								setValue={setRoomCode}
								onEnterDown={handleJoin}
								placeholder={t('meeting_room.title.placeholder_meeting_id')}
							/>
						</Grid>
						<Grid size={{ xs: 12, md: 3 }}>
							<Button
								variant='contained'
								size='large'
								onClick={handleJoin}
								startIcon={<Search />}
								fullWidth
								sx={{ px: 3.5, py: 1.5, fontSize: '10px' }}
							>
								{t('meeting_room.title.join_meeting')}
							</Button>
						</Grid>
					</Grid>
				</Paper>
			</Box>
		</Container>
	)
}

export default JoinMeetingRoomPage

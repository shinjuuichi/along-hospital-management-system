import { ApiUrls } from '@/configs/apiUrls'
import useTranslation from '@/hooks/useTranslation'
import { getEnv } from '@/utils/commons'
import * as signalR from '@microsoft/signalr'
import { Chat, Send } from '@mui/icons-material'
import {
	Box,
	ClickAwayListener,
	Divider,
	Fab,
	IconButton,
	Paper,
	Stack,
	TextField,
	Typography,
} from '@mui/material'
import { useTheme } from '@mui/material/styles'
import { useEffect, useRef, useState } from 'react'
import ChatBotMessageBuble from './ChatBotMessageBubble'

const ChatBotFloatingButton = () => {
	const theme = useTheme()
	const { t } = useTranslation()
	const [isOpen, setIsOpen] = useState(false)

	const [messages, setMessages] = useState([])
	const [inputValue, setInputValue] = useState('')

	const [isConnected, setIsConnected] = useState(false)
	const [isSending, setIsSending] = useState(false)

	const connectionRef = useRef(null)
	const messagesEndRef = useRef(null)

	const createMessage = (role, content) => {
		return {
			id: `${Date.now()}_${Math.random().toString(16).slice(2)}`,
			role,
			content,
			createdAt: new Date().toISOString(),
		}
	}

	useEffect(() => {
		const createConnection = async () => {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(getEnv('VITE_BASE_API_URL') + ApiUrls.HUB.CHATBOT)
				.withAutomaticReconnect()
				.build()

			await connection.start()

			connectionRef.current = connection
			setIsConnected(true)
			setMessages((prev) => {
				if (prev.length > 0) return prev
				return [createMessage('assistant', t('chatbot.placeholder.greeting'))]
			})

			connection.on('ReceiveBotMessage', (role, message) => {
				const botMessage = createMessage(role, message)
				setMessages((prev) => [...prev, botMessage])
				setIsSending(false)
			})
		}

		createConnection()

		return () => {
			if (connectionRef.current) {
				connectionRef.current.stop()
			}
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [])

	useEffect(() => {
		if (messagesEndRef.current) {
			messagesEndRef.current.scrollIntoView({ behavior: 'smooth' })
		}
	}, [messages])

	const canSend =
		inputValue.trim().length > 0 &&
		isConnected &&
		connectionRef.current &&
		connectionRef.current.state === signalR.HubConnectionState.Connected &&
		!isSending

	const handleSend = () => {
		if (!canSend) return

		const text = inputValue.trim()
		if (!text) return

		const connection = connectionRef.current
		if (!connection) return

		const userMessage = createMessage('user', text)
		setMessages((prev) => [...prev, userMessage])
		setInputValue('')
		setIsSending(true)
		connection.invoke('SendUserMessage', text).catch(() => setIsSending(false))
	}

	const handleKeyDown = (event) => {
		if (event.key === 'Enter' && !event.shiftKey) {
			event.preventDefault()
			handleSend()
		}
	}

	return (
		<ClickAwayListener onClickAway={() => setIsOpen(false)}>
			<Box
				sx={{
					position: 'fixed',
					bottom: 24,
					right: 24,
					zIndex: (themeArg) => themeArg.zIndex.modal + 1,
				}}
			>
				{isOpen && (
					<Paper
						elevation={6}
						sx={{
							mb: 2,
							width: { xs: 320, sm: 380, md: 400 },
							height: { xs: 420, sm: 480, md: 520 },
							borderRadius: 1,
							overflow: 'hidden',
							display: 'flex',
							flexDirection: 'column',
							position: 'absolute',
							bottom: '100%',
							right: 0,
							backgroundColor: theme.palette.background.paper,
						}}
					>
						<Box
							sx={{
								px: 2,
								py: 1.5,
								backgroundImage: theme.palette.gradients.brand_135deg,
								color: theme.palette.primary.contrastText,
							}}
						>
							<Typography variant='subtitle1' sx={{ fontWeight: 600 }}>
								{t('chatbot.title.title')}
							</Typography>
							<Typography variant='caption'>{t('chatbot.title.caption')}</Typography>
						</Box>
						<Stack px={2} py={1} direction='row' spacing={1} alignItems='center'>
							<Box
								sx={{
									width: 10,
									height: 10,
									borderRadius: '50%',
									backgroundColor: isConnected ? theme.palette.success.main : theme.palette.error.main,
								}}
							/>
							<Typography variant='caption' sx={{ color: theme.palette.text.secondary }}>
								{isConnected ? t('chatbot.status.connected') : t('chatbot.status.disconnected')}
							</Typography>
						</Stack>
						<Divider />
						<Box
							sx={{
								flex: 1,
								px: 2,
								py: 1,
								overflowY: 'auto',
								backgroundColor: theme.palette.background.lightGray || theme.palette.background.default,
							}}
						>
							{messages.map((message) => (
								<ChatBotMessageBuble key={message.id} message={message} />
							))}
							{isSending && (
								<ChatBotMessageBuble
									message={createMessage('assistant', t('chatbot.placeholder.typing'))}
								/>
							)}
							<div ref={messagesEndRef} />
						</Box>
						<Divider />
						<Box
							sx={{
								px: 1.5,
								py: 1,
								display: 'flex',
								alignItems: 'center',
								gap: 1,
								backgroundColor: theme.palette.background.paper,
							}}
						>
							<TextField
								variant='outlined'
								size='small'
								fullWidth
								multiline
								maxRows={4}
								placeholder={t('chatbot.placeholder.input')}
								value={inputValue}
								onChange={(e) => setInputValue(e.target.value)}
								onKeyDown={handleKeyDown}
								disabled={!isConnected || isSending}
								sx={{
									'& .MuiOutlinedInput-root': {
										backgroundColor: theme.palette.background.default,
									},
								}}
							/>
							<IconButton onClick={handleSend} disabled={!canSend} loading={isSending}>
								<Send color={canSend ? 'primary' : 'disabled'} fontSize='small' />
							</IconButton>
						</Box>
					</Paper>
				)}
				<Fab color='info' hidden={false} onClick={() => setIsOpen((prev) => !prev)}>
					<Chat />
				</Fab>
			</Box>
		</ClickAwayListener>
	)
}

export default ChatBotFloatingButton

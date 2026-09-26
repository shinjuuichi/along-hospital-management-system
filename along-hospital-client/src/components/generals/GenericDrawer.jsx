import { Close } from '@mui/icons-material'
import { Box, Divider, Drawer, IconButton, Stack, Typography } from '@mui/material'
import React from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import DrawerInfoRow from '../infoRows/DrawerInfoRow'
import EmptyBox from '../placeholders/EmptyBox'

/**
 * @typedef {Object} GenericDrawerProps
 * @property {boolean} open
 * @property {() => void} onClose
 * @property {string|React.ReactNode} title
 * @property {Array<{title: string|React.ReactNode, of: Array<{label: string, value: string|React.ReactNode|function}>}>} fields
 * @property {React.ReactNode} [buttons]
 */
const GenericDrawer = ({ open, onClose, title, fields, buttons = <React.Fragment /> }) => {
	return (
		<Drawer
			anchor='right'
			open={open}
			onClose={onClose}
			slotProps={{ paper: { sx: { width: { xs: '100%', sm: 420, md: 480, lg: 520 } } } }}
		>
			<Box sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
				{/* Drawer Header */}
				<Box
					sx={{
						p: 2,
						borderBottom: (theme) => `1px solid ${theme.palette.divider}`,
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'space-between',
					}}
				>
					<Typography variant='h6'>{title}</Typography>
					<IconButton onClick={onClose} size='small'>
						<Close />
					</IconButton>
				</Box>

				{/* Drawer Content */}
				<Box sx={{ flex: 1, overflowY: 'auto', p: 2 }}>
					{!fields || fields.length === 0 ? (
						<EmptyBox />
					) : (
						<Stack spacing={2}>
							{fields.map((field, index) => {
								const rows = field?.of ?? []

								return (
									<Box
										key={index}
										sx={{
											p: 2,
											border: (theme) => `1px solid ${theme.palette.divider}`,
											borderRadius: 1,
											bgcolor: 'background.paper',
										}}
									>
										{React.isValidElement(field.title) ? (
											field.title
										) : (
											<Typography variant='subtitle1'>{field.title}</Typography>
										)}

										<Divider sx={{ my: 1.5 }} />

										{rows.length === 0 ? (
											<Typography variant='body2' sx={{ color: 'text.secondary' }}>
												{renderEmptyFallback(null)}
											</Typography>
										) : (
											<Stack spacing={1}>
												{rows.map((row, rIdx) => {
													if (row?.fullWidth) {
														const renderValue =
															typeof row.value === 'function'
																? row.value()
																: React.isValidElement(row.value)
																	? row.value
																	: row.value ?? row.value === 0
																		? String(row.value)
																		: renderEmptyFallback(null)
														return (
															<Box key={rIdx} sx={{ width: '100%' }}>
																{renderValue}
															</Box>
														)
													}

													return <DrawerInfoRow key={rIdx} label={row.label} value={row.value} />
												})}
											</Stack>
										)}
									</Box>
								)
							})}
						</Stack>
					)}
				</Box>

				{/* Drawer Buttons */}
				<Box
					sx={{
						position: 'sticky',
						bottom: 0,
						bgcolor: 'background.paper',
						borderTop: (theme) => `1px solid ${theme.palette.divider}`,
						p: 2,
					}}
				>
					{buttons}
				</Box>
			</Box>
		</Drawer>
	)
}

export default GenericDrawer

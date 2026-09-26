import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import AddIcon from '@mui/icons-material/Add'
import CloseIcon from '@mui/icons-material/Close'
import { Avatar, Box, Chip, IconButton, Paper, Stack, Typography } from '@mui/material'

const AssignmentCardSection = ({
	location,
	locationLabel,
	locationSubLabel,
	locationMetaLabel,
	locationRoles = [],
	assignments,
	canManageAssignments = true,
	getStaffRoles = (staff) => staff?.roles || [],
	onAdd,
	onRemove,
	onViewSegments,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const roleOptions = _enum.roleOptions

	return (
		<Paper
			variant='outlined'
			sx={{
				p: 2,
				maxHeight: 400,
				overflowY: 'auto',
				borderRadius: 2,
			}}
		>
			<Stack spacing={1.5}>
				<Box>
					<Typography variant='subtitle1' fontWeight={700}>
						{locationLabel}
					</Typography>
					{locationSubLabel && (
						<Typography variant='body2' color='text.secondary'>
							{locationSubLabel}
						</Typography>
					)}
					{locationMetaLabel && (
						<Typography variant='caption' color='text.secondary'>
							{locationMetaLabel}
						</Typography>
					)}
				</Box>

				{locationRoles.length > 0 && (
					<Stack direction='row' spacing={0.5} flexWrap='wrap' useFlexGap alignItems='center'>
						<Typography variant='caption' color='text.secondary' sx={{ mr: 0.5 }}>
							{t('work_schedule_template.text.allowed_roles_for_room')}:
						</Typography>
						{locationRoles.map((role) => (
							<Chip
								key={role}
								label={getEnumLabelByValue(roleOptions, role) || role}
								size='small'
								variant='outlined'
								color='info'
								sx={{ height: 22, fontSize: '0.75rem' }}
							/>
						))}
					</Stack>
				)}

				<Stack spacing={1}>
					{assignments.length > 0 ? (
						assignments.map((a) => {
							const staff = a.staff
							return (
								<Stack
									key={a.staffId || a.id}
									direction='row'
									alignItems='center'
									gap={1.5}
									sx={{
										p: 1,
										borderRadius: 1,
										bgcolor: 'action.hover',
										cursor: onViewSegments ? 'pointer' : 'default',
										'&:hover': {
											bgcolor: onViewSegments ? 'action.selected' : 'action.hover',
										},
									}}
									onClick={() => onViewSegments?.(a)}
								>
									<Avatar
										src={getImageFromCloud(staff?.image)}
										alt={staff?.fullName || staff?.name}
										sx={{ width: 36, height: 36 }}
									/>
									<Stack sx={{ flex: 1, minWidth: 0 }} gap={0.2}>
										<Stack direction='row' gap={1} alignItems='center' flexWrap='wrap'>
											<Typography variant='body2' fontWeight={600} noWrap>
												{staff?.fullName || staff?.name || a.staffId}
											</Typography>
											{staff?.phone && (
												<Typography variant='caption' color='text.secondary' noWrap>
													{staff.phone}
												</Typography>
											)}
											{staff?.email && (
												<Typography variant='caption' color='text.secondary' noWrap>
													{staff.email}
												</Typography>
											)}
										</Stack>
										<Typography variant='caption' color='text.secondary'>
											{getStaffRoles(staff)
												.filter((r) => r !== EnumConfig.Role.Patient)
												.map((r) => getEnumLabelByValue(roleOptions, r) || r)
												.join(', ')}
										</Typography>
									</Stack>
									{canManageAssignments && (
										<IconButton
											size='small'
											onClick={(e) => {
												e.stopPropagation()
												onRemove?.(a)
											}}
											sx={{ p: 0.25 }}
										>
											<CloseIcon fontSize='small' />
										</IconButton>
									)}
								</Stack>
							)
						})
					) : (
						<Typography variant='body2' color='text.disabled'>
							{t('work_schedule_template.text.no_assignments_yet')}
						</Typography>
					)}
				</Stack>

				{locationRoles.length > 0 && canManageAssignments && (
					<Box>
						<IconButton
							size='small'
							color='primary'
							onClick={() => onAdd?.(location.id)}
							sx={{ p: 0.25 }}
						>
							<AddIcon fontSize='small' />
						</IconButton>
					</Box>
				)}
			</Stack>
		</Paper>
	)
}

export default AssignmentCardSection

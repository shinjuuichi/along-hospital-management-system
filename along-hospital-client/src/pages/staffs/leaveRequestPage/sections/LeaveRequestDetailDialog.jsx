import DrawerInfoRow from '@/components/infoRows/DrawerInfoRow'
import { defaultLeaveRequestStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatLeaveShiftValue } from '@/pages/staffs/leaveRequestPage/helpers/leaveRequestHelper'
import { getImageFromCloud } from '@/utils/commons'
import {
	formatDateBasedOnCurrentLanguage,
	formatDatetimeStringBasedOnCurrentLanguage,
} from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Close } from '@mui/icons-material'
import {
	Avatar,
	Box,
	Chip,
	Dialog,
	DialogContent,
	DialogTitle,
	Divider,
	IconButton,
	Stack,
	Typography,
} from '@mui/material'

const UserInfoSection = ({ title, user, t }) => (
	<Stack spacing={1.25}>
		<Typography variant='subtitle2' sx={{ fontWeight: 700 }}>
			{title}
		</Typography>
		<Stack direction='row' spacing={1.5} alignItems='center'>
			<Avatar src={getImageFromCloud(user?.image)} sx={{ width: 40, height: 40 }} />
			<Box sx={{ minWidth: 0 }}>
				<Typography variant='body2' sx={{ fontWeight: 600 }} noWrap>
					{renderEmptyFallback(user?.name)}
				</Typography>
				<Typography variant='caption' sx={{ color: 'text.secondary' }} noWrap>
					{renderEmptyFallback(user?.email)}
				</Typography>
			</Box>
		</Stack>
		<DrawerInfoRow label='ID' value={renderEmptyFallback(user?.id)} />
		<DrawerInfoRow label={t('profile.field.name')} value={renderEmptyFallback(user?.name)} />
		<DrawerInfoRow label={t('profile.field.email')} value={renderEmptyFallback(user?.email)} />
		<DrawerInfoRow label={t('profile.field.phone')} value={renderEmptyFallback(user?.phone)} />
		<DrawerInfoRow label={t('staff.field.role')} value={renderEmptyFallback(user?.role)} />
	</Stack>
)
const LeaveRequestDetailDialog = ({
	open = false,
	onClose = () => {},
	data = null,
	loading = false,
}) => {
	const _enum = useEnum()
	const { t } = useTranslation()

	if (!open) return null

	if (loading && !data) {
		return (
			<Dialog open={open} onClose={onClose} maxWidth='sm' fullWidth>
				<DialogTitle>
					<Stack direction='row' alignItems='center' justifyContent='space-between'>
						<Typography variant='h6'>{t('leave_request.title.leave_request')}</Typography>
						<IconButton onClick={onClose} size='small'>
							<Close />
						</IconButton>
					</Stack>
				</DialogTitle>
				<Divider />
				<DialogContent>
					<Typography variant='body2' sx={{ color: 'text.secondary', pt: 1 }}>
						{t('text.loading')}
					</Typography>
				</DialogContent>
			</Dialog>
		)
	}

	if (!data) return null

	const leaveTypeLabel =
		getEnumLabelByValue(_enum.leaveTypeOptions, data.leaveType) || data.leaveType
	const leaveUnitLabel =
		getEnumLabelByValue(_enum.leaveUnitOptions, data.leaveUnit) || data.leaveUnit
	const statusLabel =
		getEnumLabelByValue(_enum.leaveRequestStatusOptions, data.status) || data.status

	const statusChip = (
		<Chip
			label={renderEmptyFallback(statusLabel)}
			color={defaultLeaveRequestStatusStyle(data.status)}
			size='small'
		/>
	)

	return (
		<Dialog open={open} onClose={onClose} maxWidth='sm' fullWidth>
			<DialogTitle>
				<Stack direction='row' alignItems='center' justifyContent='space-between'>
					<Typography variant='h6'>
						{t('leave_request.title.leave_request')} #{data.id}
					</Typography>
					<IconButton onClick={onClose} size='small'>
						<Close />
					</IconButton>
				</Stack>
			</DialogTitle>
			<Divider />
			<DialogContent>
				<Stack spacing={1.5} sx={{ pt: 1 }}>
					<UserInfoSection title={t('leave_request.field.staff_name')} user={data.staff} t={t} />

					<Divider />

					<DrawerInfoRow label={t('leave_request.field.leave_type')} value={leaveTypeLabel} />
					<DrawerInfoRow label={t('leave_request.field.leave_unit')} value={leaveUnitLabel} />
					{data.leaveUnit === EnumConfig.LeaveUnit.Shift && (
						<DrawerInfoRow label={t('leave_request.field.shift')} value={formatLeaveShiftValue(data)} />
					)}
					<DrawerInfoRow
						label={t('leave_request.field.from')}
						value={renderEmptyFallback(formatDateBasedOnCurrentLanguage(data.fromDate))}
					/>
					<DrawerInfoRow
						label={t('leave_request.field.to')}
						value={renderEmptyFallback(formatDateBasedOnCurrentLanguage(data.toDate))}
					/>

					<DrawerInfoRow label={t('leave_request.field.reason')} value={renderEmptyFallback(data.reason)} />

					<Divider />

					<DrawerInfoRow label={t('leave_request.field.status')} value={statusChip} />
					<DrawerInfoRow
						label={t('leave_request.field.decided_at')}
						value={renderEmptyFallback(
							data.decidedAt ? formatDatetimeStringBasedOnCurrentLanguage(data.decidedAt) : null
						)}
					/>

					<Divider />

					<UserInfoSection title={t('leave_request.field.decider')} user={data.decider} t={t} />
				</Stack>
			</DialogContent>
		</Dialog>
	)
}

export default LeaveRequestDetailDialog

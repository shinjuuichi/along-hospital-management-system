import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Avatar, Stack, Typography } from '@mui/material'

const StaffRenderOption = ({ staff }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	return (
		<Stack direction='row' alignItems='center' gap={2}>
			<Avatar src={getImageFromCloud(staff?.image)} alt={staff?.name} />
			<Stack gap={0.2}>
				<Stack direction='row' gap={1} alignItems='center'>
					<Typography>{staff?.name}</Typography>
					{staff?.phone && (
						<Typography lineHeight='normal' variant='caption' color='text.secondary'>
							{staff.phone}
						</Typography>
					)}
				</Stack>
				<Typography variant='caption' color='text.secondary'>
					{getEnumLabelByValue(_enum.roleOptions, staff?.role) || staff?.role}
				</Typography>
				{staff?.specialtyName && (
					<Typography variant='caption' color='text.secondary'>
						{t('staff.field.specialty')}: {staff.specialtyName}
					</Typography>
				)}
			</Stack>
		</Stack>
	)
}

export default StaffRenderOption

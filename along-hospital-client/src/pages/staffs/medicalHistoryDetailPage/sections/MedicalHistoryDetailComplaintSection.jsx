import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { defaultComplaintResolveStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'

const MedicalHistoryDetailComplaintSection = ({
	complaint,
	role,
	loading = false,
	onClickCreateComplaint,
	onClickRespondComplaint,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const hasComplaint = Boolean(complaint)

	const isPatientRole = role === EnumConfig.Role.Patient
	const isHotlineAgentOrManager =
		role === EnumConfig.Role.HotlineAgent || role === EnumConfig.Role.Manager

	const isResolvedStatus =
		complaint?.complaintResolveStatus === EnumConfig.ComplaintResolveStatus.Resolved
	const isClosedStatus =
		complaint?.complaintResolveStatus === EnumConfig.ComplaintResolveStatus.Closed

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2, height: '100%' }}>
				<Typography variant='h6' gutterBottom>
					{t('medical_history.title.complaint')}
				</Typography>
				<SkeletonBox numberOfBoxes={5} heights={[30, 30, 30, 30, 30]} />
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 3, borderRadius: 2, height: '100%' }}>
			<Typography variant='h6' gutterBottom>
				{t('medical_history.title.complaint')}
			</Typography>
			{!hasComplaint ? (
				<Stack alignItems='center' justifyContent='center' spacing={2} sx={{ py: 4 }}>
					<Typography color='text.secondary'>{t('medical_history.placeholder.no_complaint')}</Typography>
					{isPatientRole && (
						<Button variant='contained' onClick={onClickCreateComplaint}>
							{t('medical_history.button.create_complaint')}
						</Button>
					)}
				</Stack>
			) : (
				<Stack spacing={1}>
					{[
						{ label: t('complaint.field.topic'), value: complaint?.complaintTopic },
						{ label: t('complaint.field.content'), value: complaint?.content },
						{
							label: t('complaint.field.response'),
							value: complaint?.response,
						},
						{
							label: t('complaint.field.resolve_status'),
							value: (
								<Chip
									label={getEnumLabelByValue(
										_enum.complaintResolveStatusOptions,
										complaint?.complaintResolveStatus
									)}
									size='small'
									color={defaultComplaintResolveStatusStyle(complaint?.complaintResolveStatus)}
								/>
							),
						},
					].map((field) => (
						<Typography key={field.label} variant='body2'>
							{field.label}: {renderEmptyFallback(field.value)}
						</Typography>
					))}
					{isHotlineAgentOrManager && !isResolvedStatus && !isClosedStatus && (
						<Button variant='contained' sx={{ mt: 2 }} onClick={onClickRespondComplaint}>
							{t('medical_history.button.respond_complaint')}
						</Button>
					)}
				</Stack>
			)}
		</Paper>
	)
}

export default MedicalHistoryDetailComplaintSection

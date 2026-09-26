import ManagementMedicalHistoryDetailDrawerSection from '@/components/basePages/manageMedicalHistoryBasePage/sections/ManagementMedicalHistoryDetailDrawerSection'
import GenericDrawer from '@/components/generals/GenericDrawer'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import useTranslation from '@/hooks/useTranslation'

const MedicalHistoryPreviewDrawer = ({ open, onClose, loading, medicalHistory }) => {
	const { t } = useTranslation()

	if (!loading && medicalHistory) {
		return (
			<ManagementMedicalHistoryDetailDrawerSection
				open={open}
				onClose={onClose}
				item={medicalHistory}
			/>
		)
	}

	const fields = open
		? [
				{
					title: t('medical_history.title.medical_history_detail'),
					of: [
						{
							fullWidth: true,
							value: <SkeletonBox numberOfBoxes={5} heights={[28, 28, 28, 28, 120]} />,
						},
					],
				},
			]
		: []

	return (
		<GenericDrawer
			open={open}
			onClose={onClose}
			title={t('medical_history.title.medical_history_detail')}
			fields={fields}
		/>
	)
}

export default MedicalHistoryPreviewDrawer

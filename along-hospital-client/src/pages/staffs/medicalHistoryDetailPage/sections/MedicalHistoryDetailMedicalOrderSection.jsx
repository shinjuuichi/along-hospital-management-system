import GenericTabs from '@/components/generals/GenericTabs'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { defaultMedicalOrderTypeStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import AddCircleOutlineOutlinedIcon from '@mui/icons-material/AddCircleOutlineOutlined'
import {
	Timeline,
	TimelineConnector,
	TimelineContent,
	TimelineDot,
	TimelineItem,
	TimelineSeparator,
} from '@mui/lab'
import { Box, Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import MedicalHistoryDetailMedicalOrderDetailDialog from '../dialogs/medicalOrderDialogs/MedicalHistoryDetailMedicalOrderDetailDialog'
import {
	canRenderMedicalOrderByRole,
	getMedicalOrderStatusInfo,
	getProcessedMedicalOrderDetailCountInfo,
} from '../helpers/medicalOrderHelper'

const MedicalOrderTimelineCard = ({ medicalOrder, typeColor, typeLabel, statusInfo, onClick }) => {
	const { t } = useTranslation()
	const processedDetailInfo = getProcessedMedicalOrderDetailCountInfo(medicalOrder)

	return (
		<Paper
			variant='outlined'
			onClick={onClick}
			sx={{
				p: 2,
				borderRadius: 2,
				cursor: 'pointer',
				transition: 'all 0.2s ease',
				'&:hover': {
					transform: 'translateY(-2px)',
					boxShadow: 3,
				},
			}}
		>
			<Stack spacing={1.25}>
				<Stack direction='row' justifyContent='space-between' alignItems='center' gap={1}>
					<Stack direction='row' spacing={0.75} alignItems='center' flexWrap='wrap'>
						<Chip size='small' color={typeColor} label={typeLabel} />
						<Typography variant='caption' color='text.secondary'>
							{renderEmptyFallback(formatDatetimeStringBasedOnCurrentLanguage(medicalOrder?.creationDate))}
						</Typography>
					</Stack>
					{statusInfo?.value && <Chip size='small' label={statusInfo.label} color={statusInfo.color} />}
				</Stack>
				<Box textAlign={'start'}>
					<Typography variant='body2' color='text.secondary'>
						{t('medical_order.field.instruction')}: {renderEmptyFallback(medicalOrder?.instruction)}
					</Typography>
					{processedDetailInfo && (
						<Typography variant='body2' color='text.secondary'>
							{t('medical_order.text.processed_details')}: {processedDetailInfo.processed}/
							{processedDetailInfo.total}
						</Typography>
					)}
				</Box>
			</Stack>
		</Paper>
	)
}

const MedicalHistoryDetailMedicalOrderSection = ({
	medicalOrders = [],
	role,
	loading = false,
	readOnly = false,
	// Medical Order actions
	onClickCreateMedicalOrder = () => {},
	onClickUpdateInstructionMedicalOrder = () => {},
	// Clinical Medical Order actions
	onClickCancelClinicalMedicalOrder = () => {},
	onClickCompleteClinicalMedicalOrderDetail = () => {},
	onClickFailClinicalMedicalOrderDetail = () => {},
	onClickPrintRefundClinicalMedicalOrderDetail = () => {},
	// Infusion Medical Order actions
	onClickCompleteInfusionMedicalOrderDetail = () => {},
	onClickFailInfusionMedicalOrderDetail = () => {},
	// Instruction Medical Order actions
	onClickIssueInstructionMedicalOrder = () => {},
	onClickCancelInstructionMedicalOrder = () => {},
}) => {
	const _enum = useEnum()
	const { t } = useTranslation()

	const [currentTypeKey, setCurrentTypeKey] = useState('')
	const [selectedMedicalOrder, setSelectedMedicalOrder] = useState(null)

	useEffect(() => {
		if (selectedMedicalOrder) {
			setSelectedMedicalOrder(medicalOrders.find((order) => order.id === selectedMedicalOrder.id))
		}
	}, [medicalOrders])

	const typeTabs = [
		{ key: '', title: t('text.all') },
		..._enum.medicalOrderTypeOptions.map((option) => ({
			key: option.value,
			title: option.label,
		})),
	]

	const currentTypeTab = typeTabs.find((tab) => tab.key === currentTypeKey) || typeTabs[0]

	const filteredMedicalOrders = medicalOrders
		.filter((medicalOrder) => canRenderMedicalOrderByRole(medicalOrder, role))
		.filter((medicalOrder) => {
			if (currentTypeKey === '') return true
			return medicalOrder?.medicalOrderType === currentTypeKey
		})
		.sort(
			(a, b) => new Date(b?.creationDate || 0).getTime() - new Date(a?.creationDate || 0).getTime()
		)

	const getMedicalOrderTypeIcon = (medicalOrder) => {
		const Icon = defaultMedicalOrderTypeStyle(medicalOrder?.medicalOrderType).icon
		return <Icon sx={{ fontSize: 18 }} />
	}

	if (loading) {
		return (
			<Paper sx={{ p: 3, borderRadius: 2 }}>
				<Typography variant='h6' gutterBottom>
					{t('medical_order.title.medical_orders')}
				</Typography>
				<SkeletonBox numberOfBoxes={4} heights={[44, 56, 120, 120]} rounded />
			</Paper>
		)
	}

	return (
		<Paper sx={{ p: 3, borderRadius: 2 }}>
			<Stack spacing={2}>
				<Stack
					direction={{ xs: 'column', md: 'row' }}
					justifyContent='space-between'
					alignItems={{ xs: 'flex-start', md: 'center' }}
					gap={1.5}
				>
					<Box>
						<Typography variant='h6'>{t('medical_order.title.medical_orders')}</Typography>
						<Typography variant='body2' color='text.secondary'>
							{t('medical_order.title.medical_order_subtitle')}
						</Typography>
					</Box>
					<Box width={{ xs: '100%', md: 'auto' }}>
						<GenericTabs
							tabs={typeTabs}
							currentTab={currentTypeTab}
							setCurrentTab={(tab) => setCurrentTypeKey(tab?.key || '')}
							maxWidth='fit-content'
						/>
					</Box>
				</Stack>

				{!readOnly && (
					<Box>
						<Button
							variant='contained'
							startIcon={<AddCircleOutlineOutlinedIcon />}
							onClick={() => onClickCreateMedicalOrder?.()}
						>
							{t('button.create')}
						</Button>
					</Box>
				)}

				{filteredMedicalOrders.length === 0 ? (
					<Stack alignItems='center' justifyContent='center' sx={{ py: 5 }}>
						<Typography color='text.secondary'>
							{t('medical_order.placeholder.no_medical_orders')}
						</Typography>
					</Stack>
				) : (
					<Timeline position='alternate-reverse'>
						{filteredMedicalOrders.map((medicalOrder, index) => {
							const typeColor = defaultMedicalOrderTypeStyle(medicalOrder?.medicalOrderType).color
							const typeLabel = getEnumLabelByValue(
								_enum.medicalOrderTypeOptions,
								medicalOrder?.medicalOrderType
							)
							const statusInfo = getMedicalOrderStatusInfo(medicalOrder, _enum)

							return (
								<TimelineItem key={`${medicalOrder?.id || 'medical-order'}-${index}`}>
									<TimelineSeparator>
										<TimelineConnector />
										<TimelineDot color={typeColor}>{getMedicalOrderTypeIcon(medicalOrder)}</TimelineDot>
										<TimelineConnector />
									</TimelineSeparator>

									<TimelineContent>
										<MedicalOrderTimelineCard
											medicalOrder={medicalOrder}
											typeColor={typeColor}
											typeLabel={typeLabel}
											statusInfo={statusInfo}
											onClick={() => setSelectedMedicalOrder(medicalOrder)}
										/>
									</TimelineContent>
								</TimelineItem>
							)
						})}
					</Timeline>
				)}
			</Stack>

			<MedicalHistoryDetailMedicalOrderDetailDialog
				open={Boolean(selectedMedicalOrder)}
				onClose={() => setSelectedMedicalOrder(null)}
				medicalOrder={selectedMedicalOrder}
				readOnly={readOnly}
				onClickCancelClinicalMedicalOrder={onClickCancelClinicalMedicalOrder}
				onClickCompleteClinicalMedicalOrderDetail={onClickCompleteClinicalMedicalOrderDetail}
				onClickFailClinicalMedicalOrderDetail={onClickFailClinicalMedicalOrderDetail}
				onClickCompleteInfusionMedicalOrderDetail={onClickCompleteInfusionMedicalOrderDetail}
				onClickFailInfusionMedicalOrderDetail={onClickFailInfusionMedicalOrderDetail}
				onClickPrintRefundClinicalMedicalOrderDetail={onClickPrintRefundClinicalMedicalOrderDetail}
				onClickUpdateInstructionMedicalOrder={onClickUpdateInstructionMedicalOrder}
				onClickIssueInstructionMedicalOrder={onClickIssueInstructionMedicalOrder}
				onClickCancelInstructionMedicalOrder={onClickCancelInstructionMedicalOrder}
			/>
		</Paper>
	)
}

export default MedicalHistoryDetailMedicalOrderSection

import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useNavigate, useParams } from 'react-router-dom'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const MedicineUnitDetailPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { id } = useParams()
	const medicineUnitId = id ? Number(id) : null

	const fetchUnit = useFetch(
		medicineUnitId ? ApiUrls.MEDICINE_UNIT.MANAGEMENT.DETAIL(medicineUnitId) : null,
		{},
		[medicineUnitId]
	)

	const options = fetchUnit.data?.options || []

	const toggleOptionStatus = useAxiosSubmit({
		url: ApiUrls.MEDICINE_UNIT_OPTION.MANAGEMENT.UPDATE_STATUS,
		method: 'PUT',
		onSuccess: async () => {
			await fetchUnit.fetch()
		},
	})

	const tableFields = [
		{ key: 'option.id', title: 'ID', width: 10 },
		{
			key: 'option.optionName',
			title: t('medicine_unit.field.options'),
			width: 50,
		},
		{
			key: 'isActive',
			title: t('medicine_unit.status'),
			width: 20,
			render: (val) => {
				const label = val ? t('medicine_unit.text.active') : t('medicine_unit.text.inactive')
				const color = val ? 'success' : 'default'
				return <Chip size='small' color={color} label={label} />
			},
		},
		{
			key: 'actions',
			title: t('medicine_unit.actions'),
			width: 20,
			render: (_, row) => {
				const isActive = row.isActive ?? true
				return (
					<ActionMenu
						actions={[
							{
								title: isActive
									? t('medicine_unit.button.inactivate')
									: t('medicine_unit.button.activate'),
								onClick: async () => {
									await toggleOptionStatus.submit({
										overrideData: {
											medicineUnitId: medicineUnitId,
											optionId: row.option.id,
											isActive: !isActive,
										},
									})
								},
							},
						]}
					/>
				)
			},
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={3}>
				<Stack direction='row' alignItems='center' spacing={2}>
					<Button
						onClick={() =>
							navigate(
								routeUrls.BASE_ROUTE.PHARMACIST(
									routeUrls.PHARMACIST.MEDICINE_UNIT_MANAGEMENT.INDEX
								)
							)
						}
					>
						{t('button.back')}
					</Button>
					<Typography variant='h5'>
						{fetchUnit.data?.name || t('text.loading')}
					</Typography>
				</Stack>

				<Paper variant='outlined' sx={{ p: 2 }}>
					<Stack spacing={1}>
						<Typography variant='h6'>{t('medicine_unit.information')}</Typography>
						<Stack direction='row' spacing={2}>
							<Typography variant='body2' fontWeight='bold'>
								{t('medicine_unit.field.name')}:
							</Typography>
							<Typography variant='body2'>{renderEmptyFallback(fetchUnit.data?.name)}</Typography>
						</Stack>
						<Stack direction='row' spacing={2}>
							<Typography variant='body2' fontWeight='bold'>
								{t('medicine_unit.field.description')}:
							</Typography>
							<Typography variant='body2'>{renderEmptyFallback(fetchUnit.data?.description)}</Typography>
						</Stack>
					</Stack>
				</Paper>

				<Paper variant='outlined' sx={{ p: 2 }}>
					<Typography variant='h6' sx={{ mb: 2 }}>
						{t('medicine_unit.field.options')}
					</Typography>
					<GenericTable data={options} fields={tableFields} rowKey='option.id' loading={fetchUnit.loading} />
				</Paper>
			</Stack>
		</Paper>
	)
}

export default MedicineUnitDetailPage

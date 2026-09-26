import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useEffect, useMemo, useState } from 'react'

const SCHEDULE_TYPES = {
	MANUAL: 'MANUAL',
	TEMPLATE: 'TEMPLATE',
}

const WorkScheduleManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreate,
	onUpdate,
	shiftOptions = [],
	templateOptions = [],
}) => {
	const { t } = useTranslation()

	const createInitialValues = useMemo(
		() => ({
			scheduleType: SCHEDULE_TYPES.MANUAL,
			workScheduleTemplateId: '',
			shiftId: '',
			fromDate: '',
			toDate: '',
		}),
		[]
	)

	const [createValues, setCreateValues] = useState(createInitialValues)

	const createFields = useMemo(
		() => [
			{
				key: 'scheduleType',
				title: t('work_schedule.field.template') + ' / ' + t('work_schedule.field.shift'),
				type: 'radio',
				options: [
					{ label: t('work_schedule.field.shift'), value: SCHEDULE_TYPES.MANUAL },
					{ label: t('work_schedule.field.template'), value: SCHEDULE_TYPES.TEMPLATE },
				],
			},
			...(createValues?.scheduleType === SCHEDULE_TYPES.TEMPLATE
				? [
						{
							key: 'workScheduleTemplateId',
							title: t('work_schedule.field.template'),
							type: 'select',
							required: false,
							options: templateOptions,
						},
					]
				: []),
			...(createValues?.scheduleType === SCHEDULE_TYPES.MANUAL
				? [
						{
							key: 'shiftId',
							title: t('work_schedule.field.shift'),
							type: 'select',
							required: false,
							options: shiftOptions,
						},
					]
				: []),
			{
				key: 'fromDate',
				title: t('work_schedule.field.from_date'),
				type: 'date',
			},
			{
				key: 'toDate',
				title: t('work_schedule.field.to_date'),
				type: 'date',
			},
		],
		[t, templateOptions, shiftOptions, createValues?.scheduleType]
	)

	const updateFields = useMemo(
		() => [
			{
				key: 'workDate',
				title: t('work_schedule.field.work_date'),
				type: 'date',
			},
			{
				key: 'shiftId',
				title: t('work_schedule.field.shift'),
				type: 'select',
				options: shiftOptions,
			},
		],
		[t, shiftOptions]
	)

	useEffect(() => {
		if (openCreate) {
			setCreateValues(createInitialValues)
		}
	}, [openCreate, createInitialValues])

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				title={t('work_schedule.dialog.create_title')}
				fields={createFields}
				initialValues={createInitialValues}
				onSubmit={onCreate}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				textFieldVariant='outlined'
				onValuesChange={setCreateValues}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				title={t('work_schedule.dialog.edit_title')}
				fields={updateFields}
				initialValues={selectedRow}
				onSubmit={onUpdate}
				submitLabel={t('button.save')}
				submitButtonColor='info'
				textFieldVariant='outlined'
			/>
		</>
	)
}

export default WorkScheduleManagementFormSection

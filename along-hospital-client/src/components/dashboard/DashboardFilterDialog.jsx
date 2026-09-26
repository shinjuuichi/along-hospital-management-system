import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import {
	getDashboardFilterFields,
	getDashboardFilterInitialValues,
	resolveDashboardFilterValues,
} from '@/utils/dashboardUtils'
import { useEffect, useMemo, useState } from 'react'

const DashboardFilterDialog = ({
	open,
	onClose,
	filterValues,
	onApply,
	translationPrefix = 'manager_dashboard',
	dateRangeOnly = false,
}) => {
	const { t } = useTranslation()
	const initialValues = useMemo(
		() => getDashboardFilterInitialValues(filterValues),
		[filterValues]
	)
	const [values, setValues] = useState(initialValues)

	useEffect(() => {
		if (open) {
			setValues(initialValues)
		}
	}, [initialValues, open])

	const fields = useMemo(
		() =>
			getDashboardFilterFields({
				preset: values.preset,
				t,
				translationPrefix,
				dateRangeOnly,
			}),
		[t, dateRangeOnly, translationPrefix, values.preset]
	)

	const handleSubmit = async ({ values, closeDialog }) => {
		onApply?.(resolveDashboardFilterValues(values, new Date(), { dateRangeOnly }))
		closeDialog()
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t(`${translationPrefix}.title.filters`)}
			fields={fields}
			initialValues={initialValues}
			onValuesChange={setValues}
			onSubmit={handleSubmit}
			submitLabel={t('button.filter')}
			textFieldVariant='outlined'
		/>
	)
}

export default DashboardFilterDialog

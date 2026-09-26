import { formatDateBasedOnCurrentLanguage, formatDateKey, parseDateKey } from '@/utils/formatDateUtil'

export const DashboardFilterPreset = {
	THIS_MONTH: 'thisMonth',
	THIS_YEAR: 'thisYear',
	CUSTOM: 'custom',
}

export const getThisYearRange = (baseDate = new Date()) => ({
	fromDate: formatDateKey(new Date(baseDate.getFullYear(), 0, 1)),
	toDate: formatDateKey(baseDate),
})

export const getThisMonthRange = (baseDate = new Date()) => ({
	fromDate: formatDateKey(new Date(baseDate.getFullYear(), baseDate.getMonth(), 1)),
	toDate: formatDateKey(baseDate),
})

export const getDefaultDashboardFilter = (baseDate = new Date()) => ({
	preset: DashboardFilterPreset.THIS_YEAR,
	...getThisYearRange(baseDate),
})

export const getDashboardFilterInitialValues = (filterValues = {}) => ({
	preset: filterValues?.preset || DashboardFilterPreset.THIS_YEAR,
	fromDate: filterValues?.fromDate || '',
	toDate: filterValues?.toDate || '',
})

export const getDashboardFilterFields = ({
	preset = DashboardFilterPreset.THIS_YEAR,
	t,
	translationPrefix = 'manager_dashboard',
	dateRangeOnly = false,
}) => {
	const dateRangeField = {
		key: 'dateRange',
		title: t(`${translationPrefix}.field.date_range`),
		type: 'daterange',
		from: {
			key: 'fromDate',
			label: t(`${translationPrefix}.field.from_date`),
		},
		to: {
			key: 'toDate',
			label: t(`${translationPrefix}.field.to_date`),
		},
	}

	if (dateRangeOnly) {
		return [dateRangeField]
	}

	return [
		{
			key: 'preset',
			title: t(`${translationPrefix}.field.preset`),
			type: 'radio',
			options: [
				{ value: DashboardFilterPreset.THIS_MONTH, label: t(`${translationPrefix}.preset.this_month`) },
				{ value: DashboardFilterPreset.THIS_YEAR, label: t(`${translationPrefix}.preset.this_year`) },
				{ value: DashboardFilterPreset.CUSTOM, label: t(`${translationPrefix}.preset.custom`) },
			],
		},
		...(preset === DashboardFilterPreset.CUSTOM ? [dateRangeField] : []),
	]
}

export const resolveDashboardFilterValues = (
	values = {},
	baseDate = new Date(),
	{ dateRangeOnly = false } = {}
) => {
	if (dateRangeOnly) {
		return {
			preset: DashboardFilterPreset.CUSTOM,
			fromDate: values.fromDate || '',
			toDate: values.toDate || '',
		}
	}

	switch (values.preset) {
		case DashboardFilterPreset.THIS_MONTH:
			return {
				preset: DashboardFilterPreset.THIS_MONTH,
				...getThisMonthRange(baseDate),
			}
		case DashboardFilterPreset.CUSTOM:
			return {
				preset: DashboardFilterPreset.CUSTOM,
				fromDate: values.fromDate || '',
				toDate: values.toDate || '',
			}
		case DashboardFilterPreset.THIS_YEAR:
		default:
			return {
				preset: DashboardFilterPreset.THIS_YEAR,
				...getThisYearRange(baseDate),
			}
	}
}

export const formatDashboardRangeLabel = (fromDate, toDate) => {
	if (!fromDate || !toDate) return ''

	const parsedFromDate = parseDateKey(fromDate)
	const parsedToDate = parseDateKey(toDate)

	if (!parsedFromDate || !parsedToDate) {
		return `${formatDateBasedOnCurrentLanguage(fromDate)} - ${formatDateBasedOnCurrentLanguage(toDate)}`
	}

	return `${formatDateBasedOnCurrentLanguage(parsedFromDate)} - ${formatDateBasedOnCurrentLanguage(parsedToDate)}`
}

import ValidationTextField from '@/components/textFields/ValidationTextField'
import { Delete } from '@mui/icons-material'
import { Box, Button, IconButton, Stack, Typography } from '@mui/material'

const MedicineSkuOptionValuesField = ({
	value = [],
	onChange,
	values = {},
	options = [],
	t,
	submitted = false,
}) => {
	const current = Array.isArray(value) ? value.filter((v) => v !== undefined && v !== null) : []
	const rows = current.length ? current : [{ optionId: '', optionValueIds: [] }]

	const unitOptions = options || []

	const allOptionValues = unitOptions.flatMap((opt) =>
		(opt.optionValues || []).filter((ov) => ov.isActive)
	)

	const getAvailableOptionsForRow = (rowIdx) => {
		const uniqueOptions = unitOptions.map((opt) => ({ label: opt.optionName, value: opt.id }))

		return uniqueOptions.filter((opt) => {
			const usedCount = rows.filter((r) => r.optionId === opt.value).length
			const isUsedByOthers = usedCount > 0 && rows[rowIdx]?.optionId !== opt.value
			const isUsedByCurrent = rows[rowIdx]?.optionId === opt.value
			return !isUsedByOthers || isUsedByCurrent
		})
	}

	const getFilteredOptionValues = (optionId) => {
		if (!optionId) return []
		return allOptionValues.filter((ov) => ov.optionId === optionId)
	}

	const updateRow = (idx, nextRow) => {
		const next = rows.slice()
		next[idx] = nextRow
		onChange(next)
	}

	const addRow = () => {
		onChange([...rows, { optionId: '', optionValueIds: [] }])
	}

	const removeRow = (idx) => {
		if (rows.length <= 1) return
		onChange(rows.filter((_, i) => i !== idx))
	}

	const updateOptionId = (idx, optionId) => {
		updateRow(idx, { optionId, optionValueIds: [] })
	}

	const updateOptionValueIds = (idx, optionValueIds) => {
		updateRow(idx, { ...rows[idx], optionValueIds })
	}

	const getRowError = (row) => {
		if (!submitted) return ''
		if (row.optionId && (!row.optionValueIds || row.optionValueIds.length === 0)) {
			return t('error.required')
		}
		return ''
	}

	return (
		<Stack spacing={1.5}>
			{rows.map((row, idx) => {
				const filteredOptionValues = getFilteredOptionValues(row.optionId).map((ov) => ({
					value: ov.id,
					label: ov.valueName,
				}))

				const availableOptions = getAvailableOptionsForRow(idx)
				const rowError = getRowError(row)

				return (
					<Box
						key={idx}
						sx={{
							p: 1.5,
							borderRadius: 1,
							border: '1px solid',
							borderColor: rowError ? 'error.main' : 'divider',
							bgcolor: 'background.default',
						}}
					>
						<Stack direction='row' spacing={1} alignItems='flex-start'>
							<Box sx={{ flex: 1 }}>
								<ValidationTextField
									type='select-dialog'
									size='small'
									required={false}
									fullWidth
									label={t('medicine_sku.field.option')}
									value={row.optionId ?? ''}
									options={[{ value: '', label: t('text.select_options') }, ...availableOptions]}
									onChange={(e) => updateOptionId(idx, e.target.value)}
								/>
							</Box>

							{row.optionId && (
								<Box sx={{ flex: 1 }}>
									<ValidationTextField
										type='select-dialog'
										size='small'
										required={false}
										fullWidth
										label={t('medicine_sku.field.option_value')}
										value={row.optionValueIds?.[0] ?? ''}
										options={[{ value: '', label: t('text.select_options') }, ...filteredOptionValues]}
										error={!!rowError}
										helperText={rowError}
										onChange={(e) => {
											const selected = e.target.value
											updateOptionValueIds(idx, selected ? [selected] : [])
										}}
									/>
								</Box>
							)}

							{rows.length > 1 && (
								<IconButton color='error' size='small' onClick={() => removeRow(idx)} sx={{ mt: 0.5 }}>
									<Delete fontSize='small' />
								</IconButton>
							)}
						</Stack>
					</Box>
				)
			})}

			{getAvailableOptionsForRow(-1).length > 0 && (
				<Button variant='outlined' onClick={addRow} sx={{ width: 'min(50%, 220px)' }}>
					+ {t('button.add')}
				</Button>
			)}

			<Typography variant='caption' color='text.secondary'>
				{t('medicine_sku.helper.option_unique')}
			</Typography>
		</Stack>
	)
}

export default MedicineSkuOptionValuesField

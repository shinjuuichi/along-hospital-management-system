/* eslint-disable react-hooks/exhaustive-deps */
import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { maxLen } from '@/utils/validateUtil'
import { Button, Stack, Tooltip, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const QualificationManagementTableSection = ({
	qualifications,
	loading,
	sort,
	setSort,
	refetch,
}) => {
	const [selectedIds, setSelectedIds] = useState([])
	const [selectedRow, setSelectedRow] = useState({})
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const confirm = useConfirm()
	const { t } = useTranslation()

	const qualificationPost = useAxiosSubmit({
		url: ApiUrls.QUALIFICATION.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const qualificationPut = useAxiosSubmit({
		url: ApiUrls.QUALIFICATION.MANAGEMENT.DETAIL(selectedRow.id),
		method: 'PUT',
	})

	const qualificationDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const createInitialValues = useMemo(
		() => ({
			name: '',
			description: '',
		}),
		[]
	)

	const handleUpdateSubmit = async ({ values, closeDialog }) => {
		const respond = await qualificationPut.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			refetch()
		}
	}

	const handleCreateSubmit = async ({ values, closeDialog }) => {
		const respond = await qualificationPost.submit({ overrideData: values })

		if (respond) {
			closeDialog()
			refetch()
		}
	}

	const handleDeleteClick = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('qualification.title.delete'),
			description: `${t('qualification.title.delete_confirm')} ${row.name}?`,
		})

		if (isConfirmed) {
			await qualificationDelete.submit({
				overrideUrl: ApiUrls.QUALIFICATION.MANAGEMENT.DETAIL(row.id),
			})
			refetch()
		}
	}

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('qualification.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'name', title: t('qualification.field.name'), width: 20, sortable: true },
			{
				key: 'description',
				title: t('qualification.field.description'),
				width: 50,
				sortable: false,
				render: (value) => {
					const text = value ?? ''
					return (
						<Tooltip title={text} arrow placement='top' disableInteractive>
							<Typography
								variant='body2'
								noWrap
								sx={{ width: '100%', overflow: 'hidden', textOverflow: 'ellipsis' }}
							>
								{renderEmptyFallback(text)}
							</Typography>
						</Tooltip>
					)
				},
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.edit'),
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
							{
								title: t('button.delete'),
								onClick: () => handleDeleteClick(row),
							},
						]}
					/>
				),
			},
		],
		[t]
	)

	const upsertField = useMemo(
		() => [
			{
				key: 'name',
				title: t('qualification.field.name'),
				type: 'text',
				validate: [maxLen(255)],
				required: true,
			},
			{
				key: 'description',
				title: t('qualification.field.description'),
				type: 'text',
				required: false,
				validate: [maxLen(1000)],
			},
		],
		[t]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={qualifications}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={createInitialValues}
				fields={upsertField}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('qualification.title.create')}
				onSubmit={handleCreateSubmit}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={upsertField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('qualification.title.update')}
				onSubmit={handleUpdateSubmit}
			/>
		</>
	)
}

export default QualificationManagementTableSection

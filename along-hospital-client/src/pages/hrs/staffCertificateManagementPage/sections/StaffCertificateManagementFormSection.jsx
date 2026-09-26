import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ConfirmationDialog from '@/components/dialogs/commons/ConfirmationDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { useCallback, useMemo, useState } from 'react'

const StaffCertificateManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onSuspendSubmit,
	onActivateSubmit,
	onApproveSubmit,
	staffCertificateTypeOptions = [],
	staffOptions = [],
}) => {
	const [openSuspendDialog, setOpenSuspendDialog] = useState(false)
	const [openActivateExpiredDialog, setOpenActivateExpiredDialog] = useState(false)
	const [pendingActivate, setPendingActivate] = useState(null)
	const [pendingApprove, setPendingApprove] = useState(null)
	const { t } = useTranslation()
	const _enum = useEnum()

	const isValid = selectedItem?.status === EnumConfig.StaffCertificateStatus.Valid
	const isSuspended = selectedItem?.status === EnumConfig.StaffCertificateStatus.Suspended
	const isExpired = selectedItem?.status === EnumConfig.StaffCertificateStatus.Expired

	const canShowButtons = isValid || isSuspended || isExpired

	const getStatusLabel = useCallback(
		(status) => getEnumLabelByValue(_enum.staffCertificateStatusOptions, status) ?? '',
		[_enum]
	)

	const createFormFields = useMemo(
		() => [
			{
				key: 'staffId',
				title: t('staff_certificate.field.staff'),
				type: 'select',
				options: staffOptions,
			},
			{
				key: 'staffCertificateTypeId',
				title: t('staff_certificate.field.certificate_type'),
				type: 'select',
				options: staffCertificateTypeOptions,
			},
			{
				key: 'certificateNo',
				title: t('staff_certificate.field.certificate_no'),
				type: 'text',
			},
			{
				key: 'issuedDate',
				title: t('staff_certificate.field.issued_date'),
				type: 'date',
				maxValue: new Date().toISOString().split('T')[0],
			},
			{
				key: 'expiredDate',
				title: t('staff_certificate.field.expired_date'),
				type: 'date',
				minValue: new Date().toISOString().split('T')[0],
			},
			{
				key: 'issuedBy',
				title: t('staff_certificate.field.issued_by'),
				type: 'text',
			},
		],
		[t, staffOptions, staffCertificateTypeOptions]
	)

	const updateFormFields = useMemo(
		() => [
			{
				key: 'certificateNo',
				title: t('staff_certificate.field.certificate_no'),
				type: 'text',
				props: { readOnly: true },
			},
			{
				key: 'staffId',
				title: t('staff_certificate.field.staff'),
				type: 'select',
				options: staffOptions,
				props: { readOnly: true },
			},
			{
				key: 'staffCertificateTypeId',
				title: t('staff_certificate.field.certificate_type'),
				type: 'select',
				options: staffCertificateTypeOptions,
				props: { readOnly: true },
			},
			{
				key: 'issuedDate',
				title: t('staff_certificate.field.issued_date'),
				type: 'date',
				props: { readOnly: true },
			},
			{
				key: 'expiredDate',
				title: t('staff_certificate.field.expired_date'),
				type: 'date',
				props: { readOnly: true },
			},
			{
				key: 'issuedBy',
				title: t('staff_certificate.field.issued_by'),
				type: 'text',
				props: { readOnly: true },
			},
			{
				key: 'status',
				title: t('staff_certificate.table.status'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
			{
				key: 'reason',
				title: t('staff_certificate.field.reason'),
				type: 'text',
				required: false,
				props: { readOnly: true },
			},
		],
		[t, staffCertificateTypeOptions, staffOptions]
	)

	const updateInitialValues = useMemo(() => {
		if (!selectedItem) return {}
		return {
			id: selectedItem.id,
			certificateNo: selectedItem.certificateNo,
			staffCertificateTypeId: selectedItem.staffCertificateTypeId,
			staffId: selectedItem.staffId,
			issuedDate: selectedItem.issuedDate,
			expiredDate: selectedItem.expiredDate,
			issuedBy: selectedItem.issuedBy,
			status: getStatusLabel(selectedItem.status),
			reason: selectedItem.reason || '',
		}
	}, [selectedItem, getStatusLabel])

	const updateAdditionalButtons = useMemo(() => {
		if (!canShowButtons) return []

		const buttons = []

		if (isValid) {
			buttons.push({
				label: t('staff_certificate.button.suspend'),
				color: 'warning',
				variant: 'contained',
				onClick: () => setOpenSuspendDialog(true),
			})
		}

		if (isSuspended) {
			buttons.push({
				label: t('staff_certificate.button.approve'),
				color: 'primary',
				variant: 'contained',
				onClick: () => setPendingApprove({ id: selectedItem?.id }),
			})
		}

		if (isExpired) {
			buttons.push({
				label: t('staff_certificate.button.activate'),
				color: 'success',
				variant: 'contained',
				onClick: () => setOpenActivateExpiredDialog(true),
			})
		}

		return buttons
	}, [canShowButtons, isValid, isSuspended, isExpired, t, selectedItem, setOpenSuspendDialog, setOpenActivateExpiredDialog, setPendingApprove])

	const handleCreateSubmit = useCallback(
		async ({ values, closeDialog }) => {
			await onCreateSubmit({ values, closeDialog })
		},
		[onCreateSubmit]
	)

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={createFormFields}
				initialValues={{}}
				submitLabel={t('button.create')}
				title={t('staff_certificate.dialog.create_title')}
				maxWidth='md'
				onSubmit={handleCreateSubmit}
			/>

			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={updateFormFields}
				initialValues={updateInitialValues}
				title={t('staff_certificate.dialog.view_title')}
				maxWidth='md'
				additionalButtons={updateAdditionalButtons}
				showSubmit={false}
			/>

			<GenericFormDialog
				open={openSuspendDialog}
				onClose={() => setOpenSuspendDialog(false)}
				fields={[
					{
						key: 'reason',
						title: t('staff_certificate.field.reason'),
						type: 'text',
						required: false,
					},
				]}
				initialValues={{}}
				submitLabel={t('staff_certificate.button.suspend')}
				title={t('staff_certificate.dialog.suspend_title')}
				maxWidth='sm'
				onSubmit={async ({ values }) => {
					if (!selectedItem?.id) return
					await onSuspendSubmit(selectedItem.id, values.reason)
					setOpenSuspendDialog(false)
					setOpenUpdateForm(false)
				}}
			/>

			<GenericFormDialog
				open={openActivateExpiredDialog}
				onClose={() => setOpenActivateExpiredDialog(false)}
				fields={[
					{
						key: 'expiredDate',
						title: t('staff_certificate.field.new_expired_date'),
						type: 'date',
						minValue: new Date().toISOString().split('T')[0],
					},
				]}
				initialValues={{}}
				submitLabel={t('button.next')}
				title={t('staff_certificate.dialog.activate_expired_title')}
				maxWidth='sm'
				onSubmit={async ({ values }) => {
					if (!selectedItem?.id) return
					setPendingActivate({ id: selectedItem.id, expiredDate: values.expiredDate })
					setOpenActivateExpiredDialog(false)
				}}
			/>

			<ConfirmationDialog
				open={!!pendingActivate}
				onClose={() => setPendingActivate(null)}
				title={t('staff_certificate.dialog.confirm_activate_title')}
				description={t('staff_certificate.dialog.confirm_activate_description')}
				confirmButtonText={t('staff_certificate.button.activate')}
				confirmButtonColor='success'
				onConfirm={async () => {
					if (!pendingActivate?.id) return
					await onActivateSubmit(pendingActivate.id, pendingActivate.expiredDate)
					setPendingActivate(null)
					setOpenUpdateForm(false)
				}}
			/>

			<ConfirmationDialog
				open={!!pendingApprove}
				onClose={() => setPendingApprove(null)}
				title={t('staff_certificate.dialog.confirm_approve_title')}
				description={t('staff_certificate.dialog.confirm_approve_description')}
				confirmButtonText={t('staff_certificate.button.approve')}
				confirmButtonColor='primary'
				onConfirm={async () => {
					if (!pendingApprove?.id) return
					await onApproveSubmit(pendingApprove.id)
					setPendingApprove(null)
					setOpenUpdateForm(false)
				}}
			/>
		</>
	)
}

export default StaffCertificateManagementFormSection

import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Cancel, Edit, Save } from '@mui/icons-material'
import { Avatar, Button, ButtonBase, Chip, Input, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'

const ProfileHeaderCardSection = ({
	profile,
	role,
	editMode = false,
	formValues = {},
	onFieldChange,
	onEdit,
	onCancel,
	onSave,
	saving = false,
	loading = false,
	showActions = true,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const isDoctor = role === EnumConfig.Role.Doctor
	const isPatient = role === EnumConfig.Role.Patient
	const fileInputRef = useRef(null)
	const [imagePreview, setImagePreview] = useState(null)
	const specialtyLabel = profile?.specialtyName
	const qualificationLabel = profile?.qualificationName

	const rawDisplayName = editMode ? formValues.name || profile?.name : profile?.name
	const displayName = renderEmptyFallback(rawDisplayName)

	useEffect(() => {
		if (editMode && formValues.image instanceof File) {
			const url = URL.createObjectURL(formValues.image)
			setImagePreview(url)
			return () => URL.revokeObjectURL(url)
		} else {
			setImagePreview(null)
		}
	}, [formValues.image, editMode])

	const imageSrc = editMode
		? imagePreview || (profile?.image ? getImageFromCloud(profile.image) : undefined)
		: profile?.image
		? getImageFromCloud(profile.image)
		: undefined

	const handleAvatarClick = () => {
		if (editMode && fileInputRef.current) {
			fileInputRef.current.click()
		}
	}

	const handleImageChange = (e) => {
		const file = e.target.files?.[0]
		if (file) {
			onFieldChange('image', file)
		}
		if (e.target) {
			e.target.value = ''
		}
	}

	if (loading) {
		return (
			<Paper
				sx={{
					p: 3,
					borderRadius: 2,
					background: (theme) => theme.palette.gradients.calm,
					border: '1px solid',
					borderColor: 'divider',
				}}
			>
				<SkeletonBox numberOfBoxes={1} heights={[100]} />
			</Paper>
		)
	}

	return (
		<Paper
			sx={{
				p: 3,
				borderRadius: 2,
				background: (theme) => theme.palette.gradients.calm,
				border: '1px solid',
				borderColor: 'divider',
			}}
		>
			<Input
				type='file'
				inputRef={fileInputRef}
				onChange={handleImageChange}
				sx={{ display: 'none' }}
				inputProps={{ accept: 'image/*' }}
			/>
			<Stack
				direction='row'
				spacing={3}
				alignItems='center'
				justifyContent='space-between'
				flexWrap='wrap'
			>
				<Stack direction='row' spacing={3} alignItems='center' flex={1} minWidth={0}>
					<ButtonBase
						onClick={handleAvatarClick}
						sx={{
							borderRadius: '50%',
							cursor: editMode ? 'pointer' : 'default',
							'&:hover': editMode
								? {
										opacity: 0.8,
								  }
								: {},
						}}
						disabled={!editMode}
					>
						<Avatar
							src={imageSrc}
							alt={displayName}
							sx={{
								width: 100,
								height: 100,
								border: '3px solid',
								borderColor: 'background.paper',
								boxShadow: 2,
								bgcolor: 'primary.main',
								color: 'primary.contrastText',
								fontSize: '2.5rem',
								fontWeight: 600,
							}}
						>
							{rawDisplayName
								? rawDisplayName
										.split(' ')
										.map((p) => p[0])
										.join('')
										.slice(0, 2)
										.toUpperCase()
								: 'U'}
						</Avatar>
					</ButtonBase>
					<Stack spacing={1} flex={1} minWidth={0}>
						<Typography variant='h5' sx={{ fontWeight: 600, color: 'text.primary' }}>
							{displayName}
						</Typography>
						{isDoctor && (
							<Stack direction='row' spacing={1} flexWrap='wrap'>
								{specialtyLabel && (
									<Chip
										label={`${t('profile.field.specialty')}: ${specialtyLabel}`}
										size='small'
										sx={{
											bgcolor: 'primary.softBg',
											color: 'primary.main',
											border: '1px solid',
											borderColor: 'primary.softBorder',
										}}
									/>
								)}
								{qualificationLabel && (
									<Chip
										label={`${t('profile.field.qualification')}: ${qualificationLabel}`}
										size='small'
										sx={{
											bgcolor: 'secondary.softBg',
											color: 'secondary.main',
											border: '1px solid',
											borderColor: 'secondary.softBorder',
										}}
									/>
								)}
							</Stack>
						)}
						{isPatient && (
							<Stack direction='row' spacing={1} flexWrap='wrap'>
								{profile?.weight > 0 && (
									<Chip
										label={`${t('profile.field.weight')}: ${profile.weight} kg`}
										size='small'
										sx={{
											bgcolor: 'primary.softBg',
											color: 'primary.main',
											border: '1px solid',
											borderColor: 'primary.softBorder',
										}}
									/>
								)}
								{profile?.bloodType && profile.bloodType !== EnumConfig.BloodType.Unknown && (
									<Chip
										label={`${t('profile.field.blood_type')}: ${getEnumLabelByValue(_enum.bloodTypeOptions, profile.bloodType) || profile.bloodType}`}
										size='small'
										sx={{
											bgcolor: 'secondary.softBg',
											color: 'secondary.main',
											border: '1px solid',
											borderColor: 'secondary.softBorder',
										}}
									/>
								)}
							</Stack>
						)}
					</Stack>
				</Stack>
				{showActions && (
					<>
						{editMode ? (
							<Stack direction='row' spacing={2}>
								<Button
									variant='outlined'
									color='inherit'
									onClick={onCancel}
									startIcon={<Cancel />}
									disabled={saving}
								>
									{t('button.cancel')}
								</Button>
								<Button
									variant='contained'
									color='primary'
									onClick={onSave}
									startIcon={<Save />}
									disabled={saving}
								>
									{saving ? t('button.submitting') : t('button.save')}
								</Button>
							</Stack>
						) : (
							<Button variant='contained' color='primary' onClick={onEdit} startIcon={<Edit />}>
								{t('button.update')}
							</Button>
						)}
					</>
				)}
			</Stack>
		</Paper>
	)
}

export default ProfileHeaderCardSection

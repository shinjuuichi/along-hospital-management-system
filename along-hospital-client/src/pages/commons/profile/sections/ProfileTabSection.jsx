import useTranslation from '@/hooks/useTranslation'
import { Lock, Person } from '@mui/icons-material'
import { Box, Tab, Tabs } from '@mui/material'
import { useState } from 'react'
import ChangePasswordSection from './ChangePasswordSection'
import ProfileInfoTabSection from './ProfileInfoTabSection'

const ProfileTabSection = ({
	profile,
	role,
	editMode,
	formValues,
	onFieldChange,
	onFieldHandleChange,
	onFieldRegisterRef,
	submitted,
	loading,
	onTabChange,
}) => {
	const { t } = useTranslation()
	const [activeTab, setActiveTab] = useState(0)

	const handleTabChange = (_, newValue) => {
		setActiveTab(newValue)
		onTabChange?.(newValue)
	}

	const tabs = [
		{
			label: t('profile.title.update_profile'),
			icon: <Person />,
		},
		{
			label: t('profile.title.change_password'),
			icon: <Lock />,
		},
	]

	return (
		<Box sx={{ display: 'flex', gap: 3, flexDirection: { xs: 'column', md: 'row' } }}>
			<Tabs
				orientation='vertical'
				value={activeTab}
				onChange={handleTabChange}
				sx={{
					minWidth: 220,
					borderRight: 1,
					borderColor: 'divider',
					'& .MuiTab-root': {
						alignItems: 'center',
						textAlign: 'left',
						minHeight: 56,
						justifyContent: 'flex-start',
						px: 2,
					},
					display: { xs: 'none', md: 'flex' },
				}}
			>
				{tabs.map((tab, index) => (
					<Tab
						key={index}
						icon={tab.icon}
						iconPosition='start'
						label={tab.label}
						sx={{ gap: 1.5, alignItems: 'center' }}
					/>
				))}
			</Tabs>

			<Tabs
				value={activeTab}
				onChange={handleTabChange}
				variant='fullWidth'
				sx={{
					borderBottom: 1,
					borderColor: 'divider',
					display: { xs: 'flex', md: 'none' },
					'& .MuiTab-root': {
						minHeight: 48,
					},
				}}
			>
				{tabs.map((tab, index) => (
					<Tab key={index} icon={tab.icon} iconPosition='start' label={tab.label} sx={{ gap: 1 }} />
				))}
			</Tabs>

			<Box sx={{ flex: 1, minHeight: 400 }}>
				<Box sx={{ display: activeTab === 0 ? 'block' : 'none', height: '100%' }}>
					<ProfileInfoTabSection
						profile={profile}
						role={role}
						editMode={editMode}
						formValues={formValues}
						onFieldChange={onFieldChange}
						onFieldHandleChange={onFieldHandleChange}
						onFieldRegisterRef={onFieldRegisterRef}
						submitted={submitted}
						loading={loading}
					/>
				</Box>
				<Box sx={{ display: activeTab === 1 ? 'block' : 'none', height: '100%' }}>
					<ChangePasswordSection />
				</Box>
			</Box>
		</Box>
	)
}

export default ProfileTabSection

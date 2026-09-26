/* eslint-disable no-unused-vars */
import { Button, Stack } from '@mui/material'

const GenericTab = ({ label, icon, active = false, onClick, disabled = false }) => {
	return (
		<Button
			variant='outlined'
			onClick={onClick}
			disabled={disabled}
			size='medium'
			sx={(theme) => ({
				bgcolor: active ? theme.palette.background.lightBlue : theme.palette.background.lightGray,
				borderColor: active ? theme.palette.text.blue.dark : theme.palette.text.secondary,
				color: active ? theme.palette.text.blue.main : theme.palette.text.secondary,
				textTransform: 'none',
			})}
			startIcon={icon}
		>
			{label}
		</Button>
	)
}

/**
 *
 * @typedef {Object} CustomProps
 * @property {Array<{key: string, title: string, icon?: React.ReactNode, disabled?: boolean}>} props.tabs
 * @property {{key: string, title: string}} props.currentTab
 * @property {function} props.setCurrentTab
 * @property {'row' | 'column'} props.direction
 * @property {string | number} props.maxWidth
 * @property {string | number} props.maxHeight
 * @property {boolean} props.loading
 */
const GenericTabs = ({
	tabs = [],
	currentTab,
	setCurrentTab = (tab) => {},
	onReclickTab = (tab) => {},
	direction = 'row',
	maxWidth = '100%',
	maxHeight,
	loading = false,
}) => {
	const isActive = (tab) => {
		if (currentTab === undefined || currentTab === null) {
			return tab.key === '' || tab.key === null || tab.key === undefined
		}

		if (typeof currentTab === 'string') return currentTab === tab.key
		if (typeof currentTab === 'object') return currentTab.key === tab.key
		return false
	}

	const handleClickTab = (tab) => {
		if (isActive(tab)) {
			onReclickTab?.(tab)
		} else {
			setCurrentTab(tab)
		}
	}

	return (
		<Stack
			width={'100%'}
			gap={1}
			justifyContent={'flex-start'}
			alignItems={'center'}
			direction={direction}
			maxWidth={maxWidth}
			maxHeight={maxHeight}
			flexWrap={'wrap'}
		>
			{tabs.map((tab) => (
				<GenericTab
					key={tab.key}
					label={tab.title}
					icon={tab.icon}
					active={isActive(tab)}
					disabled={tab.disabled || loading}
					onClick={() => handleClickTab(tab)}
				/>
			))}
		</Stack>
	)
}

export default GenericTabs

// Usage example:
/*
const statusTabs = [
	{ key: 'all', title: 'All', icon: <ListAlt /> },
	{ key: 'active', title: 'Active', icon: <Settings /> },
	{ key: 'inactive', title: 'Inactive', icon: <DisabledVisible /> },
]

const [currentStatusTab, setCurrentStatusTab] = useState(statusTabs.find((tab) => tab.key === 'all'))

<GenericTabs
	tabs={statusTabs}
	currentTab={currentCategoryTab}
	setCurrentTab={setCurrentCategoryTab}
/>

*/

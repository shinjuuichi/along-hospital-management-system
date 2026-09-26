import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Avatar,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	List,
	ListItem,
	ListItemIcon,
	ListItemText,
	Typography,
} from '@mui/material'

const PayrollPolicyStaffDialogSection = ({ open, onClose, staffs = [] }) => {
	const { t } = useTranslation()

	return (
		<Dialog open={open} onClose={onClose} maxWidth='sm' fullWidth>
			<DialogTitle>{t('button.detail')}</DialogTitle>
			<DialogContent dividers>
				{staffs.length === 0 ? (
					<Typography variant='body2' color='text.secondary'>
						{t('text.placeholder.no_data')}
					</Typography>
				) : (
					<List sx={{ p: 0 }}>
						{staffs.map((staff) => (
							<ListItem key={staff.id} divider>
								<ListItemIcon>
									<Avatar src={getImageFromCloud(staff?.image)} alt={staff?.name} />
								</ListItemIcon>
								<ListItemText
									primary={renderEmptyFallback(staff?.name)}
									secondary={`${staff?.phone || ''} ${staff?.email || ''}`.trim()}
								/>
							</ListItem>
						))}
					</List>
				)}
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose} variant='outlined'>
					{t('button.close')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default PayrollPolicyStaffDialogSection

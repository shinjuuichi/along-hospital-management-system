import { Box } from '@mui/material'

const PayrollPrintQrSection = ({ qrCode }) => {
	return (
		<Box sx={{ display: 'flex', justifyContent: 'center', mt: 2 }}>
			{qrCode ? (
				<Box
					component='img'
					src={qrCode}
					alt='QR Code'
					sx={{
						width: 120,
						height: 120,
						border: '1px solid #B0B7C3',
						borderRadius: 1,
						objectFit: 'contain',
						p: 0.5,
						bgcolor: '#fff',
					}}
				/>
			) : (
				<Box
					sx={{
						width: 120,
						height: 120,
						border: '1px dashed #B0B7C3',
						borderRadius: 1,
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						fontSize: 10,
						color: '#777',
						letterSpacing: 0.2,
					}}
				>
					QR CODE
				</Box>
			)}
		</Box>
	)
}

export default PayrollPrintQrSection

import SearchBar from '@/components/generals/SearchBar'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button, CircularProgress, Divider, Paper, Stack, Typography } from '@mui/material'
import { alpha } from '@mui/material/styles'

const MAX_DESCRIPTION_WORDS = 20
const LETTER_BACKGROUND_COLORS = [
	'#80C4FF',
	'#7FD1AE',
	'#FFCF70',
	'#C99CFF',
	'#7ADCF0',
	'#FF9BB0',
	'#7FD4C2',
]

const getLetterBackgroundColor = (letter) => {
	const key = `${letter ?? ''}`
	if (!key) {
		return LETTER_BACKGROUND_COLORS[0]
	}

	let hash = 0
	for (let i = 0; i < key.length; i += 1) {
		hash = (hash << 5) - hash + key.charCodeAt(i)
		hash |= 0
	}

	return LETTER_BACKGROUND_COLORS[Math.abs(hash) % LETTER_BACKGROUND_COLORS.length]
}

const buildSpecialtyDescription = (specialty, fallback) => {
	const rawDescription = specialty?.shortDescription?.trim() || specialty?.description?.trim() || ''

	if (!rawDescription) {
		return fallback
	}

	const words = rawDescription.split(/\s+/)
	if (words.length <= MAX_DESCRIPTION_WORDS) {
		return rawDescription
	}

	return `${words.slice(0, MAX_DESCRIPTION_WORDS).join(' ')}...`
}

const SpecialtyListSection = ({
	letters = [],
	activeLetter,
	onLetterChange,
	searchTerm,
	onSearchChange,
	groupedSpecialties = [],
	onViewDetail,
	loading = false,
	availableLetters = new Set(),
}) => {
	const { t } = useTranslation()

	return (
		<Stack component='section' spacing={3}>
			<Stack spacing={1}>
				<Typography variant='h4' fontWeight={700}>
					{t('specialty.title.list')}
				</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('specialty.description.list')}
				</Typography>
			</Stack>

			<SearchBar widthPercent={0} value={searchTerm} setValue={onSearchChange} />

			<Stack direction='row' flexWrap='wrap' gap={1}>
				{letters
					.filter((letter) => {
						return letter === 'ALL' ? groupedSpecialties.length > 0 : availableLetters.has(letter)
					})
					.map((letter) => {
						const isActive = activeLetter === letter
						const backgroundColor = getLetterBackgroundColor(letter)

						return (
							<Button
								key={letter}
								variant='contained'
								size='small'
								onClick={() => onLetterChange(letter)}
								sx={({ palette }) => {
									const baseColor = backgroundColor
									return {
										backgroundColor: isActive ? palette.primary.main : baseColor,
										color: isActive ? palette.primary.contrastText : palette.text.primary,
										borderColor: 'transparent',
										transition: 'background-color 150ms ease, color 150ms ease, transform 150ms ease',
										boxShadow: 'none',
										'&:hover': {
											backgroundColor: isActive ? palette.primary.dark : alpha(baseColor, 0.85),
										},
										'&:focus-visible': {
											outline: `2px solid ${alpha(isActive ? palette.primary.light : baseColor, 0.7)}`,
											outlineOffset: 2,
										},
									}
								}}
							>
								{letter === 'ALL' ? t('text.all') : letter}
							</Button>
						)
					})}
			</Stack>

			{loading ? (
				<Stack alignItems='center' justifyContent='center' py={6}>
					<CircularProgress />
				</Stack>
			) : groupedSpecialties.length === 0 ? (
				<Paper variant='outlined' sx={{ p: 4, textAlign: 'center' }}>
					<Typography variant='subtitle1' fontWeight={600}>
						{t('specialty.placeholder.no_result_title')}
					</Typography>
					<Typography variant='body2' color='text.secondary'>
						{t('specialty.placeholder.no_result_subtitle')}
					</Typography>
				</Paper>
			) : (
				<Stack spacing={3}>
					{groupedSpecialties.map(({ letter, items }) => (
						<Box key={letter}>
							<Typography variant='h6' fontWeight={700} gutterBottom>
								{letter}
							</Typography>
							<Stack spacing={1.5}>
								{items.map((specialty) => (
									<Paper
										key={specialty?.id || specialty?.name}
										variant='outlined'
										sx={{
											p: { xs: 2, md: 3 },
											transition: 'transform 120ms ease, box-shadow 120ms ease, border-color 120ms ease',
											'&:hover': {
												transform: 'translateY(-4px)',
												boxShadow: 3,
												borderColor: 'transparent',
											},
										}}
									>
										<Stack spacing={1.5}>
											<Stack direction={{ xs: 'column', md: 'row' }} spacing={1}>
												<Typography variant='subtitle1' fontWeight={600} flexGrow={1}>
													{specialty?.name}
												</Typography>
												<Button size='small' onClick={() => onViewDetail?.(specialty)}>
													{t('specialty.button.view_detail')}
												</Button>
											</Stack>
											<Divider />
											<Typography variant='body2' color='text.secondary' sx={{ whiteSpace: 'pre-line' }}>
												{buildSpecialtyDescription(specialty, t('specialty.placeholder.no_description'))}
											</Typography>
										</Stack>
									</Paper>
								))}
							</Stack>
						</Box>
					))}
				</Stack>
			)}
		</Stack>
	)
}

export default SpecialtyListSection

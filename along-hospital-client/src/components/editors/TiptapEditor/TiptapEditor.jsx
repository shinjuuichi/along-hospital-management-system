import {
	Code,
	FormatBold,
	FormatColorText,
	FormatItalic,
	FormatListBulleted,
	FormatListNumbered,
	FormatQuote,
	FormatUnderlined,
	Highlight as HighlightIcon,
	HorizontalRule as HRIcon,
	Image as ImageIcon,
	Link as LinkIcon,
	Redo,
	StrikethroughS,
	Undo,
} from '@mui/icons-material'
import {
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
	IconButton,
	Menu,
	MenuItem,
	TextField,
	Tooltip,
} from '@mui/material'
import { Color } from '@tiptap/extension-color'
import Highlight from '@tiptap/extension-highlight'
import Link from '@tiptap/extension-link'
import { TextStyle } from '@tiptap/extension-text-style'
import Underline from '@tiptap/extension-underline'
import { EditorContent, useEditor } from '@tiptap/react'
import StarterKit from '@tiptap/starter-kit'
import { useCallback, useEffect, useId, useState } from 'react'
import ImageUpload from './extensions/ImageUpload'
import './styles/tiptap.css'

const HEADING_LEVELS = [1, 2, 3, 4, 5, 6]

const TEXT_COLORS = [
	{ name: 'Black', value: '#000000' },
	{ name: 'Red', value: '#FF0000' },
	{ name: 'Blue', value: '#0000FF' },
	{ name: 'Green', value: '#008000' },
	{ name: 'Orange', value: '#FFA500' },
	{ name: 'Purple', value: '#800080' },
]

const HIGHLIGHT_COLORS = [
	{ name: 'Yellow', value: '#FFFF00' },
	{ name: 'Green', value: '#90EE90' },
	{ name: 'Blue', value: '#ADD8E6' },
	{ name: 'Pink', value: '#FFB6C1' },
	{ name: 'None', value: null },
]

const Toolbar = ({ editor, onImageClick }) => {
	const [headingAnchor, setHeadingAnchor] = useState(null)
	const [colorAnchor, setColorAnchor] = useState(null)
	const [highlightAnchor, setHighlightAnchor] = useState(null)
	const [linkDialogOpen, setLinkDialogOpen] = useState(false)
	const [linkUrl, setLinkUrl] = useState('')
	const [linkText, setLinkText] = useState('')
	const [hasSelection, setHasSelection] = useState(false)

	if (!editor) return null

	const handleLinkClick = () => {
		const previousUrl = editor.getAttributes('link').href
		const { from, to } = editor.state.selection
		const selection = from !== to

		setLinkUrl(previousUrl || '')
		setLinkText('')
		setHasSelection(selection)
		setLinkDialogOpen(true)
	}

	const handleLinkDialogClose = () => {
		setLinkDialogOpen(false)
		setLinkUrl('')
		setLinkText('')
	}

	const handleLinkDialogSubmit = () => {
		if (!linkUrl.trim()) {
			editor.chain().focus().extendMarkRange('link').unsetLink().run()
			handleLinkDialogClose()
			return
		}

		if (hasSelection) {
			editor.chain().focus().setLink({ href: linkUrl }).run()
		} else {
			editor
				.chain()
				.focus()
				.insertContent({
					type: 'text',
					text: linkText || linkUrl,
					marks: [{ type: 'link', attrs: { href: linkUrl } }],
				})
				.run()
		}

		handleLinkDialogClose()
	}

	const isHeadingActive = HEADING_LEVELS.some((level) => editor.isActive('heading', { level }))

	const buttons = [
		{
			icon: <Undo />,
			action: () => editor.chain().focus().undo().run(),
			disabled: !editor.can().undo(),
			label: 'Undo',
		},
		{
			icon: <Redo />,
			action: () => editor.chain().focus().redo().run(),
			disabled: !editor.can().redo(),
			label: 'Redo',
		},
		{ divider: true },
		{
			icon: <FormatBold />,
			action: () => editor.chain().focus().toggleBold().run(),
			active: editor.isActive('bold'),
			label: 'Bold',
		},
		{
			icon: <FormatItalic />,
			action: () => editor.chain().focus().toggleItalic().run(),
			active: editor.isActive('italic'),
			label: 'Italic',
		},
		{
			icon: <FormatUnderlined />,
			action: () => editor.chain().focus().toggleUnderline().run(),
			active: editor.isActive('underline'),
			label: 'Underline',
		},
		{
			icon: <StrikethroughS />,
			action: () => editor.chain().focus().toggleStrike().run(),
			active: editor.isActive('strike'),
			label: 'Strike',
		},
		{
			icon: <Code />,
			action: () => editor.chain().focus().toggleCode().run(),
			active: editor.isActive('code'),
			label: 'Inline Code',
		},
		{ divider: true },
		{
			icon: <span style={{ fontSize: '14px', fontWeight: 'bold' }}>H</span>,
			action: (e) => setHeadingAnchor(e.currentTarget),
			active: isHeadingActive,
			label: 'Headings',
		},
		{ divider: true },
		{
			icon: <FormatListBulleted />,
			action: () => editor.chain().focus().toggleBulletList().run(),
			active: editor.isActive('bulletList'),
			label: 'Bullet List',
		},
		{
			icon: <FormatListNumbered />,
			action: () => editor.chain().focus().toggleOrderedList().run(),
			active: editor.isActive('orderedList'),
			label: 'Numbered List',
		},
		{
			icon: <FormatQuote />,
			action: () => editor.chain().focus().toggleBlockquote().run(),
			active: editor.isActive('blockquote'),
			label: 'Blockquote',
		},
		{
			icon: <Code style={{ transform: 'scale(1.2)' }} />,
			action: () => editor.chain().focus().toggleCodeBlock().run(),
			active: editor.isActive('codeBlock'),
			label: 'Code Block',
		},
		{ divider: true },
		{
			icon: <FormatColorText />,
			action: (e) => setColorAnchor(e.currentTarget),
			label: 'Text Color',
		},
		{
			icon: <HighlightIcon />,
			action: (e) => setHighlightAnchor(e.currentTarget),
			label: 'Highlight',
		},
		{ divider: true },
		{
			icon: <LinkIcon />,
			action: handleLinkClick,
			active: editor.isActive('link'),
			label: 'Insert Link',
		},
		{
			icon: <ImageIcon />,
			action: onImageClick,
			label: 'Insert Image',
			disabled: !onImageClick,
		},
		{
			icon: <HRIcon />,
			action: () => editor.chain().focus().setHorizontalRule().run(),
			label: 'Horizontal Rule',
		},
	]

	const renderButton = (btn, idx) => {
		if (btn.divider) {
			return <Divider key={`div-${idx}`} orientation='vertical' flexItem />
		}

		return (
			<Tooltip key={idx} title={btn.label} arrow>
				<IconButton
					size='small'
					onClick={btn.action}
					disabled={btn.disabled}
					color={btn.active ? 'primary' : 'default'}
					sx={{
						borderRadius: 1,
						...(btn.active && { bgcolor: 'action.selected' }),
					}}
				>
					{btn.icon}
				</IconButton>
			</Tooltip>
		)
	}

	const handleHeadingSelect = (level) => {
		if (level === null) {
			editor.chain().focus().setParagraph().run()
		} else {
			editor.chain().focus().toggleHeading({ level }).run()
		}
		setHeadingAnchor(null)
	}

	const handleColorSelect = (color) => {
		if (color === null) {
			editor.chain().focus().unsetColor().run()
		} else {
			editor.chain().focus().setColor(color).run()
		}
		setColorAnchor(null)
	}

	const handleHighlightSelect = (color) => {
		if (color === null) {
			editor.chain().focus().unsetHighlight().run()
		} else {
			editor.chain().focus().setHighlight({ color }).run()
		}
		setHighlightAnchor(null)
	}

	return (
		<>
			<Box
				sx={{
					borderBottom: 1,
					borderColor: 'divider',
					p: 0.5,
					display: 'flex',
					gap: 0.5,
					flexWrap: 'wrap',
					bgcolor: 'background.paper',
				}}
			>
				{buttons.map(renderButton)}
			</Box>

			<Menu
				anchorEl={headingAnchor}
				open={Boolean(headingAnchor)}
				onClose={() => setHeadingAnchor(null)}
			>
				<MenuItem onClick={() => handleHeadingSelect(null)} selected={!editor.isActive('heading')}>
					Paragraph
				</MenuItem>
				{HEADING_LEVELS.map((level) => (
					<MenuItem
						key={level}
						onClick={() => handleHeadingSelect(level)}
						selected={editor.isActive('heading', { level })}
					>
						Heading {level}
					</MenuItem>
				))}
			</Menu>

			<Menu anchorEl={colorAnchor} open={Boolean(colorAnchor)} onClose={() => setColorAnchor(null)}>
				<MenuItem onClick={() => handleColorSelect(null)}>Default</MenuItem>
				{TEXT_COLORS.map((color) => (
					<MenuItem key={color.value} onClick={() => handleColorSelect(color.value)}>
						<Box
							sx={{
								width: 20,
								height: 20,
								bgcolor: color.value,
								border: 1,
								borderColor: 'divider',
								mr: 1,
							}}
						/>
						{color.name}
					</MenuItem>
				))}
			</Menu>

			<Menu
				anchorEl={highlightAnchor}
				open={Boolean(highlightAnchor)}
				onClose={() => setHighlightAnchor(null)}
			>
				{HIGHLIGHT_COLORS.map((color) => (
					<MenuItem key={color.value || 'none'} onClick={() => handleHighlightSelect(color.value)}>
						{color.value && (
							<Box
								sx={{
									width: 20,
									height: 20,
									bgcolor: color.value,
									border: 1,
									borderColor: 'divider',
									mr: 1,
								}}
							/>
						)}
						{color.name}
					</MenuItem>
				))}
			</Menu>

			<Dialog open={linkDialogOpen} onClose={handleLinkDialogClose} maxWidth='sm' fullWidth>
				<DialogTitle>Insert Link</DialogTitle>
				<DialogContent>
					<TextField
						autoFocus
						margin='dense'
						label='URL'
						type='url'
						fullWidth
						variant='outlined'
						value={linkUrl}
						onChange={(e) => setLinkUrl(e.target.value)}
						placeholder='https://example.com'
						onKeyPress={(e) => {
							if (e.key === 'Enter' && linkUrl.trim()) {
								e.preventDefault()
								if (!hasSelection && !linkText.trim()) return
								handleLinkDialogSubmit()
							}
						}}
						sx={{ mb: 2 }}
					/>
					{!hasSelection && (
						<TextField
							margin='dense'
							label='Link Text (optional)'
							type='text'
							fullWidth
							variant='outlined'
							value={linkText}
							onChange={(e) => setLinkText(e.target.value)}
							placeholder='Leave empty to use URL'
							onKeyPress={(e) => {
								if (e.key === 'Enter' && linkUrl.trim()) {
									e.preventDefault()
									handleLinkDialogSubmit()
								}
							}}
						/>
					)}
				</DialogContent>
				<DialogActions>
					<Button onClick={handleLinkDialogClose}>Cancel</Button>
					<Button onClick={handleLinkDialogSubmit} variant='contained' disabled={!linkUrl.trim()}>
						{linkUrl.trim() ? 'Insert' : 'Remove Link'}
					</Button>
				</DialogActions>
			</Dialog>
		</>
	)
}

const TiptapEditor = ({
	content,
	onChange,
	onImageUpload,
	placeholder = 'Start writing...',
	editable = true,
	error = false,
	minHeight = 400,
}) => {
	const editor = useEditor({
		extensions: [
			StarterKit.configure({
				heading: {
					levels: HEADING_LEVELS,
					HTMLAttributes: { class: 'tiptap-heading' },
				},
				link: false,
			}),
			Link.configure({
				openOnClick: true,
				HTMLAttributes: {
					class: 'tiptap-link',
					rel: 'noopener noreferrer',
				},
				validate: (href) => /^https?:\/\//.test(href) || href.startsWith('/'),
			}),
			TextStyle,
			Color,
			Underline,
			Highlight.configure({ multicolor: true }),
			ImageUpload.configure({ uploadFn: onImageUpload }),
		],
		content,
		editable,
		onUpdate: ({ editor }) => onChange?.(editor.getHTML()),
		editorProps: {
			attributes: { 'data-placeholder': placeholder },
		},
	})

	useEffect(() => {
		if (!editor || editor.getHTML() === content) return
		editor.commands.setContent(content || '')
	}, [content, editor])

	const handleImageUpload = useCallback(
		async (event) => {
			const file = event.target.files?.[0]
			if (!file || !editor) return

			if (!file.type.startsWith('image/')) {
				return
			}

			try {
				const fileName = await onImageUpload?.(file)
				if (fileName) {
					editor.chain().focus().setImage({ src: fileName }).run()
				}
			} catch (err) {}

			event.target.value = ''
		},
		[editor, onImageUpload]
	)

	const inputId = useId()
	const handleImageClick = () => document.getElementById(inputId)?.click()

	return (
		<Box>
			<Toolbar editor={editor} onImageClick={handleImageClick} />
			<Box
				sx={{
					border: 1,
					borderColor: error ? 'error.main' : 'divider',
					borderRadius: 1,
					'&:focus-within': {
						borderColor: error ? 'error.main' : 'primary.main',
					},
				}}
			>
				<EditorContent
					editor={editor}
					className='tiptap-content'
					style={{ minHeight: `${minHeight}px` }}
				/>
			</Box>
			<input
				type='file'
				accept='image/*'
				style={{ display: 'none' }}
				id={inputId}
				onChange={handleImageUpload}
			/>
		</Box>
	)
}

export default TiptapEditor

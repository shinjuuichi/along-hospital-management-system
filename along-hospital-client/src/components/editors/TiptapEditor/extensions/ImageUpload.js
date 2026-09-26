import { getImageFromCloud } from '@/utils/commons'
import Image from '@tiptap/extension-image'
import { Plugin, PluginKey } from '@tiptap/pm/state'

const stripBaseUrlFromPath = (src) => {
	if (!src) return null

	if (src.startsWith('http://') || src.startsWith('https://')) {
		try {
			const lastHttpIndex = Math.max(src.lastIndexOf('http://'), src.lastIndexOf('https://'))

			if (lastHttpIndex > 0) {
				src = src.substring(lastHttpIndex)
			}

			const url = new URL(src)
			const pathname = url.pathname
			return pathname.startsWith('/') ? pathname.substring(1) : pathname
		} catch (error) {
			return src
		}
	}

	return src
}

const handleImageUpload = async (file, view, uploadFn, pos) => {
	try {
		const fileName = await uploadFn(file)
		if (!fileName) return

		const { schema } = view.state
		const node = schema.nodes.image.create({ src: fileName })
		const transaction = view.state.tr.insert(pos || view.state.selection.from, node)
		view.dispatch(transaction)
	} catch (error) {
	}
}

const ImageUpload = Image.extend({
	name: 'imageUpload',

	addOptions() {
		return {
			...this.parent?.(),
			uploadFn: null,
		}
	},

	addAttributes() {
		return {
			...this.parent?.(),
			src: {
				default: null,
				parseHTML: (element) => stripBaseUrlFromPath(element.getAttribute('src')),
				renderHTML: (attributes) => (attributes.src ? { src: attributes.src } : {}),
			},
		}
	},

	addNodeView() {
		return ({ node }) => {
			const container = document.createElement('div')
			container.style.display = 'inline-block'

			const img = document.createElement('img')
			img.src = getImageFromCloud(node.attrs.src)
			img.alt = node.attrs.alt || ''
			img.title = node.attrs.title || ''
			img.style.maxWidth = '100%'
			img.style.height = 'auto'

			container.appendChild(img)

			return {
				dom: container,
				update: (updatedNode) => {
					if (updatedNode.type.name !== this.name) return false

					img.src = getImageFromCloud(updatedNode.attrs.src)
					img.alt = updatedNode.attrs.alt || ''
					img.title = updatedNode.attrs.title || ''
					return true
				},
			}
		}
	},

	addProseMirrorPlugins() {
		const { uploadFn } = this.options

		return [
			new Plugin({
				key: new PluginKey('imageUploadPlugin'),
				props: {
					handleDOMEvents: {
						paste(view, event) {
							const items = Array.from(event.clipboardData?.items || [])
							const imageItem = items.find((item) => item.type.indexOf('image') !== -1)

							if (imageItem) {
								event.preventDefault()
								const file = imageItem.getAsFile()
								if (file && uploadFn) {
									handleImageUpload(file, view, uploadFn)
								}
								return true
							}
							return false
						},

						drop(view, event) {
							const hasFiles = event.dataTransfer?.files?.length
							if (!hasFiles) return false

							const images = Array.from(event.dataTransfer.files).filter((file) =>
								/image/i.test(file.type)
							)

							if (images.length === 0) return false

							event.preventDefault()

							const coordinates = view.posAtCoords({
								left: event.clientX,
								top: event.clientY,
							})

							Promise.all(
								images.map((image) =>
									uploadFn
										? handleImageUpload(image, view, uploadFn, coordinates?.pos)
										: Promise.resolve()
								)
							).catch(() => {
							})

							return true
						},
					},
				},
			}),
		]
	},
})

export default ImageUpload

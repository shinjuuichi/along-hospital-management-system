# Along Hospital Client - AI Assistant Instructions

## Architecture Overview

React 19 + Vite hospital management SPA using Material-UI v7 and React Router v7.

### Tech Stack
- **UI**: Material-UI v7 (Grid v2), Emotion, Lexend font
- **State**: Redux Toolkit (3 slices: `auth`, `patient`, `management`) + React Context (`AuthProvider`, `ConfirmationProvider`)
- **API**: Axios (centralized in `configs/axiosConfig.js`) + SignalR for real-time
- **Editor**: Tiptap rich text editor
- **Notifications**: React Toastify

### Role-Based Architecture
12 user roles, each with a dedicated layout and route file:
- **Roles**: Manager, Doctor, Nurse, HR, Pharmacist, Accountant, Marketer, HeadNurse, Receptionist, HotlineAgent, InventoryClerk, Patient
- **Public**: `LayoutGuest` / `RouteGuest.jsx` for unauthenticated users
- **Authenticated**: `Layout<Role>.jsx` / `Route<Role>.jsx` per role, wrapped with `ProtectedRoute`
- **Role config**: `roleBasedConfig.js` maps roles to layouts (`getLayoutByRole`) and dashboards (`getReturnUrlByRole`)

### Key Config Files
- `configs/apiUrls.js` — API endpoints (static strings + functions for dynamic IDs: `ApiUrls.DOCTOR.MANAGEMENT.DETAIL(id)`)
- `configs/routeUrls.js` — Client route paths (e.g., `routeUrls.PATIENT.APPOINTMENT.CREATE`)
- `configs/enumConfig.js` — Shared enums
- `configs/axiosConfig.js` — Axios instance with auto Bearer token, 401 refresh, success toasts

## Development Patterns

### Import Convention
Always use `@/` alias for internal imports (configured in `vite.config.js`):
```jsx
import useAuth from '@/hooks/useAuth'
import { ApiUrls } from '@/configs/apiUrls'
```

### MUI Grid v2
Use the `size` prop (NOT `xs`):
```jsx
<Grid item size={6} />
<Grid item size={{ xs: 12, md: 6 }} />
```

### Component Structure
```
components/
├── basePages/       # Shared base page logic (manageAppointmentBasePage, etc.)
├── buttons/         # Custom button components
├── dialogs/commons/ # GenericFormDialog, MultipleSelectDialog, ConfirmationDialog
├── editors/         # Tiptap rich text editors
├── fieldRenderers/  # Dynamic field rendering system
├── generals/        # ActionMenu, ConfirmationButton, DetailCard, GenericDrawer,
│                    #   GenericPagination, GenericTabs, SearchBar
├── infoRows/        # Info display rows
├── layouts/         # Header, Footer, navigation
├── menus/           # Menu components
├── placeholders/    # Empty states (EmptyRow, etc.)
├── skeletons/       # Loading skeleton components
├── tables/          # GenericTable
└── textFields/      # Form input components
```

### Custom Hooks
Located in `src/hooks/`:
| Hook | Purpose |
|------|---------|
| `useAuth()` | Auth state: `{ auth, login(), logout(), initialized }` |
| `useFetch(url, params, deps, fetchOnMount)` | API GET: `{ loading, error, data, setData, fetch() }` |
| `useReduxStore({ selector, setStore })` | Redux + API combo: `{ loading, data, fetch(), resetStore() }` |
| `useAxiosSubmit({ method, onSuccess })` | Mutations: `{ loading, response, submit() }` |
| `useForm(initialValues)` | Form state: `{ values, handleChange, setField, validateAll() }` |
| `useTranslation()` | i18n: `{ language, setLanguage, t() }` |
| `useConfirm()` | Confirmation dialogs via `ConfirmationProvider` |

### Validation (`utils/validateUtil.js`)
```jsx
import { isRequired, isEmail, maxLen } from '@/utils/validateUtil'
// In field definitions:
{ key: 'email', title: 'Email', validate: [isRequired(), isEmail()] }
```

### Generic Components

#### GenericTable
```jsx
<GenericTable
  data={items}
  fields={[
    { key: 'id', title: 'ID', width: 10, sortable: true, fixedColumn: true },
    { key: 'name', title: 'Name', width: 30, sortable: true },
    { key: 'actions', title: 'Actions', render: (_, row) => <ActionButtons id={row.id} /> }
  ]}
  sort={sort} setSort={setSort} rowKey="id" loading={loading} stickyHeader
/>
```

#### GenericFormDialog (`components/dialogs/commons/GenericFormDialog.jsx`)
```jsx
<GenericFormDialog
  open={dialogOpen}
  onClose={() => setDialogOpen(false)}
  title="Add Item"
  fields={[
    { key: 'name', title: 'Name', validate: [maxLen(255)], required: true },
    { key: 'email', title: 'Email', type: 'email' },
    { key: 'role', title: 'Role', type: 'select', options: ['User', 'Admin'] }
  ]}
  initialValues={initialData}
  onSubmit={({ values, closeDialog }) => { /* ... */ }}
/>
```

### CRUD Page Pattern
```jsx
const MyPage = () => {
  const store = useReduxStore({ selector: s => s.patient.cart, setStore: setCartStore })
  const deleteItem = useAxiosSubmit({ method: 'DELETE', onSuccess: () => store.fetch() })

  const handleDelete = async (id) => {
    await deleteItem.submit({ overrideUrl: ApiUrls.CART.DELETE(id) })
  }
  // ...
}
```

### Theme System
- Dual themes: `hospitalLightTheme` / `hospitalDarkTheme` in `themeConfig.js`
- Extended palette: `softBg`, `softBorder`, `gradients`
- Font: Lexend (primary), Roboto (fallback)

### Environment Variables
Accessed via `getEnv()` from `utils/commons.js`:
```jsx
const baseUrl = getEnv('VITE_BASE_API_URL', 'https://localhost:5000/api/v1')
```
Docker: `env-config.sh` injects vars at runtime into `window.__ENV__`

## Translation System

### Enum & Style System

#### useEnum Pattern
Use `useEnum()` hook for all enum options. Never create local enum arrays.

```jsx
const _enum = useEnum()

// For select fields in forms
{ key: 'status', type: 'select', options: _enum.appointmentStatusOptions }
```

#### getEnumLabelByValue Pattern
Use `getEnumLabelByValue()` from `@/utils/handleStringUtil` to display enum labels in tables/chips:

```jsx
import { getEnumLabelByValue } from '@/utils/handleStringUtil'

// In table field render
render: (value) => getEnumLabelByValue(_enum.appointmentStatusOptions, value)
```

#### defaultStylesConfig Pattern
Use style functions from `@/configs/defaultStylesConfig` for consistent Chip/Badge colors:

```jsx
import { defaultAppointmentStatusStyle } from '@/configs/defaultStylesConfig'

// Status chip with custom style (using theme)
const styleForStatus = defaultAppointmentStatusStyle(theme, status)
<Chip
  label={getEnumLabelByValue(_enum.appointmentStatusOptions, status)}
  size='small'
  sx={{
    bgcolor: styleForStatus.bg,
    color: styleForStatus.color,
    border: `1px solid ${styleForStatus.border}`,
  }}
/>
```

### Translation System
- `useTranslation()` hook provides `t()` function
- Locale files in `src/locales/` (en.json, vi.json)
- Translation keys follow dot notation: `t('header.home')`
### Structure
Locale files live in `src/locales/{en,vi}/` as feature-scoped JSON files, auto-loaded and deep-merged by `locales/index.js`:
```
locales/en/
├── 0general/     # commons, header, sidebar, etc.
├── appointment/
├── guest/
├── medical_history/
├── staff/
├── user/
├── blog.json, cart.json, medicine.json, ...
```

### Usage Rules
- `t('feature.category.key')` — dot-notation keys, NO interpolation params
- All user-facing text **must** use translation keys. Never hardcode strings in components.
- Add keys to **both** `src/locales/en/` and `src/locales/vi/` directories.
- Reuse common texts via `commons.json` (buttons, placeholders). Feature-specific copy under its own namespace.
- Key naming convention: `title.*`, `text.*`, `button.*`, `placeholder.*`, `field.*`, `error.*`
- Typical examples:
  - `t('doctor.title.team')`, `t('doctor.text.no_doctors')`, `t('doctor.button.load_more')`
- Create locale keys **before** wiring UI. Avoid in-code string fallbacks.

## File Naming Conventions
- **Components/Pages**: PascalCase (`CartPage.jsx`, `GenericTable.jsx`)
- **Hooks**: camelCase with `use` prefix (`useAuth.js`, `useFetch.js`)
- **Utils/Configs**: camelCase (`formatDateUtil.js`, `axiosConfig.js`)
- **Layouts**: `Layout<Role>.jsx` — **Routes**: `Route<Role>.jsx`

## Build & Deploy

```bash
npm run dev     # Dev server (port 3000, HTTPS)
npm run build   # Production build
npm run lint    # ESLint check
npm run preview # Preview production build
```

**Docker**: Multi-stage build (Node Alpine → Nginx Alpine). Nginx serves SPA with `try_files $uri $uri/ /index.html` fallback. `env-config.js` is served with no-cache headers for runtime env injection.

- `src/App.jsx` - Provider setup and main routing
- `src/configs/themeConfig.js` - Material-UI theme customization
- `src/configs/defaultStylesConfig.js` - Chip/Badge color styles for enums
- `src/configs/enumConfig.js` - Enum constants
- `src/hooks/useEnum.js` - Enum options with translations
- `src/utils/handleStringUtil.js` - `getEnumLabelByValue` and string utilities
- `src/components/tables/GenericTable.jsx` - Complex table implementation
- `src/components/dialogs/GenericFormDialog.jsx` - Form dialog patterns
- `src/hooks/useReduxStore.js` - Redux + API integration pattern
- `src/routes/RouteGuest.jsx` & `src/routes/RoutePatient.jsx` - Route organization
## Key Files

- `src/App.jsx` — Provider hierarchy and top-level routing (14 role routes)
- `src/configs/roleBasedConfig.js` — Role → layout/dashboard mapping
- `src/configs/apiUrls.js` — All API endpoint definitions
- `src/configs/routeUrls.js` — All client route paths
- `src/components/tables/GenericTable.jsx` — Table implementation
- `src/components/dialogs/commons/GenericFormDialog.jsx` — Form dialog pattern
- `src/hooks/useReduxStore.js` — Redux + API integration hook
- `src/locales/index.js` — Dynamic locale loader

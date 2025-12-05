# Getting Started Checklist

Follow these steps to get your IronAmbit API integration up and running.

## ✅ Prerequisites

- [x] Astro 5.16.2 installed
- [x] React support enabled
- [x] Tailwind CSS configured
- [x] All dependencies installed

## 📋 Quick Start

### 1. Configure Environment

```bash
cd Client
cp .env.example .env
```

Edit `.env`:

```env
# For development with mock data
PUBLIC_API_URL=http://localhost:8080/api
# PUBLIC_USE_MOCK_API=true  # Optional: uncomment to use mock data
```

### 2. Verify Installation

```bash
npm install
```

All required dependencies are already in `package.json`:

- ✅ astro@^5.16.2
- ✅ @astrojs/react@^4.4.2
- ✅ react@^19.2.0
- ✅ react-dom@^19.2.0
- ✅ tailwindcss@^4.1.17

### 3. Start Development Server

```bash
npm run dev
```

Expected output:

```
🚀 astro dev
  ┃ Local    http://localhost:4321/
  ┃ Network  use --host to expose
```

### 4. Test the Pages

Visit these URLs in your browser:

1. **Users Page**: http://localhost:4321/users
   - Should show user management interface
   - Table with pagination controls

2. **Exercises Page**: http://localhost:4321/exercises
   - Should show exercise library
   - Color-coded badges for categories

3. **Workouts Page**: http://localhost:4321/workouts
   - Should show workout tracking
   - Sets, reps, weight, duration columns

### 5. Expected Behavior

#### With Mock Data (Default)

✅ Pages load immediately  
✅ Sample data displays  
✅ Pagination works  
✅ Loading spinner shows briefly  
✅ No API errors

#### Without Backend API

⚠️ "Error Loading Data" message displays  
⚠️ "Try Again" button appears  
⚠️ Network error or connection refused

**This is expected!** You'll need to either:

- Use mock data (see step 7), or
- Connect to a real backend (see step 8)

### 6. Verify Components

Check that all files were created:

```bash
# Types
ls src/types/
# Expected: index.ts

# Utilities
ls src/utils/
# Expected: api.ts, mockApi.ts

# Components
ls src/components/
# Expected: DataTable.tsx, UsersTable.tsx, ExercisesTable.tsx,
#           WorkoutsTable.tsx, DemoUsersTable.tsx

# Pages
ls src/pages/
# Expected: users.astro, exercises.astro, workouts.astro
```

### 7. Test with Mock Data (No Backend Needed)

To test UI without a backend, update each table component:

**Example: `src/components/UsersTable.tsx`**

```typescript
// Change this line:
import { apiService } from '../utils/api';

// To this:
import { mockApiService } from '../utils/mockApi';

// Change this function:
const fetchUsers = async (page: number, pageSize: number) => {
  const response = await apiService.getPaginated<User>('/users', page, pageSize);
  // ...
};

// To this:
const fetchUsers = async (page: number, pageSize: number) => {
  const response = await mockApiService.getPaginatedUsers(page, pageSize);
  return {
    data: response.data,
    total: response.total,
  };
};
```

Repeat for `ExercisesTable.tsx` and `WorkoutsTable.tsx`.

### 8. Connect to Real Backend

#### 8.1 Ensure Backend is Running

Your backend should implement these endpoints:

- `GET /api/users?page=1&pageSize=10`
- `GET /api/exercises?page=1&pageSize=10`
- `GET /api/workouts?page=1&pageSize=10`

See `API_ENDPOINTS.md` for complete specifications.

#### 8.2 Update API URL

Edit `.env`:

```env
PUBLIC_API_URL=http://localhost:8080/api
```

Replace `8080` with your backend's port.

#### 8.3 Enable CORS on Backend

Your backend must allow requests from `http://localhost:4321`.

Example for Node.js/Express:

```javascript
app.use(
  cors({
    origin: 'http://localhost:4321',
    credentials: true,
  })
);
```

#### 8.4 Test Connection

```bash
# Test if backend is accessible
curl http://localhost:8080/api/users?page=1&pageSize=10
```

Should return JSON with paginated data.

### 9. Verify Features

#### ✅ Pagination

- [ ] Can change rows per page (5, 10, 25, 50, 100)
- [ ] Can navigate between pages
- [ ] Shows correct page numbers
- [ ] Shows "Showing X-Y of Z results"

#### ✅ Loading State

- [ ] Spinner appears when loading
- [ ] "Loading data..." message shows

#### ✅ Error Handling

- [ ] Error message displays when API fails
- [ ] Shows error details
- [ ] "Try Again" button works

#### ✅ Navigation

- [ ] Header has links to Users, Exercises, Workouts
- [ ] Links navigate correctly
- [ ] Active page is accessible

#### ✅ Responsive Design

- [ ] Tables work on desktop
- [ ] Tables work on mobile
- [ ] Pagination adapts to screen size

### 10. Build for Production

```bash
npm run build
```

Expected output:

```
✓ 250 modules transformed.
✓ Completed in Xms.
```

Preview production build:

```bash
npm run preview
```

## 🐛 Troubleshooting

### Issue: "Cannot find module" errors

**Solution:**

```bash
rm -rf node_modules package-lock.json
npm install
```

### Issue: CORS errors in browser console

**Solution:**

- Add CORS headers to your backend
- Ensure backend allows `http://localhost:4321`

### Issue: "Failed to fetch" errors

**Check:**

1. Is backend running?
2. Is `PUBLIC_API_URL` correct in `.env`?
3. Are backend endpoints accessible?

**Test:**

```bash
curl -i http://localhost:8080/api/users?page=1&pageSize=10
```

### Issue: No data showing

**Check:**

1. Open browser DevTools → Network tab
2. Look for API requests
3. Check response status and data

**Temporary Solution:**
Use mock data (see step 7)

### Issue: TypeScript errors

**Solution:**

```bash
npm run astro check
```

If errors persist, check that all files were created correctly.

## 📚 Documentation

| Document                    | Purpose                     |
| --------------------------- | --------------------------- |
| `API_INTEGRATION.md`        | Complete integration guide  |
| `API_ENDPOINTS.md`          | API specification reference |
| `IMPLEMENTATION_SUMMARY.md` | Architecture overview       |
| `MOCK_API_GUIDE.md`         | Mock vs real API switching  |
| `GETTING_STARTED.md`        | This file                   |

## 🎯 Next Steps

1. ✅ Complete this checklist
2. 📖 Read `API_INTEGRATION.md` for detailed usage
3. 🛠️ Develop your backend API
4. 🎨 Customize the UI to match your design
5. ➕ Add create/edit/delete functionality
6. 🔐 Implement authentication
7. 📊 Add charts and visualizations

## 🆘 Need Help?

1. Check the documentation files in `Client/` directory
2. Review the mock data in `src/utils/mockApi.ts`
3. Examine component examples in `src/components/`
4. Test with mock data first before connecting to real API

## ✅ Success Indicators

You're ready to proceed when:

- [x] All three pages load without errors
- [x] Pagination controls are visible and functional
- [x] Data displays in tables (mock or real)
- [x] Loading states work correctly
- [x] Error handling displays properly
- [x] Navigation links work

## 🚀 You're All Set!

Your IronAmbit API integration is ready. Start developing your backend or customize the frontend components as needed.

**Happy coding!** 💪

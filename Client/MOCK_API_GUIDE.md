# Switching Between Mock and Real API

This guide shows how to easily switch between mock data (for development/testing) and real API calls.

## Option 1: Environment Variable Toggle (Recommended)

### Setup

Add to `.env`:
```env
PUBLIC_API_URL=http://localhost:8080/api
PUBLIC_USE_MOCK_API=false  # Set to 'true' to use mock data
```

### Create API Wrapper

Create `src/utils/apiFactory.ts`:

```typescript
import { apiService } from './api';
import { mockApiService } from './mockApi';
import type { PaginatedResponse, User, Exercise, Workout } from '../types';

const useMockApi = import.meta.env.PUBLIC_USE_MOCK_API === 'true';

export const api = {
  async getPaginatedUsers(page: number, pageSize: number): Promise<PaginatedResponse<User>> {
    if (useMockApi) {
      return mockApiService.getPaginatedUsers(page, pageSize);
    }
    return apiService.getPaginated<User>('/users', page, pageSize);
  },

  async getPaginatedExercises(page: number, pageSize: number): Promise<PaginatedResponse<Exercise>> {
    if (useMockApi) {
      return mockApiService.getPaginatedExercises(page, pageSize);
    }
    return apiService.getPaginated<Exercise>('/exercises', page, pageSize);
  },

  async getPaginatedWorkouts(page: number, pageSize: number): Promise<PaginatedResponse<Workout>> {
    if (useMockApi) {
      return mockApiService.getPaginatedWorkouts(page, pageSize);
    }
    return apiService.getPaginated<Workout>('/workouts', page, pageSize);
  },
};
```

### Update Table Components

Update `UsersTable.tsx`:

```typescript
import DataTable, { type Column } from './DataTable';
import { api } from '../utils/apiFactory';  // Changed import
import type { User } from '../types';

export default function UsersTable() {
  const columns: Column<User>[] = [
    // ... column definitions
  ];

  const fetchUsers = async (page: number, pageSize: number) => {
    const response = await api.getPaginatedUsers(page, pageSize);  // Changed call
    return {
      data: response.data,
      total: response.total,
    };
  };

  return (
    <DataTable<User>
      columns={columns}
      fetchData={fetchUsers}
      initialPageSize={10}
      emptyMessage="No users found"
    />
  );
}
```

Repeat for `ExercisesTable.tsx` and `WorkoutsTable.tsx`.

### Usage

**Development with mock data:**
```env
PUBLIC_USE_MOCK_API=true
```

**Production with real API:**
```env
PUBLIC_USE_MOCK_API=false
PUBLIC_API_URL=https://api.ironambit.com/api
```

---

## Option 2: Manual Component Update

### Using Mock API

In `UsersTable.tsx`:

```typescript
import { mockApiService } from '../utils/mockApi';

const fetchUsers = async (page: number, pageSize: number) => {
  const response = await mockApiService.getPaginatedUsers(page, pageSize);
  return {
    data: response.data,
    total: response.total,
  };
};
```

### Using Real API

In `UsersTable.tsx`:

```typescript
import { apiService } from '../utils/api';

const fetchUsers = async (page: number, pageSize: number) => {
  const response = await apiService.getPaginated<User>('/users', page, pageSize);
  return {
    data: response.data,
    total: response.total,
  };
};
```

---

## Option 3: Feature Flag Pattern

### Setup

Create `src/config/features.ts`:

```typescript
export const features = {
  useMockApi: import.meta.env.DEV, // Auto-enable in dev mode
  // Or manually control:
  // useMockApi: import.meta.env.PUBLIC_USE_MOCK_API === 'true',
};
```

### Use in Components

```typescript
import { features } from '../config/features';
import { apiService } from '../utils/api';
import { mockApiService } from '../utils/mockApi';

const fetchUsers = async (page: number, pageSize: number) => {
  const response = features.useMockApi
    ? await mockApiService.getPaginatedUsers(page, pageSize)
    : await apiService.getPaginated<User>('/users', page, pageSize);
  
  return {
    data: response.data,
    total: response.total,
  };
};
```

---

## Option 4: Custom Hook (Advanced)

### Create Hook

Create `src/hooks/useApi.ts`:

```typescript
import { useState, useEffect } from 'react';
import { apiService } from '../utils/api';
import { mockApiService } from '../utils/mockApi';
import type { ApiError } from '../types';

const useMockApi = import.meta.env.PUBLIC_USE_MOCK_API === 'true';

export function useApi() {
  return {
    get: useMockApi ? mockApiService : apiService,
    isMockMode: useMockApi,
  };
}
```

### Use in Components

```typescript
import { useApi } from '../hooks/useApi';

export default function UsersTable() {
  const { get, isMockMode } = useApi();
  
  const fetchUsers = async (page: number, pageSize: number) => {
    const response = isMockMode
      ? await get.getPaginatedUsers(page, pageSize)
      : await get.getPaginated<User>('/users', page, pageSize);
    
    return {
      data: response.data,
      total: response.total,
    };
  };

  // Show indicator if using mock data
  return (
    <div>
      {isMockMode && (
        <div className="mb-4 p-2 bg-yellow-100 text-yellow-800 rounded">
          ⚠️ Using Mock Data
        </div>
      )}
      <DataTable<User>
        columns={columns}
        fetchData={fetchUsers}
        initialPageSize={10}
        emptyMessage="No users found"
      />
    </div>
  );
}
```

---

## Testing Different Scenarios

### 1. Test with Mock Data
```bash
# In .env
PUBLIC_USE_MOCK_API=true

npm run dev
```

### 2. Test with Real API (Local)
```bash
# In .env
PUBLIC_USE_MOCK_API=false
PUBLIC_API_URL=http://localhost:8080/api

# Start your backend server first
npm run dev
```

### 3. Test with Real API (Remote)
```bash
# In .env
PUBLIC_USE_MOCK_API=false
PUBLIC_API_URL=https://api.example.com/api

npm run dev
```

### 4. Test Error Handling

Temporarily modify mock API to simulate errors:

```typescript
// In mockApi.ts
async getPaginatedUsers(page: number, pageSize: number) {
  await new Promise((resolve) => setTimeout(resolve, 500));
  
  // Simulate error
  throw {
    message: 'Failed to fetch users',
    status: 500,
  };
}
```

---

## Recommendations

1. **Development**: Use mock API to develop UI independently
2. **Integration Testing**: Switch to real API to test integration
3. **Production**: Always use real API
4. **Demo/Presentation**: Use mock API for reliable demos

## Environment File Examples

### `.env.development`
```env
PUBLIC_API_URL=http://localhost:8080/api
PUBLIC_USE_MOCK_API=true
```

### `.env.production`
```env
PUBLIC_API_URL=https://api.ironambit.com/api
PUBLIC_USE_MOCK_API=false
```

### `.env.staging`
```env
PUBLIC_API_URL=https://staging-api.ironambit.com/api
PUBLIC_USE_MOCK_API=false
```

---

## Debugging Tips

### Check which API is being used

Add console log:

```typescript
const fetchUsers = async (page: number, pageSize: number) => {
  console.log('Using mock API:', import.meta.env.PUBLIC_USE_MOCK_API === 'true');
  // ... rest of code
};
```

### Add visual indicator

```typescript
export default function UsersTable() {
  const isMockMode = import.meta.env.PUBLIC_USE_MOCK_API === 'true';
  
  return (
    <div>
      {isMockMode && (
        <div className="fixed top-4 right-4 bg-yellow-500 text-white px-4 py-2 rounded-lg shadow-lg z-50">
          🧪 Mock Data Mode
        </div>
      )}
      {/* ... rest of component */}
    </div>
  );
}
```

---

**Choose the option that best fits your workflow!**

Option 1 (Environment Variable Toggle) is recommended for most use cases.

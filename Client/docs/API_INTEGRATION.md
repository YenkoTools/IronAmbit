# API Integration Guide

This guide explains how to use the REST API integration in IronAmbit.

## Overview

The application includes three main management pages that interact with a RESTful API:
- **Users** (`/users`) - Manage system users
- **Exercises** (`/exercises`) - Manage exercises and activities
- **Workouts** (`/workouts`) - Manage user workouts by day

## Architecture

### Components

1. **Type Definitions** (`src/types/index.ts`)
   - TypeScript interfaces for User, Exercise, Workout
   - API response types with pagination support
   - Error handling types

2. **API Service** (`src/utils/api.ts`)
   - Centralized API client with error handling
   - Support for GET, POST, PUT, DELETE operations
   - Built-in pagination support
   - Configurable base URL via environment variables

3. **DataTable Component** (`src/components/DataTable.tsx`)
   - Reusable React component with pagination
   - User-controlled page size (5, 10, 25, 50, 100 rows)
   - Loading states and error handling
   - Responsive design with Tailwind CSS

4. **Table Components**
   - `UsersTable.tsx` - User data table
   - `ExercisesTable.tsx` - Exercise data table
   - `WorkoutsTable.tsx` - Workout data table

## Configuration

### 1. Set API Base URL

Create a `.env` file in the `Client` directory:

```bash
cp .env.example .env
```

Edit `.env` and set your API URL:

```env
PUBLIC_API_URL=http://localhost:8080/api
```

### 2. Expected API Endpoints

Your backend should implement these endpoints:

#### Users
- `GET /api/users?page=1&pageSize=10`

#### Exercises
- `GET /api/exercises?page=1&pageSize=10`

#### Workouts
- `GET /api/workouts?page=1&pageSize=10`

### 3. Expected Response Format

All endpoints should return paginated data in this format:

```json
{
  "data": [
    // Array of items (users, exercises, or workouts)
  ],
  "total": 100,
  "page": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

#### Example: User Object

```json
{
  "id": 1,
  "username": "john_doe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-20T14:45:00Z"
}
```

#### Example: Exercise Object

```json
{
  "id": 1,
  "name": "Bench Press",
  "description": "Compound chest exercise",
  "category": "Strength",
  "muscleGroup": "Chest",
  "equipment": "Barbell",
  "difficulty": "intermediate",
  "createdAt": "2024-01-15T10:30:00Z"
}
```

#### Example: Workout Object

```json
{
  "id": 1,
  "userId": 5,
  "userName": "John Doe",
  "date": "2024-01-20",
  "exerciseId": 3,
  "exerciseName": "Bench Press",
  "sets": 3,
  "reps": 10,
  "weight": 185,
  "duration": 30,
  "notes": "Felt strong today",
  "createdAt": "2024-01-20T14:45:00Z"
}
```

### 4. Error Handling

The API service handles errors automatically:

```json
{
  "message": "Error description",
  "status": 404,
  "details": {}
}
```

Errors are displayed in the UI with a retry button.

## Features

### Pagination
- User can select rows per page: 5, 10, 25, 50, or 100
- Page navigation with Previous/Next buttons
- Page numbers with ellipsis for large datasets
- Shows current range (e.g., "Showing 1-10 of 100 results")

### Loading States
- Spinner animation while fetching data
- "Loading data..." message

### Error Handling
- Error message display with status code
- "Try Again" button to retry failed requests
- Network error detection

### Responsive Design
- Mobile-friendly table layout
- Responsive pagination controls
- Tailwind CSS styling

## Development

### Running the App

```bash
cd Client
npm install
npm run dev
```

Navigate to:
- http://localhost:4321/users
- http://localhost:4321/exercises
- http://localhost:4321/workouts

### Testing Without Backend

To test the UI without a backend, you can:

1. Use a mock API service like [JSON Server](https://github.com/typicode/json-server)
2. Use API mocking tools like [MSW](https://mswjs.io/)
3. Modify the API service to return mock data temporarily

### Customization

#### Modify Column Definitions

Edit the respective table components to customize columns:

```typescript
const columns: Column<User>[] = [
  {
    key: 'id',
    header: 'ID',
    render: (value) => <span>#{value}</span>,
  },
  // Add more columns...
];
```

#### Change Default Page Size

In each table component, modify `initialPageSize`:

```typescript
<DataTable
  columns={columns}
  fetchData={fetchData}
  initialPageSize={25} // Default: 10
/>
```

#### Add Custom Page Size Options

```typescript
<DataTable
  columns={columns}
  fetchData={fetchData}
  pageSizeOptions={[10, 20, 50, 100, 200]}
/>
```

## Type Safety

All API interactions are fully typed with TypeScript:

```typescript
import type { User, Exercise, Workout } from '../types';
```

The API service ensures type safety for all requests and responses.

## Next Steps

1. Implement your RESTful API backend
2. Ensure API responses match the expected format
3. Set the `PUBLIC_API_URL` environment variable
4. Test the integration with real data
5. Customize table columns and styling as needed
6. Add filtering, sorting, and search capabilities if desired

## Additional Resources

- [Astro Documentation](https://docs.astro.build/)
- [React Documentation](https://react.dev/)
- [Tailwind CSS](https://tailwindcss.com/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)

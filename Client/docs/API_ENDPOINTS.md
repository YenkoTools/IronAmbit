# Quick Reference - REST API Endpoints

## Base URL Configuration

Set in `.env` file:
```env
PUBLIC_API_URL=http://localhost:8080/api
```

## Required Endpoints

### Users Management

#### List Users (Paginated)
```http
GET /api/users?page=1&pageSize=10
```

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "username": "john_doe",
      "email": "john@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "createdAt": "2024-01-15T10:30:00Z",
      "updatedAt": "2024-01-20T14:45:00Z"
    }
  ],
  "total": 100,
  "page": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

### Exercise Management

#### List Exercises (Paginated)
```http
GET /api/exercises?page=1&pageSize=10
```

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "name": "Bench Press",
      "description": "Compound chest exercise",
      "category": "Strength",
      "muscleGroup": "Chest",
      "equipment": "Barbell",
      "difficulty": "intermediate",
      "createdAt": "2024-01-10T08:00:00Z",
      "updatedAt": "2024-01-15T12:00:00Z"
    }
  ],
  "total": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

**Difficulty Values:**
- `"beginner"`
- `"intermediate"`
- `"advanced"`

### Workout Management

#### List Workouts (Paginated)
```http
GET /api/workouts?page=1&pageSize=10
```

**Response:**
```json
{
  "data": [
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
      "createdAt": "2024-01-20T14:45:00Z",
      "updatedAt": "2024-01-20T15:00:00Z"
    }
  ],
  "total": 200,
  "page": 1,
  "pageSize": 10,
  "totalPages": 20
}
```

**Optional Fields:**
- `userName` - If not provided, UI shows "User #{userId}"
- `exerciseName` - If not provided, UI shows "Exercise #{exerciseId}"
- `weight` - Can be null/undefined for bodyweight exercises
- `duration` - Can be null/undefined
- `notes` - Can be null/undefined
- `updatedAt` - Can be null/undefined

## Error Response Format

When an error occurs, return:

```json
{
  "message": "Error description",
  "status": 404,
  "details": {
    "field": "Additional error details"
  }
}
```

**Common HTTP Status Codes:**
- `200` - Success
- `400` - Bad Request (invalid parameters)
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `500` - Internal Server Error

## Query Parameters

### Pagination (All Endpoints)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `page` | integer | Yes | Page number (1-indexed) |
| `pageSize` | integer | Yes | Items per page (5, 10, 25, 50, 100) |

### Example Queries

```http
# First page with 10 items
GET /api/users?page=1&pageSize=10

# Third page with 25 items
GET /api/exercises?page=3&pageSize=25

# Large page with 100 items
GET /api/workouts?page=1&pageSize=100
```

## Testing with cURL

### Get Users
```bash
curl -X GET "http://localhost:8080/api/users?page=1&pageSize=10" \
  -H "Content-Type: application/json"
```

### Get Exercises
```bash
curl -X GET "http://localhost:8080/api/exercises?page=1&pageSize=10" \
  -H "Content-Type: application/json"
```

### Get Workouts
```bash
curl -X GET "http://localhost:8080/api/workouts?page=1&pageSize=10" \
  -H "Content-Type: application/json"
```

## CORS Configuration

Your backend must allow requests from the Astro development server:

```javascript
// Example: Node.js/Express
app.use(cors({
  origin: 'http://localhost:4321', // Astro dev server
  credentials: true
}));
```

## Headers

### Required Request Headers
```
Content-Type: application/json
```

### Optional Request Headers (for future auth)
```
Authorization: Bearer <token>
```

## Field Validation

### Users
- `username`: 3-30 characters, alphanumeric + underscore
- `email`: Valid email format
- `firstName`, `lastName`: 1-50 characters

### Exercises
- `name`: 1-100 characters, required
- `description`: 1-500 characters
- `category`: Non-empty string
- `muscleGroup`: Non-empty string
- `difficulty`: One of: "beginner", "intermediate", "advanced"

### Workouts
- `userId`: Valid user ID
- `exerciseId`: Valid exercise ID
- `date`: ISO date format (YYYY-MM-DD)
- `sets`: Positive integer
- `reps`: Positive integer
- `weight`: Positive number (optional)
- `duration`: Positive integer in minutes (optional)

## Performance Recommendations

- Implement database indexing on frequently queried fields
- Use pagination to limit response size
- Cache frequently accessed data
- Add query optimization for large datasets
- Consider implementing search/filter parameters for future features

## Future Extension Ideas

Add these query parameters to enhance functionality:

```http
# Search
GET /api/users?page=1&pageSize=10&search=john

# Filtering
GET /api/exercises?page=1&pageSize=10&category=Strength&difficulty=intermediate

# Sorting
GET /api/workouts?page=1&pageSize=10&sortBy=date&order=desc

# Date range
GET /api/workouts?page=1&pageSize=10&startDate=2024-01-01&endDate=2024-01-31
```

---

**Need Help?**
- See `API_INTEGRATION.md` for detailed implementation guide
- Check `IMPLEMENTATION_SUMMARY.md` for architecture overview
- Review `src/utils/mockApi.ts` for example data structures

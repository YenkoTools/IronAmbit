/**
 * Mock API Service for Development and Testing
 * 
 * This file provides mock data responses that match the expected API format.
 * Useful for development when the backend API is not yet available.
 * 
 * To use: Import and replace the real apiService in your table components temporarily.
 */

import type { User, Exercise, Workout, PaginatedResponse } from '../types';

// Mock Users Data
const mockUsers: User[] = [
  {
    id: 1,
    username: 'john_doe',
    email: 'john.doe@example.com',
    firstName: 'John',
    lastName: 'Doe',
    createdAt: '2024-01-15T10:30:00Z',
    updatedAt: '2024-01-20T14:45:00Z',
  },
  {
    id: 2,
    username: 'jane_smith',
    email: 'jane.smith@example.com',
    firstName: 'Jane',
    lastName: 'Smith',
    createdAt: '2024-01-16T09:15:00Z',
  },
  {
    id: 3,
    username: 'bob_wilson',
    email: 'bob.wilson@example.com',
    firstName: 'Bob',
    lastName: 'Wilson',
    createdAt: '2024-01-17T11:20:00Z',
  },
  {
    id: 4,
    username: 'alice_johnson',
    email: 'alice.johnson@example.com',
    firstName: 'Alice',
    lastName: 'Johnson',
    createdAt: '2024-01-18T13:45:00Z',
  },
  {
    id: 5,
    username: 'charlie_brown',
    email: 'charlie.brown@example.com',
    firstName: 'Charlie',
    lastName: 'Brown',
    createdAt: '2024-01-19T15:30:00Z',
  },
];

// Mock Exercises Data
const mockExercises: Exercise[] = [
  {
    id: 1,
    name: 'Bench Press',
    description: 'Compound chest exercise using a barbell',
    category: 'Strength',
    muscleGroup: 'Chest',
    equipment: 'Barbell',
    difficulty: 'intermediate',
    createdAt: '2024-01-10T08:00:00Z',
  },
  {
    id: 2,
    name: 'Squat',
    description: 'Fundamental lower body compound movement',
    category: 'Strength',
    muscleGroup: 'Legs',
    equipment: 'Barbell',
    difficulty: 'intermediate',
    createdAt: '2024-01-10T08:05:00Z',
  },
  {
    id: 3,
    name: 'Deadlift',
    description: 'Full body compound exercise',
    category: 'Strength',
    muscleGroup: 'Back',
    equipment: 'Barbell',
    difficulty: 'advanced',
    createdAt: '2024-01-10T08:10:00Z',
  },
  {
    id: 4,
    name: 'Pull-ups',
    description: 'Bodyweight back exercise',
    category: 'Strength',
    muscleGroup: 'Back',
    equipment: 'Pull-up Bar',
    difficulty: 'intermediate',
    createdAt: '2024-01-10T08:15:00Z',
  },
  {
    id: 5,
    name: 'Push-ups',
    description: 'Classic bodyweight chest exercise',
    category: 'Strength',
    muscleGroup: 'Chest',
    difficulty: 'beginner',
    createdAt: '2024-01-10T08:20:00Z',
  },
  {
    id: 6,
    name: 'Running',
    description: 'Cardiovascular endurance exercise',
    category: 'Cardio',
    muscleGroup: 'Full Body',
    difficulty: 'beginner',
    createdAt: '2024-01-10T08:25:00Z',
  },
];

// Mock Workouts Data
const mockWorkouts: Workout[] = [
  {
    id: 1,
    userId: 1,
    userName: 'John Doe',
    date: '2024-01-20',
    exerciseId: 1,
    exerciseName: 'Bench Press',
    sets: 3,
    reps: 10,
    weight: 185,
    duration: 30,
    notes: 'Felt strong today',
    createdAt: '2024-01-20T14:45:00Z',
  },
  {
    id: 2,
    userId: 1,
    userName: 'John Doe',
    date: '2024-01-20',
    exerciseId: 2,
    exerciseName: 'Squat',
    sets: 4,
    reps: 8,
    weight: 225,
    duration: 35,
    createdAt: '2024-01-20T15:30:00Z',
  },
  {
    id: 3,
    userId: 2,
    userName: 'Jane Smith',
    date: '2024-01-20',
    exerciseId: 5,
    exerciseName: 'Push-ups',
    sets: 3,
    reps: 15,
    duration: 10,
    notes: 'Great form',
    createdAt: '2024-01-20T16:00:00Z',
  },
  {
    id: 4,
    userId: 3,
    userName: 'Bob Wilson',
    date: '2024-01-21',
    exerciseId: 3,
    exerciseName: 'Deadlift',
    sets: 5,
    reps: 5,
    weight: 315,
    duration: 40,
    createdAt: '2024-01-21T10:15:00Z',
  },
  {
    id: 5,
    userId: 2,
    userName: 'Jane Smith',
    date: '2024-01-21',
    exerciseId: 6,
    exerciseName: 'Running',
    sets: 1,
    reps: 1,
    duration: 45,
    notes: '5K run',
    createdAt: '2024-01-21T07:30:00Z',
  },
];

/**
 * Mock API Service with simulated delays
 */
export const mockApiService = {
  /**
   * Get paginated users
   */
  async getPaginatedUsers(page: number, pageSize: number): Promise<PaginatedResponse<User>> {
    // Simulate network delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    const paginatedData = mockUsers.slice(start, end);

    return {
      data: paginatedData,
      total: mockUsers.length,
      page,
      pageSize,
      totalPages: Math.ceil(mockUsers.length / pageSize),
    };
  },

  /**
   * Get paginated exercises
   */
  async getPaginatedExercises(page: number, pageSize: number): Promise<PaginatedResponse<Exercise>> {
    // Simulate network delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    const paginatedData = mockExercises.slice(start, end);

    return {
      data: paginatedData,
      total: mockExercises.length,
      page,
      pageSize,
      totalPages: Math.ceil(mockExercises.length / pageSize),
    };
  },

  /**
   * Get paginated workouts
   */
  async getPaginatedWorkouts(page: number, pageSize: number): Promise<PaginatedResponse<Workout>> {
    // Simulate network delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    const paginatedData = mockWorkouts.slice(start, end);

    return {
      data: paginatedData,
      total: mockWorkouts.length,
      page,
      pageSize,
      totalPages: Math.ceil(mockWorkouts.length / pageSize),
    };
  },
};

/**
 * Example usage in a table component:
 * 
 * import { mockApiService } from '../utils/mockApi';
 * 
 * const fetchUsers = async (page: number, pageSize: number) => {
 *   const response = await mockApiService.getPaginatedUsers(page, pageSize);
 *   return {
 *     data: response.data,
 *     total: response.total,
 *   };
 * };
 */

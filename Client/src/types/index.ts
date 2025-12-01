// Common types for the IronAmbit application

export interface User {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  createdAt: string;
  updatedAt?: string;
  [key: string]: unknown;
}

export interface Exercise {
  id: number;
  name: string;
  description: string;
  category: string;
  muscleGroup: string;
  equipment?: string;
  difficulty?: 'beginner' | 'intermediate' | 'advanced';
  createdAt: string;
  updatedAt?: string;
  [key: string]: unknown;
}

export interface Workout {
  id: number;
  userId: number;
  userName?: string;
  date: string;
  exerciseId: number;
  exerciseName?: string;
  sets: number;
  reps: number;
  weight?: number;
  duration?: number;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
  [key: string]: unknown;
}

export interface PaginatedResponse<T> {
  data: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface ApiError {
  message: string;
  status?: number;
  details?: unknown;
}

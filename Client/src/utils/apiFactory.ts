/**
 * API Factory - Switches between mock and real API based on environment configuration
 *
 * Set PUBLIC_USE_MOCK_API=true in .env to use mock data
 * Set PUBLIC_USE_MOCK_API=false in .env to use real API
 */

import { apiService } from './api';
import { mockApiService } from './mockApi';
import type { PaginatedResponse, User, Exercise, Workout } from '../types';

const useMockApi = import.meta.env.PUBLIC_USE_MOCK_API === 'true';

console.log(`API Mode: ${useMockApi ? 'Mock API' : 'Real API'}`);

/**
 * Unified API interface that switches between mock and real API
 */
export const api = {
  /**
   * Get paginated users
   */
  async getPaginatedUsers(page: number, pageSize: number): Promise<PaginatedResponse<User>> {
    if (useMockApi) {
      return mockApiService.getPaginatedUsers(page, pageSize);
    }
    return apiService.getPaginated<User>('/users', page, pageSize);
  },

  /**
   * Get paginated exercises
   */
  async getPaginatedExercises(
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<Exercise>> {
    if (useMockApi) {
      return mockApiService.getPaginatedExercises(page, pageSize);
    }
    return apiService.getPaginated<Exercise>('/exercises', page, pageSize);
  },

  /**
   * Get paginated workouts
   */
  async getPaginatedWorkouts(page: number, pageSize: number): Promise<PaginatedResponse<Workout>> {
    if (useMockApi) {
      return mockApiService.getPaginatedWorkouts(page, pageSize);
    }
    return apiService.getPaginated<Workout>('/workouts', page, pageSize);
  },
};

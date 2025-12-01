import type { PaginatedResponse, ApiError } from '../types';

/**
 * Configuration for API requests
 */
export interface ApiConfig {
  baseUrl?: string;
  headers?: HeadersInit;
}

/**
 * API service class for handling RESTful API calls
 */
class ApiService {
  private baseUrl: string;
  private defaultHeaders: HeadersInit;

  constructor(config: ApiConfig = {}) {
    // Default to environment variable or localhost
    this.baseUrl = config.baseUrl || import.meta.env.PUBLIC_API_URL || 'http://localhost:3000/api';
    this.defaultHeaders = {
      'Content-Type': 'application/json',
      ...config.headers,
    };
  }

  /**
   * Handle API responses and errors
   */
  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const error: ApiError = {
        message: `API Error: ${response.statusText}`,
        status: response.status,
      };

      try {
        const errorData = await response.json();
        error.message = errorData.message || error.message;
        error.details = errorData;
      } catch {
        // If response is not JSON, keep the default error message
      }

      throw error;
    }

    return response.json();
  }

  /**
   * Generic GET request
   */
  async get<T>(endpoint: string, params?: Record<string, string | number>): Promise<T> {
    const url = new URL(`${this.baseUrl}${endpoint}`);
    
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        url.searchParams.append(key, String(value));
      });
    }

    try {
      const response = await fetch(url.toString(), {
        method: 'GET',
        headers: this.defaultHeaders,
      });

      return this.handleResponse<T>(response);
    } catch (error) {
      if (error && typeof error === 'object' && 'status' in error) {
        throw error;
      }
      throw {
        message: error instanceof Error ? error.message : 'Network error occurred',
        status: 0,
      } as ApiError;
    }
  }

  /**
   * Generic POST request
   */
  async post<T>(endpoint: string, data?: unknown): Promise<T> {
    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method: 'POST',
        headers: this.defaultHeaders,
        body: data ? JSON.stringify(data) : undefined,
      });

      return this.handleResponse<T>(response);
    } catch (error) {
      if (error && typeof error === 'object' && 'status' in error) {
        throw error;
      }
      throw {
        message: error instanceof Error ? error.message : 'Network error occurred',
        status: 0,
      } as ApiError;
    }
  }

  /**
   * Generic PUT request
   */
  async put<T>(endpoint: string, data?: unknown): Promise<T> {
    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method: 'PUT',
        headers: this.defaultHeaders,
        body: data ? JSON.stringify(data) : undefined,
      });

      return this.handleResponse<T>(response);
    } catch (error) {
      if (error && typeof error === 'object' && 'status' in error) {
        throw error;
      }
      throw {
        message: error instanceof Error ? error.message : 'Network error occurred',
        status: 0,
      } as ApiError;
    }
  }

  /**
   * Generic DELETE request
   */
  async delete<T>(endpoint: string): Promise<T> {
    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method: 'DELETE',
        headers: this.defaultHeaders,
      });

      return this.handleResponse<T>(response);
    } catch (error) {
      if (error && typeof error === 'object' && 'status' in error) {
        throw error;
      }
      throw {
        message: error instanceof Error ? error.message : 'Network error occurred',
        status: 0,
      } as ApiError;
    }
  }

  /**
   * Get paginated data
   */
  async getPaginated<T>(
    endpoint: string,
    page: number = 1,
    pageSize: number = 10,
    additionalParams?: Record<string, string | number>
  ): Promise<PaginatedResponse<T>> {
    const params = {
      page: String(page),
      pageSize: String(pageSize),
      ...additionalParams,
    };

    return this.get<PaginatedResponse<T>>(endpoint, params);
  }
}

// Export singleton instance
export const apiService = new ApiService();

// Export class for custom instances
export default ApiService;

import DataTable, { type Column } from './DataTable';
import { mockApiService } from '../utils/mockApi';
import type { User } from '../types';

/**
 * Demo Users Table using Mock API
 * 
 * This component uses mock data instead of real API calls.
 * Perfect for testing the UI without a backend.
 * 
 * To use the real API, replace mockApiService with apiService
 * from '../utils/api'
 */
export default function DemoUsersTable() {
  const columns: Column<User>[] = [
    {
      key: 'id',
      header: 'ID',
      render: (value) => <span className="font-mono text-gray-600">#{String(value)}</span>,
    },
    {
      key: 'username',
      header: 'Username',
      render: (value) => <span className="font-semibold">{String(value)}</span>,
    },
    {
      key: 'email',
      header: 'Email',
      render: (value) => (
        <a href={`mailto:${value}`} className="text-blue-600 hover:underline">
          {String(value)}
        </a>
      ),
    },
    {
      key: 'firstName',
      header: 'First Name',
    },
    {
      key: 'lastName',
      header: 'Last Name',
    },
    {
      key: 'createdAt',
      header: 'Created',
      render: (value) => {
        if (!value) return '-';
        const date = new Date(String(value));
        return date.toLocaleDateString('en-US', {
          year: 'numeric',
          month: 'short',
          day: 'numeric',
        });
      },
    },
  ];

  // Using mock API service
  const fetchUsers = async (page: number, pageSize: number) => {
    const response = await mockApiService.getPaginatedUsers(page, pageSize);
    return {
      data: response.data,
      total: response.total,
    };
  };

  return (
    <div>
      <div className="mb-4 p-4 bg-yellow-50 border border-yellow-200 rounded-lg">
        <div className="flex items-start">
          <svg className="h-5 w-5 text-yellow-600 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
            <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
          </svg>
          <div className="ml-3">
            <h3 className="text-sm font-medium text-yellow-800">Demo Mode</h3>
            <p className="mt-1 text-sm text-yellow-700">
              This table is using mock data. To connect to a real API, update the component to use apiService instead of mockApiService.
            </p>
          </div>
        </div>
      </div>
      
      <DataTable<User>
        columns={columns}
        fetchData={fetchUsers}
        initialPageSize={3}
        pageSizeOptions={[3, 5, 10]}
        emptyMessage="No users found"
      />
    </div>
  );
}

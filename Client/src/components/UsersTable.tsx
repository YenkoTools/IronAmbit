import DataTable, { type Column } from './DataTable';
import { api } from '../utils/apiFactory';
import type { User } from '../types';

/**
 * UsersTable component - displays users with pagination
 */
export default function UsersTable() {
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

  const fetchUsers = async (page: number, pageSize: number) => {
    const response = await api.getPaginatedUsers(page, pageSize);
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

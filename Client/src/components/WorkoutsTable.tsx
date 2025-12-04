import DataTable, { type Column } from './DataTable';
import { api } from '../utils/apiFactory';
import type { Workout } from '../types';

/**
 * WorkoutsTable component - displays workouts with pagination
 */
export default function WorkoutsTable() {
  const columns: Column<Workout>[] = [
    {
      key: 'id',
      header: 'ID',
      render: (value) => <span className="font-mono text-gray-600">#{String(value)}</span>,
    },
    {
      key: 'date',
      header: 'Date',
      render: (value) => {
        if (!value) return '-';
        const date = new Date(String(value));
        return (
          <span className="font-medium">
            {date.toLocaleDateString('en-US', {
              year: 'numeric',
              month: 'short',
              day: 'numeric',
            })}
          </span>
        );
      },
    },
    {
      key: 'userName',
      header: 'User',
      render: (value, item) => {
        if (value) return String(value);
        return <span className="text-gray-500">User #{item.userId}</span>;
      },
    },
    {
      key: 'exerciseName',
      header: 'Exercise',
      render: (value, item) => {
        if (value) {
          return <span className="font-semibold text-gray-900">{String(value)}</span>;
        }
        return <span className="text-gray-500">Exercise #{item.exerciseId}</span>;
      },
    },
    {
      key: 'sets',
      header: 'Sets',
      render: (value) => (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
          {String(value)} sets
        </span>
      ),
    },
    {
      key: 'reps',
      header: 'Reps',
      render: (value) => (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
          {String(value)} reps
        </span>
      ),
    },
    {
      key: 'weight',
      header: 'Weight',
      render: (value) => {
        if (!value) return '-';
        return <span className="font-medium">{String(value)} lbs</span>;
      },
    },
    {
      key: 'duration',
      header: 'Duration',
      render: (value) => {
        if (!value) return '-';
        return <span>{String(value)} min</span>;
      },
    },
    {
      key: 'notes',
      header: 'Notes',
      render: (value) => {
        if (!value) return '-';
        const notes = String(value);
        return (
          <span className="text-gray-600 text-sm" title={notes}>
            {notes.length > 30 ? `${notes.substring(0, 30)}...` : notes}
          </span>
        );
      },
    },
  ];

  const fetchWorkouts = async (page: number, pageSize: number) => {
    const response = await api.getPaginatedWorkouts(page, pageSize);
    return {
      data: response.data,
      total: response.total,
    };
  };

  return (
    <DataTable<Workout>
      columns={columns}
      fetchData={fetchWorkouts}
      initialPageSize={10}
      emptyMessage="No workouts found"
    />
  );
}

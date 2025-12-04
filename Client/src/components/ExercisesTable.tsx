import DataTable, { type Column } from './DataTable';
import { api } from '../utils/apiFactory';
import type { Exercise } from '../types';

/**
 * ExercisesTable component - displays exercises with pagination
 */
export default function ExercisesTable() {
  const columns: Column<Exercise>[] = [
    {
      key: 'id',
      header: 'ID',
      render: (value) => <span className="font-mono text-gray-600">#{String(value)}</span>,
    },
    {
      key: 'name',
      header: 'Exercise Name',
      render: (value) => <span className="font-semibold text-gray-900">{String(value)}</span>,
    },
    {
      key: 'category',
      header: 'Category',
      render: (value) => (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
          {String(value)}
        </span>
      ),
    },
    {
      key: 'muscleGroup',
      header: 'Muscle Group',
      render: (value) => (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
          {String(value)}
        </span>
      ),
    },
    {
      key: 'difficulty',
      header: 'Difficulty',
      render: (value) => {
        if (!value) return '-';
        const colors = {
          beginner: 'bg-gray-100 text-gray-800',
          intermediate: 'bg-yellow-100 text-yellow-800',
          advanced: 'bg-red-100 text-red-800',
        };
        const difficultyValue = String(value) as keyof typeof colors;
        return (
          <span
            className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${colors[difficultyValue] || 'bg-gray-100 text-gray-800'}`}
          >
            {String(value).charAt(0).toUpperCase() + String(value).slice(1)}
          </span>
        );
      },
    },
    {
      key: 'equipment',
      header: 'Equipment',
      render: (value) => value ? String(value) : 'Bodyweight',
    },
    {
      key: 'description',
      header: 'Description',
      render: (value) => {
        const desc = String(value || '');
        return (
          <span className="text-gray-600 text-sm" title={desc}>
            {desc.length > 50 ? `${desc.substring(0, 50)}...` : desc}
          </span>
        );
      },
    },
  ];

  const fetchExercises = async (page: number, pageSize: number) => {
    const response = await api.getPaginatedExercises(page, pageSize);
    return {
      data: response.data,
      total: response.total,
    };
  };

  return (
    <DataTable<Exercise>
      columns={columns}
      fetchData={fetchExercises}
      initialPageSize={10}
      emptyMessage="No exercises found"
    />
  );
}

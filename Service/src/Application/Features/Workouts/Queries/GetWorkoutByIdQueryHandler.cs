using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Features.Workouts.Queries;

public class GetWorkoutByIdQueryHandler : IQueryHandler<GetWorkoutByIdQuery, Result<Workout>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutByIdQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }
    
    public Task<Result<Workout>> Handle(GetWorkoutByIdQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
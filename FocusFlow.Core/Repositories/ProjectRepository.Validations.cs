using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public partial class ProjectRepository
    {
        private async Task<Result<bool>> IsValidCreateAsync(Project project)
        {
            var getByIdResult = await this.GetByIdAsync(project.Id);
            if (!getByIdResult.IsSuccess)
                return Result<bool>.From(getByIdResult);

            if (getByIdResult.Value != null)
                return _logger.FailureLog<bool>(LogLevel.Error, StatusCodes.Status400BadRequest,
                   $"Trying to insert duplicate with {nameof(project.Id)} : {project.Id}");

            var getFilteredResult = await this.GetFilteredAsync(new() { Name = project.Name });
            if (!getFilteredResult.IsSuccess)
                return Result<bool>.From(getFilteredResult);

            if (getFilteredResult.Value.Where(x => x.Id != x.Id).Any())
                return _logger.FailureLog<bool>(LogLevel.Error, StatusCodes.Status400BadRequest,
                   $"Trying to insert duplicate with {nameof(project.Name)} : {project.Name}");

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>> IsValidUpdateAsync(Project project)
        {
            var getFilteredResult = await this.GetFilteredAsync(new() { Name = project.Name });
            if (!getFilteredResult.IsSuccess)
                return Result<bool>.From(getFilteredResult);

            if (getFilteredResult.Value.Any(x => x.Id != project.Id))
                return _logger.FailureLog<bool>(
                    LogLevel.Error,
                    StatusCodes.Status400BadRequest,
                    $"Project with {nameof(project.Name)} : {project.Name} already exists");

            return Result<bool>.Success(true);
        }
    }
}
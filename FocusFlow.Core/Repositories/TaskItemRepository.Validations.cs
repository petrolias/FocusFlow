using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public partial class TaskItemRepository
    {
        private async Task<Result<bool>> IsValidCreateAsync(TaskItem taskItem)
        {
            var getByIdResult = await this.GetByIdAsync(taskItem.Id);
            if (!getByIdResult.IsSuccess)
                return Result<bool>.From(getByIdResult);

            if (getByIdResult.Value != null)
                return logger.FailureLog<bool>(
                  LogLevel.Error,
                  StatusCodes.Status400BadRequest,
                  $"Trying to insert duplicate with {nameof(taskItem.Id)} : {taskItem.Id}");

            var getFilteredResult = await this.GetFilteredAsync(new() { Title = taskItem.Title });
            if (!getFilteredResult.IsSuccess)
                return Result<bool>.From(getFilteredResult);

            if (getFilteredResult.Value.Where(x => x.Id != x.Id).Any())
                return logger.FailureLog<bool>(
                   LogLevel.Error,
                   StatusCodes.Status400BadRequest,
                   $"Trying to insert duplicate with {nameof(taskItem.Title)} : {taskItem.Title}");

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>> IsValidUpdateAsync(TaskItem taskItem)
        {
            var getFilteredResult = await this.GetFilteredAsync(new() { Title = taskItem.Title });
            if (!getFilteredResult.IsSuccess)
                return Result<bool>.From(getFilteredResult);

            if (getFilteredResult.Value.Any(x => x.Id != taskItem.Id))
                return logger.FailureLog<bool>(
                    LogLevel.Error,
                    StatusCodes.Status400BadRequest,
                    $"TaskItem with {nameof(taskItem.Title)} : {taskItem.Title} already exists");

            return Result<bool>.Success(true);
        }
    }
}
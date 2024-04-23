using Microsoft.AspNetCore.Identity;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class AnalyticsService(IRepositoryWrapper repositoryWrapper, UserManager<AppUser> userManager)
{
    public AnalyticsDto Get()
    {
        return new AnalyticsDto
        {
            CategoryCount = repositoryWrapper.Category.Count(),
            QuestionCount = repositoryWrapper.Question.Count(),
            QuestionSetterCount = userManager.GetUsersInRoleAsync("QuestionSetter").Result.Count,
            ReviewerCount = userManager.GetUsersInRoleAsync("Reviewer").Result.Count
        };
    }
}
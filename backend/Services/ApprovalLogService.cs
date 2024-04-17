using AutoMapper;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Repositories.Wrapper;

namespace QWiz.Services;

public class ApprovalLogService(IRepositoryWrapper repositoryWrapper, IMapperBase mapper)
{
    public List<ApprovalLog> Get(HttpRequest request)
    {
        return repositoryWrapper.ApprovalLog.GetAll();
    }

    public ApprovalLog Create(ApprovalLogDto approvalLog)
    {
        return repositoryWrapper.ApprovalLog.Insert(mapper.Map<ApprovalLog>(approvalLog));
    }

    public ApprovalLog Update(long id, ApprovalLogDto approvalLog)
    {
        approvalLog.Id = id;
        return repositoryWrapper.ApprovalLog.Update(mapper.Map<ApprovalLog>(approvalLog));
    }
}
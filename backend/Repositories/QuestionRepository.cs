using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class QuestionRepository(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor)
    : BaseRepository<Question>(context, uriService, httpContextAccessor), IQuestionRepository;
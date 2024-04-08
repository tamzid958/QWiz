using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class QuestionRepository(AppDbContext context, IUriService uriService)
    : BaseRepository<Question>(context, uriService), IQuestionRepository;
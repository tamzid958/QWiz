using QWiz.Databases;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.Paginator;
using QWiz.Repositories.Abstract;

namespace QWiz.Repositories;

public class QuestionRepository(
    AppDbContext context,
    IUriService uriService,
    IAuthenticationService authenticationService)
    : BaseRepository<Question>(context, uriService, authenticationService), IQuestionRepository;
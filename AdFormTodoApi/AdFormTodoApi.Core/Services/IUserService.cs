using AdFormTodoApi.Core.Models;

namespace AdFormTodoApi.Core.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        User GetById(int id);
    }
}

using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Models;

namespace AdFormTodoApi.Services
{
    public interface IAuthenicateService
    {
        User Authenticate(string username, string password);
    }
}

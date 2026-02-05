using System.Collections.Generic;

namespace Api.ViewModels
{
    public class LoginViewModel
    {
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public UserViewModel User { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }

    public class ClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}

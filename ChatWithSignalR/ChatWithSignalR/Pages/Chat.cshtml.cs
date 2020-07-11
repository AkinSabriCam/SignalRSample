using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ChatWithSignalR.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private UserManager<IdentityUser> _userManager;

        public IList<IdentityUser> Users;

        public ChatModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
            Users = _userManager.Users.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWithSignalR.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<RedirectToPageResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}

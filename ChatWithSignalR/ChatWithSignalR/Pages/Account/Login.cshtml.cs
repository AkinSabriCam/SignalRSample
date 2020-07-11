using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatWithSignalR.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserLoginViewModel UserModel { get; set; }

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<RedirectToPageResult> OnPost()
        {
            var user = await _userManager.FindByNameAsync(UserModel.UserName);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }
            await AddUserIdClaim(user);

            var result = await _signInManager.PasswordSignInAsync
                                (UserModel.UserName, UserModel.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            return RedirectToPage("/Account/Login");

        }

        private async Task AddUserIdClaim(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            if (!userClaims.Any(x => x.Type == "UserId"))
            {
                var claim = new Claim("UserId", user.Id);
                await _userManager.AddClaimAsync(user, claim);
            }
        }
    }

    public class UserLoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

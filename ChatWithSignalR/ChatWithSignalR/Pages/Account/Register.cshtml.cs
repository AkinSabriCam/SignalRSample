using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ChatWithSignalR.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserCreateViewModel UserModel { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<RedirectToPageResult> OnPost()
        {
            var result = await _userManager.CreateAsync(
                new IdentityUser
                {
                    UserName = UserModel.UserName,
                    Email = UserModel.Email
                }, UserModel.Password);

            if (result.Succeeded)
            {
                return RedirectToPage("/Account/Login");

            }

            return RedirectToPage("/Account/Register");
        }

    }

    public class UserCreateViewModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

}

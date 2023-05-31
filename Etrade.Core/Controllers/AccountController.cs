using Etrade.Entity.Models.Identity;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Etrade.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager,RoleManager<Role> roleManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult SignUp()
        {
            if(User.Identity.Name == null)
                return View();
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var user = new User(model.Username)
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.Phone
            };
            var result = await _userManager.CreateAsync(user,user.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            return View(model);
        }



        public IActionResult SignIn()
        {
            if(User.Identity.Name==null)
                return View();
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult>SignIn(SignInViewModel model)
        {
            User user;
            if(model.UsernameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            if(user != null)
            {
                var rseult = await _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,true);
                var userRoles = await _userManager.GetRolesAsync(user);
                if(rseult.Succeeded && userRoles.Count>0)
                {
                   // return RedirectToAction("Index","Home");
                    return Redirect("~/Admin/Home");
                }
                else if(rseult.Succeeded && userRoles.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Profile(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            if(user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
                    
            }
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            var user = await _userManager.FindByIdAsync($"{model.Id}");
            if(user == null)
            {
                user = new User(model.UserName)
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Profile");
            }
            return View(model);
        }
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack","Account",new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl=null, string remoteError=null)
        {
            if(remoteError != null)
            {
                //hata olduğunda
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info ==  null)
            {
                //google ile giriş bilgisi alamadı
                return RedirectToAction("Login");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,isPersistent:true,bypassTwoFactor:true);
            if(signInResult.Succeeded)
                return RedirectToAction("Index","Home");
            else if(signInResult.IsLockedOut)
                return RedirectToAction("SignOut");
            else
            {
                //kullanıcı google ile kayıt olmadıysa
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new User { UserName = email, Email = email, Name="user",Surname="user",Password="123" };
                var result = await _userManager.CreateAsync(user);
                if(result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user,info);
                    if(result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent:true);
                        return RedirectToLocal(returnUrl);
                    }
                }
                return RedirectToAction("Login");
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index","Home");
        }
    }
}

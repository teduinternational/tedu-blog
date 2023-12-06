using HandlebarsDotNet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TeduBlog.Core.ConfigOptions;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.Events.LoginSuccessed;
using TeduBlog.Core.Events.RegisterSuccessed;
using TeduBlog.Core.SeedWorks.Constants;
using TeduBlog.WebApp.Extensions;
using TeduBlog.WebApp.Models;
using TeduBlog.WebApp.Services;

namespace TeduBlog.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        private readonly SystemConfig _systemConfig;
        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMediator mediator,
            IEmailSender emailSender, IOptions<SystemConfig> systemConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _emailSender = emailSender;
            _systemConfig = systemConfig.Value;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegsiterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userManager.CreateAsync(new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            }, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                await _signInManager.SignInAsync(user, true);
                await _mediator.Publish(new RegisterSuccessedEvent(model.Email));
                return Redirect(UrlConsts.Profile);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email not found!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _mediator.Publish(new LoginSuccessedEvent(model.Email));

                return Redirect(UrlConsts.Profile);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed");
            }
            return View();
        }

        [HttpGet]
        [Route("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Cannot find any user match with this email");
            }

            // For more information on how to enable account confirmation and password reset please
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);

            //var emailData = new EmailData
            //{
            //    ToEmail = user.Email ?? string.Empty,
            //    Subject = $"{_systemConfig.AppName} - Lấy lại mật khẩu",
            //    Content = $"Chào {user.FirstName}. Bạn vừa gửi yêu cầu lấy lại mật khẩu tại {_systemConfig.AppName}. Click: <a href='{callbackUrl}'>vào đây</a> để đặt lại mật khẩu. Trân trọng."
            //};
            //await _emailSender.SendEmail(emailData);


            TempData[SystemConsts.FormSuccessMsg] = "You need to check mail to reset password";
            return Redirect(UrlConsts.Login);
        }

        [HttpGet]
        [Route("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("Code is required");
            }
            return View(new ResetPasswordViewModel { Code = code });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, "Email is not existed");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Reset password successful";
                return Redirect(UrlConsts.Login);
            }
            return View();
        }

    }
}

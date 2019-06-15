using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Enterprise.DTO;
using Microsoft.AspNetCore.Identity;

namespace Enterprise.Services
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterUser(UserRegistrationDTO registerUserDTO);
        Task<bool> PasswordLoginUser(UserPasswordLoginDTO userPasswordLoginDTO);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private SignInManager<IdentityUser<int>> _signInManager;
        private UserManager<IdentityUser<int>> _userManager;
        public AuthenticationService(
            SignInManager<IdentityUser<int>> signInManager,
            UserManager<IdentityUser<int>> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<bool> RegisterUser(UserRegistrationDTO registerUserDTO)
        {
            var user = new IdentityUser<int> { UserName = registerUserDTO.Email, Email = registerUserDTO.Email };
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            return result.Succeeded;
        }
        public async Task<bool> PasswordLoginUser(UserPasswordLoginDTO userPasswordLoginDTO)
        {
            var result = await _signInManager
                                .PasswordSignInAsync(userPasswordLoginDTO.Email,
                                                    userPasswordLoginDTO.Password,
                                                    true,
                                                    true);
            return result.Succeeded;
        }
    }
}

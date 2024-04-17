using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register
{
    // Apply security-related headers to the response
    [SecurityHeaders]
    // Allow unauthenticated users to access this page
    [AllowAnonymous]
    public class Index : PageModel
    {
        // UserManager is used to manage users in ASP.NET Core Identity
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor that takes UserManager as a dependency
        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Input model for the page, bound to the form fields
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        // Indicates whether registration was successful
        [BindProperty]
        public bool RegisterSuccess { get; set; }

        // Handles GET requests to the page
        public IActionResult OnGet(string returnUrl)
        {
            // Initialize the input model with the return URL
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            };

            // Render the page
            return Page();
        }

        // Handles POST requests to the page
        public async Task<IActionResult> OnPost()
        {
            // If the button clicked was not the "register" button, redirect to the home page
            if (Input.Button != "register") return Redirect("~/");

            // If the form data is valid
            if (ModelState.IsValid)
            {
                // Create a new user
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                // Attempt to create the user
                var result = await _userManager.CreateAsync(user, Input.Password);

                // If the user was created successfully
                if (result.Succeeded)
                {
                    // Add a claim for the user's full name
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {   // JwtClaimTypes.Name is a standard claim type defined by the OpenID Connect specification
                        new Claim(JwtClaimTypes.Name, Input.FullName)
                    });

                    // Indicate that registration was successful
                    RegisterSuccess = true;
                }
            }

            // Render the page (with validation errors if any)
            return Page();
        }
    }
}
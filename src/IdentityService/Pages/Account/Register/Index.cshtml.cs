using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders] // Apply security-related headers to the response
    [AllowAnonymous] // Allow unauthenticated users to access this page
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); // Guard clause to ensure userManager is not null
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; } = new RegisterViewModel(); // Initialize to prevent null

        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public void OnGet(string returnUrl = "")
        {
            // Ensure Input is not null and initialize properties
            Input ??= new RegisterViewModel();
            Input.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost()
        {
            // Check if Input is null or the Button clicked was not the "register" button
            if (Input == null || Input.Button != "register")
            {
                return Redirect("~/"); // Redirect if conditions are not met
            }

            // Ensure ModelState is valid and Input.Password is not null
            if (!ModelState.IsValid || string.IsNullOrEmpty(Input.Password))
            {
                // Add model error if password is null or empty
                ModelState.AddModelError("Input.Password", "Password is required.");
                return Page(); // Return the page with validation errors
            }

            // Create a new user with non-null password
            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                EmailConfirmed = true
            };

            // Attempt to create the user with the validated password
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Add a claim for the user's full name, ensuring no null full name
                await _userManager.AddClaimsAsync(user, new Claim[]
                {
            new Claim(JwtClaimTypes.Name, Input.FullName ?? "") // Handle null FullName using null-coalescing
                });

                // Indicate that registration was successful
                RegisterSuccess = true;
            }

            return Page(); // Return the page to show the result
        }

    }
}

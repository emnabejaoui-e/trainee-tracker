using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Trainee_Tracker.Views.Admin
{
    public class CreateMentor : PageModel
    {
        private readonly ILogger<CreateMentor> _logger;

        public CreateMentor(ILogger<CreateMentor> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
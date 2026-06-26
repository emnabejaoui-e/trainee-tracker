using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Trainee_Tracker.Views.Admin
{
    public class CreateTrainee : PageModel
    {
        private readonly ILogger<CreateTrainee> _logger;

        public CreateTrainee(ILogger<CreateTrainee> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
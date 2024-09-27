using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages;


[Authorize(Policy = "MustBelongToHRDepartment")]
public class HumanResource : PageModel
{
    public void OnGet()
    {
        
    }
}
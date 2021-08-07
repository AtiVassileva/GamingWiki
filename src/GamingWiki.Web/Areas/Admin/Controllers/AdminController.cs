using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static GamingWiki.Web.Areas.Admin.AdminConstants;

namespace GamingWiki.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]

    public abstract class AdminController : Controller
    {
    }
}

using DesignABear.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if (result.Error.Code == "NotFound")
            {
                return NotFound(result);
            }

            return BadRequest(result);
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        protected Guid GetCurrentUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;
            return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
        }

        protected int GetCurrentRoleId()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            return int.TryParse(roleClaim, out var roleId) ? roleId : 0;
        }

    }
}

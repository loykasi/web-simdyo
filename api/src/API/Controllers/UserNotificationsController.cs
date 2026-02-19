using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/notifications")]
    public class UserNotificationsController(IUserNotificationService userNotificationService) : BaseController
    {
        [HttpGet("unread")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            var result = await userNotificationService.GetUnreadCount();
            return ToApiResult(result);
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> Get()
        //{
        //    var result = await userNotificationService.Get();
        //    return ToApiResult(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create()
        //{
        //    var result = await userNotificationService.Create();
        //    return ToApiResult(result);
        //}
    }
}

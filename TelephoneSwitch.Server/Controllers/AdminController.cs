
namespace TelephoneSwitch.Server.Controllers
{
    [ApiController]
    [Route("api/admin")]
    //[Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly BillRepository _billRepository;

        public AdminController(UserRepository userRepository, BillRepository billRepository)
        {
            _userRepository = userRepository;
            _billRepository = billRepository;
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetSubscribersAsync();
            return Ok(users);
        }

        [HttpPost("add-user/{name}/{phoneNumber}")]
        public async Task<IActionResult> AddUser(string name, string phoneNumber)
        {
            await _userRepository.AddSubscriberAsync(name, phoneNumber);
            return Ok();
        }

        [HttpGet("unpaid-bills")]
        public async Task<IActionResult> GetUnpaidBills()
        {
            var unpaidBills = await _billRepository.GetUnpaidBillsAsync();
            return Ok(unpaidBills);
        }

        [HttpPost("block-user/{userId}")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            await _userRepository.UpdateUserBlockStatusAsync(userId, true);
            return Ok("User blocked.");
        }

        [HttpPost("unblock-user/{userId}")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            await _userRepository.UpdateUserBlockStatusAsync(userId, false);
            return Ok("User unblocked.");
        }
    }
}

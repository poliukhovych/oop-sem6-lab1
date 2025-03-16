
namespace TelephoneSwitch.Server.Controllers
{
    [ApiController]
    [Route("api/subscriber")]
    //[Authorize(Roles = "subscriber")]
    public class SubscriberController : ControllerBase
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly BillRepository _billRepository;

        public SubscriberController(ServiceRepository serviceRepository, BillRepository billRepository)
        {
            _serviceRepository = serviceRepository;
            _billRepository = billRepository;
        }

        private User GetAuthenticatedUser()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
                throw new UnauthorizedAccessException("User not found in middleware.");

            return user;
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetAvailableServices()
        {
            var services = await _serviceRepository.GetServicesAsync();
            return Ok(services);
        }

        [HttpGet("my-services")]
        public async Task<IActionResult> GetMyServices()
        {
            var user = GetAuthenticatedUser();
            var services = await _serviceRepository.GetSubscriberServicesAsync(user.Id);
            return Ok(services);
        }

        [HttpPost("add-service/{serviceId}")]
        public async Task<IActionResult> AddService(int serviceId)
        {
            var user = GetAuthenticatedUser();
            await _serviceRepository.AssignServiceToSubscriberAsync(user.Id, serviceId);

            decimal servicePrice = await _serviceRepository.GetServicePriceAsync(serviceId);
            await _billRepository.AddBillAsync(user.Id, servicePrice);

            return Ok("Service added.");
        }

        [HttpGet("my-bills")]
        public async Task<IActionResult> GetMyBills()
        {
            var user = GetAuthenticatedUser();
            var bills = await _billRepository.GetSubscriberBillsAsync(user.Id);
            return Ok(bills);
        }

        [HttpPost("pay-bill/{billId}")]
        public async Task<IActionResult> PayBill(int billId)
        {
            var user = GetAuthenticatedUser();
            await _billRepository.PayBillAsync(billId);
            return Ok("Bill paid successfully.");
        }
    }
}

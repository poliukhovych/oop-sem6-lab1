
public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, UserRepository userRepository)
    {
        var claims = context.User.Claims.ToList();
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }

        var auth0Id = context.User.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

        var name = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        _logger.LogInformation($"AuthMiddleware: auth0Id = {auth0Id}, name = {name}");

        if (!string.IsNullOrEmpty(auth0Id))
        {
            var user = await userRepository.GetUserByAuth0IdAsync(auth0Id);
            if (user == null)
            {
                _logger.LogInformation("User not found in DB. Creating new user...");
                user = await userRepository.CreateUserAsync(auth0Id, name ?? "Unknown");
                _logger.LogInformation("User created successfully.");
            }
            else
            {
                _logger.LogInformation($"User {user.Name} already exists in DB.");
            }

            context.Items["User"] = user;
        }
        else
        {
            _logger.LogWarning("Auth0 ID is missing from JWT claims.");
        }

        await _next(context);
    }
}

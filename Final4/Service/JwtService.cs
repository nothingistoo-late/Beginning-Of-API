using Final4.Data;

namespace Final4.Service
{
    public class JwtService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        public JwtService(ApplicationDBContext dBContext, IConfiguration configuration)
        {
            _dbContext = dBContext;
            _configuration = configuration;
        }

    }
}

using Final4.Data;
using Final4.IRepository;
using Final4.Model.Entities;

namespace Final4.Repository
{
    public class FlowerRepository : GenericRepository<Flower>, IFlowerRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public FlowerRepository(ApplicationDBContext dbContext, IConfiguration configuration, EmailService emailService) : base(dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;
        }

    }
}

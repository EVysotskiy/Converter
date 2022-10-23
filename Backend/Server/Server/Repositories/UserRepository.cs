using System.Linq.Expressions;
using Domain.Model;
using Server.Database;

namespace Server.Repositories
{
    public class UserRepository : Repository<User, long, AppDbContext>
    {
        private readonly AppDbContext _dbContext;
        protected override Expression<Func<User, long>> Key => model => model.IdChat;
        public UserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
            (appDbContext) => appDbContext.Users)
        {
            _dbContext = dbContext;
        }
    }
}
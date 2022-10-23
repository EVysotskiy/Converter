using System.Linq.Expressions;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Repositories;

public class DocumentRepository : Repository<Document, long, AppDbContext>
{
    private readonly AppDbContext _dbContext;
    protected override Expression<Func<Document, long>> Key => model => model.Id;
    public DocumentRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
        (appDbContext) => appDbContext.Documents)
    {
        _dbContext = dbContext;
    }
}
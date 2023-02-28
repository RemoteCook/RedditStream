using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RedditStream.Domains;

namespace RedditStream.Repositories.Data;

public class RedditStreamContext : DbContext
{
    //private readonly IConfiguration _configuration;

    public RedditStreamContext(DbContextOptions options, IConfiguration configuration)
        : base(options)
    {
        //_configuration = configuration;
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{

    //    optionsBuilder.UseSqlite(_configuration.GetConnectionString("RedditStream"));
    //    base.OnConfiguring(optionsBuilder);
    //}


    public DbSet<LiveThreadMessage> LiveThreadMessages { get; set; }
    public DbSet<Domains.Data>  Datas { get; set; }
    public DbSet<Payload> Payloads{ get; set; }

}

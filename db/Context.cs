using GeoPetWebApi.db.Repository;
using Microsoft.EntityFrameworkCore;
using projetoFinal.db.Models.PessoaCuidadora;
using projetoFinal.db.Models.Pets;

namespace projetoFinal.db;
public class Context : DbContext, IGeoPetContext
{
    public DbSet<PessoaCuidadoraModel> PessoasCuidadoras {get; set;}
    public DbSet<PetModel> Pets {get; set;}

    private readonly IConfiguration _configuration;
 
    public Context(DbContextOptions<Context> options, IConfiguration configuration) : base(options){
        _configuration = configuration;
    }

    public Context() { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        if (!optionsBuilder.IsConfigured)
            {
                // var connectionString =  _configuration.GetValue<string>("DOTNET_CONNECTION_STRING");

                var connectionString =
                @"Server=127.0.0.1;
                Database=GeoPetDB;
                User=SA;
                Password=Password12;
                Encrypt=False";
                optionsBuilder.UseSqlServer(connectionString);
            }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<PetModel>().HasOne(p => p.PessoaCuidadora).WithMany(a => a.Pets).HasForeignKey("PESSOA_CUIDADORA");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EquilibriumData
{
  public class AplicattionDBContextFactory : IDesignTimeDbContextFactory<AplicattionDBContext>
  {
    public AplicattionDBContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<AplicattionDBContext>();
      optionsBuilder.UseOracle("User Id=rm99679;Password=030205;Data Source=oracle.fiap.com.br:1521/orcl;");

      return new AplicattionDBContext(optionsBuilder.Options);
    }
  }
}

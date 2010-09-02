using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.DataProviders;
using Xunit;

namespace SubSonic.Tests.Repositories
{
    public class PostgreSqlSimpleRepositoryTests : SimpleRepositoryTests
    {
		 public PostgreSqlSimpleRepositoryTests() :
			 base(ProviderFactory.GetProvider(@"host=localhost;database=subsonic;user id=subsonic; password=pass;", "Npgsql.PostgreSqlClient"))
        {
        }
    }
}

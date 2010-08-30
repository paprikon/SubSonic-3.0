using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.DataProviders;

namespace SubSonic.Tests.Repositories
{
    public class PostgreSqlSimpleRepositoryTests : SimpleRepositoryTests
    {
		 public PostgreSqlSimpleRepositoryTests() :
			 base(ProviderFactory.GetProvider(@"host=localhost;database=SubSonic;user id=subsonic; password=pass;", "NpSql.PostgreSqlCient"))
        {
        }
    }
}

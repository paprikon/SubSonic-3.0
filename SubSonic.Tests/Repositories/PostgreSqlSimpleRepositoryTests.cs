using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.DataProviders;
using Xunit;
using SubSonic.Query;

namespace SubSonic.Tests.Repositories
{
    public class PostgreSqlSimpleRepositoryTests : SimpleRepositoryTests
    {
		 public PostgreSqlSimpleRepositoryTests() :
			 base(ProviderFactory.GetProvider("Postgresql"))
        {
        }

		 protected override void CleanTables()
		 {
			 try
			 {
				 var qry = new CodingHorror(_provider, "DROP TABLE \"Shwerkos\"").Execute();
			 }
			 catch { }

			 try
			 {
				 new CodingHorror(_provider, "DROP TABLE \"DummyForDeletes\"").Execute();
			 }
			 catch { }

			 try
			 {
				 new CodingHorror(_provider, "DROP TABLE \"Shwerko2s\"").Execute();
			 }
			 catch { }

			 try
			 {
				 new CodingHorror(_provider, "DROP TABLE \"NonAutoIncrementingIdWithDefaultSettings\"").Execute();
			 }
			 catch { }
		 }
    }
}

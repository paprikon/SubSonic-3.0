using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.DataProviders;
using Xunit;
using SubSonic.Tests.Repositories.TestBases;

namespace SubSonic.Tests.Repositories
{
	public class iDB2SimpleRepositoryTests : SimpleRepositoryTests
	{
		public iDB2SimpleRepositoryTests() :
			base(ProviderFactory.GetProvider(@"DataSource=test;DefaultCollection=test;Naming=SQL;UserID=test;Password=test", "IBM.Data.DB2.iSeries"), SubSonic.Repository.SimpleRepositoryOptions.RunMigrations, true)
		{

		}

		[Fact]
		public void Simple_Repo_Should_Support_Contains_Enumerable()
		{
			List<int> ids = new List<int>();
			var shwerko = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test1");
			_repo.Add(shwerko);
			ids.Add(shwerko.ID);

			var shwerko2 = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test2");
			_repo.Add(shwerko2);
			ids.Add(shwerko2.ID);

			var result = _repo.All<Shwerko>().Where(o => ids.Contains(o.ID)).ToList();
			Assert.NotEmpty(result);
			Assert.True(result.Count == 2);
		}
		[Fact]
		public void Simple_Repo_Should_Support_Contains_String_Enumerable()
		{
			List<string> names = new List<string>();
			var shwerko = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test1");
			_repo.Add(shwerko);
			names.Add(shwerko.Name);

			var shwerko2 = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test2");
			_repo.Add(shwerko2);
			names.Add(shwerko2.Name);

			var result = _repo.All<Shwerko>().Where(o => names.Contains(o.Name)).ToList();
			Assert.NotEmpty(result);
			Assert.True(result.Count == 2);
		}

		public void Simple_Repo_Should_Return_Zero_Results_When_Using_Contains_With_Empty_Enumerable()
		{
			//I'm building this using the same logic as Linq to Sql Classes.  They append a where clause of 0 = 1
			//	to force the statment to return no results.  This does not result in an exception being thrown and 
			//	allows the logic of Contains(id) to be valid. 
			var shwerko = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test1");
			_repo.Add(shwerko);

			var shwerko2 = CreateTestRecord<Shwerko>(Guid.NewGuid(), s => s.Name = "test2");
			_repo.Add(shwerko2);

			List<int> ids = new List<int>();
			var result = _repo.All<Shwerko>().Where(o => ids.Contains(o.ID)).ToList();
			Assert.Empty(result);
		}


		void sandbox()
		{
			//var prov = ProviderFactory.GetProvider(@"DataSource=test;DefaultCollection=NBRIDGE;Naming=SQL;UserID=NBRIDGE;Password=Jet5ki12", "IBM.Data.DB2.iSeries");
			//var prov = IBM.Data.DB2.iSeries.iDB2Factory.Instance.CreateConnection();
			//var con = new IBM.Data.DB2.iSeries.iDB2Connection();
			//con.GetSchema(
			//var fac = IBM.Data.DB2.iSeries.iDB2Factory.Instance;
			//base.prov
		}

		/* Wrappers for test runner. 
		 */
		public new void Issue183_ToString_Should_Generate_Valid_Sql() { base.Issue183_ToString_Should_Generate_Valid_Sql(); }
		public new void Simple_Repo_All_Should_Have_Count_10() { base.Simple_Repo_All_Should_Have_Count_10(); }
		public new void Simple_Repo_Exists_Should_Be_True_With_Name_Charlie_False_With_Name_Chuck() { base.Simple_Repo_Exists_Should_Be_True_With_Name_Charlie_False_With_Name_Chuck(); }
		public new void Simple_Repo_GetPaged_Should_Allow_Descending_Order() { base.Simple_Repo_GetPaged_Should_Allow_Descending_Order(); }
		public new void Simple_Repo_GetPaged_Should_Not_Expect_Case_Sensitive_Order() { base.Simple_Repo_GetPaged_Should_Not_Expect_Case_Sensitive_Order(); }
		public new void Simple_Repo_Should_Add_Multiple_Shwerko_Item() { base.Simple_Repo_Should_Add_Multiple_Shwerko_Item(); }
		public new void Simple_Repo_Should_Create_IQueryable() { base.Simple_Repo_Should_Create_IQueryable(); }
		public new void Simple_Repo_Should_Create_Schema_And_Save_Shwerko() { base.Simple_Repo_Should_Create_Schema_And_Save_Shwerko(); }
		public new void Simple_Repo_Should_Delete_Multiple_Shwerko_Item_With_Expression() { base.Simple_Repo_Should_Delete_Multiple_Shwerko_Item_With_Expression(); }
		public new void Simple_Repo_Should_Delete_Multiple_Shwerko_Item_With_List() { base.Simple_Repo_Should_Delete_Multiple_Shwerko_Item_With_List(); }
		public new void Simple_Repo_Should_Delete_Single_Shwerko_Item() { base.Simple_Repo_Should_Delete_Single_Shwerko_Item(); }
		public new void Simple_Repo_Should_Find_Shwerkos_By_Enums() { base.Simple_Repo_Should_Find_Shwerkos_By_Enums(); }
		public new void Simple_Repo_Should_Find_Shwerkos_By_NullableEnums() { base.Simple_Repo_Should_Find_Shwerkos_By_NullableEnums(); }
		public new void Simple_Repo_Should_Get_Single() { base.Simple_Repo_Should_Get_Single(); }
		public new void Simple_Repo_Should_Implement_String_Contains() { base.Simple_Repo_Should_Implement_String_Contains(); }
		public new void Simple_Repo_Should_Implement_String_StartsWith() { base.Simple_Repo_Should_Implement_String_StartsWith(); }
		public new void Simple_Repo_Should_Load_Enums() { base.Simple_Repo_Should_Load_Enums(); }
		public new void Simple_Repo_Should_Load_Nullable_Enums() { base.Simple_Repo_Should_Load_Nullable_Enums(); }
		public new void Simple_Repo_Should_Load_Shwerkos_With_Binaries() { base.Simple_Repo_Should_Load_Shwerkos_With_Binaries(); }
		public new void Simple_Repo_Should_Not_Increment_Id_When_Overridden() { base.Simple_Repo_Should_Not_Increment_Id_When_Overridden(); }
		public new void Simple_Repo_Should_Query_For_IsNotNull() { base.Simple_Repo_Should_Query_For_IsNotNull(); }
		public new void Simple_Repo_Should_Query_For_IsNull() { base.Simple_Repo_Should_Query_For_IsNull(); }
		public new void Simple_Repo_Should_Return_Integer_For_Integer_Autogenerated_Keys() { base.Simple_Repo_Should_Return_Integer_For_Integer_Autogenerated_Keys(); }
		public new void Simple_Repo_Should_Run_Migrations_Before_Delete() { base.Simple_Repo_Should_Run_Migrations_Before_Delete(); }
		public new void Simple_Repo_Should_Run_Migrations_Before_DeleteMany() { base.Simple_Repo_Should_Run_Migrations_Before_DeleteMany(); }
		public new void Simple_Repo_Should_Run_Migrations_Before_DeleteMany_WithItemsCollection() { base.Simple_Repo_Should_Run_Migrations_Before_DeleteMany_WithItemsCollection(); }
		public new void Simple_Repo_Should_Select_Anonymous_Types() { base.Simple_Repo_Should_Select_Anonymous_Types(); }
		public new void Simple_Repo_Should_Select_System_Types() { base.Simple_Repo_Should_Select_System_Types(); }
		public new void Simple_Repo_Should_Select_Value_Types() { base.Simple_Repo_Should_Select_Value_Types(); }
		public new void Simple_Repo_Should_Set_DefaultSetting_When_Saved() { base.Simple_Repo_Should_Set_DefaultSetting_When_Saved(); }
		public new void Simple_Repo_Should_Support_Joins() { base.Simple_Repo_Should_Support_Joins(); }
		public new void Simple_Repo_Should_Support_Projection_Joins() { base.Simple_Repo_Should_Support_Projection_Joins(); }
		public new void Simple_Repo_Should_Support_Projection_Joins_With_Anon_Types() { base.Simple_Repo_Should_Support_Projection_Joins_With_Anon_Types(); }
		public new void Simple_Repo_Should_Support_SingleQuote_In_Queries() { base.Simple_Repo_Should_Support_SingleQuote_In_Queries(); }
		public new void Simple_Repo_Should_Update_Multiple_Shwerko_Item() { base.Simple_Repo_Should_Update_Multiple_Shwerko_Item(); }
		public new void Simple_Repo_Should_Update_Single_Shwerko_Item() { base.Simple_Repo_Should_Update_Single_Shwerko_Item(); }
		public new void SimpleRepo_Should_Set_The_Guid_PK_On_Add() { base.SimpleRepo_Should_Set_The_Guid_PK_On_Add(); }
		public new void SimpleRepo_Should_Set_The_PK_On_Add() { base.SimpleRepo_Should_Set_The_PK_On_Add(); }
	}
}

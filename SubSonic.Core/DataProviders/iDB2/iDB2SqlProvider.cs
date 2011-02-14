using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.Schema;
using System.Data.Common;
using SubSonic.Linq.Structure;
using SubSonic.Query;


namespace SubSonic.DataProviders.iDB2
{

    public class iDB2Provider : DbDataProvider, IDataProvider
    {
        public override string InsertionIdentityFetchString { get { return String.Empty; } }

		  public iDB2Provider(string connectionString, string providerName)
			  : base(connectionString, providerName)
        {}

        public override string QualifyTableName(ITable table)
        {
            string qualifiedTable;

            qualifiedTable = qualifiedTable = String.Format("\"{0}\"", table.Name);


            return qualifiedTable;
        }

        public override string QualifyColumnName(IColumn column)
        {
            string qualifiedFormat;
            qualifiedFormat = "\"{2}\"";
            return String.Format(qualifiedFormat, column.Table.SchemaName, column.Table.Name, column.Name);
        }

        public override ISchemaGenerator SchemaGenerator
        {
            get { return new iDB2Schema(); }
        }

        public override ISqlGenerator GetSqlGenerator(SqlQuery query)
        {
            return new iDB2Generator(query);
        }

        public override IQueryLanguage QueryLanguage { get { return new iDB2Language(this); } }

    }
}

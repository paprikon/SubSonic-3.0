// 
//   SubSonic - http://subsonicproject.com
// 
//   The contents of this file are subject to the New BSD
//   License (the "License"); you may not use this file
//   except in compliance with the License. You may obtain a copy of
//   the License at http://www.opensource.org/licenses/bsd-license.php
//  
//   Software distributed under the License is distributed on an 
//   "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or
//   implied. See the License for the specific language governing
//   rights and limitations under the License.
// 
using System;
using System.Data;
using System.Text;
using SubSonic.Schema;
using SubSonic.SqlGeneration.Schema;
using SubSonic.Extensions;


namespace SubSonic.DataProviders.iDB2
{

	public class iDB2Schema : ANSISchemaGenerator
	{
		public iDB2Schema()
		{
			ADD_COLUMN = "ALTER TABLE \"{0}\" ADD \"{1}\"{2};";
			//can't do this
			ALTER_COLUMN = @"";
			CREATE_TABLE = "CREATE TABLE \"{0}\" ({1} \r\n);";
			//can't do this
			DROP_COLUMN = @"";
			DROP_TABLE = "DROP TABLE \"{0}\";";

			UPDATE_DEFAULTS = "UPDATE \"{0}\" SET \"{1}\"={2};";

			ClientName = "IBM.Data.DB2.iSeries";
		}

		public override string GetNativeType(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.Object:
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
					return "varchar";
				case DbType.Boolean:
					return "boolean";
				case DbType.SByte:
				case DbType.Binary:
				case DbType.Byte:
					return "bytea";
				case DbType.Currency:
					return "money";
				case DbType.Time:
				case DbType.Date:
				case DbType.DateTime:
					return "timestamp";
				case DbType.Decimal:
					return "decimal";
				case DbType.Double:
					return "real";
				case DbType.Guid:
					return "uuid";
				case DbType.UInt32:
				case DbType.Int32:
					return "integer";
				case DbType.Int16:
				case DbType.UInt16:
					return "smallint";
				case DbType.UInt64:
				case DbType.Int64:
					return "bigint";
				case DbType.Single:
					return "smallint";
				case DbType.VarNumeric:
					return "numeric";
				case DbType.Xml:
					return "xml";
				default:
					return "varchar";
			}
		}

		public override string BuildDropColumnStatement(string tableName, string columnName)
		{
			//TODO: drop column
			return "";
		}

		public override string BuildAlterColumnStatement(IColumn column)
		{
			//TODO: alter column
			return "";
		}

		/// <summary>
		/// Sets the column attributes.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public override string GenerateColumnAttributes(IColumn column)
		{
			StringBuilder sb = new StringBuilder();
			if (column.DataType == DbType.String && column.MaxLength > 8000)
				sb.Append(" TEXT ");
			else if (column.IsPrimaryKey && column.DataType == DbType.Int32
				 || column.IsPrimaryKey && column.DataType == DbType.Int16
				 || column.IsPrimaryKey && column.DataType == DbType.Int64
				 )
			{
				if (column.AutoIncrement)
					sb.Append(" SERIAL");
				else
					sb.Append(" INTEGER ");
			}
			else
				sb.Append(" " + GetNativeType(column.DataType));

			if (column.IsString && column.MaxLength < 8000)
				sb.Append("(" + column.MaxLength + ")");
			else if (column.DataType == DbType.Double || column.DataType == DbType.Decimal)
				sb.Append("(" + column.NumericPrecision + ", " + column.NumberScale + ")");

			if (column.IsPrimaryKey)
			{
				sb.Append(" NOT NULL");
			}
			else
			{
				if (!column.IsNullable)
					sb.Append(" NOT NULL");
				else
					sb.Append(" NULL");

				if (column.DefaultSetting != null)
					sb.Append(" DEFAULT '" + column.DefaultSetting + "'");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets the type of the db.
		/// </summary>
		/// <param name="sqlType">Type of the SQL.</param>
		/// <returns></returns>
		public override DbType GetDbType(string sqlType)
		{
			switch (sqlType.ToLowerInvariant())
			{
				case "longtext":
				case "nchar":
				case "ntext":
				case "text":
				case "sysname":
				case "varchar":
				case "nvarchar":
					return DbType.String;
				case "bit":
				case "boolean":
					return DbType.Boolean;
				case "decimal":
				case "newdecimal":
				case "numeric":
				case "double":
				case "real":
					return DbType.Decimal;
				case "int":
				case "int4" :
				case "integer" :
					return DbType.Int32;
				case "int8" :
				case "bigint":
					return DbType.Int64;
				case "int2" :
				case "smallint":
					return DbType.Int16;
				case "date":
				case "time":
				case "timestamp":
					return DbType.DateTime;
				case "lo":
				case "bytea" :
					return DbType.Binary;
				case "char":
					return DbType.AnsiStringFixedLength;
				case "money":
					return DbType.Currency;
				case "uuid":
					return DbType.Guid;
				default:
					return DbType.String;
			}
		}


		public override void SetColumnDefaults(IColumn column)
		{
			if (column.IsNumeric)
				column.DefaultSetting = 0;
			else if (column.IsDateTime)
				column.DefaultSetting = "1900-01-01";
			else if (column.IsString)
				column.DefaultSetting = "";
			else if (column.DataType == DbType.Boolean)
				column.DefaultSetting = false;
		}

		public override ITable GetTableFromDB(IDataProvider provider, string tableName)
		{
			ITable result = null;
			DataTable schema;

			using (var scope = new AutomaticConnectionScope(provider))
			{
				var restrictions = new string[4] { null, null, tableName, null };
				schema = scope.Connection.GetSchema("Columns", restrictions); //case difference between default and the iDB2 provider
			}

			if (schema.Rows.Count > 0)
			{
				result = new DatabaseTable(tableName, provider);
				foreach (DataRow dr in schema.Rows)
				{
					IColumn col = new DatabaseColumn(dr["column_name"].ToString(), result);
					col.DataType = GetDbType(dr["DATA_TYPE"].ToString()); //TODO: might have to derive this from obvervation of attributes
					col.IsNullable = dr["is_nullable"].ToString() == "YES";

					string maxLength = dr["character_maximum_length"].ToString();

					int iMax = 0;
					int.TryParse(maxLength, out iMax);
					col.MaxLength = iMax;
					result.Columns.Add(col);
				}
			}

			return result;
		}

		public override string BuildCreateTableStatement(ITable table)
		{
			return base.BuildCreateTableStatement(table);
		}

		public override string GenerateColumns(ITable table)
		{
			StringBuilder createSql = new StringBuilder();

			foreach (IColumn col in table.Columns)
			{
				createSql.AppendFormat("\r\n  {0}{1},", col.QualifiedName, GenerateColumnAttributes(col));
			}

			if (table.HasPrimaryKey)
				createSql.AppendFormat("PRIMARY KEY({0}),", table.PrimaryKey.QualifiedName);
			
			string columnSql = createSql.ToString();
			return columnSql.Chop(",");
		}
	}
}
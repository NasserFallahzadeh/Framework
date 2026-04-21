using System.Data;
using System.Data.Common;
using DDDZamin.Utilities.Extensions;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.Extensions;

public static class DbCommandExtension
{
    public static void ApplyCorrectYeKe(this DbCommand command)
    {
        command.CommandText = command.CommandText.ApplyCorrectYeKe();

        foreach (DbParameter parameter in command.Parameters)
        {
            parameter.Value = parameter.DbType switch
            {
                DbType.AnsiString or 
                    DbType.AnsiStringFixedLength or 
                    DbType.String or 
                    DbType.StringFixedLength
                    or DbType.Xml => parameter.Value is DBNull 
                        ? parameter.Value : parameter.Value.ApplyCorrectYeKe()
            };
        }
    }
}
using Contracts.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{//NEVEIKIA!!!
    public class MySqlDifficultyTypeHandler : SqlMapper.TypeHandler<Difficulty>
    {
        public override Difficulty Parse(object value)
        {
            return (Difficulty)Enum.Parse(typeof(Difficulty), (string)value, true);
        }

        public override void SetValue(IDbDataParameter parameter, Difficulty value)
        {
            parameter.Value = value.ToString();
        }
    }
}

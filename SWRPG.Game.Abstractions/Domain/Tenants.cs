using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWRPG.Game.Abstractions.Domain
{
    public interface IAppTenantRegistry : ITenantRegistry<AppTenant>
    {
        ITenant GetByPrimaryKey(string primaryKey);
    }

    public class AppTenant : ITenant
    {
        private string DebuggerDisplay => $"Key: '{Key}', Name: '{Name}'";

        public string Key { get; set; }
        public string Name { get; set; }
    }

    public interface ITenant
    {
        string Key { get; set; }
    }


    public interface ITenantRegistry<out TTenant> where TTenant : class, ITenant
    {
        TTenant Get(string tenant);
        IEnumerable<TTenant> GetAll();
    }
}

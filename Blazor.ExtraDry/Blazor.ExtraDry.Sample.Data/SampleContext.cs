using Blazor.ExtraDry.Sample.Shared;
using Effort;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Sample.Data {
    public class SampleContext : DbContext {

        /// <summary>
        /// Note: In non-sample/production app, replace this with DI.
        /// This property should be removed completely when no longer referenced by the sample services.
        /// </summary>
        public static SampleContext Current {
            get {
                if(singleton == null) {
                    var connection = DbConnectionFactory.CreateTransient();
                    singleton = new SampleContext(connection);
                    var dummy = new DummyData();
                    dummy.PopulateEmployees(singleton, 100).GetAwaiter().GetResult();
                }
                return singleton;
            }
        }
        private static SampleContext singleton;

        public SampleContext(DbConnection connection) : base(connection, false)
        {
        }

        public DbSet<Employee> Employees { get; set; }

    }
}

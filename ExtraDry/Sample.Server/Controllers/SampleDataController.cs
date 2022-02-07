#nullable enable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    /// <summary>
    /// Manages collections of sectors for companies.
    /// </summary>
    [ApiController]
    [SkipStatusCodePages]
    public class SampleDataController {

        /// <summary>
        /// Stanard DI Constructor
        /// </summary>
        public SampleDataController(SampleContext sampleContext)
        {
            context = sampleContext;
        }

        private readonly SampleContext context;

        /// <summary>
        /// Load the set of sample data, idempotent so allowed to be anonymous.
        /// </summary>
        [HttpGet("api/load-sample-data")]
        [AllowAnonymous]
        public async Task LoadDataRpc()
        {
            var shouldLoadSamples = !await context.Sectors.AnyAsync();
            if(shouldLoadSamples) {
                var sampleData = new DummyData();
                sampleData.PopulateServices(context);
                sampleData.PopulateCompanies(context, 50);
                sampleData.PopulateEmployees(context, 5000);
                sampleData.PopulateContents(context);
            }
        }
    }
}

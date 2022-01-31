using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Linq;
using Core.Arango.Serialization;
using Core.Arango.Serialization.Json;
using Core.Arango.Serialization.Newtonsoft;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        public IArangoContext Arango { get; protected set; }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try
            {
                foreach (var db in await Arango.Database.ListAsync())
                    await Arango.Database.DropAsync(db);
            }
            catch
            {
                //
            }
        }

        public async Task SetupAsync(string serializer, string createDatabase = "test")
        {
#if NETSTANDARD2_0
            IArangoSerializer arangoSerializer;
            switch (serializer)
            {
                case "newton-default":
                    arangoSerializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
                    break;
                case "newton-camel":
                    arangoSerializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftCamelCaseContractResolver());
                    break;
                case "system-default":
                    arangoSerializer = new ArangoJsonSerializer(new ArangoJsonDefaultPolicy());
                    break;
                case "system-camel":
                    arangoSerializer = new ArangoJsonSerializer(new ArangoJsonCamelCasePolicy());
                    break;
                default:
                    arangoSerializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
                    break;
            }

            Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
            {
                Serializer = arangoSerializer
            });
#else
            Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
            {
                // todo: find a netstandard 2.0 compatible implementation
                Serializer = serializer switch
                {
                    "newton-default" => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver()),
                    "newton-camel" => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftCamelCaseContractResolver()),
                    "system-default" => new ArangoJsonSerializer(new ArangoJsonDefaultPolicy()),
                    "system-camel" => new ArangoJsonSerializer(new ArangoJsonCamelCasePolicy()),
                    _ => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver())
                }
        });
#endif

            if (!string.IsNullOrEmpty(createDatabase))
                await Arango.Database.CreateAsync("test");
        }

#if NETSTANDARD2_0
        private static IArangoSerializer GetIt(string serializer)
        {
            switch (serializer)
            {
                case "newton-default":
                    return new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
                case "newton-camel":
                    return new ArangoNewtonsoftSerializer(new ArangoNewtonsoftCamelCaseContractResolver());
                case "system-default":
                    return new ArangoJsonSerializer(new ArangoJsonDefaultPolicy());
                case "system-camel":
                    return new ArangoJsonSerializer(new ArangoJsonCamelCasePolicy());
                default:
                    return new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
            }
        }
#endif

        protected string UniqueTestRealm()
        {
            var cs = Environment.GetEnvironmentVariable("ARANGODB_CONNECTION");

            if (string.IsNullOrWhiteSpace(cs))
                cs = "Server=http://localhost:8529;Realm=CI-{UUID};User=root;Password=;";

            return cs.Replace("{UUID}", Guid.NewGuid().ToString("D"));
        }

        protected void PrintQuery<T>(IQueryable<T> query, ITestOutputHelper output)
        {
            var aql = query.ToAql();
            output.WriteLine("QUERY:");
            output.WriteLine(aql.aql);
            output.WriteLine("VARS:");
            output.WriteLine(JsonConvert.SerializeObject(aql.bindVars));
        }
    }
}
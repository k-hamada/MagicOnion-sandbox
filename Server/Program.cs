using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using MagicOnion.Hosting;
using MagicOnion.Server;
using Grpc.Core;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

            await MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion(
                    new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true),
                    new ServerPort("localhost", 12345, ServerCredentials.Insecure))
                .RunConsoleAsync();
        }
    }
}

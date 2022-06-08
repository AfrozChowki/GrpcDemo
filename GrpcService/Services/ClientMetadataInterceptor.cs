using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GrpcService.Services
{
    public class ClientMetadataInterceptor : Interceptor
    {
        private readonly ILogger<ClientMetadataInterceptor> _logger;

        public ClientMetadataInterceptor(ILogger<ClientMetadataInterceptor> logger)
        {
            this._logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
        {

            try
            {
                var Name = context.RequestHeaders.GetValue("name");
                var guid = context.RequestHeaders.GetValue("guid");
                Guid clientGuid;

                if (string.IsNullOrWhiteSpace(Name))
                {
                    Console.WriteLine("Name is missing");
                }

                if(!Guid.TryParse(guid, out clientGuid))
                {
                    Console.WriteLine("Invalid Guid");
                }
                //CustomerServerCallContext customerServerCallContext = new CustomerServerCallContext();
                //customerServerCallContext.Guid = clientGuid;
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error thrown by {context.Method}.");
                throw;
            }
        }
    }
}

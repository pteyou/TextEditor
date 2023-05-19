using Grpc.Core;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using TextEditor.Messages.gRPC;
using TextEditor.Configuration;

namespace TextEditor.CoreAppClient;

public class SanityChecker
{
    private static string CoreHostname; 
    private static int GrpcCorePort;

    public static int timeoutInSeconds;

    public static string FailureMessage = "Unable to connect to Core App";
    
    static SanityChecker()
    {
        var config = Hosting.Instance.Config;
        GrpcCorePort = config.GrpcCorePort;
        CoreHostname = config.Hostname;
        timeoutInSeconds = 5;
    } 

    public static async Task<CheckServiceReply> Call()
    {
        var channel = GrpcChannel.ForAddress($"http://{CoreHostname}:{GrpcCorePort}");
        var client = new CheckService.CheckServiceClient(channel);
        CheckServiceReply response;
        try
        {
            response = await client.BackSanitiyCheckAsync(new BaseServiceInfo {
                CallDateTime =  Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                ServiceName = nameof(SanityChecker),
                Ok = true,
                RequestTimeoutInSeconds = timeoutInSeconds
            }, deadline: DateTime.UtcNow.AddSeconds(timeoutInSeconds));
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            response = new CheckServiceReply {
                Ok = false,
                ServiceInfo = new BaseServiceInfo {
                    CallDateTime =  Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                    ServiceName = nameof(SanityChecker),
                    Ok = false,
                    StatusMessage = FailureMessage
                }
            };
        }
        catch (RpcException ex)
        {
            response = new CheckServiceReply {
                Ok = false,
                ServiceInfo = new BaseServiceInfo {
                    CallDateTime =  Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                    ServiceName = nameof(SanityChecker),
                    Ok = false,
                    StatusMessage = ex.Status.Detail
                }
            };
        }
        return response;
    }

}
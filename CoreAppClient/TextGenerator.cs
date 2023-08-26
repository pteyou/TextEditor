using Grpc.Core;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using TextEditor.Messages.gRPC;
using TextEditor.Configuration;

namespace TextEditor.CoreAppClient;

public class TextGenerator
{
    private static string _CoreHostname; 
    private static int _GrpcCorePort;
    private static int _timeoutInSeconds;

    
    static TextGenerator()
    {
        var config = Hosting.Instance.Config;
        _GrpcCorePort = config.GrpcCorePort;
        _CoreHostname = config.Hostname;
        _timeoutInSeconds = config.TextGeneratorTimeoutInSeconds;
    }

    public static async Task<TextGenerationReply> Call(
        string inputText, 
        int requiredExtraSize, 
        int numReturnedSequences)
    {
        var channel = GrpcChannel.ForAddress($"http://{_CoreHostname}:{_GrpcCorePort}");
        var client = new NLPTasks.NLPTasksClient(channel);
        TextGenerationReply response;
        try
        {
            response = await client.GenerateTextAsync(new TextGenerationRequest {
                ServiceInfo = new BaseServiceInfo {
                    CallDateTime =  Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                    ServiceName = nameof(TextGenerator),
                    Ok = true,
                    RequestTimeoutInSeconds = _timeoutInSeconds
                },
                InputText = inputText,
                RequiredExtraSize = requiredExtraSize,
                NumReturnedSequences = numReturnedSequences
            }, deadline: DateTime.UtcNow.AddSeconds(_timeoutInSeconds));
        }
        catch (RpcException ex)
        {
            response = new TextGenerationReply {
                ServiceInfo = new BaseServiceInfo {
                    CallDateTime =  Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                    ServiceName = nameof(TextGenerator),
                    Ok = false,
                    StatusMessage = ex.Status.Detail
                }
            };
        }
        return response;
    } 
}
import logging
from concurrent import futures
import grpc
import Protofiles.backend_pb2
import Protofiles.backend_pb2_grpc


class CheckServiceServicer(Protofiles.backend_pb2_grpc.CheckServiceServicer):
    def __init__(self):
        pass

    def BackSanitiyCheck(self, request, context):
        print("Received health check")
        service_reply = Protofiles.backend_pb2.CheckServiceReply()
        service_reply.ServiceInfo.Ok = True
        service_reply.ServiceInfo.ServiceName = "Core App Health Check"
        service_reply.ServiceInfo.CallDateTime.GetCurrentTime()

        service_reply.Ok = True
        return service_reply


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    Protofiles.backend_pb2_grpc.add_CheckServiceServicer_to_server(CheckServiceServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == "__main__":
    logging.basicConfig()
    serve()

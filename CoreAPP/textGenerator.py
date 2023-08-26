import grpc
import Protofiles.backend_pb2
import Protofiles.backend_pb2_grpc
import logging
from baseScripts import TextGeneratorBase

class NLPTasksServicer(Protofiles.backend_pb2_grpc.NLPTasksServicer):
    def __init__(self) -> None:
        pass

    def GenerateText(self, request, context):
        logging.info("Received textGen request")
        inputText = request.InputText
        requiredExtraSize = request.RequiredExtraSize
        textGenerator = TextGeneratorBase(request.NumReturnedSequences)
        return textGenerator.generateText(inputText, requiredExtraSize)

from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline
import string
import torch
import Protofiles.backend_pb2
import conf


def get_field(field_name: str, data):
    result = None
    err_message = ""
    try:
        result = data[field_name]
    except KeyError:
        err_message = f"ERROR : {field_name} not present in the data"
    except:
        err_message = f"ERROR : Impossible to extract {field_name} from the data"
    return result, err_message



class TextGeneratorBase:
    def __init__(self, numReturnedSeq: int) -> None:
        self.model_path = conf.gpt2_model_path
        self.task = "text-generation"
        self.outputField = "generated_text"
        self.numReturnedSequences = numReturnedSeq
        self.HTTP_ERR_CODE = 400
        self.HTTP_SUCCESS_CODE = 200
        self.tol_length = 5


    def run(self, inputText, **kwargs):
        device = torch.device("cuda:0")
        tokenizer = AutoTokenizer.from_pretrained(self.model_path)
        model = AutoModelForCausalLM.from_pretrained(self.model_path).to(device)
        if 'GPT2' in self.model_path:
            # specific to GPT2
            model.config.pad_token_id = model.config.eos_token_id

        generator = pipeline(task=self.task, model=model, tokenizer=tokenizer, device=device.index)
        result = generator(inputText, **kwargs)
        return result


    def generateText(self, inputText: string, requiredExtraSize: int) -> string:
        kwargs = {}
        kwargs['return_full_text'] = False
        kwargs['num_return_sequences'] = self.numReturnedSequences
        input_size = sum([i.strip(string.punctuation).isalnum() for i in inputText.split()])
        kwargs['max_length'] = requiredExtraSize + input_size + self.tol_length

        runResult = self.run(inputText, **kwargs)
        service_reply = Protofiles.backend_pb2.TextGenerationReply()
        service_reply.ServiceInfo.Ok = True
        service_reply.ServiceInfo.ServiceName = "Text Generation"
        service_reply.ServiceInfo.CallDateTime.GetCurrentTime()
        generatedsequences = []
        for res in runResult:
            outputText, errMessage = get_field(self.outputField, res)
            if len(errMessage):
                generatedsequences.append('ERROR')
            else:
                generatedsequences.append(outputText)
        service_reply.OuptutText.extend(generatedsequences)
        return service_reply

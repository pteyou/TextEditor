import flask
from flask import request, jsonify, json, make_response
from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline
import string
import torch

app = flask.Flask(__name__)


model_path_key = 'ModelPath'
task_key = 'Task'
input_key = "InputString"
output_size_key = "OutputSize"
output_field_key = "OutputField"
HTTP_ERR_CODE = 400
HTTP_SUCCESS_CODE = 400
tol_length = 5


def get_field(field_name: str, data):
    result = None
    err_message = ""
    try:
        result = data[field_name]
    except KeyError:
        err_message = f"ERROR : {field_name} not present in the input or output JSON file"
    except:
        err_message = f"ERROR : Impossible to extract {field_name} from the input or output JSON file"
    return result, err_message


def get_error_result(err_message: str):
    response = make_response(jsonify({'ErrorMessage': err_message}), HTTP_ERR_CODE)
    response.headers["Content-Type"] = "application/json"
    return response


def run(model_path: str, task: str, input_str: str, output_field: str, **kwargs):
    device = torch.device("cuda:0")
    tokenizer = AutoTokenizer.from_pretrained(model_path)
    model = AutoModelForCausalLM.from_pretrained(model_path).to(device)
    if 'GPT2' in model_path:
        # specific to GPT2
        model.config.pad_token_id = model.config.eos_token_id

    generator = pipeline(task=task, model=model, tokenizer=tokenizer, device=device.index)
    print(kwargs)
    result = generator(input_str, **kwargs)
    return get_field(output_field, result[0])


@app.route('/', methods=["GET"])
def sanity_check():
    response = make_response(jsonify({'Text': 'ok'}), HTTP_SUCCESS_CODE)
    response.headers["Content-Type"] = "application/json"
    return response


@app.route('/api/v1', methods=['POST'])
def generate():
    data = request.json
    # data = json.load(open('test.json'))
    model_path, err_message = get_field(model_path_key, data)
    if err_message:
        return get_error_result(err_message)
    task, err_message = get_field(task_key, data)
    if err_message:
        return get_error_result(err_message)
    input_str, err_message = get_field(input_key, data)
    if err_message:
        return get_error_result(err_message)
    output_field, err_message = get_field(output_field_key, data)
    if err_message:
        return get_error_result(err_message)

    kwargs = {}
    if task == "text-generation":
        kwargs['return_text'] = True
        kwargs['return_full_text'] = False
        output_size, err_message = get_field(output_size_key, data)
        if err_message:
            return get_error_result(err_message)
        input_size = sum([i.strip(string.punctuation).isalnum() for i in input_str.split()])
        kwargs['max_length'] = output_size + input_size + tol_length

    result, err_message = run(model_path, task, input_str, output_field, **kwargs)
    if err_message:
        return get_error_result(err_message)

    response = make_response(jsonify({'Text': result, 'Field': output_field}), HTTP_SUCCESS_CODE)
    response.headers["Content-Type"] = "application/json"
    return response


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8000, debug=True)
    # with app.app_context():
    #     app.config['JSONIFY_PRETTYPRINT_REGULAR'] = True
    #     print(json.dumps(generate().json))

import sys
from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline


def generate(model_path: str, task: str, input_str: str):
    tokenizer = AutoTokenizer.from_pretrained(model_path)
    model = AutoModelForCausalLM.from_pretrained(model_path)
    if 'GPT2' in model_path:
        # specific to GPT2
        model.config.pad_token_id = model.config.eos_token_id

    generator = pipeline(task=task, model=model, tokenizer=tokenizer)
    print(generator(input_str, max_length=50))


if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Error on arguments for the execution of BaseScript.py")
        exit(1)
    else:
        model_path = sys.argv[1]
        task = sys.argv[2]
        input_str = sys.argv[3]
        generate(model_path, task, input_str)


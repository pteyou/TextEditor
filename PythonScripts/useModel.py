from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline

model_path = r"D:\Transformers\Models\GPT2XL"

tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(model_path)

#specific to GPT2
model.config.pad_token_id = model.config.eos_token_id

generator = pipeline(task="text-generation", model=model, tokenizer=tokenizer)

print(generator(
    "Three Rings for the Elven-kings under the sky, Seven for the Dwarf-lords in their halls of stone",
    max_length=10
))

print("done")


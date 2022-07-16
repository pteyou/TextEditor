from transformers import AutoTokenizer, AutoModel

model_to_download = 'gpt2'
target_path = r"D:\Transformers\Models\GPT2"

tokenizer = AutoTokenizer.from_pretrained(model_to_download)
model = AutoModel.from_pretrained(model_to_download)

tokenizer.save_pretrained(target_path)
model.save_pretrained(target_path)

print('done')


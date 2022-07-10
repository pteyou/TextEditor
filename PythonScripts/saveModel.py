from transformers import AutoTokenizer, AutoModel

model_to_download = 'gpt2-xl'
target_path = r"C:\Users\pat\source\PythonHF\models\gpt2XL"

tokenizer = AutoTokenizer.from_pretrained(model_to_download)
model = AutoModel.from_pretrained(model_to_download)

tokenizer.save_pretrained(target_path)
model.save_pretrained(target_path)

print('done')


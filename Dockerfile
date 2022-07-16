FROM pytorch/pytorch:1.12.0-cuda11.3-cudnn8-runtime
RUN pip install transformers flask
RUN mkdir -p Models/GPT2
COPY ./Models/GPT2/* ./Models/GPT2/
EXPOSE 8000
CMD ["python", "app.py"]
COPY ./PythonScripts/EndPoints.py app.py
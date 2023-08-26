FROM huggingface/transformers-pytorch-gpu:4.29.2
WORKDIR /sources
RUN python3 -m pip install --upgrade pip
RUN pip install grpcio-tools
ADD HFModels/gpt2/* hfmodels/gpt2/
ADD CoreAPP/protoCommand.sh CoreAPP/
ADD Protofiles/* Protofiles/
WORKDIR /sources/CoreAPP
RUN bash protoCommand.sh
COPY CoreAPP/*.py ./
EXPOSE 50051
ENTRYPOINT [ "python3", "server.py" ]
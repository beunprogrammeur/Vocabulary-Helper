## how to make the neural translator work.

1. Download the translator model from [hugging face](https://huggingface.co/bartowski/aya-expanse-8b-GGUF)
2. Select the file `Q4_K_M` to download it; put the `aya-expanse-8b-Q4_K_M.gguf` file in this folder.
3. Make sure you have installed Docker (with WSL integration)
4. Run the "Boot translator" script in the Docker folder (one folder up from here).

After a bit of time, the API of the model should be live and start giving responses.
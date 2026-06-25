# Vocabulary-Helper
Helps creating anki decks from ebooks


## What you'll need:

* Anki installed
  * Anki-Connect plugin installed (and running)
* an ebook (epub file)

# The idea
Running the app allows you to load all deck (names) from anki.
Opening an ebook (or paste a news article etc.) will read the following from the book:
* each `word` in each `sentence`

This means that for learning the vocabulary in your book, you now have: words and example sentences.
The words are filtered by their unique-ness, so the same word won't appear multiple times.
In addition, a stemming algorithm (will be added soon) can be applied. this means that your words are first conjugated to their root form.

for instance in indonesian:
* bosan
* membosankan
* kebosanan

Are all the same word, just conjugated differently. by stemming, they all become `bosan` (and are therefore filtered/only appearing once)

using the `match` checkbox at the anki decks grid, we can select which decks to search through for duplicates. if a word has been studied before, we can automatically ignore it from the list. this becomes extra powerful on your 2nd book in the same language etc.


## LLM

Unfortunately, languages seem to be very diverse, to support both indonesian and korean, we would need 2 different algoritms, many of which all lean on LLMs or python scripts with a gazillion dependencies. Ultimately, I've chosen the route of locally running a 'languages specific' LLM which can be downloaded for free, running _on your machine_ without telemetry etc. I'm not the biggest fan of using LLMs like this, but for educational purposes (the anki decks), this doesn't sound half bad.

To start the server, follow the instructions in [Docker/models/help.md]
* download the model
* install docker
* run the powershell script

once the model is running AND you have installed Anki + the `AnkiConnect` plugin, the app will be able to talk to your anki app to read and insert cards etc.


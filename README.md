# Vocabulary-Helper
Helps creating anki decks from ebooks


## What you'll need:

* Anki installed
  * Anki-Connect plugin installed (and running)
* an ebook (epub file)

# The idea
Running the app allows you to load all deck (names) from anki.
Opening an ebook will read the following from the book:
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

after that, another TODO is a translation service integration. this allows for instance google translate to translate all not-ignored entries (words and example sentences) to english or whatever you desire.
at last, we choose a target deck and bulk insert our words in there.

Don't forget this is a lot of TODO's and this project is still at the early stages.

what we have so far:
* a window
* anki integration
    * read deck names
    * read card info (but we don't parse it yet)
* read words from an ebook
    * list the words in our grid

# TODO list
So the todo's are still:
* translate words
* stem / filter words
* import into anki deck

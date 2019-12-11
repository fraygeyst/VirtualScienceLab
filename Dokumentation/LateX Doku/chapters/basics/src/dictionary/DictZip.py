# chapters/basics/src/dictionary/DictZip.py
# Verwendung der zip-Methode

sprache = ["englisch", "deutsch", "franzoesisch"]
laender = ["England", "Deutschland", "Frankreich"]

laendersprache = dict(zip(laender, sprache))

print(laendersprache)
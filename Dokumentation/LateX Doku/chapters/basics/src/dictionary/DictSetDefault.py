# chapters/basics/src/dictionary/DictSetDefault.py
# Verwendung der setdefault-Methode

dictionary = {
    "k1": "v1",
    "k2": "v2",
    "k3": "v3"
}
x = dictionary.setdefault("k2", "v4")
print(x)
print(dictionary)
x = dictionary.setdefault("k4", "v5")
print(x)
print(dictionary)

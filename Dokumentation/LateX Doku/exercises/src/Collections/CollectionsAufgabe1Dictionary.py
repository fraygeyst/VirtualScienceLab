dictionary = dict(A="Alpha", L="Lima", E="Echo", X="Xray")
print(dictionary)
# Ausgabe: {'A': 'Alpha', 'L': 'Lima', 'E': 'Echo',
#           'X': 'Xray'}

print(dictionary.values())
# Augabe: dict_values(['Alpha', 'Lima', 'Echo', 'Xray'])

dictionary1 = dict(N="November", A="Alpha",
                   T="Tango", O="Oscar" )
dictionary2 = {}

for d in (dictionary, dictionary1):
    dictionary2.update(d)

print(dictionary2)
# Ausgabe: {'A': 'Alpha', 'L': 'Lima', 'E': 'Echo', 
# 'X': 'Xray', 'N': 'November', 'T': 'Tango', 'O': 'Oscar'}

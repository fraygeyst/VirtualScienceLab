# chapters/basics/src/set/SetSymDiffUpdate.py
# Verwendung der symmetric_difference_update-Methode

set1 = {1, 2, 3, 4, 5}
set2 = {4, 5, 6, 7, 8, 9, 10}
set1.symmetric_difference_update(set2)
print(set1)
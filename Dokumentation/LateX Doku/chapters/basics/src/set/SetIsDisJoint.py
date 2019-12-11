# chapters/basics/src/set/SetIsDisJoint.py
# Verwendung der isdisjoint-Methode

setX = {1, 2, 3, 4, 5}
setY = {6, 7, 8, 9, 10}
print(setX.isdisjoint(setY))  # Liefert True

setX = {1, 2, 3, 4, 5}
setY = {3, 4, 9, 5, 8, 7}
print(setX.isdisjoint(setY))  # Liefert False

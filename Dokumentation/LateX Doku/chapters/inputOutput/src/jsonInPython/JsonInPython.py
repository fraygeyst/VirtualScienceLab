# chapters/inputOutput/src/jsonInPython/JsonInPython.py

# JSON Package

import json

# JSON zu Python

student = '{"name":"Student", "age": 24, "height": 1.8}'
studentDict = json.loads(student)

print(student)
print(studentDict)
print(studentDict["height"])

# Ausgabe:
# {"name":"Student", "age":"24", "height":"1.8"}
# {'name': 'Student', 'age': '24', 'height': '1.8'}
# 1.8

# Python zu JSON

newStudent = {
    "name": "Neuer Student",
    "age": 26,
    "height": 2.0
}

newStudentJSON = json.dumps(newStudent)
print(newStudent)
print(newStudentJSON)

# Ausgabe:
# {'name': 'Neuer Student', 'age': 26, 'height': 2.0}
# {"name": "Neuer Student", "age": 26, "height": 2.0}

# JSON-string formatieren

demoStudent = {
    "name": "Harry",
    "age": 20,
    "height": 1.78,
    "assistant": False,
    "pets": ("Katze", "Maus"),
    "cars": None,
    "projects": [
        {"name": "Pythonkurs", "done": True},
        {"name": "Giraffen zaehmen", "done": False}
    ]
}

print(json.dumps(demoStudent))

# Ausgabe:
# {"name": "Harry", "age": 20, "height": 1.78 . . .}

print(json.dumps(demoStudent, indent=4,
                 sort_keys=True, separators=(" & ", " = ")))

# Ausgabe:
# {
#     "age" = 20 &
#     "assistant" = false &
#     "cars" = null &
#     "height" = 1.78 &
#     "name" = "Harry" &
#     "pets" = [
#         "Katze" &
#         "Maus"
#     ] &
#     "projects" = [
#         {
#             "done" = true &
#             "name" = "Pythonkurs"
#         } &
#         {
#             "done" = false &
#             "name" = "Giraffen zaehmen"
#         }
#     ]
# }

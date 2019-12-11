# variable Parameter
def greetMultiple(*targets):
    for target in targets:
        print("Hello, " + target)

greetMultiple("Max", "Rainer", 
        "Denis", "Frauke", 
        "Marc", "Melissa", "Nadine")
# chapters/nebenlaufigkeit/src/producer_consumer.py
# Producer-Consumer-Pattern mit Condition-Variablen

import threading


def an_item_is_available():
    # Method stub
    return True


def get_an_available_item():
    # Method stub
    return


def make_an_item_available():
    # Method stub
    return


cv = threading.Condition()

# Element konsumieren
with cv:
    while not an_item_is_available():
        cv.wait()
    get_an_available_item()

# Element produzieren
with cv:
    make_an_item_available()
    cv.notify()

# Element konsumieren
with cv:
    cv.wait_for(an_item_is_available)
    get_an_available_item()

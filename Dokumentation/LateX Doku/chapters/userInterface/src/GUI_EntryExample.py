# gui/GUI_EntryExample.py
# Einlesen und Ausgeben eines String ueber Entry

from tkinter import *

def printStringFromEntry():
    label_1["text"] = entry_1.get()

root = Tk()

entry_1 = Entry(root)
entry_1.pack()

button_1 = Button(root, text="Click me!",
                  command=printStringFromEntry)
button_1.pack()

label_1 = Label(root, text="")
label_1.pack()

root.mainloop()

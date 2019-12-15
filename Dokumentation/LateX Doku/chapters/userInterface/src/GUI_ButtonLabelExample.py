# gui/GUI_ButtonLabelExample.py
# Button der den Text eines Labels veraendert

from tkinter import *

def printHelloPythonWorld():
    label_1["text"] = "Hallo Python-World!"

root = Tk()

button_1 = Button(root, text="Click me!",
                  command=printHelloPythonWorld)
button_1.pack()

label_1 = Label(root, text="Hello World")
label_1.pack()

root.mainloop()

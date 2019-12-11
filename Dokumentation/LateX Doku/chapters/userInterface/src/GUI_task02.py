# gui/GUI_task02.py
# Loesung der Aufgabe 2 Kapitel Benutzeroberflaechen

from tkinter import *

root = Tk()

label = Label(root, text="Hello Python-World!").grid(row=0,
column=0)
label2 = Label(root, text="Label 2").grid(row=0, column=1)
label3 = Label(root, text="Label 3").grid(row=1, column=0)
label4 = Label(root, text="Label 4").grid(row=1, column=1)

mainloop()

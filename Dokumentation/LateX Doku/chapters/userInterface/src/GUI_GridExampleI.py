# gui/GUI_PackExampleIV.py
# Labels und Inputs anordnen - Beispiel im Grid-Manager

from tkinter import *

root = Tk()

labelRed = Label(root, text="Label One", bg="red",
                 fg="white").grid(row=0)
labelGreen = Label(root, text="Label Two", bg="green",
                   fg="black").grid(row=1)

e1 = Entry(root)
e2 = Entry(root)

e1.grid(row=0, column=1)
e2.grid(row=1, column=1)

mainloop()

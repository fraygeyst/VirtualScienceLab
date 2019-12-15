# gui/GUI_ListboxExample.py
# Einlesen und Hinzufuegen in Listbox

from tkinter import *

def addStringToList():
    if entry_1.get() != "":
        myList.insert(END, entry_1.get())

def removeItemFromList():
    selectedItem = myList.curselection()
    if len(selectedItem):
        myList.delete(selectedItem[0])

root = Tk()

entry_1 = Entry(root)
entry_1.pack()

button_frame = Frame(root)
button_frame.pack()

button_add = Button(button_frame, text="ADD",
                    command=addStringToList)
button_add.pack(side=LEFT)

button_remove = Button(button_frame, text="REMOVE",
                       command=removeItemFromList)
button_remove.pack(side=RIGHT)

myList = Listbox(root)
myList.pack()

root.mainloop()

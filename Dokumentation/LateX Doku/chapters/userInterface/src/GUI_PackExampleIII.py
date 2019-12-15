# gui/GUI_PackExampleIII.py
# Labels anordnen - Beispiel im Pack-Manager

from tkinter import *

root = Tk()

labelRed = Label(root, text="Red", bg="red", fg="white")
labelRed.pack()
labelGreen = Label(root, text="Green", bg="green", fg="black")
labelGreen.pack()
labelBlue = Label(root, text="Blue", bg="blue", fg="white")
labelBlue.pack()

mainloop()

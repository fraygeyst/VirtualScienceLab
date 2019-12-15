# gui/GUI_PackExampleIV.py
# Labels anordnen - Beispiel im Pack-Manager

from tkinter import *

root = Tk()

labelRed = Label(root, text="Red", bg="red", fg="white")
labelRed.pack(fill=X)
labelGreen = Label(root, text="Green", bg="green", fg="black")
labelGreen.pack(fill=X)
labelBlue = Label(root, text="Blue", bg="blue", fg="white")
labelBlue.pack(fill=X)

mainloop()

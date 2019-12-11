# gui/GUI_HelloWorld.py
# "Hello World" Ausgabe in einem Fenster

from tkinter import *

root = Tk()
root.title('tkinter Example - Hello World')
root.minsize(300,300)

label_1 = Label(root, text='Hello World')
label_1.grid()

root.mainloop()
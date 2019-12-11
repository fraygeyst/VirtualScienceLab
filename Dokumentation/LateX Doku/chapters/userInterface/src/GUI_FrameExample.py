# gui/GUI_FrameExample.py
# Beispiel eines Fensters mit Frames

from tkinter import *

root = Tk()
root.minsize(400,400)

headFrame = Frame(root, bg="red")
headFrame.pack(side=TOP, fill="both", expand=True)
centerFrame = Frame(root, bg="blue")
centerFrame.pack(fill="both", expand=True)
bottomFrame = Frame(root, bg="green")
bottomFrame.pack(side=BOTTOM, fill="both", expand=True)

label_head = Label(headFrame, text="HEAD")
label_head.pack()
label_center = Label(centerFrame, text="CENTER")
label_center.pack()
label_bottom = Label(bottomFrame, text="BOTTOM")
label_bottom.pack()

root.mainloop()
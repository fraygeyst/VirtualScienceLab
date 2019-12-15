# gui/GUI_Geometrics.py
# Geometrie Beispiel

from tkinter import *
from tkinter.colorchooser import *
from tkinter import messagebox
import tkinter
import random

# Klasse um geometrische Figur anzulegen
class Geo:
    def __init__(self, name, points, color):
        self.name = name
        self.points = points
        self.color = color

    def draw(self):
        canvas.create_polygon(self.points, fill=self.color)

#Funktion zum hinzufuegen von geometrischen Figuren
def addGeo():
    #Popup-Fenster anlegen
    popup = Tk()
    popup.wm_title("Add Geometrics")
    popup.geometry("340x180")
    popup.resizable(0, 0)

    #Popup in obere und untere Haelfte unterteilen
    headFrame = Frame(popup)
    headFrame.pack(side=TOP, fill="both", expand=True,
    padx=10, pady=10)

    bottomFrame = Frame(popup)
    bottomFrame.pack(side=BOTTOM, fill="both", padx=10,
    pady=10)

    #Funktion zum ueberpruefen der Eingabefelder anlegen
    def checkInput():
        if nameEntry.index("end") == 0:
            messagebox.showinfo("Error",
            "Please insert a name")
        else:
            if colorFrame['bg'] == "white":
                messagebox.showinfo("Error",
                "Please choose a color")
            else:
                createGeo(nameEntry.get(), colorFrame['bg'])

    #Funktion zum anzeigen der ausgewaehlten Farbe anlegen
    def getColor():
        color = askcolor()
        hex_tuple = color[1]
        colorFrame['bg'] = hex_tuple

    #Funktion zum erstellen von neuer geometrischer Form
    #mit ausgewaehlten Parametern
    def createGeo(name, color):
        points = []
        for i in range(numberPoints.get()):
            points.append(random.randint(0,350))
            points.append(random.randint(0,350))
        item = Geo(name, points, color);
        geometrics.append(item)
        listbox.delete(0, END)
        popup.destroy()
        for item in geometrics:
            listbox.insert(END, item.name)

    #Label-Widget, Eingabe-Widget, Farbauswahl-Widget und
    #Schieberegler-Widget anlegen und in obere Haelfte des
    #Popup-Fensters einfuegen
    nameLabel = Label(headFrame, text="Name:")
    nameEntry = Entry(headFrame)
    colorLabel = Label(headFrame, text="Color:")
    colorFrame = Frame(headFrame, bg="white",
    highlightbackground="gray", highlightthickness=1,
    height=20, width=20)
    colorPicker = Button(headFrame, text="Select Color",
    command=getColor)
    numberPointsLabel = Label(headFrame,
    text="Number of Points:")
    numberPoints = Scale(headFrame, length=180, from_=3,
    to=10, orient=HORIZONTAL)

    nameLabel.grid(row=0, column=0, sticky=W)
    nameEntry.grid(row=0, column=1, sticky=E)
    colorLabel.grid(row=1, column=0, sticky=W)
    colorFrame.grid(row=1, column=1, sticky=W, padx=2)
    colorPicker.grid(row=1, column=1, sticky=E)
    numberPointsLabel.grid(row=2, column=0, sticky=SW)
    numberPoints.grid(row=2, column=1, sticky=E)

    #Button-Widgets zum uebernehmen der Eingabewerte bzw
    #abbrechen anlegen und in untere Haelfte des Popup-
    #Fensters einfuegen
    okButton = Button(bottomFrame, text="Okay", command=
    checkInput)
    cancelButton = Button(bottomFrame, text="Cancel",
    command = popup.destroy)
    okButton.pack(side=RIGHT)
    cancelButton.pack(side=RIGHT)

    #Eventschleife des Popup-Fensters
    popup.mainloop()

#Funktion zum saeubern der Zeichenflaeche anlegen
def clearCanvas():
    canvas.delete(ALL)

#Funktion zum zeichnen der ausgewaehlten geometrischen Figur
#in dem Listbox-Widget anlegen
def curSelect():
    clearCanvas()
    selection = listbox.curselection()
    if len(selection):
        picked = listbox.get(selection[0])
        for item in geometrics:
            if picked == item.name:
                item.draw()

#Funktion zum entfernen von geometrischen Figuren aus
#dem Listbox-Widget anlegen
def removeItemFromList():
    selection = listbox.curselection()
    if len(selection):
        listbox.delete(selection[0])
        geometrics.pop(selection[0])
        clearCanvas()


# Fenster anlegen
root = Tk()
root.title('tkinter Geometrics')
root.minsize(640,400)

#Fenster in linke und rechte Haelfte unterteilen
leftFrame = Frame(root)
leftFrame.pack(side=LEFT, fill="both", expand=True,
padx=10, pady=10)

rightFrame = Frame(root)
rightFrame.pack(side=RIGHT, fill="both", expand=True,
padx=10, pady=10)

#Listbox-Widget anlegen und in linke Haelfte einfuegen
listbox = Listbox(leftFrame)
listbox.pack(fill="both", expand=1)

#Zwei geometrische Figuren anlegen
square = Geo("Square", [120, 120, 120, 220, 220, 220, 220,
120], "yellow")
triangle = Geo("Triangle", [120, 120, 220, 220, 220, 120],
"blue")
geometrics = [square, triangle]

#Figuren dem Listbox-Widget uebergeben
for item in geometrics:
    listbox.insert(END, item.name)

#Button zum hinzufuegen und entfernen von geometrischen
#Figuren anlegen und in linke Haelfte einfuegen
addButton = Button(leftFrame, text="Add", command=addGeo)
removeButton = Button(leftFrame, text="Remove", command=
removeItemFromList)

addButton.pack(side=RIGHT)
removeButton.pack(side=RIGHT)

#Canvas-Widget anlegen und in rechte Haelfte einfuegen
canvas = Canvas(rightFrame, highlightbackground="black",
highlightthickness=1)
canvas.pack(fill="both", expand=True)

#Button zum zeichnen und loeschen der geometrischen Figuren
#anlegen und in rechte Haelfte einfuegen
drawButton = Button(rightFrame, text="Draw", command=
curSelect)
clearButton = Button(rightFrame, text="Clear", command=
clearCanvas)

drawButton.pack(side=LEFT)
clearButton.pack(side=LEFT)

#Eventschleife des Hauptfensters
mainloop()

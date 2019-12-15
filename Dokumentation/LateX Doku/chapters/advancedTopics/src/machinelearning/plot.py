# Bibliotheken einbinden
import matplotlib.pyplot as pyplot
from sklearn import datasets

def plotplot():
    # Daten laden
    dt_iris = datasets.load_iris()
    iris = dt_iris.data[:, :2]

    # Plot definieren
    pyplot.plot(iris)
    pyplot.title("I'm the title of the plot")
    pyplot.xlabel("X-Label")
    pyplot.ylabel("Y-Label")

    # Plot anzeigen
    pyplot.show()

plotplot()
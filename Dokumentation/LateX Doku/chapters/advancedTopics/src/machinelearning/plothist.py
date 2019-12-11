# Bibliotheken einbinden
import matplotlib.pyplot as pyplot
from sklearn import datasets

def plothist():
    # Daten laden
    dt_iris = datasets.load_iris()
    iris = dt_iris.data[:, :4]

    # Plot definieren
    pyplot.hist(iris)
    pyplot.title("I'm the title of the histogram")
    pyplot.xlabel("X-Label")
    pyplot.ylabel("Y-Label")

    # Plot anzeigen
    pyplot.show()

plothist()
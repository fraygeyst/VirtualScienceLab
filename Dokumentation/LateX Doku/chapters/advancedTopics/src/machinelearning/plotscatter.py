# Bibliotheken einbinden
import matplotlib.pyplot as pyplot
from sklearn import datasets

def plotscatter():
    # Daten laden
    dt_iris = datasets.load_iris()
    dataSet = dt_iris.data[:, :3]

    # Plot definieren
    fig = pyplot.figure()
    ax = fig.add_subplot(111)

    # Plot mit Daten versorgen
    ax.scatter(dataSet[:, 0], dataSet[:, 1], dataSet[:, 2],
               marker='o')

    # Achsenbeschriftungen festlegen
    ax.set_title("I'm a scatter plot")
    ax.set_xlabel("X-Label")
    ax.set_ylabel("Y-Label")

    # Plot anzeigen
    pyplot.show()

# Aufruf
plotscatter()

import numpy as numpy
from sklearn import datasets
from scipy.stats import linregress
import matplotlib.pyplot as plt

# Daten laden
dt = datasets.load_boston()
boston = dt.data[:, :4]
datasetny = numpy.array(boston)

# Lineare Regression berechnen
b, a, r, p, std = linregress(datasetny[2,], datasetny[1,])

# Plot erzeugen
plt.scatter(datasetny[2,], datasetny[1,])
plt.plot([0, 130], [a, a + 130 * b], c="red", alpha=0.5)
plt.plot()
plt.title("LinRegress")
plt.xlim(0, 120)
plt.ylim(0, 800)
plt.xlabel("X-Label")
plt.ylabel("Y-Label")
plt.grid(alpha=0.4)
plt.xticks([x for x in range(42) if x % 10 == 0])
plt.yticks([x for x in range(42) if x % 100 == 0])
plt.show()

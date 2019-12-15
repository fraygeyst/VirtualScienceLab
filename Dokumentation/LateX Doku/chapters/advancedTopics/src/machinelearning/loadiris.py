# https://scikit-learn.org/stable/modules/generated/sklearn.datasets.load_iris.html

import numpy as numpy
from sklearn import datasets

# Daten laden
dt_iris = datasets.load_iris()
iris = dt_iris.data[:, :4]

# targets von 10, 25 und 50
t = dt_iris.target[[10, 25, 50]]
print(t)

# target_names
tn = list(dt_iris.target_names)
print(tn)

# In numpy Array unwandeln
dataasny = numpy.array(iris)

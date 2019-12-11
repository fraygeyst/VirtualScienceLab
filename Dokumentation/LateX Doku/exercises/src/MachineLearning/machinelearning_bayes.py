# Import
from sklearn.naive_bayes import GaussianNB
from sklearn import datasets
from sklearn.model_selection import train_test_split
import numpy as numpy


# Daten laden
dt = datasets.load_wine()
wineX = dt.data[-1:]
wineY = dt.target_names
datasetX = numpy.array(wineX)
datasetY = numpy.array(wineY)

# Gaussian Classifier erzeugen
model = GaussianNB()

# Train the model using the training sets
docs_train, docs_test, y_train, y_test = train_test_split(
	wineY, wineY, test_size=0.25, random_state=None)
model.fit(datasetX, datasetY)

# Vorhersage
predicted = model.predict([[1, 2], [3, 4]])
print(predicted)

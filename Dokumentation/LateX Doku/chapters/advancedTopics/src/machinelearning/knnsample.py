# https://stackabuse.com/k-nearest-neighbors-algorithm-in-
# python-and-scikit-learn/

# Importing Libraries
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import classification_report, 
	confusion_matrix

# Importing the Dataset
url = "https://archive.ics.uci.edu/ml/machine-learning-
	databases/iris/iris.data"

# Assign colum names to the dataset
names = ['sepal-length', 'sepal-width', 'petal-length', 
	'petal-width', 'Class']

# Read dataset to pandas dataframe
dataset = pd.read_csv(url, names=names)

# Preprocessing
X = dataset.iloc[:, :-1].values
y = dataset.iloc[:, 4].values

# training and test splits
X_train, X_test, y_train, y_test = 
	train_test_split(X, y, test_size=0.20)

# feature scaling:
scaler = StandardScaler()
scaler.fit(X_train)
X_train = scaler.transform(X_train)
X_test = scaler.transform(X_test)

# training und predictions
classifier = KNeighborsClassifier(n_neighbors=5)
classifier.fit(X_train, y_train)

# make predictions
y_pred = classifier.predict(X_test)

# print matrix
print(confusion_matrix(y_test, y_pred))
# print report
print(classification_report(y_test, y_pred))

# define error array
error = []

# calculate the mean of error for all the predicted
# values where K ranges from 1 and 40
# Calculating error for K values between 1 and 40
for i in range(1, 40):
    knn = KNeighborsClassifier(n_neighbors=i)
    knn.fit(X_train, y_train)
    pred_i = knn.predict(X_test)
    error.append(np.mean(pred_i != y_test))


# plot the error values against K values
plt.figure(figsize=(12, 6))
plt.plot(range(1, 40), error, color='red', 
		 linestyle='dashed', marker='o',
         markerfacecolor='blue', markersize=10)
plt.title('Error Rate K Value')
plt.xlabel('K Value')
plt.ylabel('Mean Error')
plt.show()

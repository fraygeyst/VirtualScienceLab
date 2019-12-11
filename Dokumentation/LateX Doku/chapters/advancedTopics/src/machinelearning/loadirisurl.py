import pandas as pd

# Importing the Dataset
url = "https://archive.ics.uci.edu/ml/machine-
	learning-databases/iris/iris.data"

# Assign colum names to the dataset
names = ['sepal-length', 'sepal-width', 
	'petal-length', 'petal-width', 'Class']

# Read dataset to pandas dataframe
dataset = pd.read_csv(url, names=names)

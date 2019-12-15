from numpy import loadtxt
from urllib.request import urlopen

url = 'https://raw.githubusercontent.com/jbrownlee/
	Datasets/master/pima-indians-diabetes.data.csv'
raw_data = urlopen(url)
dataset = loadtxt(raw_data, delimiter=",")

print(dataset.shape)
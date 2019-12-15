import pandas

url = "https://raw.githubusercontent.com/jbrownlee/
	Datasets/master/pima-indians-diabetes.data.csv"
names = ['preg', 'plas', 'pres', 'skin', 'test', 
	'mass', 'pedi', 'age', 'class']
data = pandas.read_csv(url, names=names)
print(data.shape)
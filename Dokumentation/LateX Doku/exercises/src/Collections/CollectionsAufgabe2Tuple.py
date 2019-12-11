t1 = (2, 3)
tmpList = list(t1)
tmpList.append(4)
tmpList.append(5)
t1 = tuple(tmpList)
print(t1)
# Ausgabe: (2, 3, 4, 5)

x = ("hallo", 1, 2.1, False, "string")
y = []
for i in x:
    y.append(str(i))

x = tuple(y)
print(x)
# Ausgabe: ('hallo', '1', '2.1', 'False', 'string')

for i in x:
    print(i + " ,is type string: " + str(type(i) is str))
# Ausgabe:
# hallo ,is type string: True
# 1 ,is type string: True
# 2.1 ,is type string: True
# False ,is type string: True
# string ,is type string: True


def getGeometryWithMostCorners(*geoBodys):
    global geometryWithMostCorners
    geometryWithMostCorners = geoBodys[0]
    for x in geoBodys:
        if(geometryWithMostCorners[0] < x[0]):
            geometryWithMostCorners = x

getGeometryWithMostCorners(
            (3, "Dreieck"),
            (8, "Achteck"),
            (5, "Fuenfeck"))
print(geometryWithMostCorners[1])
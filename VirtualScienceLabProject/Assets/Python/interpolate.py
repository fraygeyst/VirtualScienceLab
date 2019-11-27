# -*- coding: utf-8 -*-
"""
Beispiel für die kubische Interpolation von Werten

Wir geben die Daten und die berechnete Funktion grafisch
aus und exportieren einen linearen Polygonzug in eine
csv-Datei.

Aktuell werden zwei Interpolationslösungen implementiert.
Einmal eine kubische Interpolation und eine Interpolation
mit Hilfe einer Spline-Funktion!
"""

import numpy as np
from scipy import interpolate
import matplotlib.pyplot as plt

class Greeter():
    lut = np.genfromtxt(fname='data.csv', delimiter=';', dtype=float)

    # x und y Felder aus look up table ziehen
    x = lut[:, 0]
    y = lut[:, 1]

    cubic_poly = interpolate.interp1d(x, y, kind='cubic')
    cubic_spline = interpolate.splrep(x,y, s=0)

    # Ergebnis ausgeben
    x2 = np.linspace(0, 6, 100)
    y2 = cubic_poly(x2)
    yspline = interpolate.splev(x2, cubic_spline, der=0)

    fig, axs = plt.subplots(2)

    style = 'seaborn'
    plt.style.use(style)

    axs[0].set_title('Kubische Interpolation')
    axs[0].plot(x2, y2, 'tab:orange')
    axs[0].plot(x, y, 'o', color='C0')

    axs[1].set_title('Spline-Interpolation')
    axs[1].plot(x2, yspline, 'tab:green')
    axs[1].plot(x, y, 'o', color='C0')

    for ax in axs.flat:
        ax.set(xlabel='x', ylabel='y')
        ax.label_outer()
    
    # Ergebnis der Spline-Interpolation in den Feldern x2 und y2 mit np.savetxt ausgeben
    export_data = np.array([(x,y) for x,y in zip(x2, yspline)])
    export_data

    np.savetxt('interpolate.csv', export_data, delimiter=';',  fmt='%f')

# chapters/nebenlaufigkeit/src/lock_aquire_release.py
# Beispiel zur Verwendung von Lock-Objekten

import threading

lock = threading.Lock()

lock.aquire()
try:
    # kritischer Code
finally:
    lock.release()

with lock:
    # kritischer Code

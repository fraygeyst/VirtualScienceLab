# chapters/nebenlaufigkeit/src/semaphore.py
# Beispiel zur Verwendung von Semaphore-Objekten

import threading

# Initialisierung der Semaphore
maxconnections = 5
# ...
pool_sema = threading.BoundedSemaphore(value=maxconnections)


# Zugriff auf die Datenbank in den Arbeiterthreads
with pool_sema:
    conn = connectdb()
    try:
        # ... Verbindung nutzen ...
    finally:
        conn.close()

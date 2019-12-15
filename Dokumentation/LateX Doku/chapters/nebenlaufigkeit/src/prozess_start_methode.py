# chapters/nebenlaufigkeit/src/prozess_start_methode.py
# Wahl einer Prozess-Startmethode

import multiprocessing


def foo():
    # Methode-Stub
    return


if __name__ == "__main__":
    multiprocessing.set_start_method("spawn")
    process = multiprocessing.Process(target=foo)
    process.start()
    process.join()

if __name__ == "__main__":
    context = multiprocessing.get_context("spawn")
    process = context.Process(target=foo)
    process.start()
    process.join()

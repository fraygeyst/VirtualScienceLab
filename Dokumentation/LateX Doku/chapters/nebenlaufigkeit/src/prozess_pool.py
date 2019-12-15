# chapters/nebenlaufigkeit/src/prozess_pool.py
# Beispiel zu Prozess-Pools

import multiprocessing


def square(x):
    return x * x


if __name__ == "__main__":
    with multiprocessing.Pool(5) as p:
        results = p.map(square, range(1, 11))
        print(results)

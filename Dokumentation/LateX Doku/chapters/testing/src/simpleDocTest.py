# simpleDocTest.py
# Einfaches Beispiel eines DocTests in Python mit Ausgabe

"""
Outside of a function
>>> area(5, 5)
25
"""


def area(x, y):
    """Inside of a function
    Return the area of an recangle.

    >>> area(1, 2)
    2

    >>> area(0, 2)
    Traceback (most recent call last):
        ...
    ValueError: x must be > 0

    >>> area(2, 0)
    Traceback (most recent call last):
        ...
    ValueError: y must be > 0
    """

    if not x > 0:
        raise ValueError("x must be > 0")
    if not y > 0:
        raise ValueError("y must be > 0")
    return x * y


if __name__ == "__main__":
    import doctest
    doctest.testmod()

"""
Trying:
    area(5, 5)
Expecting:
    25
ok
Trying:
    area(1, 2)
Expecting:
    2
ok
Trying:
    area(0, 2)
Expecting:
    Traceback (most recent call last):
        ...
    ValueError: x must be > 0
ok
Trying:
    area(2, 0)
Expecting:
    Traceback (most recent call last):
        ...
    ValueError: y must be > 0
ok
2 items passed all tests:
   1 tests in __main__
   3 tests in __main__.area
4 tests in 2 items.
4 passed and 0 failed.
Test passed.
"""

"""
**************************************************
File "simpleDocTest.py", line 6, in __main__
Failed example:
    area(5, 5)
Expected:
    25
Got:
    10
**************************************************
File "simpleDocTest.py", line 14, in __main__.area
Failed example:
    area(1, 2)
Expected:
    2
Got:
    3
**************************************************
2 items had failures:
   1 of   1 in __main__
   1 of   3 in __main__.area
***Test Failed*** 2 failures.
"""

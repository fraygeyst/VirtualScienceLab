# exercises/src/testen02.py
# Beispielimplementierung f�r die Tests in Aufgabe 2

import unittest
from testmod import testDiv


class testen02(unittest.TestCase):

    def test_div(self):
        self.assertEqual(testDiv(5, 2), 2)

    def test_sub(self):
        with self.assertRaises(ZeroDivisionError):
            testDiv(2, 0)


if __name__ == '__main__':
    unittest.main()

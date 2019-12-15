# simpleTest.py
# einfacher Test in Python

import unittest


class SimpleTest(unittest.TestCase):

    def test_add(self):
        self.assertEqual(5 + 5, 10)

    def test_sub(self):
        self.assertTrue(15 - 5 == 10)

    def test_neg(self):
        self.assertFalse(5 - 5 == 9)

    def test_exception(self):
        with self.assertRaises(TypeError):
            3 - "hello"


if __name__ == '__main__':
    unittest.main()

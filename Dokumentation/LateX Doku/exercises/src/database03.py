# \exercises\src\database03
import sqlite3

connection = sqlite3.connect("person.db")
cursor = connection.cursor()

with connection:

    cursor.execute("CREATE TABLE IF NOT EXISTS \
    person(vname VARCHAR(20),nname VARCHAR(30))")

    sql_insert = """INSERT INTO person (vname, nname)
    VALUES ("Peter", "Maffay")"""

    cursor.execute(sql_insert)
    sql_insert = """INSERT INTO person (vname, nname)
    VALUES ("Barack", "Obama")"""
    cursor.execute(sql_insert)
    sql_command = """SELECT * FROM person;"""

    cursor.execute(sql_command)
    print(cursor.fetchall())
connection.commit()
connection.close()

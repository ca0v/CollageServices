import sqlalchemy
from sqlalchemy.orm import sessionmaker
from sqlalchemy import create_engine
from sqlalchemy import Column, Integer, String, MetaData, Table, ForeignKey, DateTime, Boolean, Text, Float


class User(object):
    def __init__(self, username, email, password):
        self.username = username
        self.email = email
        self.password = password

    def __repr__(self):
        return


# create metadata
metadata = MetaData()

# create table
users = Table('users', metadata,
              Column('id', Integer, primary_key=True),
              Column('username', String(50), unique=True),
              Column('email', String(120), unique=True),
              Column('password', String(120)),
              )

# create engine
engine = create_engine('sqlite:///users.sqlite', echo=True)

# create tables
metadata.create_all(engine)

# create a Session
Session = sessionmaker(engine)

# map Users to the users table
sqlalchemy.orm.mapper(User, users)

# create a user
with Session() as session:
    user = User(username='admin', email='admin@localhost', password='admin')
    session.add(user)
    session.commit()

# close the engine
engine.dispose()

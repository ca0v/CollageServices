from flask_sqlalchemy import SQLAlchemy
from dataclasses import dataclass
from flask_restful import Resource, Api, reqparse
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
from sqlalchemy import create_engine
from sqlalchemy import Column, Integer, String, MetaData, Table, ForeignKey, DateTime, Boolean, Text, Float

# start a webapi
app = Flask(__name__)

# Either 'SQLALCHEMY_DATABASE_URI' or 'SQLALCHEMY_BINDS' must be set
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///users.sqlite'

api = Api(app)
db = SQLAlchemy(app)


@dataclass
class User(db.Model):
    __tablename__ = "users"
    id: int
    email: str

    id = db.Column(db.Integer, primary_key=True, auto_increment=True)
    email = db.Column(db.String(200), unique=False)


@ app.route('/users/')
def users():
    users = User.query.all()
    return jsonify(users)


with app.app_context():
    db.create_all()
    users = User.query.all()
    print(users)

# start the server
app.run(debug=True)

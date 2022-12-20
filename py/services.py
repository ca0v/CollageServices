from flask_sqlalchemy import SQLAlchemy
from dataclasses import dataclass
from flask_restful import Api
from flask import Flask, jsonify, request

from flask import Flask, render_template, redirect, url_for
from flask_bootstrap import Bootstrap
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired


class NameForm(FlaskForm):
    name = StringField('Which actor is your favorite?',
                       validators=[DataRequired()])
    submit = SubmitField('Submit')


# start a webapi
app = Flask(__name__)

# Flask-WTF requires an encryption key - the string can be anything
app.config['SECRET_KEY'] = 'C2HWGVoMGfNTBsrYQg8EcMrdTimkZfAb'

# Flask-Bootstrap requires this line
Bootstrap(app)

# Either 'SQLALCHEMY_DATABASE_URI' or 'SQLALCHEMY_BINDS' must be set
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///users.sqlite'

api = Api(app)
db = SQLAlchemy(app)


@app.route('/', methods=['GET', 'POST'])
def index():
    users = User.query.all()
    form = NameForm()
    message = ""
    if form.validate_on_submit():
        name = form.name.data
        message = f"Hello {name}"
    return render_template('index.html', users=users, form=form, message=message)


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


@ app.route('/users/<int:id>')
def user(id):
    user = User.query.filter_by(id=id).first()
    return jsonify(user)


@ app.route('/users/<int:id>', methods=['DELETE'])
def delete_user(id):
    user = User.query.filter_by(id=id).first()
    db.session.delete(user)
    db.session.commit()
    return jsonify(user)


@ app.route('/users/', methods=['POST'])
def create_user():
    data = request.get_json()
    user = User(email=data['email'])
    db.session.add(user)
    db.session.commit()
    return jsonify(user)


with app.app_context():
    db.create_all()
    users = User.query.all()
    print(users)


# start the server
app.run(debug=True)

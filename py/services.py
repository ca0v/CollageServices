import json
from flask_sqlalchemy import SQLAlchemy
from dataclasses import dataclass
from flask_restful import Api
from flask import Flask, jsonify, request

from flask import Flask, render_template, redirect, url_for
from flask_bootstrap import Bootstrap
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired

# start a webapi
app = Flask(__name__)

# Flask-WTF requires an encryption key - the string can be anything
app.config['SECRET_KEY'] = "fdd89hf3809fdjkhidf409ruvn-0q325873-4 hfg"

# Flask-Bootstrap requires this line
Bootstrap(app)

# Either 'SQLALCHEMY_DATABASE_URI' or 'SQLALCHEMY_BINDS' must be set
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///users.sqlite'

api = Api(app)
db = SQLAlchemy(app)


@app.route('/', methods=['GET', 'POST'])
def index_ux():
    return render_template('index.html')


@app.route('/collage.html', methods=['GET', 'POST'])
def collage_ux():
    if request.method == 'POST':

        # get the data from the form
        id = request.form['id']
        note = request.form['note']
        title = request.form['title']

        collage = Collage(id=id, note=note, title=title)
        db.session.add(collage)
        db.session.commit()
    return render_template('collage.html')


@dataclass
class Collage(db.Model):
    __tablename__ = "collages"

    id: str
    data: str
    note: str
    title: str

    id = db.Column(db.Text, primary_key=True)
    data = db.Column(db.Text)
    note = db.Column(db.Text)
    title = db.Column(db.Text)


@ app.route('/collage/')
def collages():
    result = Collage.query.all()
    # limit what is transmitted to just ids
    # result = [{"id": x.id, "title": x.title, "note": x.note} for x in result]
    result = [r.id for r in result]
    return jsonify(result)


@ app.route('/collage/<string:id>')
def getCollage(id):
    result = Collage.query.filter_by(id=id).first()

    data = result.data
    # parse the json data
    data = json.loads(data)
    # return the data
    result.data = data

    if result is None:
        return jsonify({"error": "collage not found"}), 404
    return jsonify(result)


@ app.route('/collage/', methods=['POST'])
def create_collage():
    data = request.get_json()
    collage = Collage(id=data['id'], data=data['data'],
                      note=data['note'], title=data['title'])
    db.session.add(collage)
    db.session.commit()
    return jsonify(collage)


with app.app_context():
    db.create_all()

# start the server
app.run(debug=True)

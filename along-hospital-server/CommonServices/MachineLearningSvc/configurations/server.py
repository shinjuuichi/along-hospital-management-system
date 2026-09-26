from flask import Flask
from .configs import Config
from .extensions import db, cors, migrate
from controllers import list_blueprints
from middlewares import register_exception_handlers


def create_app():
    app = Flask(__name__)
    app.config.from_object(Config)

    cors.init_app(app)
    db.init_app(app)
    migrate.init_app(app, db)

    for blueprint in list_blueprints:
        app.register_blueprint(blueprint)

    register_exception_handlers(app)
    
    return app
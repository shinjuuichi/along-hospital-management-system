from flask import Flask, request, jsonify

def register_exception_handlers(app: Flask):

    @app.errorhandler(Exception)
    def handle_exception(e):
        message = str(e)

        app.logger.error(
            f"[{request.method}] {request.path} ::: {message}"
        )

        return jsonify({
            "error": message,
        }), 500
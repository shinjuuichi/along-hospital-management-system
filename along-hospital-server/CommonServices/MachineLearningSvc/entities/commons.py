from sqlalchemy.types import TypeDecorator, Integer

class IntEnum(TypeDecorator):
    impl = Integer
    cache_ok = True

    def __init__(self, enumclass, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.enumclass = enumclass

    def process_bind_param(self, value, dialect):
        if value is None:
            return None
        if isinstance(value, self.enumclass):
            return value.value
        if isinstance(value, int):
            return value
        raise Exception(f"Invalid value for {self.enumclass.__name__}: {value!r}")

    def process_result_value(self, value, dialect):
        if value is None:
            return None
        return self.enumclass(value)
from sqlalchemy import Integer
from sqlalchemy.orm import Mapped, mapped_column
from configurations.extensions import db
from enum import Enum
from .commons import IntEnum


class FeedbackTypeEnum(Enum):
    Positive = 1
    Neutral = 2
    Negative = 3


class Feedback(db.Model):
    __bind_key__ = "feedback"
    __tablename__ = "feedback"

    id: Mapped[int] = mapped_column(Integer, primary_key=True)

    content: Mapped[str] = mapped_column(
        db.String(1000), name="Content", nullable=False, default=""
    )

    rating: Mapped[float] = mapped_column(db.Float, name="Rating", nullable=False)

    feedbackTypeEnum: Mapped[FeedbackTypeEnum] = mapped_column(
        IntEnum(FeedbackTypeEnum),
        name="FeedbackTypeEnum",
        nullable=False,
        default=FeedbackTypeEnum.Neutral,
    )

    is_ban: Mapped[bool] = mapped_column(
        db.Boolean, name="IsBan", nullable=False, default=False
    )

    def __repr__(self):
        return (
            f"<Feedback id={self.id}, "
            f"content={self.content}, "
            f"status={self.feedbackStatusEnum.name}, "
            f"is_ban={self.is_ban}>"
        )
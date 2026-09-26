from sqlalchemy import Integer, String
from sqlalchemy.orm import Mapped, mapped_column
from configurations.extensions import db
from enum import Enum
from .commons import IntEnum


class ComplaintTopicEnum(Enum):
    Service = 0
    Billing = 1
    Doctor = 2
    Medicine = 3
    Others = 4


class ComplaintTypeEnum(Enum):
    Neutral = 0
    Positive = 1
    Negative = 2



class Complaint(db.Model):
    __bind_key__ = "medical_history"
    __tablename__ = "complaint"

    id: Mapped[int] = mapped_column(Integer, primary_key=True)

    complaint_topic: Mapped[ComplaintTopicEnum] = mapped_column(
        IntEnum(ComplaintTopicEnum),
        name="ComplaintTopic",
        nullable=False,
        default=ComplaintTopicEnum.Others,
    )

    content: Mapped[str] = mapped_column(String(1000), nullable=False, default="")

    complaint_type: Mapped[ComplaintTypeEnum] = mapped_column(
        IntEnum(ComplaintTypeEnum),
        name="ComplaintType",
        nullable=False,
        default=ComplaintTypeEnum.Neutral,
    )

    def __repr__(self):
        return (
            f"<Complaint id={self.id}, "
            f"content={self.content}, "
            f"topic={self.complaint_topic.name}, "
            f"type={self.complaint_type.name}>"
        )

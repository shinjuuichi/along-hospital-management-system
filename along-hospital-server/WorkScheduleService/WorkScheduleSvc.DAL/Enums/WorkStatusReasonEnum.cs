namespace WorkScheduleSvc.DAL.Enums
{
    public enum WorkStatusReasonEnum
    {
        WorkedFromAttendance = 0,
        NoAttendanceLog = 1,
        MissingCheckIn = 2,
        MissingCheckOut = 3,
        InvalidAttendanceRange = 4,
        LateArrival = 5,
        EarlyLeave = 6
    }
}

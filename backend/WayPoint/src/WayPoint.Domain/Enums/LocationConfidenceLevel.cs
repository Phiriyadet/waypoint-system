namespace WayPoint.Domain.Enums;

public enum LocationConfidenceLevel
{
    Low = 0,        //-- 0-30 score
    Medium = 1,     //-- 31-60 score
    High = 2,       //-- 61-90 score
    Verified = 3   //-- 91-100 score
}

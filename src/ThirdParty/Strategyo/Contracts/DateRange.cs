namespace Strategyo.Contracts;

public readonly struct DateRange(DateTime startDate, DateTime endDate) : IEquatable<DateRange>
{
    public DateRange() : this(DateTime.UtcNow, DateTime.UtcNow.AddDays(1))
    {
    }

    public DateRange(DateOnly startDate, DateOnly endDate) : this(DateTime.Parse(startDate.ToString()), DateTime.Parse(endDate.ToString()))
    {
    }
    
    public DateTime Start { get; init; } = startDate;
    public DateTime End { get; init; } = endDate;

    public static DateRange Create() 
        => new();
    
    public static DateRange Create(DateTime startDate, DateTime endDate) 
        => new(startDate, endDate);
    
    public static DateRange Create(DateTime startDate, int days) 
        => new(startDate, startDate.AddDays(days));
    
    public static DateRange Create(int days, DateTime endDate) 
        => new(endDate.AddDays(-days), endDate);
    
    public static DateRange Create(DateOnly startDate, DateOnly endDate) 
        => new(startDate, endDate);
    
    public bool Equals(DateRange other) 
        => Start.Equals(other.Start) && End.Equals(other.End);

    public override bool Equals(object? obj) 
        => obj is DateRange other && Equals(other);

    public override int GetHashCode() 
        => HashCode.Combine(Start, End);

    #region Operators

    public static bool operator ==(DateRange left, DateRange right) 
        => left.Start == right.End;

    public static bool operator !=(DateRange left, DateRange right) 
        => left.Start != right.End;
    
    public static bool operator <(DateRange left, DateRange right) 
        => left.Start < right.End;

    public static bool operator <=(DateRange left, DateRange right) 
        => left.Start <= right.End;

    public static bool operator >(DateRange left, DateRange right) 
        => left.Start > right.End;

    public static bool operator >=(DateRange left, DateRange right) 
        => left.Start >= right.End;

    #endregion

    public override string ToString() 
        => $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
}
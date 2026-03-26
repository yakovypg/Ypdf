using System;
using System.Data;
using System.Globalization;

namespace Ypdf.Core.Geometry;

public class MathExpression
{
    public MathExpression(string expression)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));
        Expression = expression;
    }

    public string Expression { get; }

    public static MathExpression Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));
        return new MathExpression(data);
    }

    public double CalculateFloat()
    {
        object result = Calculate();
        return Convert.ToSingle(result, CultureInfo.InvariantCulture);
    }

    public double CalculateDouble()
    {
        object result = Calculate();
        return Convert.ToDouble(result, CultureInfo.InvariantCulture);
    }

    public long CalculateInt()
    {
        object result = Calculate();
        return Convert.ToInt32(result, CultureInfo.InvariantCulture);
    }

    public long CalculateLong()
    {
        object result = Calculate();
        return Convert.ToInt64(result, CultureInfo.InvariantCulture);
    }

    protected object Calculate()
    {
        using var dataTable = new DataTable();
        return dataTable.Compute(Expression, null);
    }
}

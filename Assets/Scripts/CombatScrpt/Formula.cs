using System;
using System.Data;
using System.Globalization;

public static class Formula
{
    private static readonly DataTable _dt = new DataTable();

    public static double Eval(string expr, int a, int b, int c)  // ΩΩ∑‘ 3∞≥ ±‚¡ÿ  abc 
    {
        expr = ReplaceVar(expr, "a", a);
        expr = ReplaceVar(expr, "b", b);
        expr = ReplaceVar(expr, "c", c);

        object result = _dt.Compute(expr, null);
        return Convert.ToDouble(result, CultureInfo.InvariantCulture);
    }

    private static string ReplaceVar(string expr, string varName, int value)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            expr,
            $@"\b{varName}\b",
            value.ToString(CultureInfo.InvariantCulture)
        );
    }
}


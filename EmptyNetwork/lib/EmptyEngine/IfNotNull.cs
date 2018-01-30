using EmptyEngine;
using System;

public static class ExtensionMethods
{
    /// <summary>
    /// Use this method to safely invoke properties, methods or other members of a nullable
    /// object. It will perform a null reference check on the object and if it is not null will
    /// invoke the specified expression. The expression has to return a reference type.
    /// </summary>
    /// <returns>Returns a reference type.</returns>
    public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> func, Action ifNull = null)
        where TResult : class
    {
        if (obj != null)
            return func(obj);
        else
        {
            ifNull.Execute();
            return default(TResult);
        }
    }

    public static void IfNotNull<T>(this T obj, Action<T> func, Action ifNull = null)
    {
        if (obj != null)
            func(obj);
        else
        {
            ifNull.Execute();
        }
    }

    /// <summary>
    /// Use this method to safely invoke properties, methods or other members of a nullable
    /// object. It will perform a null reference check on the object and if it is not null will
    /// invoke the specified expression. The expression has to return a value type.
    /// </summary>
    /// <returns>Returns a nullable value type.</returns>
    public static TResult IfNotNullVal<T, TResult>(this T obj, Func<T, TResult> func, Action ifNull = null)
        where TResult : struct
    {
        if (obj != null)
            return func(obj);
        else
        {
            ifNull.Execute();
            return default(TResult);
        }
    }

    public static void Dump(this object obj)
    {
        Debugs.Log(obj);
    }
}

﻿namespace Sqliste.Core.SqlAnnotations.SqlAnnotations;

public class InvalidSqlAnnotationPropertyValueException : InvalidOperationException
{
    public InvalidSqlAnnotationPropertyValueException(string? propertyName, string value) 
        : base($"{value} isn't assignable to {propertyName}")
    {
        //
    }
}
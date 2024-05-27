﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rscconventer.JavaGenerator.Exceptions;

public class NoSuchFieldException : Exception
{
    public string? FieldName  { get; private set; }
    public FieldDefinition? Field { get; private set; }
    public NoSuchFieldException(string message, string fieldName) : base(message)
    {
        FieldName = fieldName;
    }
    public NoSuchFieldException(string fieldName) : base()
    {
        FieldName = fieldName;
    }
    public NoSuchFieldException(FieldDefinition field) : base()
    {
        Field = field;
        FieldName = field.Name;
    }
}

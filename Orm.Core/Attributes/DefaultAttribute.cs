using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DefaultAttribute : Attribute
{
    public object Value { get; }

    public DefaultAttribute(object value)
    {
        Value = value;
    }
}

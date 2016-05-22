using System;

namespace OneDo.Model.Business.Validation
{
    public interface IPropertyRule
    {
        bool Test(object value);
    }

    public interface IPropertyRule<T> : IPropertyRule
    {
        bool Test(T value);
    }
}
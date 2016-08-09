using System;

namespace OneDo.Model.Data.Objects
{
    public interface IModel<T> where T : class
    {
        Guid Id { get; set; }

#warning Správně klonovat!
        T Clone();
    }
}

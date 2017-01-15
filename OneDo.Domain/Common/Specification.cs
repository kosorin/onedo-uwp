using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Common
{
    public abstract class Specification<T>
    {
        private Func<T, bool> compiledExpression;
        private Func<T, bool> CompiledExpression
        {
            get { return compiledExpression ?? (compiledExpression = ToExpression().Compile()); }
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T @object)
        {
            return CompiledExpression(@object);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain
{
    public abstract class Specification<TEntity> where TEntity : Entity
    {
        private Func<TEntity, bool> compiledExpression;
        private Func<TEntity, bool> CompiledExpression
        {
            get { return compiledExpression ?? (compiledExpression = ToExpression().Compile()); }
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public bool IsSatisfiedBy(TEntity entity)
        {
            return CompiledExpression(entity);
        }
    }
}

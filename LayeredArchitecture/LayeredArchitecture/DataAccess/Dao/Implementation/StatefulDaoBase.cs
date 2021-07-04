using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Base.Model;
using LayeredArchitecture.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.DataAccess.Dao
{
    /// <summary>
    /// A Stateful Dao to for a Save method on Entities, which have a <see cref="EntityStateEnum"/> attached. The Entity State will be reset 
    /// to <see cref="EntityStateEnum.None"/> after invoking a Save.
    /// </summary>
    /// <typeparam name="TEntityType"></typeparam>
    public abstract class StatefulDaoBase<TEntityType> : DaoBase<TEntityType>
        where TEntityType: class, IStatefulEntity
    {
        public StatefulDaoBase(IDbConnectionScopeResolver dbConnectionScopeResolver, IUserAccessor userAccessor, SqlClient sqlClient) : base(dbConnectionScopeResolver, userAccessor, sqlClient)
        {
        }

        public async Task SaveAsync(IEnumerable<TEntityType> entities)
        {
            // Maybe return an IAsyncEnumerable here?
            foreach(var entity in entities)
            {
                await SaveAsync(entity);
            }
        }

        public async Task SaveAsync(TEntityType entity, CancellationToken cancellationToken = default)
        {
            switch(entity.EntityState)
            {
                case EntityStateEnum.Add:
                    await InsertAsync(entity, cancellationToken);
                    break;
                case EntityStateEnum.Update:
                    await UpdateAsync(entity, cancellationToken);
                    break;
                case EntityStateEnum.Remove:
                    await DeleteAsync(entity, cancellationToken);
                    break;
            }

            entity.EntityState = EntityStateEnum.None;
        }
    }
}

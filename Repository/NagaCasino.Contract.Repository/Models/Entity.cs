using Invedia.Data.EF.Models;
using TechDrum.Core.Utils;
using System;

namespace TechDrum.Contract.Repository.Models
{
    public abstract class Entity : StringEntity
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");

            CreatedTime = LastUpdatedTime = CoreHelper.SystemTimeNow;
        }
    }
}

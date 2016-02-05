using System;

namespace Messy.DDD
{
    public abstract class Entity : IEntity, IEquatable<IEntity>
    {
        public Guid Id { get; protected set; }

        public virtual bool Equals(IEntity other)
        {
            return null != other && other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IEntity);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

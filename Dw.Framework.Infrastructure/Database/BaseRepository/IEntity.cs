using System;

namespace Dw.Framework.Infrastructure.Database
{
    public interface IEntity : IEntity<int>
    {
    }

    public interface IEntity<TPrimaryKey> : IPrimaryKeyTrack<TPrimaryKey>, ITrack
    {
    }
    public interface IPrimaryKeyTrack<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}

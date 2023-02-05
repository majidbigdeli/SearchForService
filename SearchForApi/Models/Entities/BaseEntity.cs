using System;

namespace SearchForApi.Models.Entities
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
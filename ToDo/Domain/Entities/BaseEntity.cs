﻿namespace Domain.Entities
{
	public abstract class BaseEntity
	{
        public Guid Id { get; set; } = Guid.NewGuid();
		public DateTime CreatedAt { get; set; }

		protected BaseEntity(Guid id, DateTime createdAt)
        {
			Id = id;
			CreatedAt = createdAt;
		}
    }
}

﻿using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class Project : IEntityModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}
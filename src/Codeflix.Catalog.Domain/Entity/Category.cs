﻿using System.Net.Http.Headers;
using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.Domain.Entity
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Category(string name, string description, bool isActive = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.Now;

            Validate();
        }
        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
            if (Name.Length < 3)
                throw new EntityValidationException($"{nameof(Name)} should be at leats 3 characters long");
            if (Description == null)
                throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
        }

    } }
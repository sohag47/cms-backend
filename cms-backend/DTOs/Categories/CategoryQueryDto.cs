﻿using cms_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.DTOs.Categories
{
    public class CategoryQueryDto
    {
        public string? Search { get; set; }
        public int? ParentId { get; set; }
        public CategoryStatus? Status { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

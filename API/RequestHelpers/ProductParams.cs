﻿namespace eCommerceAPI.API.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
    }
}

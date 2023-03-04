﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021198.DomainModels;

namespace _19T1021198.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm phân trang đối với Nhân viên
    /// </summary>
    public class EmployeeSearchOutput : PaginationSearchOutput
    {
        public List<Employee> Data { get; set; }
    }
}
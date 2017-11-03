﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Entities.Models
{
    [Table("CategoryRelationships")]
    public class CategoryRelationships : BaseModel
    {
      
        public  CategoryInfo CategoryInfo { get; set; }
        public  CategoryInfo ParentCategoryInfo { get; set; }
 
    }
}

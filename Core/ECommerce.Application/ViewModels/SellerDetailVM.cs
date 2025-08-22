using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ViewModels
{
    public class SellerDetailVM
    {
        public SellerDTO Seller { get; set; }
        public AppUserDTO AppUser { get; set; }
    }
}

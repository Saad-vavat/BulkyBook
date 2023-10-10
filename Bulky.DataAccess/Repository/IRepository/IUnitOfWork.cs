using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork 
    {
        public ICategoryRepository category { get; }

        public IProductRepository product { get; }

        public ICompanyRepository company { get; }

        public IShoppingCartRepository shoppingCart { get; }

        public IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}

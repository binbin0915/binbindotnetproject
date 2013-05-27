using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.IBLL;
namespace LYZJ.UserLimitMVC.BLL
{

    public class RoleService : BaseService<Role>, IRoleService
    {

        //重写抽象方法，设置当前仓储为Role仓储

        public override void SetCurrentRepository()
        {

            //设置当前仓储为Role仓储

            CurrentRepository = DAL.RepositoryFactory.RoleRepository;

        }

    }

}
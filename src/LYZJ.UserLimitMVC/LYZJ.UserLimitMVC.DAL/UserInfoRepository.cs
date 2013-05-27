using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.IDAL;

namespace LYZJ.UserLimitMVC.DAL
{
    public  class UserInfoRepository : BaseRepository<UserInfo>, IUserInfoRepository
    {
    }
}

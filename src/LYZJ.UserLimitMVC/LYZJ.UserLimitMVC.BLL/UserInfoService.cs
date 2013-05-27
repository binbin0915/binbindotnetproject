using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYZJ.UserLimitMVC.IDAL;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.Model;
using LYZJ.UserLimitMVC.IBLL;
namespace LYZJ.UserLimitMVC.BLL
{

    /// <summary>

    /// UserInfo业务逻辑

    /// </summary>

    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {

        public override void SetCurrentRepository()
        {

            CurrentRepository = DAL.RepositoryFactory.UserInfoRepository;

        }

        //访问DAL实现CRUD

        //private DAL.UserInfoRepository _userInfoRepository = new UserInfoRepository();

        //依赖接口编程

        //private IUserInfoRepository _userInfoRepository = new UserInfoRepository();

        //private IUserInfoRepository _userInfoRepository = RepositoryFactory.UserInfoRepository;

        //public  UserInfo AddUserInfo(UserInfo userInfo)

        //{

        //    return _userInfoRepository.AddEntity(userInfo);

        //}

        //public bool UpdateUserInfo(UserInfo userInfo)

        //{

        //    return _userInfoRepository.UpdateEntity(userInfo);

        //}

    }

}
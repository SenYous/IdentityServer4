using MicroService.Interface;
using MicroService.Model;
using System;
using System.Collections.Generic;

namespace MicroService.Service
{
    public class UserService : IUserService
    {
        #region DataInit

        private List<User> _UserList = new List<User>()
        {
            new User(){
                Id=1,
                Account="Administrator",
                Email="1358798471@qq.com",
                Name="CK",
                Password="123456",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
            new User(){
                Id=2,
                Account="Apple",
                Email="1358798471@qq.com",
                Name="AppleName",
                Password="123123",
                LoginTime=DateTime.Now,
                Role="User"
            }
        };

        #endregion

        public User FindUser(int id)
        {
            return this._UserList.Find(f => f.Id == id);
        }

        public IEnumerable<User> UserAll()
        {
            return this._UserList;
        }
    }
}

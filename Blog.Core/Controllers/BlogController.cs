using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 发表博客
        /// </summary>
        /// <param name="love">Model类实体参数</param>
        [HttpPost]
        public void Post([FromBody] Love love)
        {
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]//隐藏接口
        public void Delete(int id)
        {
        }

        [HttpGet("{name}&{password}")]
        public async Task<object> GetJwt(string name, string password)
        {
            string jwt = string.Empty;
            bool success = false;
            //获取用户的角色名
            var userRole = await GetUserRoleName(name, password);
            if (userRole != null)
            {
                TokenModelJwt tokenModelJwt = new TokenModelJwt { Uid = 1, Role = userRole };
                jwt = JwtHelper.IssueJwt(tokenModelJwt);//获取token
                success = true;
            }
            else
            {
                jwt = "login fail!!!";
            }
            return Ok(new { success = success, token = jwt });
        }

        public static async Task<string> GetUserRoleName(string name, string password)
        {
            if (name == "admin" && password == "123")
            {
                return "Admin";
            }
            return null;
        }
    }
}

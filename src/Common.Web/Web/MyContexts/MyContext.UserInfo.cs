using Microsoft.AspNetCore.Http;

namespace Common.Web.MyContexts
{
    public class MyUserInfo
    {
        public string Site { get; set; }
        public string User { get; set; }
    }

    public static class MyUserInfoExtensions
    {
        private static string Group_UserInfos = "UserInfos";
        public static string GetGroupName_UserInfos(this MyContext context)
        {
            return Group_UserInfos;
        }

        public static MyContext SetUserInfo(this MyContext context, HttpRequest httpRequest)
        {
            var theGroup = context.GetOrCreate(context.GetGroupName_UserInfos());
            //todo
            return context;
        }

        public static MyUserInfo GetUserInfo(this MyContext context)
        {
            var info = new MyUserInfo();
            var theGroup = context.GetOrCreate(context.GetGroupName_UserInfos());
            theGroup.Items.SetProperties(info);
            return info;
        }
    }
}
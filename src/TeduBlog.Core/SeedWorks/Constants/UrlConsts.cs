namespace TeduBlog.Core.SeedWorks.Constants
{
    public static class UrlConsts
    {
        public static string Posts = "/posts";
        public static string Home = "/";
        public static string About = "/about";
        public static string Contact = "/contact";
        public static string PostsByCategorySlug = "/posts/{0}";
        public static string PostDetails = "/post/{0}";
        public static string PostsByTagSlug = "/tag/{0}";
        public static string Login = "/login";
        public static string Register = "/register";
        public static string Profile = "/profile";
        public static string Author = "/author/{0}";
        public static string Series = "/series";
        public static string SeriesDetail = "/series/{0}";
        public static string ChangeProfile = "/profile/edit";
        public static string ForgotPassword = "/forgot-password";
        public static string ResetPassword = "/reset-password";

        public static string CreatePost = "/profile/posts/create";
        public static string PostListByUser = "/profile/posts/list";


        public static string ChangePassword = "/profile/change-password";
        public static string SendApprovalPost = "/profile/post/send-approve";

    }
}
